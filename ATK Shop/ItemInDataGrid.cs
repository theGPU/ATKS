using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATK_Shop
{
    public class ItemInDataGrid
    {
        public ItemInit ItemInit { get; set; }
        public string StorageName { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string FirstCharacteristic { get; set; }
        public string SecondCharacteristic { get; set; }
    }

    public class ItemInit
    {
        public StorageData StorageData { get; set; }
        public string UID { get; set; }
    }
}
