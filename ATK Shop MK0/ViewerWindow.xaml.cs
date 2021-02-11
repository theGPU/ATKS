using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ATK_Shop_MK0.DatabaseControllers;
using LiveCharts;
using LiveCharts.Wpf;

namespace ATK_Shop_MK0
{
    /// <summary>
    /// Логика взаимодействия для ViewerWindow.xaml
    /// </summary>
    public partial class ViewerWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }

        public IControllerBase databaseController = new SqliteController();

        public ViewerWindow()
        {
            InitializeComponent();
            ShowOrHideItemInterface(false);
            CategoryCreateGrid.Visibility = Visibility.Hidden;
            ItemsGrid.EnableColumnVirtualization = true;
            ItemsGrid.EnableRowVirtualization = true;
            databaseController.ConnectToLocalDB("sqlite.db");
            UpdateItemsList();
            UpdateCategoryCreateCategoryList();
            UpdateStatisticItemsList();

            MainStatisticChart.LegendLocation = LegendLocation.Top;
            MainStatisticChart.ChartLegend.Foreground = Brushes.White;
            MainStatisticChart.AxisY.Clear();
            MainStatisticChart.AxisY.Add(new Axis { Title = "Остаток", MinValue = 0 });
            MainStatisticChart.AxisX.Clear();
            MainStatisticChart.AxisX.Add(new Axis { Title = "Дни", Labels = Enumerable.Range(1, 30).Reverse().Select(x => x.ToString()).ToArray(), Separator = new LiveCharts.Wpf.Separator { Step = 1 } });

            SeriesCollection = new SeriesCollection();
            MainStatisticChart.Series = SeriesCollection;

            SeriesCollection.Add(new LineSeries
            {
                Title = "Количество",
                Values = new ChartValues<int>(new int[30])
            });
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Импортировано",
                Values = new ChartValues<int>(new int[30])
            });
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Экспортировано",
                Values = new ChartValues<int>(new int[30])
            });
        }

        public void UpdateItemsList()
        {
            var itemsList = databaseController.GetAllItems();
            ItemsGrid.Items.Clear();
            foreach (var item in itemsList)
            {
                ItemsGrid.Items.Add(item);
            }
        }

        private void ShowOrHideItemInterface(bool show)
        {
            var visability = show ? Visibility.Visible : Visibility.Hidden;
            ItemInfoGrid.Visibility = visability;

            /*
            selectedItemName.Visibility = visability;
            selectedItemCategory.Visibility = visability;
            selectedItemPrice.Visibility = visability;
            selectedItemCount.Visibility = visability;
            selectedItemCharacteristicsLabel.Visibility = visability;
            selectedItemCharacteristics.Visibility = visability;

            addButton.Visibility = visability;
            subButton.Visibility = visability;
            statisticButton.Visibility = visability;
            deletebutton.Visibility = visability;
            */
        }

        private ItemInDataGrid FindElementByUID(int uid)
        {
            for (var i = 0; i < ItemsGrid.Items.Count; i++)
            {
                if (((ItemInDataGrid)ItemsGrid.Items[i]).UID == uid)
                {
                    return (ItemInDataGrid)ItemsGrid.Items[i];
                }
            }
            return null;
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            var uid = (int)((Button)sender).Tag;
            var selectedItem = FindElementByUID(uid);
            var addWindow = new AddWindow(selectedItem.UID, selectedItem.Price, selectedItem.Category, selectedItem.Name, "in");
            addWindow.Owner = this;
            addWindow.ShowDialog();
        }

        private void OnSubButtonClick(object sender, RoutedEventArgs e)
        {
            var uid = (int)((Button)sender).Tag;
            var selectedItem = FindElementByUID(uid);
            var addWindow = new AddWindow(selectedItem.UID, selectedItem.Price, selectedItem.Category, selectedItem.Name, "out");
            addWindow.Owner = this;
            addWindow.ShowDialog();
        }

        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            var uid = (int)((Button)sender).Tag;
            var window = new CreateNewItemWindow(databaseController, true, uid);
            window.Owner = this;
            window.ShowDialog();
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            var uid = (int)((Button)sender).Tag;
            databaseController.DeleteItemByUID(uid);
            ShowOrHideItemInterface(false);
            UpdateItemsList();
            UpdateStatisticItemsList();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (ItemInDataGrid)((DataGridRow)sender).DataContext;
            selectedItemName.Content = $"Товар: {selectedItem.Name}";
            selectedItemCategory.Content = $"Категория: {selectedItem.Category}";
            selectedItemPrice.Content = $"Цена: {selectedItem.Price}";
            selectedItemCount.Content = $"В наличии: {selectedItem.Count}";
            selectedItemDescription.Text = selectedItem.Description;

            var uid = selectedItem.UID;
            addButton.Tag = uid;
            subButton.Tag = uid;
            StatisticButton.Tag = uid;
            EditButton.Tag = uid;

            SelectedItemCharacteristics.Items.Clear();
            var itemParams = databaseController.GetItemParams(uid);
            foreach (var param in itemParams)
                SelectedItemCharacteristics.Items.Add(new Label { Content = $"{param.Key}: {param.Value}", FontSize = 16, Foreground = Brushes.White });

            ShowOrHideItemInterface(true);
        }

        #region category

        private void OnClickCategoryCreateButton(object sender, RoutedEventArgs e)
        {
            TypesListBox.UnselectAll();
            CategoryCreateLabel.Content = "Создание новой категории";
            CategoryCreateGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
            CategoryCreateGrid.RowDefinitions[4].Height = new GridLength(1, GridUnitType.Star);
            CategoryCreateCategoryNameGrid.Visibility = Visibility.Visible;
            CategoryCreateControlButtonsGrid.Visibility = Visibility.Visible;
            CategoryCreateParamsButtonsGrid.Visibility = Visibility.Visible;
            CategoryCreateParamsListBox.Items.Clear();
            CategoryCreateCategoryNameTextBox.Text = "";
            CategoryCreateGrid.Visibility = Visibility.Visible;
        }

        private void OnClickCategoryAddParam(object sender, RoutedEventArgs e)
        {
            //var mainGrid = new Grid();
            //mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //CategoryCreateParamsListBox.Items.Add(mainGrid);
            CategoryCreateParamsListBox.Items.Add(new ListBoxItem { Content = new TextBox { MinWidth = 120 } });
        }

        private void OnClickCategoryCreateConfirmButton(object sender, RoutedEventArgs e)
        {
            void highlightBoxesWithParamName(string name)
            {
                foreach (ListBoxItem item in CategoryCreateParamsListBox.Items)
                {
                    var textBox = (TextBox)item.Content;
                    if (textBox.Text == name)
                        textBox.BorderBrush = Brushes.Red;
                }
            }

            void disableHighlight()
            {
                foreach (ListBoxItem item in CategoryCreateParamsListBox.Items)
                    ((TextBox)item.Content).BorderBrush = new SolidColorBrush(new Color { R = 171, G = 173, B = 179 });
            }

            var canCreate = true;
            disableHighlight();
            var paramsNames = new List<string>();
            foreach (ListBoxItem item in CategoryCreateParamsListBox.Items)
            {
                var textBox = (TextBox)item.Content;
                if (textBox.Text == "") 
                {
                    canCreate = false;
                    textBox.BorderBrush = Brushes.Red;
                } else if (paramsNames.Contains(textBox.Text))
                {
                    canCreate = false;
                    highlightBoxesWithParamName(textBox.Text);
                } else
                {
                    paramsNames.Add(textBox.Text);
                }
            }

            var categoryName = CategoryCreateCategoryNameTextBox.Text;
            if (categoryName == "" || databaseController.GetAllCategoriesNames().Contains(categoryName))
            {
                canCreate = false;
                CategoryCreateCategoryNameTextBox.BorderBrush = Brushes.Red;
            }
            else
                CategoryCreateCategoryNameTextBox.BorderBrush = new SolidColorBrush(new Color { R = 171, G = 173, B = 179 });

            if (canCreate)
                if (databaseController.CreateNewCategory(paramsNames, categoryName))
                {
                    UpdateCategoryCreateCategoryList();
                    CategoryCreateGrid.Visibility = Visibility.Hidden;
                }
        }

        public void UpdateCategoryCreateCategoryList()
        {
            var categories = databaseController.GetCategoriesWithItemsCount();
            TypesListBox.Items.Clear();
            foreach (var category in categories)
            {
                var categoryGrid = new Grid();
                categoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                categoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var categoryNameLabel = new Label { FontSize = 16, Content = $"{category.Key}", Foreground = Brushes.White, IsHitTestVisible = false };
                Grid.SetColumn(categoryNameLabel, 0);
                categoryGrid.Children.Add(categoryNameLabel);

                var categoryCountLabel = new Label { FontSize = 16, Content = $"({category.Value})", Foreground = Brushes.White, IsHitTestVisible = false };
                Grid.SetColumn(categoryCountLabel, 1);
                categoryGrid.Children.Add(categoryCountLabel);

                TypesListBox.Items.Add(categoryGrid);
            }
        }

        private void OnTypesListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1)
                return;

            var categoryName = (string)((Label)((Grid)e.AddedItems[0]).Children[0]).Content;
            CategoryCreateLabel.Content = $"Просмотр категории \"{categoryName}\"";
            CategoryCreateCategoryNameGrid.Visibility = Visibility.Collapsed;
            CategoryCreateControlButtonsGrid.Visibility = Visibility.Collapsed;
            CategoryCreateParamsButtonsGrid.Visibility = Visibility.Collapsed;
            CategoryCreateGrid.RowDefinitions[3].Height = GridLength.Auto;
            CategoryCreateGrid.RowDefinitions[4].Height = GridLength.Auto;

            CategoryCreateParamsListBox.Items.Clear();
            var categoryParams = databaseController.GetCategoryParams(categoryName);
            for (var i = 0; i < categoryParams.Length; i++)
                CategoryCreateParamsListBox.Items.Add(new ListBoxItem { Content = categoryParams[i], Foreground = Brushes.White, FontSize = 16 });

            CategoryCreateGrid.Visibility = Visibility.Visible;
        }

        private void OnClickCategoryCreateAbortButton(object sender, RoutedEventArgs e) => CategoryCreateGrid.Visibility = Visibility.Hidden;

        private void OnClickCategoryCreateDelParamButton(object sender, RoutedEventArgs e) => CategoryCreateParamsListBox.Items.Remove(CategoryCreateParamsListBox.SelectedItem);

        private void OnClickCategoryDeleteButton(object sender, RoutedEventArgs e)
        {
            CategoryCreateGrid.Visibility = Visibility.Hidden;
            if (databaseController.DeleteCategory((string)((Label)((Grid)TypesListBox.SelectedItem).Children[0]).Content))
            {
                TypesListBox.Items.Remove(TypesListBox.SelectedItem);
                UpdateItemsList();
                UpdateStatisticItemsList();
            }
        }

        #endregion

        private void OnClickCreateItemButton(object sender, RoutedEventArgs e)
        {
            var window = new CreateNewItemWindow(databaseController);
            window.Owner = this;
            window.ShowDialog();
        }

        private void OnClickDeleteItemButton(object sender, RoutedEventArgs e)
        {
            databaseController.DeleteItemByUID(((ItemInDataGrid)ItemsGrid.SelectedItem).UID);
            ShowOrHideItemInterface(false);
            UpdateItemsList();
            UpdateStatisticItemsList();
        }

        public void UpdateStatisticItemsList()
        {
            var items = databaseController.GetItemsListForActivity().ToList();
            items.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            StatisticItemsList.Items.Clear();
            foreach (var item in items)
            {
                StatisticItemsList.Items.Add(new Label { FontSize = 16, Content = item.Value, Tag = item.Key, Foreground = Brushes.White });
            }
        }

        private void OnStatisticItemsListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1)
                return;

            var item = (Label)e.AddedItems[0];
            var itemId = (int)item.Tag;

            var statisticDataPoints = databaseController.GetPointsForActivity(itemId);

            SeriesCollection[0].Values = new ChartValues<int>(statisticDataPoints.Select(dataPoint => dataPoint.Count));
            SeriesCollection[1].Values = new ChartValues<int>(statisticDataPoints.Select(dataPoint => dataPoint.In));
            SeriesCollection[2].Values = new ChartValues<int>(statisticDataPoints.Select(dataPoint => dataPoint.Out));
        }

        private object GetItemInStatisticListBoxWithUID(int tag)
        {
            foreach (var item in StatisticItemsList.Items)
                if ((int)((Label)item).Tag == tag)
                    return item;
            return null;
        }

        private void OnClickStatisticButton(object sender, RoutedEventArgs e)
        {
            var itemForSelect = GetItemInStatisticListBoxWithUID((int)((Button)sender).Tag);
            StatisticItemsList.SelectedItem = itemForSelect;
            StatisticItemsList.ScrollIntoView(itemForSelect);
            MainMenuTabControl.SelectedIndex = 2;
        }
    }
}
