using System.Collections.Generic;

namespace DevPortal.Model
{
    public class Menu
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public string Icon { get; set; }

        public int? Group { get; set; }

        public List<Menu> Children { get; set; }

        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }
    }
}