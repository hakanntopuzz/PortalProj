using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevPortal.Business.Abstract
{
    public interface IFileSystemFactory
    {
        LogFileModel CreateLogFile(string path, DateTime dateModified, string size);

        LogFileModel CreateLogFile(string path, DateTime dateModified, string size, string text);

        LogFileModel GetDownloadFileContents(string path, byte[] content);

        Collection<LogFileModel> CreateLogFiles();

        List<string> CreateEmptyStringList();

        string[] CreateEmptyString();
    }
}