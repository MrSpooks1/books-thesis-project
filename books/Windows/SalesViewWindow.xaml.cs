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
    /// Логика взаимодействия для SalesViewWindow.xaml
    /// </summary>
    public partial class SalesViewWindow : Window
    {
        Employee authorizedEmployee;
        int outletId;
        List<PresentableSale> presentableSales;
        public SalesViewWindow(Employee employee, int _outletId)
        {
            InitializeComponent();
            authorizedEmployee = employee;
            outletId = _outletId;
            UpdateSalesListBox();
        }
        public void UpdateSalesListBox()
        {
            List<Sale> sales = DatabaseControl.GetSalesList();
            presentableSales = new List<PresentableSale>();
            foreach (Sale sale in sales)
            {
                presentableSales.Add(new PresentableSale(sale));
            }
            salesListBox.ItemsSource = null;
            salesListBox.ItemsSource = presentableSales;
        }

        private void salesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            List<PresentableSale> presentableSales = new List<PresentableSale>();
            PresentableSale tempPresentableSale = salesListBox.SelectedItem as PresentableSale;
            if (tempPresentableSale != null)
            {
                List<Sale> sales = DatabaseControl.GetSalesList();
                foreach (Sale sale in sales)
                {
                    if (sale.ReceiptNumber == tempPresentableSale.ReceiptNumber)
                    {
                        DetailedSaleInfoWindow detailedSaleInfoWindow = new DetailedSaleInfoWindow(sale, authorizedEmployee, outletId);
                        detailedSaleInfoWindow.Owner = this;
                        detailedSaleInfoWindow.Show();
                    }
                }
            }
        }
    }
}
