using DevPortal.Business.Abstract;
using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevPortal.Business.Factories
{
    public class FileSystemFactory : IFileSystemFactory
    {
        public LogFileModel CreateLogFile(string path, DateTime dateModified, string size, string text)
        {
            var dateTime = dateModified.ToString("yyyy-MM-dd HH:mm:ss");

            return new LogFileModel
            {
                DateModified = dateTime,
                Path = path.Replace(@"\", "/"),
                Size = size,
                Text = text
            };
        }

        public LogFileModel CreateLogFile(string path, DateTime dateModified, string size)
        {
            var dateTime = dateModified.ToString("yyyy-MM-dd HH:mm:ss");

            return new LogFileModel
            {
                DateModified = dateTime,
                Path = path.Replace(@"\", "/"),
                Size = size,
            };
        }

        public LogFileModel GetDownloadFileContents(string path, byte[] content)
        {
            return new LogFileModel
            {
                FileContent = content,
                Path = path
            };
        }

        public Collection<LogFileModel> CreateLogFiles()
        {
            return new Collection<LogFileModel>();
        }

        public List<string> CreateEmptyStringList()
        {
            return new List<string>();
        }

        public string[] CreateEmptyString()
        {
            return new string[] { };
        }
    }
}