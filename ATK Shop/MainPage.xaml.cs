using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ATK_Shop
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        private static Dictionary<string, StorageData> storages = new Dictionary<string, StorageData>();
        public MainPage()
        {
            ConsoleAllocator.ShowConsoleWindow();
            InitializeComponent();
            var secondStorageData = new StorageData();
            secondStorageData.storageName = "Second";
            OnAddNewStorageAsync(secondStorageData);

            ItemsGrid.EnableColumnVirtualization = true;
            ItemsGrid.EnableRowVirtualization = true;

            for (var i = 0; i < 10; i++)
            {
                //ItemsGrid.Items.Add(new ItemInDataGrid { Category="Рыбка", Name="Сельдь", Price=100, Count=200, FirstCharacteristic="Не вкусная", SecondCharacteristic="желтая", UID="testUid1"});
                //ItemsGrid.Items.Add(new ItemInDataGrid { Category="Рыбка", Name="Сельдь", Price=200, Count=200, FirstCharacteristic ="вкусная", SecondCharacteristic="не желтая", UID="testUid2"});
            }
            var a = LogicalTreeHelper.GetChildren((DependencyObject)StoragesTabs.Items[1]);
            Console.WriteLine(a);
        }

        private async void OnAddNewStorageAsync(StorageData storageData)
        {
            var brushConverter = new BrushConverter();

            var ti = new TabItem();
            ti.Header = storageData.storageName;
            StoragesTabs.Items.Add(ti);

            var storageGrid = new Grid();
            storageGrid.Background = (Brush)brushConverter.ConvertFromString("#FF333336");
            storageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            storageGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            ti.Content = storageGrid;

            var storageTabControl = new TabControl();
            storageTabControl.Background = (Brush)brushConverter.ConvertFromString("#FF333336");
            storageTabControl.BorderBrush = (Brush)brushConverter.ConvertFromString("#FF333336");
            storageTabControl.Padding = new Thickness(0, 0, 0, 0);
            Grid.SetRow(storageTabControl, 1);

            var contentTab = new TabItem();
            contentTab.Header = "Содержимое";
            storageTabControl.Items.Add(contentTab);

            var contentGrid = new Grid();
            contentGrid.Background = (Brush)brushConverter.ConvertFromString("#FF333336");
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            contentTab.Content = contentGrid;

            var leftContetnGrid = new Grid();
            leftContetnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            leftContetnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30, GridUnitType.Star) });
            leftContetnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            contentGrid.Children.Add(leftContetnGrid);

            var contentDataGrid = new DataGrid();
            contentDataGrid.IsReadOnly = true;
            contentDataGrid.AutoGenerateColumns = false;
            contentDataGrid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("Category"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            contentDataGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            contentDataGrid.Columns.Add(new DataGridTextColumn { Header = "Количество", Binding = new Binding("Count"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            contentDataGrid.Columns.Add(new DataGridTextColumn { Header = "Цена", Binding = new Binding("Price"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            contentDataGrid.Columns.Add(new DataGridTextColumn { Header = "Главная характеристика", Binding = new Binding("FirstCharacteristic"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            contentDataGrid.Columns.Add(new DataGridTextColumn { Header = "Вторичная характеристика", Binding = new Binding("SecondCharacteristic"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

            var btn = new FrameworkElementFactory(typeof(Button));
            btn.SetValue(Button.ContentProperty, "Изменить");
            btn.SetValue(Button.CommandParameterProperty, new Binding("ItemInit"));
            btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnChangeButtonClick));
            var dgc = new DataGridTemplateColumn();
            var dtm = new DataTemplate();
            dtm.VisualTree = btn;
            dgc.CellTemplate = dtm;
            contentDataGrid.Columns.Add(dgc);

            btn = new FrameworkElementFactory(typeof(Button));
            btn.SetValue(Button.ContentProperty, "Удалить");
            btn.SetValue(Button.CommandParameterProperty, new Binding("ItemInit"));
            btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnDeleteButtonClick));
            dgc = new DataGridTemplateColumn();
            dtm = new DataTemplate();
            dtm.VisualTree = btn;
            dgc.CellTemplate = dtm;
            contentDataGrid.Columns.Add(dgc);

            btn = new FrameworkElementFactory(typeof(Button));
            btn.SetValue(Button.ContentProperty, "Поступление");
            btn.SetValue(Button.CommandParameterProperty, new Binding("ItemInit"));
            btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnAddButtonClick));
            dgc = new DataGridTemplateColumn();
            dtm = new DataTemplate();
            dtm.VisualTree = btn;
            dgc.CellTemplate = dtm;
            contentDataGrid.Columns.Add(dgc);

            btn = new FrameworkElementFactory(typeof(Button));
            btn.SetValue(Button.ContentProperty, "Отгрузка");
            btn.SetValue(Button.CommandParameterProperty, new Binding("ItemInit"));
            btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnRemoveButtonClick));
            dgc = new DataGridTemplateColumn();
            dtm = new DataTemplate();
            dtm.VisualTree = btn;
            dgc.CellTemplate = dtm;
            contentDataGrid.Columns.Add(dgc);

            contentDataGrid.EnableColumnVirtualization = true;
            contentDataGrid.EnableRowVirtualization = true;
            leftContetnGrid.Children.Add(contentDataGrid);

            var rightContetnGrid = new Grid();
            Grid.SetColumn(rightContetnGrid, 1);
            rightContetnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            rightContetnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            contentGrid.Children.Add(rightContetnGrid);

            var typesTab = new TabItem();
            typesTab.Header = "Типы";
            var typesGrid = new Grid();
            typesGrid.Background = (Brush)brushConverter.ConvertFromString("#FF333336");
            typesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            typesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            typesTab.Content = typesGrid;
            storageTabControl.Items.Add(typesTab);

            var StatisticTab = new TabItem();
            StatisticTab.Header = "Статистика";
            var StatisticGrid = new Grid();
            StatisticGrid.Background = (Brush)brushConverter.ConvertFromString("#FF333336");
            StatisticGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            StatisticGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            StatisticTab.Content = StatisticGrid;
            storageTabControl.Items.Add(StatisticTab);

            storageGrid.Children.Add(storageTabControl);

            storageData.storageItemsGrid = contentDataGrid;
            storageData.itemsInStorage = contentDataGrid.Items;
            for (var i = 0; i < 10; i++)
            {
                storageData.itemsInStorage.Add(new ItemInDataGrid { Category = "Рыбка", Name = "Сельдь", Price = 100, Count = 200, FirstCharacteristic = "Не вкусная", SecondCharacteristic = "желтая", ItemInit = new ItemInit {UID="testUID1", StorageData=storageData} });
                storageData.itemsInStorage.Add(new ItemInDataGrid { Category = "Рыбка", Name = "Сельдь", Price = 200, Count = 200, FirstCharacteristic = "вкусная", SecondCharacteristic = "не желтая", ItemInit = new ItemInit {UID="testUID2", StorageData=storageData} });
            }

            storages.Add(storageData.storageName, storageData);
            OnAddButtonClick(null, null);
        }

        private void OnChangeButtonClick(object sender, RoutedEventArgs e)
        {
            var itemInit = (ItemInit)((Button)sender).CommandParameter;
            foreach (var item in itemInit.StorageData.itemsInStorage)
            {
                ((ItemInDataGrid)item).Count -= 1000;
            }

            itemInit.StorageData.itemsInStorage.Refresh();
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddWindow();
            addWindow.Show();
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            var dt = this.FindName("ItemsGrid");
        }
    }
}
