using Npgsql;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace books
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        Employee authorizedEmployee;
        public int outletId;
        private Outlet currentOutlet;
        List<SelectedProduct> selectedProducts = new List<SelectedProduct>();
        public MainWindow(Employee _authorizedEmployee)
        {
            InitializeComponent();
            outletId = Properties.Settings.Default.outletId;
            ChangeCurrentOutlet();
            UpdateProductNamesSearchBar();
            //outletAddressViewTextBlock.Text = _outlet.OutletAddress;
            authorizedEmployee = _authorizedEmployee;
            //employeeNameViewTextBlock.Text = authorizedEmployee.FullName;
            if (authorizedEmployee.AccessLevel < 2)
            {
                openShipmentRegistrationFormButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                openShipmentRegistrationFormButton.Visibility = Visibility.Visible;
            }
            if (authorizedEmployee.AccessLevel < 3)
            {
                OpenSettingsButton.Content = "Выйти из аккаунта";
                openProviderInfoFormButton.Visibility = Visibility.Collapsed;
                openEmployeeInfoFormButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                OpenSettingsButton.Content = "Настройки";
                openProviderInfoFormButton.Visibility = Visibility.Visible;
                openEmployeeInfoFormButton.Visibility = Visibility.Visible;
            }
        }
        public void ChangeCurrentOutlet()
        {
            List<Outlet> outlets = DatabaseControl.GetOutletsList();
            foreach (Outlet outlet in outlets)
            {
                if (outlet.Id == outletId)
                {
                    currentOutlet = outlet;
                }
            }
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
                    if (productNameAutoCompleteBox.Text == null)
                    {
                        MessageBox.Show("Введите название товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    List<Product> products = DatabaseControl.GetProductsList();
                    List<Product> productInfo = new List<Product>();
                    bool productIsFound = false;
                    int productCount;
                    if (!Int32.TryParse(productCountTextBox.Text, out productCount)) { productCount = 1; }
                    foreach (Product product in products)
                    {
                        if (product.Name == productNameAutoCompleteBox.Text)
                        {
                            productIsFound = true;
                            if (selectedProducts.Count == 0) // если список полностью пустой
                            {
                                selectedProducts.Add(new SelectedProduct(product));
                                selectedProducts[selectedProducts.Count - 1].Count += productCount;
                                selectedProducts[selectedProducts.Count - 1].SalePrice = selectedProducts[selectedProducts.Count - 1].SalePrice * selectedProducts[selectedProducts.Count - 1].Count;
                            }
                            else
                            {
                                bool selectedProductIsFound = false;
                                for (int i = 0; i < selectedProducts.Count; i++)
                                {
                                    if (product.Id == selectedProducts[i].Id)
                                    {
                                        selectedProductIsFound = true;
                                        selectedProducts[i].SalePrice = selectedProducts[i].SalePrice / selectedProducts[i].Count;
                                        selectedProducts[i].Count+=productCount;
                                        selectedProducts[i].SalePrice = selectedProducts[i].SalePrice * selectedProducts[i].Count;
                                        break;
                                    }
                                }
                                if (!selectedProductIsFound)
                                {
                                    selectedProducts.Add(new SelectedProduct(product));
                                    selectedProducts[selectedProducts.Count - 1].Count += productCount;
                                    selectedProducts[selectedProducts.Count - 1].SalePrice = selectedProducts[selectedProducts.Count - 1].SalePrice * selectedProducts[selectedProducts.Count - 1].Count;
                                }
                            }
                            productInfo.Add(product);
                            UpdateProductInfoListBox(productInfo);
                            UpdateMainListBox();
                            break;
                        }
                    }
                    if (!productIsFound)
                    {
                        MessageBox.Show("Не найден товар с таким названием", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
            Decimal sumCost = 0;
            foreach (SelectedProduct selectedProduct in selectedProducts)
            {
               sumCost+= selectedProduct.SalePrice;
            }
            sumCostViewTextBlock.Text = "Суммарная стоимость: "+sumCost.ToString();
        }

        private void openShipmentRegistrationFormButton_Click(object sender, RoutedEventArgs e)
        {
            ShipmentRegistrationWindow shipmentRegistrationWindow = new ShipmentRegistrationWindow(outletId, authorizedEmployee);
            shipmentRegistrationWindow.Owner = this;
            shipmentRegistrationWindow.Show();
        }
        public static bool ConfirmationDialog(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Подтверждение действия", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void openOutletProductsInfoFormButton_Click(object sender, RoutedEventArgs e)
        {
            OutletProductsViewWindow outletProductsViewWindow = new OutletProductsViewWindow(outletId, authorizedEmployee);
            outletProductsViewWindow.Owner = this;
            outletProductsViewWindow.UpdateOutletProductsList(outletId);
            outletProductsViewWindow.Show();
        }
        private string GetClienCardNumber()
        {
            string cardNumber = Interaction.InputBox("Введите номер карты");
            if (cardNumber.Length == 0)
            {
                return "Отмена";
            }
            cardNumber = cardNumber.Replace(" ", "");
            if (cardNumber.Length > 19 || cardNumber.Length < 16)
            {
                MessageBox.Show("Произошла ошибка распознавания номера карты, пожалуйста проверьте правильность ввода.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return GetClienCardNumber();
            }
            return cardNumber;
        }
        private void ConfirmSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmationDialog("Подтвердить продажу?"))
            {
                if (selectedProducts == null)
                {
                    MessageBox.Show("Произошла ошибка! Не выбраны товары для продажи.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Sale sale = new Sale();
                MessageBoxResult isPaymentInCashless = MessageBox.Show("Оплата по карте?", "Выбор метода оплаты", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (isPaymentInCashless == MessageBoxResult.Yes)
                {
                    string cardNumber = GetClienCardNumber();
                    if (cardNumber == "Отмена")
                    {
                        return;
                    }
                    sale.CustomerCardNumber = cardNumber;
                    sale.PaymentMethodId = 2;
                }
                else
                {
                    sale.PaymentMethodId = 1;
                }
                sale.EmployeeId = authorizedEmployee.Id;
                sale.OutletId = outletId;
                List<OutletProduct> outletProducts = DatabaseControl.GetOutletProducts();
                string fullPath = System.IO.Path.Combine(Environment.CurrentDirectory, "SaleCheck.docx");
                if (!ClearWordprocessingDocument(fullPath))
                {
                    return;
                }
                foreach (SelectedProduct selectedProduct in selectedProducts)
                {
                    bool outletProductNotFound = true;
                    foreach (OutletProduct outletProduct in outletProducts)
                    {
                        if (selectedProduct.Id == outletProduct.ProductId)
                        {
                            outletProductNotFound = false;
                            if (outletProduct.Quantity < selectedProduct.Count)
                            {
                                MessageBox.Show("Внимание! Добавленное число товаров '"+selectedProduct.Name+"' превышает количество товаров на торговой точке. Убедитесь, что выбрано правильное число товаров. Если все заполнено верно, свяжитесь с администратором.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                                if (ConfirmationDialog("Отменить операцию?"))
                                {
                                    return;
                                }
                                break;
                            }
                        }
                    }
                    if (outletProductNotFound)
                    {
                        MessageBox.Show("Произошла ошибка! Товара '"+selectedProduct.Name+"' нет в наличии на складе в базе данных. Сообщите об этом администратору", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        if (ConfirmationDialog("Отменить операцию?"))
                        {
                            return;
                        }
                    }
                }
                DatabaseControl.AddSale(sale);
                CreateReceiptFirstPart(fullPath, sale);
                foreach (SelectedProduct selectedProduct in selectedProducts)
                {
                    OpenAndAddTextToWordDocument(fullPath, selectedProduct.Name, "28");
                    OpenAndAddTextToWordDocument(fullPath, "Количество: "+selectedProduct.Count.ToString(), "28");
                    OpenAndAddTextToWordDocument(fullPath, "Цена: "+(selectedProduct.Count*selectedProduct.SalePrice).ToString(), "28");
                    OpenAndAddTextToWordDocument(fullPath, "-------------------------------------", "28");
                    SoldProduct soldProduct = new SoldProduct();
                    soldProduct.ProductId = selectedProduct.Id;
                    soldProduct.SaleId = sale.ReceiptNumber;
                    soldProduct.Quantity = selectedProduct.Count;
                    DatabaseControl.AddSoldProduct(soldProduct);
                    bool outletProductNotFound = true;
                    foreach (OutletProduct outletProduct in outletProducts)
                    {
                        if (outletProduct.ProductId == selectedProduct.Id)
                        {
                            outletProductNotFound = false;
                            outletProduct.Quantity-=selectedProduct.Count;
                            DatabaseControl.UpdateOutletProduct(outletProduct);
                            break;
                        }
                    }
                    if (outletProductNotFound)
                    {
                        MessageBox.Show("Произошла ошибка! Товара '" + selectedProduct.Name + "' нет в наличии на складе в базе данных. Сообщите об этом администратору", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                OpenAndAddTextToWordDocument(fullPath, "Общая сумма: "+ PresentableSale.CalculateSumm(sale.ReceiptNumber).ToString(), "36");
                OpenAndAddTextToWordDocument(fullPath, "******************************************************", "36");
                OpenAndAddTextToWordDocument(fullPath, "Спасибо за покупку!", "48");
                selectedProducts = new List<SelectedProduct>();
                UpdateMainListBox();
                UpdateProductInfoListBox(new List<Product>());
                sumCostViewTextBlock.Text = "";
                MessageBox.Show("Операция завершена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.None);
            }
        }
        void CreateReceiptFirstPart(string fullPath, Sale sale)
        {
            OpenAndAddTextToWordDocument(fullPath, "Кассовый чек номер: " + sale.ReceiptNumber, "48");
            OpenAndAddTextToWordDocument(fullPath, "******************************************************", "36");
            OpenAndAddTextToWordDocument(fullPath, "Дата совершения операции: " + sale.DateTime.ToString(), "36"); 
            string paymentMethod = "";
            switch (sale.PaymentMethodId)
            {
                case 1:
                    paymentMethod = "Наличная оплата";
                    break;
                case 2:
                    paymentMethod = "Безналичная оплата";
                    string cardNumber = sale.CustomerCardNumber.Replace(" ", "");
                    cardNumber = cardNumber.Remove(0, cardNumber.Length - 4);
                    if (!Int32.TryParse(cardNumber, out int number))
                    {
                        MessageBox.Show("Произошла ошибка чтения номера карты. Проверьте прошла ли оплата", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    OpenAndAddTextToWordDocument(fullPath, "Номер карты: " + cardNumber, "36");
                    break;
            }
            OpenAndAddTextToWordDocument(fullPath, "Адрес: " + currentOutlet.OutletAddress, "36");
            OpenAndAddTextToWordDocument(fullPath, "Метод оплаты: " + paymentMethod, "36");
            OpenAndAddTextToWordDocument(fullPath, "******************************************************", "36");
            OpenAndAddTextToWordDocument(fullPath, "Список товаров:", "36");
            OpenAndAddTextToWordDocument(fullPath, "******************************************************", "36");
        }
        static void OpenAndAddTextToWordDocument(string filepath, string txt, string fSize)
        {
            // Open a WordprocessingDocument for editing using the filepath.
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(filepath, true))
            {

                if (wordprocessingDocument is null)
                {
                    throw new ArgumentNullException(nameof(wordprocessingDocument));
                }

                // Assign a reference to the existing document body.
                MainDocumentPart mainDocumentPart = wordprocessingDocument.MainDocumentPart ?? wordprocessingDocument.AddMainDocumentPart();
                mainDocumentPart.Document ??= new Document();
                mainDocumentPart.Document.Body ??= mainDocumentPart.Document.AppendChild(new Body());
                Body body = wordprocessingDocument.MainDocumentPart!.Document!.Body!;

                // Add new text.
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                RunProperties properties = new RunProperties();
                run.Append(properties);
                FontSize fontSize = new FontSize();
                fontSize.Val = fSize;
                properties.Append(fontSize);
                run.AppendChild(new Text(txt));
            }
        }
        static bool ClearWordprocessingDocument(string filepath)
        {
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch {
                MessageBox.Show("Ошибка! Невозможно создать чек. Пожалуйста закройте Word файл, для создания чека.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            // Create a document by supplying the filepath. 
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filepath, WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                return true;
            }
        }
        private void mainListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedProduct product = mainListBox.SelectedItem as SelectedProduct;
            if (product != null)
            {
                if (!ConfirmationDialog("Удалить '"+product.Name+"' из списка?"))
                {
                    return;
                }
                selectedProducts = selectedProducts.Where(item => item.Id != product.Id).ToList();
                UpdateMainListBox();
                UpdateProductInfoListBox(new List<Product>());
            }
        }

        private void openSalesInfoFormButton_Click(object sender, RoutedEventArgs e)
        {
            SalesViewWindow salesViewWindow = new SalesViewWindow(authorizedEmployee, outletId);
            salesViewWindow.Owner = this;
            salesViewWindow.Show();
        }

        private void openEmployeeInfoFormButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeViewWindow employeeViewWindow = new EmployeeViewWindow(authorizedEmployee);
            employeeViewWindow.Owner = this;
            employeeViewWindow.Show();
        }

        private void openProviderInfoFormButton_Click(object sender, RoutedEventArgs e)
        {
            ProviderViewWindow providerViewWindow = new ProviderViewWindow(authorizedEmployee);
            providerViewWindow.Owner = this;
            providerViewWindow.Show();
        }

        private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (authorizedEmployee.AccessLevel < 3)
            {
                AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                authorizationWindow.Show();
                this.Close();
            }
            else
            {
                SettingsWindow settingsWindow = new SettingsWindow(authorizedEmployee);
                settingsWindow.Owner = this;
                settingsWindow.Show();
            }
        }
    }
}