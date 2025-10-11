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
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        List<Outlet> outlets = DatabaseControl.GetOutletsList();
        int employeeAccessLevel;
        public SettingsWindow(Employee authorizedEmployee)
        {
            InitializeComponent();
            employeeAccessLevel = authorizedEmployee.AccessLevel;
            if (employeeAccessLevel < 3)
            {
                outletSelectionStackPanel.Visibility = Visibility.Collapsed;
                SaveSettingsButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                outletSelectionStackPanel.Visibility= Visibility.Visible;
                SaveSettingsButton.Visibility = Visibility.Visible;
            }
            List<string> outletNames = new List<string>();
            int startingOutletIndex = 0;
            for (int i = 0; i < outlets.Count; i++)
            {
                outletNames.Add(outlets[i].OutletAddress);
                if (outlets[i].Id == Properties.Settings.Default.outletId)
                {
                    startingOutletIndex = i;
                }
            }
            outletSelectionComboBox.ItemsSource = outletNames;
            outletSelectionComboBox.SelectedIndex = startingOutletIndex;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeeAccessLevel >= 3)
            {
                foreach (Outlet outlet in outlets)
                {
                    if (outletSelectionComboBox.Text == outlet.OutletAddress)
                    {
                        Properties.Settings.Default.outletId = outlet.Id;
                        (this.Owner as MainWindow).outletId = outlet.Id;
                        (this.Owner as MainWindow).ChangeCurrentOutlet();
                        Properties.Settings.Default.Save();
                        break;
                    }
                }
            }
            this.Close();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            (this.Owner as MainWindow).Close();
            authorizationWindow.Show();
            this.Close();
        }
    }
}
