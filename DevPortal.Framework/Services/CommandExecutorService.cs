using DevPortal.Framework.Abstract;
using System.Diagnostics;

namespace DevPortal.Framework.Services
{
    public class CommandExecutorService : ICommandExecutorService
    {
        /// <summary>
        /// PowerShell Scriptlerini çalıştırmak için kullanılır.
        /// </summary>
        /// <param name="workingDirectory">.exe'nin çalışmasını istediğimiz dizin yolu</param>
        /// <param name="filePath">Çalıştırmak istediğimiz PoweShell Scriptinin yolu ve adından oluşan path</param>
        /// <param name="fileName">üzerinde çalışmak istenilen dosyanın adı Örn:AB.Framework.TestPackage.nuspec , AB.Framework.TestPackage.1.0.0.nupkg </param>
        public void ExecutePowerShellScript(string workingDirectory, string filePath, string fileName)
        {
            var process = new Process();
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.Arguments = "-ExecutionPolicy Bypass -file " + filePath + " " + fileName;
            process.Start();
            process.WaitForExit();
        }
    }
}