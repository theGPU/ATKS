using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATK_Shop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OnClickLoginButton(null, null);
        }

        private async void OnClickLoginButton(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            SetLoginStatus("Соединение с сервером...", Brushes.Yellow);
            //await Task.Delay(1000);
            SetLoginStatus("Вход успешен!", Brushes.Green);
            //await Task.Delay(1000);
            SetLoginStatus();
            loginButton.IsEnabled = true;
            var mainW = new MainPage();
            App.Current.MainWindow = mainW;
            this.Close();
            mainW.Show();
        }

        private void SetLoginStatus(string status = "", Brush color = null)
        {
            loginStatusLabel.Content = status;
            if (color == null) return;
            loginStatusLabel.Foreground = color;
        }
    }
}
