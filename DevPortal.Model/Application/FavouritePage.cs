using System;
using System.Globalization;

namespace DevPortal.Model
{
    public class FavouritePage
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string PageName { get; set; }

        public string PageUrl { get; set; }

        public int Order { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public string CreatedDateFormatted => this.CreatedDate.ToString("dd MMMM yyyy HH:mm", new CultureInfo("tr-TR"));
    }
}