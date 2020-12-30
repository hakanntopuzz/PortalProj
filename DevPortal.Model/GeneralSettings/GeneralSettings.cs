using System;

namespace DevPortal.Model
{
    public class GeneralSettings : GeneralSettingsEditModel
    {
        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }
    }
}