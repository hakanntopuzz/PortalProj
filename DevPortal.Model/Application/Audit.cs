using System;

namespace DevPortal.Model
{
    public class Audit
    {
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int TotalCount { get; set; }
    }
}