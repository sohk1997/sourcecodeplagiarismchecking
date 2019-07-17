from common import Config, VocabType
from argparse import ArgumentParser
from interactive_predict import InteractivePredictor
from model import Model
import rawParser
import sys
import mysql.connector
from mysql.connector.connection import MySQLCursorPrepared
import findnearest
from treecompare import TreeCompare
import json

connectionConfig = {
  'user': 'root',
  'password': '1234567890',
  'host': '35.198.247.133',
  'database': 'SourceCodePlagiarism',
  'use_pure' : True
}

def mapResult(element):
    result = {}
    result['Vector'] = element[0]
    result['MethodString'] = element[1]
    result['tree'] = element[2]
    return result

if __name__ == '__main__':

    parser = ArgumentParser()
    listMethod = rawParser.parse()

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
    for i,method in enumerate(listMethod):
        f = open("Input.java", "w")
        f.write(method['processedContent'])
        f.close()
        predictor = InteractivePredictor(config, model)
        code_vector = predictor.predict('Input.java')
        method['Vector'] = code_vector
        try:
            connection = mysql.connector.connect(**connectionConfig)
            cursor = connection.cursor()
            cursor.execute("SELECT Vector,MethodString,ParseTree FROM Method")
            queryResult = map(mapResult,cursor.fetchall())
            nearMethods = findnearest.nearest(method, queryResult, i)
            result = {}
            maxMatchRatio = 0
            for nearMethod in nearMethods:
                treecompare = TreeCompare()
                tree_1 = method['tree']
                tree_2 = json.loads(nearMethod['tree'].replace("'",'"')) 
                ratio, match = treecompare.compare(tree_1,tree_2)
                if(maxMatchRatio < ratio):
                    result = match
            print(result)
        except mysql.connector.Error as error :
            connection.rollback()
            print("Failed to insert into MySQL table {}".format(error))
        finally:
        #closing database connection.
            if(connection.is_connected()):
                cursor.close()
                connection.close()
                print("MySQL connection is closed")
    model.close_session()
