using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        TestContext Get();
        TestContext GetNew();
    }
}
