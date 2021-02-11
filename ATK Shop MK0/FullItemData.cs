using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATK_Shop_MK0
{
    public class FullItemData
    {
        public int UID { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string[] Params { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
    }
}
