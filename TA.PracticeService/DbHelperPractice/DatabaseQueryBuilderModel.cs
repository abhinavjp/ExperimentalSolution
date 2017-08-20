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
    public class DatabaseQueryBuilderModel
    {
        protected readonly string _dataProvider;
        private readonly DbProviderFactory _dbFactory;
        public DbProviderFactory DbFactory => _dbFactory;
        protected const int DefaultTimeout = 30;

        public string Query { get; set; }
        public CommandType CommandType { get; set; }
        public List<IDbDataParameter> DbParameters { get; set; }
        public int CommandTimeout { get; set; } = -1;

        #region Constructor

        public DatabaseQueryBuilderModel(string dataProvider) : this(dataProvider, string.Empty)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, IEnumerable<IDbDataParameter> dbParameters) : this(dataProvider, string.Empty, dbParameters)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, string query) : this(dataProvider, query, CommandType.Text, DefaultTimeout)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, string query, IEnumerable<IDbDataParameter> dbParameters) : this(dataProvider, query, CommandType.Text, DefaultTimeout, dbParameters)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, string query, int commandTimeout) : this(dataProvider, query, CommandType.Text, commandTimeout)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, string query, int commandTimeout, IEnumerable<IDbDataParameter> dbParameters) : this(dataProvider, query, CommandType.Text, commandTimeout, dbParameters)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, string query, CommandType commandType) : this(dataProvider, query, commandType, DefaultTimeout)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, string query, CommandType commandType, IEnumerable<IDbDataParameter> dbParameters) : this(dataProvider, query, commandType, DefaultTimeout, dbParameters)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, string query, CommandType commandType, int commandTimeout)
            : this(dataProvider, query, commandType, commandTimeout, null)
        {

        }

        public DatabaseQueryBuilderModel(string dataProvider, string query, CommandType commandType, int commandTimeout, IEnumerable<IDbDataParameter> dbParameters)
        {
            if (string.IsNullOrWhiteSpace(dataProvider))
            {
                throw new ArgumentNullException(nameof(dataProvider), $"Data provider needs to specified");
            }
            _dataProvider = dataProvider;
            _dbFactory = DbProviderFactories.GetFactory(_dataProvider);
            Query = query;
            CommandType = commandType;
            DbParameters = dbParameters?.ToList();
            CommandTimeout = commandTimeout;
        }

        public DatabaseQueryBuilderModel(DbConnection connection) : this(connection, string.Empty)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, IEnumerable<IDbDataParameter> dbParameters) : this(connection, string.Empty, dbParameters)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, string query) : this(connection, query, CommandType.Text, DefaultTimeout)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, string query, IEnumerable<IDbDataParameter> dbParameters) : this(connection, query, CommandType.Text, DefaultTimeout, dbParameters)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, string query, int commandTimeout) : this(connection, query, CommandType.Text, commandTimeout)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, string query, int commandTimeout, IEnumerable<IDbDataParameter> dbParameters) : this(connection, query, CommandType.Text, commandTimeout, dbParameters)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, string query, CommandType commandType) : this(connection, query, commandType, DefaultTimeout)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, string query, CommandType commandType, IEnumerable<IDbDataParameter> dbParameters) : this(connection, query, commandType, DefaultTimeout, dbParameters)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, string query, CommandType commandType, int commandTimeout)
            : this(connection, query, commandType, commandTimeout, null)
        {

        }

        public DatabaseQueryBuilderModel(DbConnection connection, string query, CommandType commandType, int commandTimeout, IEnumerable<IDbDataParameter> dbParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection), $"DbConnection needs to specified");
            }
            _dbFactory = DbProviderFactories.GetFactory(connection);
            Query = query;
            CommandType = commandType;
            DbParameters = dbParameters?.ToList();
            CommandTimeout = commandTimeout;
        }

        #endregion

        #region Parameter
        public virtual IDbDataParameter AddParameter(string parameterName, object value)
        {
            return AddParameter(parameterName, value, null, null);
        }

        public virtual IDbDataParameter AddParameter(string parameterName, DbType dbType)
        {
            return AddParameter(parameterName, null, dbType, null);
        }

        public virtual IDbDataParameter AddParameter(string parameterName, object value, DbType dbType)
        {
            return AddParameter(parameterName, value, dbType, null);
        }

        public virtual IDbDataParameter AddParameter(string parameterName, object value, ParameterDirection direction)
        {
            return AddParameter(parameterName, value, null, direction);
        }

        public virtual IDbDataParameter AddParameter(string parameterName, object value, DbType? dbType, ParameterDirection? direction)
        {
            var parameter = GenerateParameter(parameterName, value, dbType, direction);
            DbParameters.Add(parameter);

            return parameter;
        }

        public virtual IDbDataParameter GenerateParameter()
        {
            return GenerateParameter(string.Empty, null, null, null);
        }

        public virtual IDbDataParameter GenerateParameter(string parameterName)
        {
            return GenerateParameter(parameterName, null, null, null);
        }

        public virtual IDbDataParameter GenerateParameter(string parameterName, object value)
        {
            return GenerateParameter(parameterName, value, null, null);
        }

        public virtual IDbDataParameter GenerateParameter(string parameterName, DbType dbType)
        {
            return GenerateParameter(parameterName, null, dbType, null);
        }

        public virtual IDbDataParameter GenerateParameter(string parameterName, object value, DbType dbType)
        {
            return GenerateParameter(parameterName, value, dbType, null);
        }

        public virtual IDbDataParameter GenerateParameter(string parameterName, object value, ParameterDirection direction)
        {
            return GenerateParameter(parameterName, value, null, direction);
        }

        public virtual IDbDataParameter GenerateParameter(string parameterName, object value, DbType? dbType, ParameterDirection? direction)
        {
            IDbDataParameter parameter = _dbFactory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            if (dbType.HasValue)
                parameter.DbType = dbType.Value;
            if (direction.HasValue)
                parameter.Direction = direction.Value;
            return parameter;
        }

        public virtual void AddParameter(IDbDataParameter parameter)
        {
            IDbDataParameter commandParameter = _dbFactory.CreateParameter();
            commandParameter = parameter;
        }

        public virtual void AddParameter(IEnumerable<IDbDataParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                AddParameter(parameter);
            }
        }
        #endregion
    }
}
