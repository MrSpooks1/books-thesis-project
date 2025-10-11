using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {

        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MainWindow.ConfirmationDialog("Зарегистрировать нового сотрудника с введенными данными?"))
                {
                    return;
                }
                if (string.IsNullOrEmpty(fullNameInputTextBox.Text) || string.IsNullOrEmpty(positionInputComboBox.Text) || string.IsNullOrEmpty(phoneNumberInputTextBox.Text) || string.IsNullOrEmpty(passportInputTextBox.Text) || string.IsNullOrEmpty(loginInputTextBox.Text) || string.IsNullOrEmpty(passwordInputTextBox.Text))
                {
                    MessageBox.Show("Заполнены не все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if ((phoneNumberInputTextBox.Text.Replace(" ", "").Length > 12) || (passportInputTextBox.Text.Replace(" ", "").Length > 10) || (passwordInputTextBox.Text.Replace(" ", "").Length > 20) || (loginInputTextBox.Text.Replace(" ", "").Length > 30))
                {
                    MessageBox.Show("Ошибка! Размер одного из полей превышает допустимые значения (Логин: 30 символов; Пароль: 20 символов).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Employee newEmployee = new Employee();
                newEmployee.FullName = fullNameInputTextBox.Text;
                newEmployee.AccessLevel = positionInputComboBox.SelectedIndex+1;
                newEmployee.PhoneNumber = phoneNumberInputTextBox.Text.Replace(" ", "");
                newEmployee.PassportSerialNumber = passportInputTextBox.Text.Replace(" ", "");
                newEmployee.Login = loginInputTextBox.Text;
                newEmployee.Password =  Hashing.Hash(passwordInputTextBox.Text);
                DatabaseControl.AddEmployee(newEmployee);
                (this.Owner as EmployeeViewWindow).UpdateEmployeeListBox();
                this.Close();
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Произошла ошибка при регистрации нового сотрудника! Проверьтре правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }

}
