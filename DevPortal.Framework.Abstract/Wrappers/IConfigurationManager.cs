using DevPortal.Framework.Model;

namespace DevPortal.Framework.Abstract
{
    public interface IConfigurationManager
    {
        string GetConnectionString(ConfigKey key);

        bool AppSettingsKeyExists(ConfigKey key);

        string GetAppSettingsValue(ConfigKey key);
    }
}