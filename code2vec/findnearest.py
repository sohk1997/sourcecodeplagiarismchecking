from scipy.spatial import distance
import numpy as np
import heapq
import json

def nearest(method, methods):
        MAX_SIMILIAR = 5    
        heaparr = []
        index = 0
        for compareMethod in methods:
                if(compareMethod['Vector'] == 'None'):
                        continue
                index += 1
                x = np.array(json.loads(compareMethod['Vector']))
                y = x.astype(np.float)

                dist = distance.cosine(method['Vector'], y)

                if(len(heaparr) < MAX_SIMILIAR):
                        heapq.heappush(heaparr,(-dist, index, compareMethod)) 
                else:
                        if(-heaparr[0][0] > dist and len([el for el in heaparr if el[0] == -dist]) == 0):
                                heapq.heappop(heaparr)
                                heapq.heappush(heaparr,(-dist, index, compareMethod))
        
        return list(map(lambda element: element[2],heaparr))
        
