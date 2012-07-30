using Microsoft.Practices.Mobile.Configuration;

namespace Com.Mobeelizer.Mobile.Wp7.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class MobeelizerPropertiesCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Configuration properies collection.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MobeelizerConfigProperty();
        }

        /// <summary>
        /// Gets configuration element key.
        /// </summary>
        /// <param name="element">Configuration element.</param>
        /// <returns>Element key.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            MobeelizerConfigProperty e = (MobeelizerConfigProperty)element;

            return e.Name;
        }

        /// <summary>
        /// Gets configuration property by name.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <returns>Configuration property.</returns>
        public new MobeelizerConfigProperty this[string name]
        {
            get
            {
                return (MobeelizerConfigProperty)this.BaseGet(name);
            }
        }
    }
}
