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
    /// Логика взаимодействия для OutletProductsViewWindow.xaml
    /// </summary>
    public partial class OutletProductsViewWindow : Window
    {
        public int outletId;
        Employee authorizedEmployee;
        public OutletProductsViewWindow(int _outletId, Employee employee)
        {
            InitializeComponent();
            outletId = _outletId;
            authorizedEmployee = employee;
            UpdateOutletProductsList(outletId);
            if (employee.AccessLevel < 3)
            {
                openAddProductFormButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                openAddProductFormButton.Visibility = Visibility.Visible;
            }
        }
        public void UpdateOutletProductsList(int outletId)
        {
            productsListBox.ItemsSource = null;
            List<OutletProduct> products = DatabaseControl.GetOutletProducts();
            List<OutletProduct> outletProducts = new List<OutletProduct>();
            foreach (OutletProduct product in products)
            {
                if ((product.OutletId == outletId) && (product.Quantity > 0))
                {
                    outletProducts.Add(product);
                }
            }
            productsListBox.ItemsSource = outletProducts;
        }
        private void openAddProductFormButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow(outletId);
            addProductWindow.Owner = this;
            addProductWindow.Show();
        }

        private void productsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (authorizedEmployee.AccessLevel <3)
            {
                return;
            }
            OutletProduct selectedProduct = productsListBox.SelectedItem as OutletProduct;
            if (selectedProduct != null)
            {
                EditProductWindow editProductWindow = new EditProductWindow(outletId, selectedProduct.Product);
                editProductWindow.Owner = this;
                editProductWindow.Show();
            }
        }
    }
}
