from scipy.spatial import distance
import numpy as np
import heapq

def nearest(method, methods):
        MAX_SIMILIAR = 5    
        heaparr = []
        for compareMethod in methods:
                if(compareMethod['Vector'] == 'None'):
                        continue
                x = np.array(compareMethod['Vector'].replace('[[','').replace(']]','').replace('[','').replace(']','').split())
                y = x.astype(np.float)

                # Anh Hoa 
                # dist = distance.euclidean(method['Vector'], y)

                # Anh Nhat
                dist = distance.cosine(method['Vector'], y)

                if(len(heaparr) < MAX_SIMILIAR):
                        heapq.heappush(heaparr,(-dist, compareMethod)) 
                else:
                        if(-heaparr[0][0] > dist and len([el for el in heaparr if el[0] == -dist]) == 0):
                                heapq.heappop(heaparr)
                                heapq.heappush(heaparr,(-dist, compareMethod))
        
        return list(map(lambda element: element[1],heaparr))
        
