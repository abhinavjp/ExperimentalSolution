using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System.Configuration;

namespace TA.PracticeService.DbHelperPractice
{
    public class DatabaseQueryRepository: IDisposable
    {
        private string _dataProvider;
        private DbProviderFactory _dbFactory;
        public string ConnectionString
        {
            get
            {
                if (DbConnection != null)
                    return DbConnection.ConnectionString;
                return string.Empty;
            }
            set
            {
                DbConnection.ConnectionString = value;
            }
        }
        public IDbConnection DbConnection { get; private set; }
        public IDbCommand DbCommand { get; private set; }
        public IEnumerable<IDbDataParameter> DbParameters
        {
            get
            {
                return DbCommand.Parameters as IEnumerable<IDbDataParameter>;
            }
        }
        public int ConnectionTimeout
        {
            get
            {
                if (DbConnection != null)
                    return DbConnection.ConnectionTimeout;
                return -1;
            }
        }
        public int CommandTimeout
        {
            get
            {
                if (DbCommand != null) return DbCommand.CommandTimeout;
                return -1;
            }
            set
            {
                DbCommand.CommandTimeout = value;
            }
        }

        public DatabaseQueryRepository(string dataProvider)
        {
            _dataProvider = dataProvider;
            _dbFactory = DbProviderFactories.GetFactory(_dataProvider);
        }

        public DatabaseQueryRepository(string dataProvider, string connectionString) : this(dataProvider)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection PrepareConnection()
        {
            return PrepareConnection(string.Empty);
        }

        public IDbConnection PrepareConnection(string connectionString)
        {
            DbConnection = _dbFactory.CreateConnection();
            ConnectionString = connectionString;
            DbConnection.ConnectionString = ConnectionString;
            return DbConnection;
        }

        public IDbConnection PrepareConnection(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
            return DbConnection;
        }

        public void PrepareCommand(string commandText, CommandType commandType, int commandTimeout,
            IDbConnection connection, IDbTransaction transaction)
        {
            DbCommand = _dbFactory.CreateCommand();
            DbCommand.CommandText = commandText;
            DbCommand.CommandType = commandType;
            if (connection != null)
                DbCommand.Connection = connection;
            if (transaction != null)
                DbCommand.Transaction = transaction;
            if (commandTimeout != -1)
                DbCommand.CommandTimeout = commandTimeout;
        }

        public void PrepareCommand(string commandText, CommandType commandType, int commandTimeout,
            IEnumerable<IDbDataParameter> parameters, IDbConnection connection, IDbTransaction transaction)
        {
            PrepareCommand(commandText, commandType, commandTimeout, connection, transaction);
            foreach (var parameter in parameters)
            {
                var commandParameter = DbCommand.CreateParameter();
                commandParameter = parameter;
            }
        }

        public void PrepareCommand(string commandText, CommandType commandType, int commandTimeout)
        {
            PrepareCommand(commandText, commandType, commandTimeout, null, null);
        }

        public void PrepareCommand(string commandText, CommandType commandType)
        {
            PrepareCommand(commandText, commandType, -1, null, null);
        }

        public void PrepareCommand(string commandText)
        {
            PrepareCommand(commandText, CommandType.Text, -1, null, null);
        }

        public void PrepareCommand()
        {
            PrepareCommand(string.Empty, CommandType.Text, -1, null, null);
        }

        public void PrepareStoredProcedureCommand(string commandText, int commandTimeout,
            IDbConnection connection, IDbTransaction transaction)
        {
            PrepareCommand(commandText, CommandType.StoredProcedure, commandTimeout, connection, transaction);
        }

        public void PrepareStoredProcedureCommand(string commandText, int commandTimeout,
            IEnumerable<IDbDataParameter> parameters, IDbConnection connection, IDbTransaction transaction)
        {
            PrepareStoredProcedureCommand(commandText, commandTimeout, connection, transaction);
            foreach (var parameter in parameters)
            {
                var commandParameter = DbCommand.CreateParameter();
                commandParameter = parameter;
            }
        }

        public void PrepareStoredProcedureCommand(string commandText, int commandTimeout)
        {
            PrepareStoredProcedureCommand(commandText, commandTimeout, null, null);
        }

        public void PrepareStoredProcedureCommand(string commandText)
        {
            PrepareStoredProcedureCommand(commandText, -1, null, null);
        }

        public void PrepareStoredProcedureCommand()
        {
            PrepareStoredProcedureCommand(string.Empty, -1, null, null);
        }

        public IDbDataParameter AddParameter(string parameterName, object value)
        {
            return AddParameter(parameterName, value, null, null);
        }

        public IDbDataParameter AddParameter(string parameterName, DbType dbType)
        {
            return AddParameter(parameterName, null, dbType, null);
        }

        public IDbDataParameter AddParameter(string parameterName, object value, DbType dbType)
        {
            return AddParameter(parameterName, value, dbType, null);
        }

        public IDbDataParameter AddParameter(string parameterName, object value, ParameterDirection direction)
        {
            return AddParameter(parameterName, value, null, direction);
        }

        public IDbDataParameter AddParameter(string parameterName, object value, DbType? dbType, ParameterDirection? direction)
        {
            if (DbCommand == null)
                PrepareCommand();
            var parameter = DbCommand.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            if (dbType.HasValue)
                parameter.DbType = dbType.Value;
            if (direction.HasValue)
                parameter.Direction = direction.Value;
            return parameter;
        }

        public int ExecuteNonQuery()
        {
            InitializeConnection();
            using (DbConnection)
            {
                DbConnection.Open();
                return DbCommand.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery(string connectionString)
        {
            InitializeConnection(connectionString);
            using (DbConnection)
            {
                DbConnection.Open();
                return DbCommand.ExecuteNonQuery();
            }
        }

        public IDataReader ExecuteReader()
        {
            InitializeConnection();
            return DbCommand.ExecuteReader();
        }

        public IDataReader ExecuteReader(string connectionString)
        {
            InitializeConnection(connectionString);
            return DbCommand.ExecuteReader();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            InitializeConnection();
            return DbCommand.ExecuteReader(behavior);
        }

        public IDataReader ExecuteReader(string connectionString, CommandBehavior behavior)
        {
            InitializeConnection(connectionString);
            return DbCommand.ExecuteReader(behavior);
        }

        public object ExecuteScalar()
        {
            InitializeConnection();
            return DbCommand.ExecuteScalar();
        }

        public object ExecuteScalar(string connectionString)
        {
            InitializeConnection(connectionString);
            return DbCommand.ExecuteScalar();
        }

        private void InitializeConnection(string connectionString)
        {
            if (DbConnection == null)
            {
                PrepareConnection(connectionString);
            }
            else
            {
                DbConnection.ConnectionString = connectionString;
            }
        }

        private void InitializeConnection()
        {
            InitializeConnection(string.Empty);
        }

        public void Dispose()
        {
            if (DbCommand != null)
                DbCommand.Dispose();
            if (DbConnection != null)
                DbConnection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
