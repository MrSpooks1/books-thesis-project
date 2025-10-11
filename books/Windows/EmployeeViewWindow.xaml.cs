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
    /// Логика взаимодействия для EmployeeViewWindow.xaml
    /// </summary>
    public partial class EmployeeViewWindow : Window
    {
        Employee authorizedEmployee;
        List<Employee> employees;
        public EmployeeViewWindow(Employee employee)
        {
            InitializeComponent();
            authorizedEmployee = employee;
            if (employee.AccessLevel < 3)
            {
                openAddEmployeeFormButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                openAddEmployeeFormButton.Visibility = Visibility.Visible;
            }
            UpdateEmployeeListBox();
        }
        public void UpdateEmployeeListBox()
        {
            employeeListBox.ItemsSource = null;
            employees = DatabaseControl.GetEmployeesList();
            List<PresentableEmployee> presentableEmployees = new List<PresentableEmployee>();
            foreach (Employee employee in employees)
            {
                presentableEmployees.Add(new PresentableEmployee(employee));
            }
            employeeListBox.ItemsSource = presentableEmployees;
        }

        private void openAddEmployeeFormButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.Owner = this;
            addEmployeeWindow.Show();
        }

        private void employeeListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (authorizedEmployee.AccessLevel < 3)
            {
                return;
            }
            PresentableEmployee selectedEmployee = employeeListBox.SelectedItem as PresentableEmployee;
            if (selectedEmployee != null)
            {
                foreach (Employee employee in employees)
                {
                    if (employee.Id == selectedEmployee.Id)
                    {
                        EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(employee);
                        editEmployeeWindow.Owner = this;
                        editEmployeeWindow.Show();
                    }
                }
            }
        }
    }
}
