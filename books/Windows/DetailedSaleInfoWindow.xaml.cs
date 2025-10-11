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
    /// Логика взаимодействия для DetailedSaleInfoWindow.xaml
    /// </summary>
    public partial class DetailedSaleInfoWindow : Window
    {
        Sale sale;
        Employee authorizedEmployee;
        int outletId;
        public DetailedSaleInfoWindow(Sale _sale, Employee employee, int _outletId)
        {
            InitializeComponent();
            authorizedEmployee = employee;
            if ((employee.AccessLevel < 2) || (_sale.PaymentMethodId == 3))
            {
                RefundButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                RefundButton.Visibility= Visibility.Visible;
            }
            outletId = _outletId;
            sale = _sale;
            receiptNumViewTextBlock.Text = "Номер чека: "+Convert.ToString(sale.ReceiptNumber);
            employeeFullNameViewTextBlock.Text = "ФИО сотрудника: " + sale.Employee.FullName;
            if (sale.PaymentMethodId != 2)
            {
                customersCardNubmerViewTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                customersCardNubmerViewTextBlock.Text = "Номер карты: " + sale.CustomerCardNumber.Replace(" ", "").Substring(sale.CustomerCardNumber.Length - 4);
            }
            dateTimeViewTextBlock.Text = "Дата совершения операции: "+sale.DateTime.ToString();
            paymentMethodViewTextBlock.Text="Метод оплаты: "+sale.PaymentMethod.Method;
            summCostViewTextBlock.Text = "Общая сумма: "+PresentableSale.CalculateSumm(sale.ReceiptNumber).ToString();
            List<SoldProduct> soldProducts = DatabaseControl.GetSoldProductsList();
            List<SoldProduct> selectedSoldProducts = new List<SoldProduct>();
            foreach (SoldProduct product in soldProducts)
            {
                if (product.SaleId == sale.ReceiptNumber)
                {
                    selectedSoldProducts.Add(product);
                }
            }
            soldProductsListBox.ItemsSource=selectedSoldProducts;
        }

        private void RefundButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.ConfirmationDialog("Вы уверены, что хотите оформить возврат денег по данной операции?"))
            {
                if (sale.PaymentMethodId == 3)
                {
                    MessageBox.Show("Ошибка! На эту операцию уже был оформлен возврат.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                sale.PaymentMethodId = 3;
                DatabaseControl.UpdateSale(sale);
                MessageBox.Show("Операция завершена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.None);
                (this.Owner as SalesViewWindow).UpdateSalesListBox();
                this.Close();
            }
        }
    }
}
