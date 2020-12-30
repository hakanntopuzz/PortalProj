using System;

namespace DevPortal.Model
{
    public class LogFileModel
    {
        public byte[] FileContent { get; set; }

        public string Text { get; set; }

        public string Path { get; set; }

        public string ColoringLogHeaderText
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    return null;
                }
                var text = this.Text.Replace("INFO", "<span class='text-info font-weight-bold'>INFO</span>");

                return text.Replace("ERROR", "<span class='text-danger font-weight-bold'>ERROR</span>");
            }
        }

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

        public string DateModified { get; set; }

        public DateTime CreationTime { get; set; }

        public string Size { get; set; }
    }
}