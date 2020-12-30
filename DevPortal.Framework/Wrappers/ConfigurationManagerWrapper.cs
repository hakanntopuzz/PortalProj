using DevPortal.Framework.Abstract;
using DevPortal.Framework.Model;
using Microsoft.Extensions.Configuration;

namespace DevPortal.Framework.Wrappers
{
    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        #region ctor

        readonly IConfiguration configuration;

        public ConfigurationManagerWrapper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #endregion

        #region connection string

        public string GetConnectionString(ConfigKey key)
        {
            if (!configuration.GetSection("ConnectionStrings").Exists())
            {
                return null;
            }

            return configuration.GetConnectionString(key.ToString());
        }

        #endregion

        #region app settings

        public bool AppSettingsKeyExists(ConfigKey key)
        {
            return GetAppSettingsValue(key) != null;
        }

        public string GetAppSettingsValue(ConfigKey key)
        {
            return configuration.GetSection("AppSettings")[key.ToString()];
        }

        #endregion
    }
}