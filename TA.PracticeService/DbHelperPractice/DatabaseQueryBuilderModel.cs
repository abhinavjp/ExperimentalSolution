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

        #endregion

        #region Parameter
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
            var parameter = GenerateParameter(parameterName, value, dbType, direction);
            DbParameters.Add(parameter);

            return parameter;
        }

        public IDbDataParameter GenerateParameter()
        {
            return GenerateParameter(string.Empty, null, null, null);
        }

        public IDbDataParameter GenerateParameter(string parameterName)
        {
            return GenerateParameter(parameterName, null, null, null);
        }

        public IDbDataParameter GenerateParameter(string parameterName, object value)
        {
            return GenerateParameter(parameterName, value, null, null);
        }

        public IDbDataParameter GenerateParameter(string parameterName, DbType dbType)
        {
            return GenerateParameter(parameterName, null, dbType, null);
        }

        public IDbDataParameter GenerateParameter(string parameterName, object value, DbType dbType)
        {
            return GenerateParameter(parameterName, value, dbType, null);
        }

        public IDbDataParameter GenerateParameter(string parameterName, object value, ParameterDirection direction)
        {
            return GenerateParameter(parameterName, value, null, direction);
        }

        public IDbDataParameter GenerateParameter(string parameterName, object value, DbType? dbType, ParameterDirection? direction)
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

        public void AddParameter(IDbDataParameter parameter)
        {
            IDbDataParameter commandParameter = _dbFactory.CreateParameter();
            commandParameter = parameter;
        }

        public void AddParameter(IEnumerable<IDbDataParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                AddParameter(parameter);
            }
        }
        #endregion
    }
}
