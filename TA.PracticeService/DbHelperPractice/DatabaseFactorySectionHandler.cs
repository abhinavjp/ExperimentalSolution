using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TA.PracticeService.DbHelperPractice
{
    public class DatabaseFactorySectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)base[nameof(Name)]; }
        }
        [ConfigurationProperty("ConnectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base[nameof(ConnectionStringName)]; }
        }
        public string ConnectionString
        {
            get
            {
                try
                {
                    return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Connection string '{ConnectionStringName}' was not found in web.config.\n{ex.Message}");
                }
            }
        }
    }
}
