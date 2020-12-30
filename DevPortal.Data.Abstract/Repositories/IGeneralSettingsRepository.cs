using DevPortal.Model;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IGeneralSettingsRepository
    {
        GeneralSettings GetGeneralSettings();

        bool UpdateGeneralSettings(GeneralSettings generalSettings);
    }
}