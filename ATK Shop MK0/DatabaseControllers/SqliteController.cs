using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATK_Shop_MK0.DatabaseControllers
{
    class SqliteController : IControllerBase
    {
        public bool IsRemoteDB => false;

        public SqliteConnection connection;
        public string dbPath;

        #region dbControl

        public bool ConnectToLocalDB(string dbPath)
        {
            this.dbPath = dbPath;
            if (File.Exists(dbPath))
            {
                TryOpenConnection(dbPath);

            } else
            {
                TryOpenConnection(dbPath);
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE ""Categories"" (
                        ""UID""   INTEGER,
	                    ""Name""  TEXT,
	                    ""Params""    TEXT,
	                    PRIMARY KEY(""UID"" AUTOINCREMENT)
                    );
                ";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                CREATE INDEX ""Name"" ON ""Categories""(
                    ""Name""  ASC
                ); ";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                    CREATE TABLE ""Items"" (

                        ""UID""   INTEGER,
	                    ""Category""  TEXT,
	                    ""Name""  TEXT,
	                    ""Params""    TEXT,
                        ""Count""	INTEGER,
                        ""Price"" INTEGER,
                        ""Description"" TEXT,
	                    PRIMARY KEY(""UID"" AUTOINCREMENT)
                    );
                ";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                CREATE INDEX ""Category"" ON ""Items""(
                    ""Category""  ASC
                ); ";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                CREATE TABLE ""Activity""(
                    ""UID""   INTEGER,
	                ""IUID""  INTEGER,
	                ""Delta"" INTEGER,
	                ""Time""  INTEGER,
	                PRIMARY KEY(""UID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                CREATE INDEX ""IUID"" ON ""Activity""(
                    ""IUID""  ASC
                ); ";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                CREATE INDEX ""Time"" ON ""Activity""(
                    ""Time""  ASC
                ); ";
                command.ExecuteNonQuery();
            }

            return true;
        }

        private void TryOpenConnection(string dbPath) //вызовет ошибку
        {
            connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
        }

        public void DeleteDb(string dbPath)
        {
            connection.Dispose();
            try { File.Delete(dbPath); } catch { };
        }

        public void DeleteDb() => DeleteDb(dbPath);

        #endregion dbControl

        #region Categories

        public string[] GetAllCategoriesNames()
        {
            var categoriesNamesList = new List<string>();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Name FROM Categories";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                    categoriesNamesList.Add(reader.GetString(0));
            }

            return categoriesNamesList.ToArray();
        }

        public SortedDictionary<string, int> GetCategoriesWithItemsCount()
        {
            var allCategoriesNames = GetAllCategoriesNames();
            var result = new SortedDictionary<string, int>();
            foreach (var categoryName in allCategoriesNames)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(Name) FROM Items WHERE Category = $categoryName";
                command.Parameters.AddWithValue("$categoryName", categoryName);
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    result.Add(categoryName, reader.GetInt32(0));
                }
            }

            return result;
        }

        public bool CreateNewCategory(List<string> paramsList, string categoryName)
        {
            var paramsText = string.Join("\n", paramsList);

            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Categories (Name, Params) VALUES ($name, $paramsList)";
            command.Parameters.AddWithValue("$name", categoryName);
            command.Parameters.AddWithValue("$paramsList", paramsText);
            return command.ExecuteNonQuery() == 1;
        }

        public string[] GetCategoryParams(string categoryName)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Params FROM Categories WHERE Name = $name";
            command.Parameters.AddWithValue("$name", categoryName);
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                var resultString = reader.GetString(0);
                return resultString.Split("\n");
            }
        }

        public bool DeleteCategory(string categoryName)
        {
            var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM Items WHERE Category = $categoryName";
            command.Parameters.AddWithValue("$categoryName", categoryName);
            command.ExecuteNonQuery();

            command.CommandText = "DELETE FROM Categories WHERE Name = $categoryName";
            return command.ExecuteNonQuery() == 1;
        }

        #endregion Categories

        #region items
        public bool CreateNewItem(string categoryName, string itemName, string[] paramsList, int price, string description)
        {
            var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Items (Category, Name, Params, Count, Price, Description) VALUES ($Category, $Name, $Params, 0, $Price, $Description)";
            command.Parameters.AddWithValue("$Category", categoryName);
            command.Parameters.AddWithValue("$Name", itemName);
            command.Parameters.AddWithValue("$Params", string.Join("\n", paramsList));
            command.Parameters.AddWithValue("$Price", price);
            command.Parameters.AddWithValue("$Description", description);
            for (var i = 1; i < 100000; i++)
            {
                command.ExecuteNonQuery();
            }

            return command.ExecuteNonQuery() == 1;
        }

        public ItemInDataGrid[] GetAllItems()
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT UID, Category, Name, Price, Count, Description FROM Items";

            var result = new List<ItemInDataGrid>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new ItemInDataGrid
                    {
                        UID = reader.GetInt32(0),
                        Category = reader.GetString(1),
                        Name = reader.GetString(2),
                        Price = reader.GetInt32(3),
                        Count = reader.GetInt32(4),
                        Description = reader.GetString(5)
                    });
                }
            }

            return result.ToArray();
        }

        public bool DeleteItemByUID(int UID)
        {
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Items WHERE UID = $UID";
            command.Parameters.AddWithValue("$UID", UID);
            return command.ExecuteNonQuery() == 1;
        }

        public bool ChangeItemCount(int UID, int Delta)
        {
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Items SET count = count+$Delta where UID = $UID";
            command.Parameters.AddWithValue("$Delta", Delta);
            command.Parameters.AddWithValue("$UID", UID);
            return command.ExecuteNonQuery() == 1;
        }

        public Dictionary<string, string> GetItemParams(int UID)
        {
            var itemCommand = connection.CreateCommand();
            itemCommand.CommandText = "SELECT Category, Params FROM Items WHERE UID = $UID";
            itemCommand.Parameters.AddWithValue("$UID", UID);
            using var itemReader = itemCommand.ExecuteReader();
            itemReader.Read();
            var categotyName = itemReader.GetString(0);
            var itemParams = itemReader.GetString(1).Split('\n');

            var categoryCommand = connection.CreateCommand();
            categoryCommand.CommandText = "SELECT Params FROM Categories WHERE Name = $CategoryName";
            categoryCommand.Parameters.AddWithValue("$CategoryName", categotyName);
            using var categoryReader = categoryCommand.ExecuteReader();
            categoryReader.Read();
            var categoryParams = categoryReader.GetString(0).Split('\n');

            var result = new Dictionary<string, string>();
            for (int i = 0; i < categoryParams.Length; i++)
            {
                result.Add(categoryParams[i], itemParams[i]);
            }

            return result;
        }

        public FullItemData GetFullItemData(int UID)
        {
            var result = new FullItemData();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Items WHERE UID = $UID";
            command.Parameters.AddWithValue("$UID", UID);
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                result.UID = reader.GetInt32(0);
                result.Category = reader.GetString(1);
                result.Name = reader.GetString(2);
                result.Params = reader.GetString(3).Split('\n');
                result.Count = reader.GetInt32(4);
                result.Price = reader.GetInt32(5);
                result.Description = reader.GetString(6);
            }

            return result;
        }

        public bool SetItemDataForUID(int UID, FullItemData item)
        {
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Items SET Category = $Category, Name = $Name, Params = $Params, Count = $Count, Price = $Price, Description = $Description WHERE UID = $UID";
            command.Parameters.AddWithValue("$UID", UID);
            command.Parameters.AddWithValue("$Category", item.Category);
            command.Parameters.AddWithValue("$Name", item.Name);
            command.Parameters.AddWithValue("$Params", string.Join('\n',item.Params));
            command.Parameters.AddWithValue("$Count", item.Count);
            command.Parameters.AddWithValue("$Price", item.Price);
            command.Parameters.AddWithValue("$Description", item.Description);
            return command.ExecuteNonQuery() == 1;
        }

        #endregion items

        #region Activity
        public bool AddEntryInActivity(int IUID, int delta, DateTime time)
        {
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Activity (IUID, Delta, Time) VALUES ($IUID, $Delta, $Time)";
            command.Parameters.AddWithValue("$IUID", IUID);
            command.Parameters.AddWithValue("$Delta", delta);
            command.Parameters.AddWithValue("$Time", time.ToString("yyyy-MM-dd HH:mm:ss"));
            return command.ExecuteNonQuery() == 1;
        }

        internal class DayPoint
        {
            public int Count { get; set; }
            public int Added { get; set; }
            public int Removed { get; set; }
        }

        public ActivityPoint[] GetPointsForActivity(int IUID)
        {
            var preResult = new SortedDictionary<string, DayPoint>();

            var command = connection.CreateCommand();
            command.CommandText = "Select Delta, Time from Activity WHERE IUID = $IUID AND time >= datetime('now','-30 days','localtime')";
            command.Parameters.AddWithValue("$IUID", IUID);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var delta = reader.GetInt32(0);
                    var day = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                    if (preResult.ContainsKey(day))
                    {
                        if (delta > 0)
                            preResult[day].Added += delta;
                        else
                            preResult[day].Removed -= delta;
                    } else
                    {
                        if (delta > 0)
                            preResult.Add(day, new DayPoint { Added = delta, Removed = 0 });
                        else
                            preResult.Add(day, new DayPoint { Added = 0, Removed = -delta });
                    }
                }
            }
            command.CommandText = "Select Count from Items WHERE UID = $IUID";
            int itemCount;
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                itemCount = reader.GetInt32(0);
            }

            bool isFirstElement = true;
            DayPoint prevValue = new DayPoint();
            foreach (var x in preResult.Reverse())
            {
                var val = x.Value;
                if (isFirstElement)
                {
                    val.Count = itemCount;
                    isFirstElement = false;
                } else
                {
                    val.Count = prevValue.Count - prevValue.Added + prevValue.Removed;
                }
                prevValue = val;
            }

            var result = new ActivityPoint[30];
            var localDate = DateTime.Now;
            for (var day = 0; day < 30; day++)
            {
                var dayKey = localDate.AddDays(-day).ToString("yyyy-MM-dd");
                if (preResult.ContainsKey(dayKey))
                {
                    var dayData = preResult[dayKey];
                    result[day] = new ActivityPoint { Count = dayData.Count, In = dayData.Added, Out = dayData.Removed };
                } else
                {
                    result[day] = new ActivityPoint { Count = -1, In = 0, Out = 0 };
                }
            }
            result = result.Reverse().ToArray();
            if (result[0].Count == -1)
                result[0].Count = 0;
            var lastCount = result[0].Count;
            for (var day = 1; day < 30; day++)
            {
                var val = result[day];
                if (val.Count == -1)
                    val.Count = lastCount;
                lastCount = val.Count;
            }

            return result;
        }

        public Dictionary<int, string> GetItemsListForActivity()
        {
            var result = new Dictionary<int, string>();

            var command = connection.CreateCommand();
            command.CommandText = "Select UID, Category, Name From Items";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var itemUID = reader.GetInt32(0);
                    var itemCategory = reader.GetString(1);
                    var itemName = reader.GetString(2);
                    result.Add(itemUID, $"{itemCategory}: {itemName}");
                }
            }

            return result;
        }

        #endregion Activity
    }
}
