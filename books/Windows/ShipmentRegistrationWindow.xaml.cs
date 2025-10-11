using Npgsql;
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
    /// Логика взаимодействия для ShipmentRegistrationWindow.xaml
    /// </summary>
    public partial class ShipmentRegistrationWindow : Window
    {
        List<SelectedProduct> selectedProducts; 
        Employee authorizedEmployee;
        List<Provider> providers;
        int outletId;
        public ShipmentRegistrationWindow(int _outletId, Employee employee)
        {
            InitializeComponent();
            UpdateProductNamesSearchBar();
            outletId = _outletId;
            selectedProducts = new List<SelectedProduct>();
            authorizedEmployee = employee;
            providers = DatabaseControl.GetProvidersList();
            List<string> providerNames = new List<string>();
            foreach (Provider provider in providers)
            {
                providerNames.Add(provider.Name);
            }
            providersSelectComboBox.ItemsSource = providerNames;
        }
        private void UpdateProductNamesSearchBar()
        {
            List<string> productNames = new List<string>();
            List<Product> products = DatabaseControl.GetProductsList();
            foreach (Product product in products)
            {
                productNames.Add(product.Name);
            }
            productNameAutoCompleteBox.ItemsSource = productNames;
        }
        public void UpdateMainListBox()
        {
            mainListBox.ItemsSource = null;
            mainListBox.ItemsSource = selectedProducts;
        }
        public void UpdateProductInfoListBox(List<Product> productInfo) // список здесь используется в качестве костыля. Нужен только 1 объект
        {
            productInfoListBox.ItemsSource = null;
            productInfoListBox.ItemsSource = productInfo;
        }

        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    if ((string.IsNullOrEmpty(productNameAutoCompleteBox.Text)))
                    {
                        MessageBox.Show("Введите идентификатор товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    int quantity = 1;
                    if (!(string.IsNullOrEmpty(productCountTextBox.Text)))
                    {
                        quantity = Convert.ToInt32(productCountTextBox.Text);
                    }
                    List<Product> products = DatabaseControl.GetProductsList();
                    List<Product> productInfo = new List<Product>();
                    bool productIsFound = false;
                    foreach (Product product in products)
                    {
                        if (product.Name == productNameAutoCompleteBox.Text)
                        {
                            if (selectedProducts.Count == 0) // если список полностью пустой
                            {
                                selectedProducts.Add(new SelectedProduct(product));
                                selectedProducts[0].Count = quantity;
                            }
                            else
                            {
                                bool selectedProductIsFound = false;
                                for (int i = 0; i < selectedProducts.Count; i++)
                                {
                                    if (product.Id == selectedProducts[i].Id)
                                    {
                                        selectedProducts[i].Count+= quantity;
                                        selectedProductIsFound = true;
                                        break;
                                    }
                                }
                                if (!selectedProductIsFound)
                                {
                                    selectedProducts.Add(new SelectedProduct(product));
                                    selectedProducts[selectedProducts.Count()-1].Count = quantity;
                                }
                            }
                            productInfo.Add(product);
                            UpdateProductInfoListBox(productInfo);
                            UpdateMainListBox();
                            return;
                        }
                    }
                    if (!productIsFound)
                    {
                    MessageBox.Show("Не найден продукт с выбранным идентификатором", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (System.FormatException ex)
                {
                    MessageBox.Show("Возникла ошибка при добавлении товара! Проверьте правильность заполнения полей.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("При попытке подключения к базе данных, произошла ошибка. Пожалуйста, попробуйте позже.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void confirmRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.ConfirmationDialog("Подтвердить регистрацию поставки"))
            {
                if ((string.IsNullOrEmpty(providersSelectComboBox.Text)))
                {
                    MessageBox.Show("Выберите поставщика.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (selectedProducts.Count == 0)
                {
                    MessageBox.Show("Добавьте продукты, полученные в поставке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Shipment shipment = new Shipment();
                shipment.OutletId = outletId;
                shipment.EmployeeId = authorizedEmployee.Id;
                foreach (Provider provider in providers)
                {
                    if (provider.Name == providersSelectComboBox.Text)
                    {
                        shipment.ProviderId = provider.Id;
                        break;
                    }
                }
                DatabaseControl.AddShipment(shipment);
                foreach (SelectedProduct product in selectedProducts)
                {
                    ReceivedProduct receivedProduct = new ReceivedProduct();
                    receivedProduct.ProductId = product.Id;
                    receivedProduct.Quantity = product.Count;
                    receivedProduct.ShipmentId = shipment.Id;
                    DatabaseControl.AddReceivedProduct(receivedProduct);
                    List<OutletProduct> outletProducts = DatabaseControl.GetOutletProducts();
                    bool outletProductExists = false;
                    foreach (OutletProduct outletProduct in outletProducts)
                    {
                        if (outletProduct.ProductId == product.Id)
                        {
                            outletProduct.Quantity += product.Count;
                            outletProductExists = true;
                            DatabaseControl.UpdateOutletProduct(outletProduct);
                            break;
                        }
                    }
                    if (!outletProductExists)
                    {
                        OutletProduct _outletProduct = new OutletProduct();
                        _outletProduct.ProductId = product.Id;
                        _outletProduct.Quantity = product.Count;
                        _outletProduct.OutletId = outletId;
                        DatabaseControl.AddOutletProduct(_outletProduct);
                    }
                }
                this.Close();
            }
        }
    }
}
