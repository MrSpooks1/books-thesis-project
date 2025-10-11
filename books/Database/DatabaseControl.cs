using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books
{
    public static class DatabaseControl
    {
        public static BooksContext BooksContext
        {
            get => default;
            set
            {
            }
        }

        public static List<Product> GetProductsList()
        {
            using (BooksContext context = new BooksContext())
            {
                return context.Products.Include(p => p.OutletProducts).Include(p => p.ProductType).Include(p => p.ReceivedProducts).Include(p => p.SoldProducts).ToList();
            }    
        }
        public static List<ProductType> GetProductTypesList()
        {
            using (BooksContext context = new BooksContext())
            {
                return context.ProductTypes.Include(p => p.Products).ToList();
            }
        }
        public static List<Outlet> GetOutletsList()
        {
            using (BooksContext context = new BooksContext())
            {
                return context.Outlets.Include(p => p.OutletProducts).Include(p => p.Sales).Include(p => p.Shipments).ToList();
            }
        }
        public static List<OutletProduct> GetOutletProducts()
        {
            using (BooksContext context = new BooksContext())
            {
                return context.OutletProducts.Include(p => p.Outlet).Include(p => p.Product).ToList();
            }
        }
        public static List<Employee> GetEmployeesList()
        {
            using (BooksContext context = new BooksContext())
            {
                return context.Employees.Include(p => p.Sales).Include(p => p.Shipments).ToList();
            }
        }
        public static List<Provider> GetProvidersList()
        {
            using (BooksContext context = new BooksContext())
            {
                return context.Providers.Include(p => p.ProviderType).Include(p => p.Shipments).ToList();
            }
        }
        public static List<Sale> GetSalesList()
        {
            using (BooksContext context = new BooksContext())
            {
                return context.Sales.Include(p => p.Outlet).Include(p => p.SoldProducts).Include(p => p.PaymentMethod).Include(p => p.Employee).ToList();
            }
        }
        public static List<SoldProduct> GetSoldProductsList()
        {
            using (BooksContext context = new BooksContext())
            {
                return context.SoldProducts.Include(p => p.Product).Include(p => p.Sale).ToList(); 
            }
        }
        public static void AddProduct(Product product)
        {
            using (BooksContext context = new BooksContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
        }
        public static void AddEmployee(Employee employee)
        {
            using (BooksContext context = new BooksContext())
            {
                context.Employees.Add(employee);
                context.SaveChanges();
            }
        }
        public static void AddSale(Sale sale)
        {
            using (BooksContext context = new BooksContext())
            {
                context.Sales.Add(sale);
                context.SaveChanges();
            }
        }
        public static void AddSoldProduct(SoldProduct soldProduct)
        {
            using (BooksContext context = new BooksContext())
            {
                context.SoldProducts.Add(soldProduct);
                context.SaveChanges();
            }
        }
        public static void AddShipment(Shipment shipment)
        {
            using (BooksContext context = new BooksContext())
            {
                context.Shipments.Add(shipment);
                context.SaveChanges();
            }
        }
        public static void AddReceivedProduct(ReceivedProduct receivedProduct)
        {
            using (BooksContext context = new BooksContext())
            {
                context.ReceivedProducts.Add(receivedProduct);
                context.SaveChanges();
            }
        }
        public static void AddOutletProduct(OutletProduct outletProduct)
        {
            using (BooksContext context = new BooksContext())
            {
                context.OutletProducts.Add(outletProduct);
                context.SaveChanges();
            }
        }
        public static void AddProvider(Provider provider)
        {
            using (BooksContext context = new BooksContext())
            {
                context.Providers.Add(provider);
                context.SaveChanges();
            }
        }
        public static void UpdateProvider(Provider provider)
        {
            using (BooksContext context = new BooksContext())
            {
                Provider _provider = context.Providers.FirstOrDefault(p => p.Id == provider.Id);
                if (_provider != null)
                {
                    _provider.Name = provider.Name;
                    _provider.EmailAddress = provider.EmailAddress;
                    _provider.PhoneNumber = provider.PhoneNumber;
                    _provider.PostalCode = provider.PostalCode;
                    _provider.ProviderTypeId = provider.ProviderTypeId;
                    context.SaveChanges();
                }
            }
        }
        public static void UpdateProduct(Product product)
        {
            using (BooksContext context = new BooksContext())
            {
                Product _product = context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (_product != null)
                {
                    _product.Name = product.Name;
                    _product.ProviderPrice = product.ProviderPrice;
                    _product.SalePrice = product.SalePrice;
                    _product.ProductTypeId = product.ProductTypeId;
                    _product.OutletProducts = product.OutletProducts;
                    _product.SoldProducts = product.SoldProducts;
                    _product.ReceivedProducts = product.ReceivedProducts;
                    _product.ProductType = product.ProductType;
                    context.SaveChanges();
                }
            }
        }
        public static void UpdateEmployee(Employee employee)
        {
            using (BooksContext context = new BooksContext())
            {
                Employee _employee = context.Employees.FirstOrDefault(p => p.Id ==employee.Id);
                if (_employee != null)
                {
                    _employee.FullName = employee.FullName;
                    _employee.PhoneNumber = employee.PhoneNumber;
                    _employee.PassportSerialNumber = employee.PassportSerialNumber;
                    _employee.Password = employee.Password;
                    _employee.Login = employee.Login;
                    _employee.AccessLevel = employee.AccessLevel;
                    context.SaveChanges();
                }
            }
        }
        public static void UpdateOutletProduct(OutletProduct outletProduct)
        {
            using (BooksContext context = new BooksContext())
            {
                OutletProduct _outletProduct = context.OutletProducts.FirstOrDefault(p => p.Id == outletProduct.Id);
                if (_outletProduct != null)
                {
                    _outletProduct.OutletId = outletProduct.OutletId;
                    _outletProduct.ProductId = outletProduct.ProductId;
                    _outletProduct.Quantity = outletProduct.Quantity;
                    context.SaveChanges();
                }
            }
        }
        public static void UpdateSale(Sale sale)
        {
            using (BooksContext context = new BooksContext())
            {
                Sale _sale = context.Sales.FirstOrDefault(p => p.ReceiptNumber == sale.ReceiptNumber);
                if (_sale != null )
                {
                    _sale.EmployeeId = sale.EmployeeId;
                    _sale.CustomerCardNumber = sale.CustomerCardNumber;
                    _sale.DateTime = sale.DateTime;
                    _sale.PaymentMethodId = sale.PaymentMethodId;
                    _sale.OutletId = sale.OutletId;
                    //_sale.Employee = sale.Employee;
                    //_sale.Outlet = sale.Outlet;
                    //_sale.PaymentMethod = sale.PaymentMethod;
                    //_sale.SoldProducts = sale.SoldProducts;
                    context.SaveChanges();
                }
            }
        }
    }
}
