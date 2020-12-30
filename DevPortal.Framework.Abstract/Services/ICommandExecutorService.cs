namespace DevPortal.Framework.Abstract
{
    public interface ICommandExecutorService
    {
        void ExecutePowerShellScript(string path, string destPath, string fileName);
    }
}