using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Data.Entity;

namespace TA.PracticeService.DbHelperPractice
{
    public class DatabaseQueryRepository<T> : DatabaseQueryRepository where T : DbContext
    {
        public DatabaseQueryRepository(T context) : base(context.Database.Connection.ConnectionString)
        {

        }
    }

    public class DatabaseQueryRepository
    {
        protected string _connectionString;

        #region Constructors
        public DatabaseQueryRepository() : this(string.Empty)
        {
        }

        public DatabaseQueryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Command Preparation
        protected IDbCommand PrepareCommand(DatabaseQueryBuilderModel dbQueryBuilderModel,
            IDbConnection connection, IDbTransaction transaction)
        {
            IDbCommand dbCommand = dbQueryBuilderModel.DbFactory.CreateCommand();
            dbCommand.CommandText = dbQueryBuilderModel.Query;
            dbCommand.CommandType = dbQueryBuilderModel.CommandType;
            if (connection != null)
                dbCommand.Connection = connection;
            if (transaction != null)
                dbCommand.Transaction = transaction;
            if (dbQueryBuilderModel.CommandTimeout != -1)
                dbCommand.CommandTimeout = dbQueryBuilderModel.CommandTimeout;
            return dbCommand;
        }
        #endregion

        #region Just Execute Query
        public virtual int ExecuteNonQuery(DatabaseQueryBuilderModel dbQueryBuilderModel, IDbConnection connection, IDbTransaction transaction)
        {
            using (var command = PrepareCommand(dbQueryBuilderModel, connection, transaction))
            {
                OpenConnection(connection);
                return command.ExecuteNonQuery();
            }
        }

        public virtual int ExecuteNonQuery(DatabaseQueryBuilderModel dbQueryBuilderModel, string connectionString, IDbTransaction transaction)
        {
            using (var connection = InitializeConnection(dbQueryBuilderModel, connectionString))
            {
                return ExecuteNonQuery(dbQueryBuilderModel, connection, transaction);
            }
        }

        public virtual int ExecuteNonQuery(DatabaseQueryBuilderModel dbQueryBuilderModel, IDbConnection connection)
        {
            return ExecuteNonQuery(dbQueryBuilderModel, connection, null);
        }

        public virtual int ExecuteNonQuery(DatabaseQueryBuilderModel dbQueryBuilderModel)
        {
            return ExecuteNonQuery(dbQueryBuilderModel, _connectionString, null);
        }
        #endregion

        #region Execute Query and Get DataTable
        public virtual DataTable GetQueryResult(DatabaseQueryBuilderModel dbQueryBuilderModel, CommandBehavior commandBehaviour, IDbConnection connection, IDbTransaction transaction)
        {
            var resultDataTable = new DataTable();
            using (var command = PrepareCommand(dbQueryBuilderModel, connection, transaction))
            {
                OpenConnection(connection);
                using (var reader = command.ExecuteReader(commandBehaviour))
                {
                    resultDataTable.Load(reader);
                    return resultDataTable;
                }
            }
        }

        public virtual DataTable GetQueryResult(DatabaseQueryBuilderModel dbQueryBuilderModel, IDbConnection connection, IDbTransaction transaction)
        {
            return GetQueryResult(dbQueryBuilderModel, CommandBehavior.Default, connection, transaction);
        }

        public virtual DataTable GetQueryResult(DatabaseQueryBuilderModel dbQueryBuilderModel, CommandBehavior commandBehaviour, string connectionString, IDbTransaction transaction)
        {
            using (var connection = InitializeConnection(dbQueryBuilderModel, connectionString))
            {
                return GetQueryResult(dbQueryBuilderModel, commandBehaviour, connection, transaction);
            }
        }

        public virtual DataTable GetQueryResult(DatabaseQueryBuilderModel dbQueryBuilderModel, string connectionString, IDbTransaction transaction)
        {
            return GetQueryResult(dbQueryBuilderModel, CommandBehavior.Default, connectionString, transaction);
        }

        public virtual DataTable GetQueryResult(DatabaseQueryBuilderModel dbQueryBuilderModel, IDbConnection connection)
        {
            return GetQueryResult(dbQueryBuilderModel, connection, null);
        }

        public virtual DataTable GetQueryResult(DatabaseQueryBuilderModel dbQueryBuilderModel, CommandBehavior commandBehaviour, IDbConnection connection)
        {
            return GetQueryResult(dbQueryBuilderModel, commandBehaviour, connection, null);
        }

        public virtual DataTable GetQueryResult(DatabaseQueryBuilderModel dbQueryBuilderModel)
        {
            return GetQueryResult(dbQueryBuilderModel, _connectionString, null);
        }

        public virtual DataTable GetQueryResult(DatabaseQueryBuilderModel dbQueryBuilderModel, CommandBehavior commandBehaviour)
        {
            return GetQueryResult(dbQueryBuilderModel, commandBehaviour, _connectionString, null);
        }
        #endregion

        #region Execute Query and Get DataSet
        public virtual DataSet GetQuerySetResult(DatabaseQueryBuilderModel dbQueryBuilderModel, IDbConnection connection, IDbTransaction transaction)
        {
            var resultDataSet = new DataSet();
            var dataAdapter = dbQueryBuilderModel.DbFactory.CreateDataAdapter();
            using (var command = PrepareCommand(dbQueryBuilderModel, connection, transaction))
            {
                OpenConnection(connection);
                dataAdapter.SelectCommand = command as DbCommand;
                dataAdapter.Fill(resultDataSet);
                return resultDataSet;
            }
        }

        public virtual DataSet GetQuerySetResult(DatabaseQueryBuilderModel dbQueryBuilderModel, string connectionString, IDbTransaction transaction)
        {
            using (var connection = InitializeConnection(dbQueryBuilderModel, connectionString))
            {
                return GetQuerySetResult(dbQueryBuilderModel, connection, transaction);
            }
        }

        public virtual DataSet GetQuerySetResult(DatabaseQueryBuilderModel dbQueryBuilderModel, IDbConnection connection)
        {
            return GetQuerySetResult(dbQueryBuilderModel, connection, null);
        }

        public virtual DataSet GetQuerySetResult(DatabaseQueryBuilderModel dbQueryBuilderModel)
        {
            return GetQuerySetResult(dbQueryBuilderModel, _connectionString, null);
        }
        #endregion

        #region Execute Scalar and Get Scalar Data
        public virtual object GetQueryScalarResult(DatabaseQueryBuilderModel dbQueryBuilderModel, IDbConnection connection, IDbTransaction transaction)
        {
            using (var command = PrepareCommand(dbQueryBuilderModel, connection, transaction))
            {
                OpenConnection(connection);
                return command.ExecuteScalar();
            }
        }

        public virtual object GetQueryScalarResult(DatabaseQueryBuilderModel dbQueryBuilderModel, string connectionString, IDbTransaction transaction)
        {
            using (var connection = InitializeConnection(dbQueryBuilderModel, connectionString))
            {
                return GetQueryScalarResult(dbQueryBuilderModel, connection, transaction);
            }
        }

        public virtual object GetQueryScalarResult(DatabaseQueryBuilderModel dbQueryBuilderModel, IDbConnection connection)
        {
            return GetQueryScalarResult(dbQueryBuilderModel, connection, null);
        }

        public virtual object GetQueryScalarResult(DatabaseQueryBuilderModel dbQueryBuilderModel)
        {
            return GetQueryScalarResult(dbQueryBuilderModel, _connectionString, null);
        }
        #endregion

        protected IDbConnection InitializeConnection(DatabaseQueryBuilderModel dbQueryBuilderModel, string connectionString)
        {
            IDbConnection connection = dbQueryBuilderModel.DbFactory.CreateConnection();

            if (!string.IsNullOrWhiteSpace(connectionString))
                connection.ConnectionString = connectionString;
            return connection;
        }

        protected IDbConnection InitializeConnection(DatabaseQueryBuilderModel dbQueryBuilderModel)
        {
            return InitializeConnection(dbQueryBuilderModel, string.Empty);
        }

        protected void OpenConnection(IDbConnection connection)
        {
            if (new[] { ConnectionState.Broken, ConnectionState.Closed }.Contains(connection.State))
                connection.Open();
        }
    }
}
