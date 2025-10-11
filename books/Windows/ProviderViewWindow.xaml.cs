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
    /// Логика взаимодействия для ProviderViewWindow.xaml
    /// </summary>
    public partial class ProviderViewWindow : Window
    {
        Employee authorizedEmployee;
        public ProviderViewWindow(Employee employee)
        {
            InitializeComponent();
            UpdateProvidersListBox();
            authorizedEmployee = employee;
            if (employee.AccessLevel < 3)
            {
                openAddProviderFormButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                openAddProviderFormButton.Visibility = Visibility.Visible;
            }
        }
        public void UpdateProvidersListBox()
        {
            providersListBox.ItemsSource = null;
            providersListBox.ItemsSource = DatabaseControl.GetProvidersList();
        }

        private void openAddProviderFormButton_Click(object sender, RoutedEventArgs e)
        {
            AddProviderWindow addProviderWindow = new AddProviderWindow();
            addProviderWindow.Owner = this;
            addProviderWindow.Show();
        }

        private void providersListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (authorizedEmployee.AccessLevel < 3)
            {
                return;
            }
                Provider provider = providersListBox.SelectedItem as Provider;
            if (provider != null)
            {
                EditProviderWindow editProviderWindow = new EditProviderWindow(provider);
                editProviderWindow.Owner = this;
                editProviderWindow.Show();
            }
        }
    }
}
