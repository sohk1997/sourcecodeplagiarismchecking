from treecompare import TreeCompare
import json

compareror = TreeCompare()

with open('json.json') as f:
    objectList = f.read()
    tree_1 = json.loads(str(objectList))
with open('json2.json') as f:
    objectList = f.read()
    tree_2 = json.loads(str(objectList))

result = compareror.compare(tree_1,tree_2)
# print(result)