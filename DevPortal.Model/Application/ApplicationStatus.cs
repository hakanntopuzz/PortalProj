using System.Collections.Generic;

namespace DevPortal.Model
{
    public class ApplicationStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ApplicationCountByTypeModel> ApplicationCountsByType { get; set; }
    }
}