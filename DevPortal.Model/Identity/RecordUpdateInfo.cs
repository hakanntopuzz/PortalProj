using System;

namespace DevPortal.Model
{
    public class RecordUpdateInfo
    {
        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public string CreatedUserEmail { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public string ModifiedUserEmail { get; set; }
    }
}