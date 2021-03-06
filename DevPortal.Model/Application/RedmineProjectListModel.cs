﻿using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Model
{
    public class RedmineProjectListModel
    {
        public IEnumerable<RedmineProject> data { get; set; }

        public int recordsTotal { get { return data.Any() ? data.FirstOrDefault().TotalCount : 0; } }

        public int recordsFiltered { get { return data.Any() ? data.FirstOrDefault().TotalCount : 0; } }
    }
}