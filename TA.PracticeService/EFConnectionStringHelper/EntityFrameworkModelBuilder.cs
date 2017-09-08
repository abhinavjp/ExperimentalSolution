using System;
using System.Configuration;

namespace TA.PracticeService.EFConnectionStringHelper
{
    public class EntityFrameworkModelBuilder : ConfigurationSection
    {
        [ConfigurationProperty("modelConfiguration", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(string), AddItemName = "model")]
        public ModelConfigurationCollection ModelConfigurations => (ModelConfigurationCollection)this["modelConfiguration"];
    }

    public class ModelConfigurationCollection : ConfigurationElementCollection
    {

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

        protected override ConfigurationElement CreateNewElement() => new ModelConfigurationElement();

        protected override Object GetElementKey(ConfigurationElement element) => ((ModelConfigurationElement)element).ModelName;

        public ModelConfigurationElement this[int index]
        {
            get
            {
                return (ModelConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ModelConfigurationElement this[string Name] => (ModelConfigurationElement)BaseGet(Name);

        public int IndexOf(ModelConfigurationElement modelconfiguration) => BaseIndexOf(modelconfiguration);

        public void Add(ModelConfigurationElement modelconfiguration)
        {
            BaseAdd(modelconfiguration);

        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ModelConfigurationElement modelconfiguration)
        {
            if (BaseIndexOf(modelconfiguration) >= 0)
            {
                BaseRemove(modelconfiguration.ModelName);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class ModelConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string ModelName => (string)this["name"];
        [ConfigurationProperty("provider", IsRequired = true)]
        public string ProviderName => (string)this["provider"];
    }
}
