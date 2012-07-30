using Microsoft.Practices.Mobile.Configuration;

namespace Com.Mobeelizer.Mobile.Wp7.Configuration
{
    /// <summary>
    /// Configuration section class. 
    /// </summary>
    public class MobeelizerConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Get the connection Items.
        /// </summary>
        [ConfigurationProperty("properties")]
        public MobeelizerPropertiesCollection AppSettings
        {
            get
            {
                return (MobeelizerPropertiesCollection)(this["properties"]);
            }
        }
    }
}
