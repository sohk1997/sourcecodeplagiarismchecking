from common import Config, VocabType
from argparse import ArgumentParser
from interactive_predict import InteractivePredictor
from model import Model
import rawParser
import sys
import mysql.connector
from mysql.connector.connection import MySQLCursorPrepared
import pika
import urllib.request as request
import os
import glob
import shutil
from treecompare import TreeCompare
import findnearest
import json
import numpy as np
from os import system

connectionConfig = {
  'user': 'root',
  'password': '1234567890',
  'host': '35.198.247.133',
  'database': 'SourceCodePlagiarism',
  'use_pure' : True
}

def mapSingleDocumentResult(element):
    result = {}
    result['DocumentId'] = element[0]
    result['FileUrl'] = element[1]
    result['DocumentName'] = element[2]
    return result

def mapResult(element):
    result = {}
    result['Id'] = element[0]
    result['Vector'] = element[1]
    result['MethodString'] = element[2]
    result['tree'] = element[3]
    result['methodName'] = element[4]
    return result

def mapDetail(element):
    result = {}
    result['Id'] = element[0]
    x = np.array(json.loads(element[1]))
    y = x.astype(np.float)
    result['Vector'] = y
    result['tree'] = json.loads(element[3])
    result['methodName'] = element[4]
    return result

def mergeResult(baseList):
    result = []
    start = baseList[0][0]
    end = baseList[0][1]
    for element in baseList:
        if(end + 1 < element[0]):
            result.append({'StartLine': start, 'EndLine':end })
            start = element[0]
            end = element[1]
        else:
            if(end < element[1]):
                end = element[1]
    result.append({'StartLine': start, 'EndLine':end })
    return result

def get_file(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        cursor.execute("""SELECT DocumentId,FileUrl, DocumentName FROM Submission WHERE Id = %(id)s""",{'id' : id})
        queryResult = list(map(mapSingleDocumentResult,cursor.fetchall()))[0]
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
        return queryResult['DocumentId'],queryResult['FileUrl'], queryResult['DocumentName']        
def get_method_peer(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        cursor.execute("SELECT M.Id, M.Vector, M.MethodString, M.ParseTree,M.MethodName FROM CodeDetail M JOIN Submission D ON M.SourceCodeId = D.DocumentId AND D.Type = 1 AND D.Id <> %(id)s", {'id' : id})
        queryResult = map(mapResult,cursor.fetchall())
        result = list(queryResult)
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
        return result 

def get_method_source(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        cursor.execute("SELECT M.Id,M.Vector,M.MethodString,M.ParseTree,M.MethodName FROM CodeDetail M JOIN Submission D ON M.SourceCodeId = D.DocumentId AND D.Id = %(id)s", {'id' : id})
        queryResult = map(mapDetail,cursor.fetchall())
        result = list(queryResult)
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
        return result 

def get_method_web(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        cursor.execute("SELECT M.Id,M.Vector,M.MethodString,M.ParseTree,M.MethodName FROM CodeDetail M JOIN Submission D ON M.SourceCodeId = D.DocumentId AND D.Type = 2 AND D.Id = %(id)s", {'id' : id})
        queryResult = map(mapResult,cursor.fetchall())
        result = list(queryResult)
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
        return result           

def insert_method(method, sourceid):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        sql_insert_query = """ INSERT INTO `CodeDetail`
                               (`StartLine`, `EndLine`, `Vector`, `ParseTree`, `MethodString`, `SourceCodeId`, `MethodName`) VALUES (%s,%s,%s,%s, %s, %s, %s)"""
        insert_tuple = (method['startLine'], method['endLine'], json.dumps(method['Vector'].tolist()), json.dumps(method['tree']), str(method['baseMethod']), str(sourceid), method['methodName'])
        cursor = connection.cursor(cursor_class=MySQLCursorPrepared)
        cursor.execute(sql_insert_query,insert_tuple)
        connection.commit()
        method['Id'] = cursor.lastrowid
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
def insert_result(method,methodResult,maxMatchRatio, detail):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor(cursor_class=MySQLCursorPrepared)
        sql_insert_query = """ INSERT INTO `Result`
                               (`BaseMethodId`, `SimMethodId`, `SimRatio`, `ResultDetail`) VALUES (%s,%s,%s,%s)"""
        insert_tuple = (method['Id'], methodResult['Id'], str(maxMatchRatio), json.dumps(detail))
        cursor.execute(sql_insert_query,insert_tuple)
        connection.commit()
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()

def update_document(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        sql_update_query = 'UPDATE Submission SET Status=2 WHERE Id=%(id)s'
        param ={'id' : id}
        cursor.execute(sql_update_query,param)
        connection.commit()
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()

def update_document_no_sim(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        sql_update_query = 'UPDATE Submission SET Status=3 WHERE Id=%(id)s AND Status = 1'
        param ={'id' : id}
        cursor.execute(sql_update_query,param)
        connection.commit()
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()

def count_method(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        cursor.execute("SELECT COUNT(*) FROM CodeDetail M JOIN Submission D ON M.SourceCodeId = D.DocumentId AND D.Id = %(id)s", {'id' : id})
        queryResult = cursor.fetchall()
        result = list(queryResult)[0][0]
        return result
    except mysql.connector.Error as error :
        print(str(error))
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
        return result 

def callback(ch, method, properties, body):
    system('clear') 
    objectString = body.decode('ascii')
    objectData = json.loads(objectString)
    value = objectData['id']
    webcheck = objectData['webCheck']
    peercheck = objectData['peerCheck']
    files = glob.glob('BaseInput/*')
    for f in files:
        os.remove(f)
    documentid,url,documentname = get_file(value)
    print('Receive checking request for file ' + documentname)
    exist = count_method(value)
    if(exist == 0):
        print('Start analyze file ' + documentname)
        # fake user agent of Safari
        fake_useragent = 'Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25'
        r = request.Request(url, headers={'User-Agent': fake_useragent})
        f = request.urlopen(r)
        wf = open('BaseInput/BaseInput.java','w+b')
        wf.write(f.read())
        wf.close()

        listMethod = rawParser.parse()
        print('Done parse AST for file ' + documentname)
        print('Total method in file ' + str(len(listMethod)))
        matchList = []
        sim = False
        for method in listMethod:
            f = open("Input.java", "w")
            print('')
            print('Start parse vector for method ' + method['methodName'])
            f.write(method['processedContent'])
            f.close()
            code_vector = predictor.predict('Input.java')
            print('Done parse vector for method ' + method['methodName'])
            if(code_vector is None):
                continue
            method['Vector'] = code_vector[0]
            insert_method(method,documentid)
    if(peercheck) :
        current_methods = get_method_peer(value)
        #Peer check
        for method in listMethod:
            if('Vector' not in method):
                continue    
            print('')
            print('Start compare method ' + method['methodName'])
            nearMethods = findnearest.nearest(method, current_methods)
            result = {}
            maxMatchRatio = 0
            for nearMethod in nearMethods:
                treecompare = TreeCompare()
                tree_1 = method['tree']
                tree_2 = json.loads(nearMethod['tree']) 
                ratio, match = treecompare.compare(tree_1,tree_2)
                if(maxMatchRatio < ratio):
                    maxMatchRatio = ratio
                    matchResult = match
                    methodResult = nearMethod
                
            if(maxMatchRatio >= 0.7):
                base_position = list(map(lambda element : element[0],matchResult))
                match_position = list(map(lambda element : element[1],matchResult))
                base_position.sort(key=lambda element: element[0])
                match_position.sort(key=lambda element: element[0])
                base_position = mergeResult(base_position)
                match_position = mergeResult(match_position)
                detail = {
                    'SourcePositions' : base_position,
                    'SimPositions' : match_position
                }
                insert_result(method, methodResult,maxMatchRatio, detail)
                sim = True
                print('There a similar method for method ' + method['methodName'] + ' named ' + methodResult['methodName'] + ' with ratio ' + str(maxMatchRatio * 100))
            else:
                print('No match result for method ' + method['methodName'])
        if(sim):
            update_document(value) 
        else: 
            if(not webcheck):
                update_document_no_sim(value)
    if(webcheck):
        rabbitMQConnection = pika.BlockingConnection(pika.URLParameters('amqp://tojbmlqr:fO9Tcz9MRDmzl1J0B_56LBT3BO1VPxWB@mustang.rmq.cloudamqp.com/tojbmlqr'))
        rabbitMQChannel = rabbitMQConnection.channel()  
        rabbitMQChannel.queue_declare(queue='webcheck')
        sendingObject = {
            "Url" : url,
            "Id" : value
        }
        print('Send web check request for file ' + documentname)
        rabbitMQChannel.basic_publish(exchange='',routing_key='webcheck',body = str(sendingObject))
        print('send message done')
    if('continueWebCheck' in objectData):
        print('Start check on web source')
        id = objectData['continueWebCheck']
        web_id = objectData['id']
        base_methods = get_method_source(id)
        current_methods = get_method_web(web_id)
        sim = False
        #Web check after find on Internet
        for method in base_methods:    
            print('')
            print('Start compare method ' + method['methodName'])
            nearMethods = findnearest.nearest(method, current_methods)
            result = {}
            maxMatchRatio = 0
            for nearMethod in nearMethods:
                treecompare = TreeCompare()
                tree_1 = method['tree']
                tree_2 = json.loads(nearMethod['tree']) 
                ratio, match = treecompare.compare(tree_1,tree_2)
                if(maxMatchRatio < ratio):
                    maxMatchRatio = ratio
                    matchResult = match
                    methodResult = nearMethod
                print('Method ' +nearMethod['methodName'] + ' ratio ' + str(ratio))
            if(maxMatchRatio >= 0.7):
                base_position = list(map(lambda element : element[0],matchResult))
                match_position = list(map(lambda element : element[1],matchResult))
                base_position.sort(key=lambda element: element[0])
                match_position.sort(key=lambda element: element[0])
                base_position = mergeResult(base_position)
                match_position = mergeResult(match_position)
                detail = {
                    'SourcePositions' : base_position,
                    'SimPositions' : match_position
                }
                # print('Insert result web check')
                insert_result(method, methodResult,maxMatchRatio, detail)
                sim = True
                # print('Insert result web check done')  
        if(sim):
            update_document(id)
        else:
            update_document_no_sim(id)
    if not webcheck and not peercheck:
        update_document_no_sim(id)


if __name__ == '__main__':

    parser = ArgumentParser()

    rabbitMQConnection = pika.BlockingConnection(pika.URLParameters('amqp://tojbmlqr:fO9Tcz9MRDmzl1J0B_56LBT3BO1VPxWB@mustang.rmq.cloudamqp.com/tojbmlqr'))
    rabbitMQChannel = rabbitMQConnection.channel()

    parser.add_argument("-d", "--data", dest="data_path",
                        help="path to preprocessed dataset", required=False)
    parser.add_argument("-te", "--test", dest="test_path",
                        help="path to test file", metavar="FILE", required=False)

    is_training = '--train' in sys.argv or '-tr' in sys.argv
    parser.add_argument("-s", "--save", dest="save_path",
                        help="path to save file", metavar="FILE", required=False)
    parser.add_argument("-w2v", "--save_word2v", dest="save_w2v",
                        help="path to save file", metavar="FILE", required=False)
    parser.add_argument("-t2v", "--save_target2v", dest="save_t2v",
                        help="path to save file", metavar="FILE", required=False)
    parser.add_argument("-l", "--load", dest="load_path",
                        help="path to save file", metavar="FILE", required=False)
    parser.add_argument('--save_w2v', dest='save_w2v', required=False,
                        help="save word (token) vectors in word2vec format")
    parser.add_argument('--save_t2v', dest='save_t2v', required=False,
                        help="save target vectors in word2vec format")
    parser.add_argument('--export_code_vectors', action='store_true', required=False,
                        help="export code vectors for the given examples")
    parser.add_argument('--release', action='store_true',
                        help='if specified and loading a trained model, release the loaded model for a lower model '
                             'size.')
    parser.add_argument('--predict', action='store_true')
    args = parser.parse_args()

    config = Config.get_default_config(args)

    model = Model(config)
    predictor = InteractivePredictor(config, model)

    
    rabbitMQChannel.queue_declare(queue='sourcecode')
    rabbitMQChannel.basic_consume(queue='sourcecode',
                      auto_ack=True,
                      on_message_callback=callback)
    print('Ready')
    rabbitMQChannel.start_consuming()

    model.close_session()
