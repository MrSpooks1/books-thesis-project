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
    /// Логика взаимодействия для EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        Employee selectedEmployee;
        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            selectedEmployee = employee;
            fullNameInputTextBox.Text = selectedEmployee.FullName;
            phoneNumberInputTextBox.Text = selectedEmployee.PhoneNumber;
            passportInputTextBox.Text = selectedEmployee.PassportSerialNumber;
            positionInputComboBox.SelectedIndex = selectedEmployee.AccessLevel - 1;
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MainWindow.ConfirmationDialog("Сохранить изменения в данных сотрудника?"))
                {
                    return;
                }
                if (string.IsNullOrEmpty(fullNameInputTextBox.Text) || string.IsNullOrEmpty(positionInputComboBox.Text) || string.IsNullOrEmpty(phoneNumberInputTextBox.Text) || string.IsNullOrEmpty(passportInputTextBox.Text))
                {
                    MessageBox.Show("Заполнены не все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if ((phoneNumberInputTextBox.Text.Replace(" ", "").Length > 12) || (passportInputTextBox.Text.Replace(" ", "").Length > 10))
                {
                    MessageBox.Show("Ошибка! Размер одного из полей превышает допустимые значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                selectedEmployee.FullName = fullNameInputTextBox.Text;
                selectedEmployee.PhoneNumber = phoneNumberInputTextBox.Text.Replace(" ", "");
                selectedEmployee.PassportSerialNumber = passportInputTextBox.Text.Replace(" ", "");
                selectedEmployee.AccessLevel = positionInputComboBox.SelectedIndex + 1;
                DatabaseControl.UpdateEmployee(selectedEmployee);
                (this.Owner as EmployeeViewWindow).UpdateEmployeeListBox();
                this.Close();
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Произошла ошибка при изменении данных сотрудника! Проверьтре правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
