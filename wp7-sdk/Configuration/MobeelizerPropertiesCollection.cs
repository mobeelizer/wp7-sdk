using Microsoft.Practices.Mobile.Configuration;

namespace Com.Mobeelizer.Mobile.Wp7.Configuration
{
    internal class MobeelizerPropertiesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MobeelizerConfigProperty();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            MobeelizerConfigProperty e = (MobeelizerConfigProperty)element;

            return e.Name;
        }

        internal new MobeelizerConfigProperty this[string name]
        {
            get
            {
                return (MobeelizerConfigProperty)this.BaseGet(name);
            }
        }
    }
}
