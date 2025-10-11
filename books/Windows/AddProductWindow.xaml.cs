using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace books
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        List<ProductType> productTypes = DatabaseControl.GetProductTypesList();
        int outletId;
        public AddProductWindow(int _outletId)
        {
            InitializeComponent();
            outletId = _outletId;
            List<string> typeNames = new List<string>();
            foreach (ProductType type in productTypes)
            {
                typeNames.Add(type.Type);
            }
            typeInputComboBox.ItemsSource = typeNames;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (!MainWindow.ConfirmationDialog("Добавить выбранный продукт в базу данных?"))
                {
                    return;
                }
                if (string.IsNullOrEmpty(nameInputTextBox.Text) || string.IsNullOrEmpty(typeInputComboBox.Text) || string.IsNullOrEmpty(salePriceInputTextBox.Text) || string.IsNullOrEmpty(providerPriceInputTextBox.Text))
                {
                    MessageBox.Show("Заполнены не все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Product newProduct = new Product();
                newProduct.Name = nameInputTextBox.Text;
                foreach (ProductType productType in productTypes)
                {
                    if (productType.Type == typeInputComboBox.Text)
                    {
                        newProduct.ProductTypeId = productType.Id;
                        break;
                    }
                }
                newProduct.SalePrice = Convert.ToDecimal(salePriceInputTextBox.Text);
                newProduct.ProviderPrice = Convert.ToDecimal(providerPriceInputTextBox.Text);
                DatabaseControl.AddProduct(newProduct);
                OutletProduct outletProduct = new OutletProduct();
                outletProduct.ProductId = newProduct.Id;
                outletProduct.Quantity = 0;
                outletProduct.OutletId = outletId;
                DatabaseControl.AddOutletProduct(outletProduct);
                (this.Owner as OutletProductsViewWindow).UpdateOutletProductsList(outletId);
                this.Close();
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении продукта! Проверьтре правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
