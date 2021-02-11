using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATK_Shop
{
    public static class PagesXaml
    {
		public static readonly string StorageXaml =
			@"<TabItem Header=""{0}"" x:Name=""{1}"">
	<Grid Background=""#FF333336"">
		<Grid.ColumnDefinitions>
			<ColumnDefinition Width=""1*""/>
		</Grid.ColumnDefinitions>
		<Grid.RowDefinitions>
			<RowDefinition Height=""1*""/>
		</Grid.RowDefinitions>
		<TabControl Background=""#FF333336"" BorderBrush=""#FF333336"" Padding=""0,0,0,0"" Grid.Row=""1"">
			<TabItem Header=""Содержимое"">
				<Grid Background=""#FF333336"">
					<Grid.ColumnDefinitions>
						<ColumnDefinition Width=""3*""/>
						<ColumnDefinition Width=""1*""/>
					</Grid.ColumnDefinitions>
					<Grid.RowDefinitions>
						<RowDefinition Height=""1*""/>
					</Grid.RowDefinitions>
					<Grid>
						<Grid.ColumnDefinitions>
							<ColumnDefinition Width=""1*""/>
						</Grid.ColumnDefinitions>
						<Grid.RowDefinitions>
							<RowDefinition Height=""30*""/>
							<RowDefinition Height=""1*""/>
						</Grid.RowDefinitions>
						<DataGrid x:Name=""{2}"" IsReadOnly=""True"" AutoGenerateColumns=""False"">
							<DataGrid.Columns>
								<DataGridTextColumn Header=""Категория"" Binding=""{{Binding Category}}"" Width=""1*""/>
								<DataGridTextColumn Header=""Название"" Binding=""{{Binding Name}}"" Width=""2*""/>
								<DataGridTextColumn Header=""Количество"" Binding=""{{Binding Count}}"" Width=""1*""/>
								<DataGridTextColumn Header=""Цена"" Binding=""{{Binding Price}}"" Width=""1*""/>
								<DataGridTextColumn Header=""Главная характеристика"" Binding=""{{Binding FirstCharacteristic}}"" Width=""2*""/>
								<DataGridTextColumn Header=""Вторичная характеристика"" Binding=""{{Binding SecondCharacteristic}}"" Width=""2*""/>
								<DataGridTemplateColumn>
									<DataGridTemplateColumn.CellTemplate>
										<DataTemplate>
											<Button CommandParameter=""{{Binding Path=UID}}"" ToolTip=""Изменить"" x:Name=""{3}"">Change</Button>
										</DataTemplate>
									</DataGridTemplateColumn.CellTemplate>
								</DataGridTemplateColumn>
								<DataGridTemplateColumn>
									<DataGridTemplateColumn.CellTemplate>
										<DataTemplate>
											<Button CommandParameter=""{{Binding Path=UID}}"" ToolTip=""Удалить"" x:Name=""{4}"">Delete</Button>
										</DataTemplate>
									</DataGridTemplateColumn.CellTemplate>
								</DataGridTemplateColumn>
								<DataGridTemplateColumn>
									<DataGridTemplateColumn.CellTemplate>
										<DataTemplate>
											<Button CommandParameter=""{{Binding Path=UID}}"" ToolTip=""Поступление"" x:Name=""{5}"">Add</Button>
										</DataTemplate>
									</DataGridTemplateColumn.CellTemplate>
								</DataGridTemplateColumn>
								<DataGridTemplateColumn>
									<DataGridTemplateColumn.CellTemplate>
										<DataTemplate>
											<Button CommandParameter=""{{Binding Path=UID}}"" ToolTip=""Отгрузка"" x:Name=""{6}"">Sub</Button>
										</DataTemplate>
									</DataGridTemplateColumn.CellTemplate>
								</DataGridTemplateColumn>
							</DataGrid.Columns>
						</DataGrid>
						<Border BorderThickness=""1"" BorderBrush=""White"" Grid.Row=""1"" Grid.Column=""0""/>
					</Grid>
					<Grid Grid.Column=""1"">
						<Grid.ColumnDefinitions>
							<ColumnDefinition Width=""1*""/>
						</Grid.ColumnDefinitions>
						<Grid.RowDefinitions>
							<RowDefinition Height=""1*""/>
						</Grid.RowDefinitions>
					</Grid>
				</Grid>
			</TabItem>
			<TabItem Header=""Типы"">
				<Grid Background=""#FF333336"">
					<Grid.ColumnDefinitions>
						<ColumnDefinition Width=""1*""/>
					</Grid.ColumnDefinitions>
					<Grid.RowDefinitions>
						<RowDefinition Height=""1*""/>
					</Grid.RowDefinitions>
				</Grid>
			</TabItem>
			<TabItem Header=""Статистика"">
				<Grid Background=""#FF333336"">
					<Grid.ColumnDefinitions>
						<ColumnDefinition Width=""3*""/>
						<ColumnDefinition Width=""1*""/>
					</Grid.ColumnDefinitions>
					<Grid.RowDefinitions>
						<RowDefinition Height=""1*""/>
					</Grid.RowDefinitions>
				</Grid>
			</TabItem>
		</TabControl>
	</Grid>
</TabItem>";

	}
}
