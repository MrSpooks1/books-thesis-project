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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(loginInputTextBox.Text) || string.IsNullOrEmpty(passwordInputTextBox.Password)))
            {
                if (!(loginInputTextBox.Text.Length > 30 || passwordInputTextBox.Password.Length > 20))
                {
                    List<Employee> employees = DatabaseControl.GetEmployeesList();
                    foreach (Employee employee in employees)
                    {
                        string password = passwordInputTextBox.Password;
                        if (employee.Id != 1 && employee.Id != 2)
                        {
                            password = Hashing.Hash(password);
                        }
                        if (loginInputTextBox.Text == employee.Login && password == employee.Password)
                        {
                            MainWindow mainWindow = new MainWindow(employee);
                            mainWindow.Show();
                            this.Close();
                            return;
                        }
                    }

                }
                MessageBox.Show("Неправильный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            else
            {
                MessageBox.Show("Заполнены не все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
