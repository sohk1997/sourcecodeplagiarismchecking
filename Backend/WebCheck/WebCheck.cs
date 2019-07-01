using System;
using System.Collections.Generic;
using System.Text;

namespace WebCheck
{
    
    public interface WebCheck
    {
        Result Check(Guid id, string url);
       
    }
}
