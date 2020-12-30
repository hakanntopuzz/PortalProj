namespace DevPortal.Model
{
    public class FileExportData
    {
        public string FileDownloadName { get; set; }

        public byte[] FileData { get; set; }

        public string ContentType { get; set; }
    }
}