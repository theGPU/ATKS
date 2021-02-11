using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATK_Shop_MK0
{
    public static class Utils
    {
        private static Regex numRegex = new Regex("[^0-9]+");
        public static void NumberValidationTextBox(object sender, TextCompositionEventArgs e) => e.Handled = numRegex.IsMatch(e.Text);
    }
}
