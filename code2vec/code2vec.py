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

# connectionConfig = {
#   'user': 'root',
#   'password': '1234567890',
#   'host': '35.198.247.133',
#   'database': 'SourceCodePlagiarism',
#   'use_pure' : True
# }

connectionConfig = {
  'user': 'root',
  'password': '12345687',
  'host': 'localhost',
  'database': 'SourceCodePlagiarism',
  'use_pure' : True
}

def mapSingleDocumentResult(element):
    result = {}
    result['DocumentId'] = element[0]
    result['FileUrl'] = element[1]
    return result

def mapResult(element):
    result = {}
    result['Id'] = element[0]
    result['Vector'] = element[1]
    result['MethodString'] = element[2]
    result['tree'] = element[3]
    return result

def mapDetail(element):
    result = {}
    result['Id'] = element[0]
    x = np.array(element[1].replace('[[','').replace(']]','').replace('[','').replace(']','').split())
    y = x.astype(np.float)
    result['Vector'] = y
    result['tree'] = json.loads(element[3].replace("'",'"'))
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
        cursor.execute("""SELECT DocumentId,FileUrl FROM Documents WHERE Id = %(id)s""",{'id' : id})
        queryResult = list(map(mapSingleDocumentResult,cursor.fetchall()))[0]
    except mysql.connector.Error as error :
        print(str(error))
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")
        return queryResult['DocumentId'],queryResult['FileUrl']        
def get_method_peer(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        # cursor.execute("SELECT M.Id, M.Vector, M.MethodString, M.ParseTree FROM Method M JOIN Documents D ON M.SourceCodeId = D.DocumentId AND D.Type = 1 AND D.Id <> %(id)s", {'id' : id})
        cursor.execute("SELECT M.Id, M.Vector, M.MethodString, M.ParseTree FROM Method M JOIN Documents D ON M.DocumentId = D.DocumentId AND D.Type = 1 AND D.Id <> %(id)s", {'id' : id})
        queryResult = map(mapResult,cursor.fetchall())
        result = list(queryResult)
    except mysql.connector.Error as error :
        print(str(error))
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")
        return result 

def get_method_source(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        # cursor.execute("SELECT M.Id,M.Vector,M.MethodString,M.ParseTree FROM Method M JOIN Documents D ON M.SourceCodeId = D.DocumentId AND D.Id = %(id)s", {'id' : id})
        cursor.execute("SELECT M.Id,M.Vector,M.MethodString,M.ParseTree FROM Method M JOIN Documents D ON M.DocumentId = D.DocumentId AND D.Id = %(id)s", {'id' : id})
        queryResult = map(mapDetail,cursor.fetchall())
        result = list(queryResult)
    except mysql.connector.Error as error :
        print(str(error))
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")
        return result 

def get_method_web():
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        # cursor.execute("SELECT M.Id,M.Vector,M.MethodString,M.ParseTree FROM Method M JOIN Documents D ON M.SourceCodeId = D.DocumentId AND D.Type = 2")
        cursor.execute("SELECT M.Id,M.Vector,M.MethodString,M.ParseTree FROM Method M JOIN Documents D ON M.DocumentId = D.DocumentId AND D.Type = 2")
        queryResult = map(mapResult,cursor.fetchall())
        result = list(queryResult)
    except mysql.connector.Error as error :
        print(str(error))
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")
        return result           

def insert_method(method, sourceid):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        sql_insert_query = """ INSERT INTO `Method`
                               (`StartLine`, `EndLine`, `Vector`, `ParseTree`, `MethodString`, `SourceCodeId`, `MethodName`) VALUES (%s,%s,%s,%s, %s, %s, %s)"""
        insert_tuple = (method['startLine'], method['endLine'], str(method['Vector']), str(method['tree']), str(method['baseMethod']), str(sourceid), method['methodName'])
        cursor = connection.cursor(cursor_class=MySQLCursorPrepared)
        cursor.execute(sql_insert_query,insert_tuple)
        connection.commit()
        method['Id'] = cursor.lastrowid
    except mysql.connector.Error as error :
        print(str(error))
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")
def insert_result(method,methodResult,maxMatchRatio, detail):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor(cursor_class=MySQLCursorPrepared)
        sql_insert_query = """ INSERT INTO `Result`
                               (`BaseMethodId`, `SimMethodId`, `SimRatio`, `ResultDetail`) VALUES (%s,%s,%s,%s)"""
        insert_tuple = (method['Id'], methodResult['Id'], str(maxMatchRatio), str(detail))
        cursor.execute(sql_insert_query,insert_tuple)
        connection.commit()
    except mysql.connector.Error as error :
        print(str(error))
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")

def update_document(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        sql_update_query = 'UPDATE Documents SET Status=2 WHERE Id=%(id)s'
        param ={'id' : id}
        cursor.execute(sql_update_query,param)
        connection.commit()
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")

def update_document_no_sim(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        sql_update_query = 'UPDATE Documents SET Status=3 WHERE Id=%(id)s AND Status = 1'
        param ={'id' : id}
        cursor.execute(sql_update_query,param)
        connection.commit()
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")

def count_method(id):
    try:
        connection = mysql.connector.connect(**connectionConfig)
        cursor = connection.cursor()
        # cursor.execute("SELECT COUNT(*) FROM Method M JOIN Documents D ON M.SourceCodeId = D.DocumentId AND D.Id = %(id)s", {'id' : id})
        cursor.execute("SELECT COUNT(*) FROM Method M JOIN Documents D ON M.DocumentId = D.DocumentId AND D.Id = %(id)s", {'id' : id})
        queryResult = cursor.fetchall()
        result = list(queryResult)[0][0]
        return result
    except mysql.connector.Error as error :
        print(str(error))
    finally:
        if(connection.is_connected()):
            cursor.close()
            connection.close()
            print("MySQL connection is closed")
        return result 

def callback(ch, method, properties, body):
    objectString = body.decode('ascii')
    objectData = json.loads(objectString)
    print(objectData)
    value = objectData['id']
    webcheck = objectData['webCheck']
    peercheck = objectData['peerCheck']
    files = glob.glob('BaseInput/*')
    for f in files:
        os.remove(f)
    documentid,url = get_file(value)
    exist = count_method(value)
    if(exist == 0):
        print(url)
        # fake user agent of Safari
        fake_useragent = 'Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25'
        r = request.Request(url, headers={'User-Agent': fake_useragent})
        f = request.urlopen(r)
        wf = open('BaseInput/BaseInput.java','w+b')
        wf.write(f.read())
        wf.close()

        listMethod = rawParser.parse()
        matchList = []
        sim = False
        for method in listMethod:
            f = open("Input.java", "w")
            f.write(method['processedContent'])
            f.close()
            code_vector = predictor.predict('Input.java')
            if(code_vector is None):
                continue
            method['Vector'] = code_vector[0]
            print('Insert method')
            insert_method(method,documentid)
            print('Insert method done')
    if(peercheck) :
        current_methods = get_method_peer(value)
        #Peer check
        for method in listMethod:
            # print(method)
            if('Vector' not in method):
                continue    
            nearMethods = findnearest.nearest(method, current_methods)
            result = {}
            maxMatchRatio = 0
            for nearMethod in nearMethods:
                treecompare = TreeCompare()
                tree_1 = method['tree']
                tree_2 = json.loads(nearMethod['tree'].replace("'",'"')) 
                ratio, match = treecompare.compare(tree_1,tree_2)
                if(maxMatchRatio < ratio):
                    maxMatchRatio = ratio
                    matchResult = match
                    methodResult = nearMethod
            if(maxMatchRatio >= 0.5):
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
                print('Insert result')
                insert_result(method, methodResult,maxMatchRatio, detail)
                sim = True
                print('Insert result done')   
        if(sim):
            update_document(value) 
        else: 
            if(not webcheck):
                update_document_no_sim(value)
    if(webcheck):
        # rabbitMQConnection = pika.BlockingConnection(pika.URLParameters('amqp://tojbmlqr:fO9Tcz9MRDmzl1J0B_56LBT3BO1VPxWB@mustang.rmq.cloudamqp.com/tojbmlqr'))
        rabbitMQConnection = pika.BlockingConnection(pika.URLParameters('amqp://xeaqxoyf:G-WMfhAerBMl2Heg0f3E3rSS5QIDD2D_@mustang.rmq.cloudamqp.com/xeaqxoyf'))
        rabbitMQChannel = rabbitMQConnection.channel()  
        rabbitMQChannel.queue_declare(queue='webcheck')
        sendingObject = {
            "Url" : url,
            "Id" : value
        }
        print('send message')
        rabbitMQChannel.basic_publish(exchange='',routing_key='webcheck',body = str(sendingObject))
        print('send message done')
    if('continueWebCheck' in objectData):
        print('Start check on web source')
        id = objectData['continueWebCheck']
        base_methods = get_method_source(id)
        current_methods = get_method_web()
        sim = False
        #Web check after find on Internet
        for method in base_methods:    
            nearMethods = findnearest.nearest(method, current_methods)
            result = {}
            maxMatchRatio = 0
            for nearMethod in nearMethods:
                treecompare = TreeCompare()
                tree_1 = method['tree']
                tree_2 = json.loads(nearMethod['tree'].replace("'",'"')) 
                ratio, match = treecompare.compare(tree_1,tree_2)
                if(maxMatchRatio < ratio):
                    maxMatchRatio = ratio
                    matchResult = match
                    methodResult = nearMethod
            if(maxMatchRatio >= 0.5):
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
                print('Insert result web check')
                insert_result(method, methodResult,maxMatchRatio, detail)
                sim = True
                print('Insert result web check done')  
        print('Update document')
        if(sim):
            update_document(id)
        else:
            update_document_no_sim(id)

    print('Done update')        

if __name__ == '__main__':

    parser = ArgumentParser()

    # rabbitMQConnection = pika.BlockingConnection(pika.URLParameters('amqp://tojbmlqr:fO9Tcz9MRDmzl1J0B_56LBT3BO1VPxWB@mustang.rmq.cloudamqp.com/tojbmlqr'))
    rabbitMQConnection = pika.BlockingConnection(pika.URLParameters('amqp://xeaqxoyf:G-WMfhAerBMl2Heg0f3E3rSS5QIDD2D_@mustang.rmq.cloudamqp.com/xeaqxoyf'))
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
