using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATK_Shop_MK0.DatabaseControllers
{
    public interface IControllerBase
    {
        bool IsRemoteDB { get; }
        bool ConnectToLocalDB(string dbPath);
        string[] GetAllCategoriesNames();
        bool CreateNewCategory(List<string> paramsList, string categoryName);
        bool DeleteCategory(string categoryName);
        SortedDictionary<string, int> GetCategoriesWithItemsCount();
        string[] GetCategoryParams(string categoryName);

        bool CreateNewItem(string categoryName, string itemName, string[] paramsDict, int price, string description);
        ItemInDataGrid[] GetAllItems();
        bool DeleteItemByUID(int UID);
        bool ChangeItemCount(int UID, int Delta);
        Dictionary<string, string> GetItemParams(int UID);
        FullItemData GetFullItemData(int UID);
        bool SetItemDataForUID(int UID, FullItemData item);

        bool AddEntryInActivity(int IUID, int delta, DateTime time);
        ActivityPoint[] GetPointsForActivity(int IUID);

        Dictionary<int, string> GetItemsListForActivity();
    }
}
