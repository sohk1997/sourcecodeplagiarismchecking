class TreeCompare:
    MIN_SUBTREE_MATCH = 4
    MAX_SUBTREE_COUNT = 0

    def __init__(self):
        self.hashMap = dict()      
        self.subtreeMap = dict()
        self.resultSet = []

    def gethashfromstring(self, string):
        BASE =  int(1e9 + 7)
        return abs(hash(string)) % BASE

    def build_hash(self, node, save_hash):
        BASE =  int(1e9 + 7)
        result = 0
        node['subtreecount'] = 1
        for childnode in node['childNodes']:
            result = result + self.build_hash(childnode, save_hash)
            result = result % BASE
            childnode['parent'] = node
            node['subtreecount'] += childnode['subtreecount']
        result = result + self.gethashfromstring(node['type'])
        result = result % BASE
        node['hash'] = result
        if(save_hash and node['subtreecount'] > self.MIN_SUBTREE_MATCH):
            if(node['hash'] not in self.hashMap):
                self.hashMap[node['hash']] = []
            self.hashMap[node['hash']].append(node)
        return result

    def find_sim(self, node):
        result = 0
        if(node['subtreecount'] > self.MIN_SUBTREE_MATCH):
            if(node['hash'] in self.hashMap):
                node['sim'] = True
                sim_node = self.hashMap[node['hash']][0]
                sim_node['sim'] = True
                self.resultSet.append(((node['startLine'] - self.base_node_1['startLine'],node['endLine'] - self.base_node_1['startLine']),(sim_node['startLine'] - self.base_node_2['startLine'],sim_node['endLine'] - self.base_node_2['startLine'])))
                return node['subtreecount']
            for childNode in node['childNodes']:
                result += self.find_sim(childNode)
        return result

    def check(node_1,node_2):
        sim_node_count = 0
        for childNode1 in node_1['childNodes']:
            childNode1['mark'] = False
        for childNode2 in node_2['childNodes']:
            childNode2['mark'] = False
        for childNode1 in node_1['childNodes']:
            for childNode2 in node_2['childNodes']:
                if(not childNode1['mark'] and not childNode2['mark']):
                    if(childNode2 in childNode1['ref']):
                        childNode1['mark'] = True
                        childNode2['mark'] = True
                        sim_node_count += 1
                        break
        if(sim_node_count == len(node_1['childNodes']) or sim_node_count == len(node_2['childNodes'])):
            return True
        


    def compare(self, tree_1,tree_2):
        self.base_node_1 = tree_1
        self.base_node_2 = tree_2
        self.build_hash(tree_1, False)
        self.build_hash(tree_2, True)

        # self.find_sim(tree_1)
        result = self.find_sim(tree_1)
        return (float(result) / tree_1['subtreecount'], self.resultSet)

        

    