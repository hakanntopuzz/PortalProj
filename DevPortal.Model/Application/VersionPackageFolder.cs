using System;
using System.Collections.Generic;

namespace DevPortal.Model
{
    public class VersionPackageFolder
    {
        public string Path { get; set; }

        public string Name
        {
            get
            {
                var splits = this.Path.Replace(@"\", "/").Split('/');

                return splits[splits.Length - 1];
            }
        }

        public string FilePath
        {
            get
            {
                return Path.Replace(@"\", "/");
            }
        }

        public DateTime DateModified { get; set; }

        public DateTime CreationTime { get; set; }

        public List<string> SubDirectory { get; set; }
    }
}