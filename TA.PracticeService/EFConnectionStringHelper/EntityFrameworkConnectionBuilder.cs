using System.Data.Entity.Core.EntityClient;

namespace TA.PracticeService.EFConnectionStringHelper
{
    public class EntityFrameworkConnectionBuilder : IEntityFrameworkConnectionBuilder
    {

        public string _providerName;
        public string _dbConnectionstring;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="dbConnectionstring"></param>
        public EntityFrameworkConnectionBuilder(string providerName, string dbConnectionstring)
        {
            _providerName = providerName;
            _dbConnectionstring = dbConnectionstring;
        }


        public string CreateEntityFrameworkConnection(string modelName)
        {
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder
            {
                Provider = _providerName,
                ProviderConnectionString = _dbConnectionstring,
                Metadata = $@"res://*/{modelName}.csdl|res://*/{modelName}.ssdl|res://*/{modelName}.msl"            };

            return entityConnectionStringBuilder.ToString();
        }
    }
}
