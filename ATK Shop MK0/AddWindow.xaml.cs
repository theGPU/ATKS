using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ATK_Shop_MK0
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private readonly int UID;
        private readonly int price;
        private readonly string mode;
        public static DateTime LocalDateTime { get { return DateTime.Now; } }
        public AddWindow(int uid, int price, string category, string name, string mode)
        {
            this.mode = mode;
            this.UID = uid;
            this.price = price;

            InitializeComponent();

            Sum.Content = $"На сумму: {price}";
            CategoryAndName.Content = $"{category}: {name}";
            DateAndTime.Value = LocalDateTime;
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            var owner = (ViewerWindow)this.Owner;
            var delta = this.mode == "in" ? int.Parse(Count.Text) : -int.Parse(Count.Text);
            owner.databaseController.AddEntryInActivity(this.UID, delta, DateAndTime.Value.HasValue ? DateAndTime.Value.Value : LocalDateTime);
            owner.databaseController.ChangeItemCount(this.UID, delta);
            owner.UpdateItemsList();
            owner.UpdateStatisticItemsList();
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) => Utils.NumberValidationTextBox(sender, e);

        private void OnCountTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                Sum.Content = $"На сумму: 0";
            else
                if (int.TryParse(((TextBox)sender).Text, out var value) && Sum != null)
                    Sum.Content = $"На сумму: {value*price}";
        }
    }
}
