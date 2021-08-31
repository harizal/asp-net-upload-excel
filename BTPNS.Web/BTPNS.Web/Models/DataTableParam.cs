using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zero_net_core.Models
{
    public class DataTableParam
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortOrder { get; set; }
        public IEnumerable<string> MyProperty { get; set; }
    }

    public class DTColumn
    {
        public string Date { get; set; }
    }

    public class DtOrder
    {
        public int Column { get; set; }
    }
}
