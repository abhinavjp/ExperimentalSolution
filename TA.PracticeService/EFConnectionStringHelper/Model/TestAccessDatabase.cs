using RepositoryFoundation.Models;
using System.Configuration;
using System.Linq;

namespace TA.PracticeService.EFConnectionStringHelper.Model
{
    public partial class ApartmentFinanceManagementDevDbEntities
    {
        public ApartmentFinanceManagementDevDbEntities(IEntityFrameworkConnectionBuilder connectionBuilder, string modelName)
            :base(connectionBuilder.CreateEntityFrameworkConnection(modelName))
        {

        }
    }
    public static class TestAccessDatabase
    {
        public static void Run()
        {
            var efBuilderConfiguration = (EntityFrameworkModelBuilder)ConfigurationManager.GetSection("efModelBuider");
            var model = efBuilderConfiguration.ModelConfigurations["EFConnectionStringHelper.Model.TestEntity"];
            using (var context = new ApartmentFinanceManagementDevDbEntities(new EntityFrameworkConnectionBuilder(model.ProviderName, System.Configuration.ConfigurationManager.ConnectionStrings["mainConnectionString"].ToString()), model.ModelName))
            {
                var data = context.TransactionTypes.ToList();
            }
        }
    }
}
