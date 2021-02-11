using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATK_Shop.Controllers
{
    public static class MySQLController
    {
        public static bool ConnectToDB(string login, string password)
        {
            var server = "";
            var connectionStr = $"server={server};user={login};database=ATKShop;port=3306;password=******";
            return false;
        }
    }
}
