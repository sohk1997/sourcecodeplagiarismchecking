using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Root.Extension {
    public class ReaderObject : IDisposable {
        private DbContext _dbContext;
        private DbDataReader _reader;
        internal ReaderObject (DbContext dbContext, string procedure, DatabaseParam[] paramList) {
            _dbContext = dbContext;
            OpenConnection(procedure,paramList);
        }

        private void OpenConnection (string procedure, params DatabaseParam[] paramList) 
        {
            DbCommand cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = procedure;
            foreach(DatabaseParam param in paramList)
            {
                var parameter = cmd.CreateParameter();
                parameter.ParameterName = param.ParamName;
                parameter.DbType = param.DbType;
                parameter.Value = param.Value;
                cmd.Parameters.Add(parameter);
            }
            _dbContext.Database.OpenConnection();
            _reader = cmd.ExecuteReader();
        }

        public List<T> GetResultSet<T>() where T : new()
        {
            var result = new List<T>();
            if (_reader != null && _reader.HasRows)
            {
                var entity = typeof(T);
                var propDict = new Dictionary<string, PropertyInfo>();
                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

                while (_reader.Read())
                {
                    T newObject = new T();
                    for (int iloop = 0; iloop < _reader.FieldCount; iloop++)
                    {
                        if (propDict.ContainsKey(_reader.GetName(iloop).ToUpper()))
                        {
                            var info = propDict[_reader.GetName(iloop).ToUpper()];
                            if (info != null && info.CanWrite)
                            {
                                var val = _reader.GetValue(iloop);
                                info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                            }
                        }
                    }
                    result.Add(newObject);
                }
            }
            return result;
        }

        public bool NextResult()
        {
            return _reader.NextResult();
        } 

        public void Dispose () {
            _dbContext.Database.CloseConnection ();
        }
    }
}