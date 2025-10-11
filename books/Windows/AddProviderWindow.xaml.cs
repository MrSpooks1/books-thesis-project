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
    /// Логика взаимодействия для AddProviderWindow.xaml
    /// </summary>
    public partial class AddProviderWindow : Window
    {
        public AddProviderWindow()
        {
            InitializeComponent();
        }

        private void AddProviderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MainWindow.ConfirmationDialog("Зарегистрировать нового поставщика с введенными данными?"))
                {
                    return;
                }
                if (string.IsNullOrEmpty(nameInputTextBox.Text) || string.IsNullOrEmpty(providerTypeInputComboBox.Text))
                {
                    MessageBox.Show("Пожалуйста введите имя поставщика и выберите его тип.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if ((phoneNumberInputTextBox.Text.Replace(" ", "").Length > 12) || (postalCodeInputTextBox.Text.Replace(" ", "").Length > 6) || (emailInputTextBox.Text.Replace(" ", "").Length > 40))
                {
                    MessageBox.Show("Ошибка! Размер одного из полей превышает допустимые значения. Проверьте правильность заполнения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Provider newProvider = new Provider();
                newProvider.Name = nameInputTextBox.Text;
                newProvider.EmailAddress = emailInputTextBox.Text;
                newProvider.PhoneNumber = phoneNumberInputTextBox.Text.Replace(" ", "");
                newProvider.PostalCode = postalCodeInputTextBox.Text.Replace(" ", "");
                newProvider.ProviderTypeId = providerTypeInputComboBox.SelectedIndex + 1;
                DatabaseControl.AddProvider(newProvider);
                (this.Owner as ProviderViewWindow).UpdateProvidersListBox();
                this.Close();
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Произошла ошибка при регистрации нового поставщика! Проверьтре правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
