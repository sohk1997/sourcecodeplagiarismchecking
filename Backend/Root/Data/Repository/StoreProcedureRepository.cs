using System;
using System.Collections.Generic;
using System.Text;
using Root.Data.Infrastructure;
using Root.Extension;

namespace Root.Data.Repository {
    public interface IStoreProcedureRepository {
        ReaderObject CallProcedure (string procedure, params DatabaseParam[] databaseParams);
    }

    public class StoreProcedureRepository : IStoreProcedureRepository {
        private TestContext _dataContext;

        protected IDatabaseFactory DatabaseFactory {
            get;
            private set;
        }

        protected TestContext DataContext {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get ()); }
        }

        public StoreProcedureRepository (IDatabaseFactory databaseFactory) {
            DatabaseFactory = databaseFactory;
        }

        public ReaderObject CallProcedure (string procedure, params DatabaseParam[] databaseParams) {
            var context = DatabaseFactory.GetNew ();
            return new ReaderObject (context, procedure, databaseParams);
        }
    }
}
