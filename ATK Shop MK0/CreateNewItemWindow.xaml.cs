using ATK_Shop_MK0.DatabaseControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для CreateNewItemWindow.xaml
    /// </summary>
    public partial class CreateNewItemWindow : Window
    {
        private IControllerBase databaseController;

        private Brush BackgroundParamBoxBrush = new SolidColorBrush(new Color { R = 68, G = 68, B = 70 });
        private Brush BorderParamBoxBrush = new SolidColorBrush(new Color { R = 102, G = 102, B = 102 });
        private Brush ForegroundParamBoxBrush = Brushes.White;

        private bool isChange;
        private int UID;
        private FullItemData prevItemData;

        public CreateNewItemWindow(IControllerBase databaseController, bool isChange = false, int UID = -1)
        {
            this.databaseController = databaseController;
            this.isChange = isChange;
            this.UID = UID;

            InitializeComponent();
            AddCategoriesToSelector();
            
            if (isChange)
            {
                MainLabel.Content = "Изменение товара";
                prevItemData = databaseController.GetFullItemData(UID);
                CategoryComboBox.SelectedIndex = FindComboBoxIndexWithCategory(prevItemData.Category).Value;
                NameTextBox.Text = prevItemData.Name;
                PriceTextBox.Text = prevItemData.Price.ToString();
                DescriptionTextBox.Text = prevItemData.Description;
                for (var i = 0; i < CategoryParamsListBox.Items.Count; i++)
                    ((TextBox)((Grid)CategoryParamsListBox.Items[i]).Children[1]).Text = prevItemData.Params[i];
            }
        }

        private int? FindComboBoxIndexWithCategory(string categoryName)
        {
            for (var i = 0; i < CategoryComboBox.Items.Count; i++)
            {
                if (((TextBlock)CategoryComboBox.Items[i]).Text == categoryName)
                    return i;
            }
            return null;
        }

        private void AddCategoriesToSelector()
        {
            var categoriesList = databaseController.GetAllCategoriesNames();
            foreach (var categoryName in categoriesList)
            {
                CategoryComboBox.Items.Add(new TextBlock { Text = categoryName });
            }
        }

        private void OnCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var categoryName = ((TextBlock)e.AddedItems[0]).Text;
            var categoryParams = databaseController.GetCategoryParams(categoryName);
            RefilCategoryParamsList(categoryParams);
        }

        private void RefilCategoryParamsList(string[] paramsNames)
        {
            CategoryParamsListBox.Items.Clear();
            foreach (var paramName in paramsNames)
            {
                var mainGrid = new Grid();
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var categoryParamNameLabel = new Label { FontSize = 16, Content = $"{paramName}", Foreground = Brushes.White, IsHitTestVisible = false };
                Grid.SetColumn(categoryParamNameLabel, 0);
                mainGrid.Children.Add(categoryParamNameLabel);

                var categoryParamValueBox = new TextBox { FontSize = 16, MinWidth = 120, Tag = paramName, VerticalContentAlignment = VerticalAlignment.Center};
                Grid.SetColumn(categoryParamValueBox, 1);
                mainGrid.Children.Add(categoryParamValueBox);
                CategoryParamsListBox.Items.Add(mainGrid);
            }
        }

        private void OnClickSaveButton(object sender, RoutedEventArgs e)
        {
            void disableHighlight()
            {
                NameTextBox.BorderBrush = new SolidColorBrush(new Color { R = 171, G = 173, B = 179 });
                PriceTextBox.BorderBrush = new SolidColorBrush(new Color { R = 171, G = 173, B = 179 });
                foreach (Grid grid in CategoryParamsListBox.Items)
                {
                    ((TextBox)grid.Children[1]).BorderBrush = new SolidColorBrush(new Color { R = 171, G = 173, B = 179 });
                }
            }

            bool canCreate = true;
            disableHighlight();
            if (CategoryComboBox.SelectedItem == null)
            {
                canCreate = false;
            }
            if (NameTextBox.Text == "")
            {
                canCreate = false;
                NameTextBox.BorderBrush = Brushes.Red;
            }
            if (PriceTextBox.Text == "")
            {
                canCreate = false;
                PriceTextBox.BorderBrush = Brushes.Red;
            }

            var paramsList = new List<string>();
            foreach (Grid grid in CategoryParamsListBox.Items)
            {
                var paramBox = (TextBox)grid.Children[1];
                //var paramName = (string)paramBox.Tag;
                var paramValue = paramBox.Text;
                if (paramValue == "")
                {
                    paramBox.BorderBrush = Brushes.Red;
                    canCreate = false;
                }
                //paramsDict.Add(paramName, paramValue);
                paramsList.Add(paramValue);
            }

            if (canCreate)
            {
                if (isChange)
                {
                    var fullItemData = new FullItemData
                    {
                        Category = ((TextBlock)CategoryComboBox.SelectedItem).Text,
                        Name = NameTextBox.Text,
                        Params = paramsList.ToArray(),
                        Price = int.Parse(PriceTextBox.Text),
                        Description = DescriptionTextBox.Text,
                        Count = prevItemData.Count
                    };
                    databaseController.SetItemDataForUID(UID, fullItemData);
                }
                else
                {
                    databaseController.CreateNewItem(((TextBlock)CategoryComboBox.SelectedItem).Text, NameTextBox.Text, paramsList.ToArray(), int.Parse(PriceTextBox.Text), DescriptionTextBox.Text);
                    ((ViewerWindow)this.Owner).UpdateCategoryCreateCategoryList();
                }
                ((ViewerWindow)this.Owner).UpdateItemsList();
                ((ViewerWindow)this.Owner).UpdateStatisticItemsList();
                this.Close();
            }
        }

        private void OnClickAbortButton(object sender, RoutedEventArgs e) => this.Close();

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) => Utils.NumberValidationTextBox(sender, e);
    }
}
