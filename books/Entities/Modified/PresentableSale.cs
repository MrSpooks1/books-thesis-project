using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books
{
    public class PresentableSale : Sale
    {
        public decimal Summ {  get; set; }
        public PresentableSale(Sale sale)
        {
            ReceiptNumber = sale.ReceiptNumber;
            ReceiptNumber = sale.ReceiptNumber;
            EmployeeId = sale.EmployeeId;
            CustomerCardNumber = sale.CustomerCardNumber;
            DateTime = sale.DateTime;
            PaymentMethodId = sale.PaymentMethodId;
            OutletId = sale.OutletId;
            Employee = sale.Employee;
            Outlet = sale.Outlet;
            PaymentMethod = sale.PaymentMethod;
            SoldProducts = sale.SoldProducts;
            Summ = CalculateSumm(ReceiptNumber);
        }
        public static decimal CalculateSumm(int receiptNumber)
        {
            decimal tempSum = 0;
            List<SoldProduct> soldProducts = DatabaseControl.GetSoldProductsList();
            foreach (SoldProduct soldProduct in soldProducts)
            {
                if (soldProduct.SaleId == receiptNumber)
                {
                    tempSum += soldProduct.Quantity * soldProduct.Product.SalePrice;
                }
            }
            return tempSum;
        }
    }
}
