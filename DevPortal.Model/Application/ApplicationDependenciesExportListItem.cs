﻿using System;

namespace DevPortal.Model
{
    public class ApplicationDependenciesExportListItem
    {
        public string Name { get; set; }

        public string ApplicationGroupName { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUserEmail { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedUserEmail { get; set; }
    }
}