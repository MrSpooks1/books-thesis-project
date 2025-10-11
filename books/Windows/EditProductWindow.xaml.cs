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
using System.Windows.Shapes;

namespace books
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        List<ProductType> productTypes = DatabaseControl.GetProductTypesList();
        int outletId;
        Product selectedProduct;
        public EditProductWindow(int _outletId, Product _selectedProduct)
        {
            InitializeComponent();
            outletId = _outletId;
            selectedProduct = _selectedProduct;
            List<string> typeNames = new List<string>();
            string typeName = "";
            foreach (ProductType type in productTypes)
            {
                typeNames.Add(type.Type);
                if (type.Id == _selectedProduct.ProductTypeId)
                {
                    typeName = type.Type;
                }
            }
            typeInputComboBox.ItemsSource = typeNames;
            typeInputComboBox.Text = typeName;
            nameInputTextBox.Text = selectedProduct.Name;
            salePriceInputTextBox.Text = Convert.ToString(selectedProduct.SalePrice);
            providerPriceInputTextBox.Text = Convert.ToString(selectedProduct.ProviderPrice);
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MainWindow.ConfirmationDialog("Изменить данные о выбранном продукте?"))
                {
                    return;
                }
                if (string.IsNullOrEmpty(nameInputTextBox.Text) || string.IsNullOrEmpty(typeInputComboBox.Text) || string.IsNullOrEmpty(salePriceInputTextBox.Text) || string.IsNullOrEmpty(providerPriceInputTextBox.Text))
                {
                    MessageBox.Show("Заполнены не все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                selectedProduct.Name = nameInputTextBox.Text;
                foreach (ProductType productType in productTypes)
                {
                    if (productType.Type == typeInputComboBox.Text)
                    {
                        selectedProduct.ProductTypeId = productType.Id;
                        break;
                    }
                }
                selectedProduct.SalePrice = Convert.ToDecimal(salePriceInputTextBox.Text);
                selectedProduct.ProviderPrice = Convert.ToDecimal(providerPriceInputTextBox.Text);
                DatabaseControl.UpdateProduct(selectedProduct);
                (this.Owner as OutletProductsViewWindow).UpdateOutletProductsList(outletId);
                this.Close();
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Произошла ошибка при изменении продукта! Проверьтре правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
