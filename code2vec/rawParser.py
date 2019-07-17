import json
import subprocess

def parse():
    path = 'target/TestAST-1.0-jar-with-dependencies.jar'
    command = ['java', '-jar', path]
    # command = ["ls", "-l", "."]
    subprocess.call(command)
    # process.communicate()

    with open('json.json') as f:
        objectList = f.read()
        jsonList = json.loads(str(objectList))
        return jsonList
