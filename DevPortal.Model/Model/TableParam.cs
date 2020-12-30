using System.Collections.Generic;

namespace DevPortal.Model
{
    public class TableParam
    {
        public int length { get; set; }

        public int start { get; set; }

        public List<TableOrder> order { get; set; }

        public List<TableColumn> columns { get; set; }
    }

    public class TableOrder
    {
        public int column { get; set; }

        public string dir { get; set; }
    }

    public class TableColumn
    {
        public string data { get; set; }

        public string name { get; set; }
    }
}