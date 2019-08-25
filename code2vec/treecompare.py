import hashlib

class TreeCompare:
    MIN_SUBTREE_MATCH = 3
    MIN_MERGE_SIM = 0.5

    def __init__(self):
        self.hashMap = dict()      
        self.subtreeMap = dict()
        self.resultSet = []

    def gethashfromstring(self, string):
        sha_signature = hashlib.sha256(string.encode()).hexdigest()
        return sha_signature

    def build_hash(self, node, save_hash):
        BASE =  int(1e9 + 7)
        result = ''
        node['subtreecount'] = 1
        for childnode in node['childNodes']:
            result = result + self.build_hash(childnode, save_hash)
            result = self.gethashfromstring(result)
            childnode['parent'] = node
            node['subtreecount'] += childnode['subtreecount']
        result = result + self.gethashfromstring(node['type'])
        result =  self.gethashfromstring(result)
        node['hash'] = result
        if(save_hash and node['subtreecount'] >= self.MIN_SUBTREE_MATCH):
            if(node['hash'] not in self.hashMap):
                self.hashMap[node['hash']] = []
            self.hashMap[node['hash']].append(node)
        # print(node['type'] + ' ' + node['hash'])
        return result

    def find_sim(self, node):
        result = 0
        if(node['subtreecount'] > self.MIN_SUBTREE_MATCH):
            if(node['hash'] in self.hashMap):
                node['sim'] = True
                sim_node = self.hashMap[node['hash']][0]
                sim_node['sim'] = True
                self.resultSet.append(((node['startLine'] - self.base_node_1['startLine'],node['endLine'] - self.base_node_1['startLine']),(sim_node['startLine'] - self.base_node_2['startLine'],sim_node['endLine'] - self.base_node_2['startLine'])))
                self.hashMap[node['hash']].pop(0)
                # print('Find pair')
                # print(str(node['startLine']) + ' ' + str(node['endLine']))
                # print(str(sim_node['startLine']) + ' ' + str(sim_node['endLine']))
                if len(self.hashMap[node['hash']]) == 0:
                    self.hashMap.pop(node['hash'], None)
                return node['subtreecount']
            for childNode in node['childNodes']:
                result += self.find_sim(childNode)
            if(result / node['subtreecount'] > self.MIN_MERGE_SIM):
                result += 1
        return result

    def compare(self, tree_1,tree_2):
        self.base_node_1 = tree_1
        self.base_node_2 = tree_2
        self.build_hash(tree_1, False)
        self.build_hash(tree_2, True)

        # self.find_sim(tree_1)
        result = self.find_sim(tree_1)
        return (float(result) / tree_1['subtreecount'], self.resultSet)

        

    