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
    /// Логика взаимодействия для EditProviderWindow.xaml
    /// </summary>
    public partial class EditProviderWindow : Window
    {
        Provider selectedProvider;
        public EditProviderWindow(Provider provider)
        {
            InitializeComponent();
            selectedProvider = provider;
            nameInputTextBox.Text = selectedProvider.Name;
            providerTypeInputComboBox.SelectedIndex = selectedProvider.ProviderTypeId - 1;
            if (selectedProvider.EmailAddress != null)
            {
                emailInputTextBox.Text = selectedProvider.EmailAddress;
            }
            if (selectedProvider.PhoneNumber != null)
            {
                phoneNumberInputTextBox.Text = selectedProvider.PhoneNumber;
            }
            if (selectedProvider.PostalCode != null)
            {
                postalCodeInputTextBox.Text = selectedProvider.PostalCode;
            }
        }

        private void EditProviderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MainWindow.ConfirmationDialog("Вы уверены, что хотите изменить данные о поставщике?"))
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
                selectedProvider.Name = nameInputTextBox.Text;
                selectedProvider.ProviderTypeId = providerTypeInputComboBox.SelectedIndex + 1;
                if (!(string.IsNullOrEmpty(emailInputTextBox.Text)))
                { 
                    selectedProvider.EmailAddress = emailInputTextBox.Text;
                }
                if (!(string.IsNullOrEmpty(postalCodeInputTextBox.Text)))
                {
                    selectedProvider.PostalCode = postalCodeInputTextBox.Text.Replace(" ", "");
                }
                if (!(string.IsNullOrEmpty(phoneNumberInputTextBox.Text)))
                {
                    selectedProvider.PhoneNumber = phoneNumberInputTextBox.Text.Replace(" ", "");
                }
                DatabaseControl.UpdateProvider(selectedProvider);
                (this.Owner as ProviderViewWindow).UpdateProvidersListBox();
                this.Close();
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Произошла ошибка при изменении информации о поставщике! Проверьтре правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
