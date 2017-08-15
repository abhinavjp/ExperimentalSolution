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
    public class DatabaseQueryRepository : IDisposable
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
            if (!string.IsNullOrWhiteSpace(connectionString))
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
            IDbConnection connection)
        {
            PrepareCommand(commandText, commandType, commandTimeout, connection, null);
        }

        public void PrepareCommand(string commandText, CommandType commandType, int commandTimeout,
            IDbTransaction transaction)
        {
            PrepareCommand(commandText, commandType, commandTimeout, null, null, transaction);
        }

        public void PrepareCommand(string commandText, CommandType commandType, int commandTimeout,
            IEnumerable<IDbDataParameter> parameters, IDbConnection connection, IDbTransaction transaction)
        {
            PrepareCommand(commandText, commandType, commandTimeout, connection, transaction);
            if (parameters == null || !parameters.Any())
                return;
            foreach (var parameter in parameters)
            {
                var commandParameter = DbCommand.CreateParameter();
                commandParameter = parameter;
            }
        }

        public void PrepareCommand(string commandText, CommandType commandType, int commandTimeout,
            IEnumerable<IDbDataParameter> parameters, IDbConnection connection)
        {
            PrepareCommand(commandText, commandType, commandTimeout, parameters, connection, null);
        }

        public void PrepareCommand(string commandText, CommandType commandType, int commandTimeout,
            IEnumerable<IDbDataParameter> parameters, IDbTransaction transaction)
        {
            PrepareCommand(commandText, commandType, commandTimeout, parameters, null, transaction);
        }

        public void PrepareCommand(string commandText, CommandType commandType, int commandTimeout)
        {
            PrepareCommand(commandText, commandType, commandTimeout, default(IDbConnection), null);
        }

        public void PrepareCommand(string commandText, CommandType commandType)
        {
            PrepareCommand(commandText, commandType, -1, default(IDbConnection), null);
        }

        public void PrepareCommand(string commandText)
        {
            PrepareCommand(commandText, CommandType.Text, -1, default(IDbConnection), null);
        }

        public void PrepareCommand()
        {
            PrepareCommand(string.Empty, CommandType.Text, -1, default(IDbConnection), null);
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
            return ExecuteReader(string.Empty);
        }

        public IDataReader ExecuteReader(string connectionString)
        {
            return ExecuteReader(connectionString, CommandBehavior.Default);
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return ExecuteReader(string.Empty, behavior);
        }

        public IDataReader ExecuteReader(string connectionString, CommandBehavior behavior)
        {
            InitializeConnection(connectionString);
            return GetReader(behavior);
        }

        private IDataReader GetReader(CommandBehavior behavior)
        {
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

        public DataTable ExecuteReaderAndGetDataResult()
        {
            var resultTable = new DataTable();
            InitializeConnection();
            using (DbConnection)
            {
                using (var reader = ExecuteReader())
                {
                    resultTable.Load(reader);
                    return resultTable;
                }
            }
        }

        public DataTable ExecuteReaderAndGetDataResult(string connectionString, CommandBehavior behavior, string commandText,
            CommandType commandType, int commandTimeout, IDbConnection connection)
        {
            var resultTable = new DataTable();
            InitializeConnection(connectionString);
            PrepareCommand(commandText, commandType, commandTimeout, connection);
            using (DbConnection)
            {
                using (var reader = GetReader(behavior))
                {
                    resultTable.Load(reader);
                    return resultTable;
                }
            }
        }

        public DataTable ExecuteReaderAndGetDataResult(string connectionString, CommandBehavior behavior, string commandText,
            CommandType commandType, int commandTimeout, IDbConnection connection, IDbTransaction transaction)
        {
            var resultTable = new DataTable();
            InitializeConnection(connectionString);
            PrepareCommand(commandText, commandType, commandTimeout, connection, transaction);
            using (DbConnection)
            {
                using (var reader = GetReader(behavior))
                {
                    resultTable.Load(reader);
                    return resultTable;
                }
            }
        }

        public DataSet ExecuteReaderAndGetDataSetResult(string connectionString, string commandText, CommandType commandType,
            int commandTimeout, IDbConnection connection, IDbTransaction transaction)
        {
            var resultSet = new DataSet();
            InitializeConnection(connectionString);
            PrepareCommand(commandText, commandType, commandTimeout, connection, transaction);
            using (DbConnection)
            {
                IDbDataAdapter adapter = _dbFactory.CreateDataAdapter();
                adapter.SelectCommand = DbCommand;
                adapter.Fill(resultSet);
                return resultSet;
            }
        }

        private void InitializeConnection(string connectionString)
        {
            if (DbConnection == null)
            {
                PrepareConnection(connectionString);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    DbConnection.ConnectionString = connectionString;
            }
            if (new[] { ConnectionState.Broken, ConnectionState.Closed }.Contains(DbConnection.State))
                DbConnection.Open();
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
