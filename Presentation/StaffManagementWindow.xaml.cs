using BusinessObjects;
using Services;
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

namespace Presentation
{
    /// <summary>
    /// Interaction logic for StaffManagementWindow.xaml
    /// </summary>
    public partial class StaffManagementWindow : Window
    {
        User _user;
        private IUserServices userServices = null;
        public StaffManagementWindow(User user)
        {
            InitializeComponent();
            userServices = new UserServices();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }

        private void loadDataInit()
        {
            dtgAllStaff.ItemsSource = userServices.GetUsers();

            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtEmail.Text = string.Empty;
        }

        private void dtgAllStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedUser = dtgAllStaff.SelectedItem as User;
            if (selectedUser != null)
            {
                txtUsername.Text = selectedUser.Username;
                txtPassword.Password = selectedUser.Password;
                txtEmail.Text = selectedUser.Email;
            }
        }

        private bool validateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Valid email is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            User dbUser = userServices.GetUsers().FirstOrDefault(u => u.Username.Equals(txtUsername.Text.Trim(), StringComparison.OrdinalIgnoreCase));
            if (dbUser != null && dbUser.RoleId != 2)
            {
                MessageBox.Show("Admin is not allowed to modify any roles other than staff.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!validateForm())
            {
                return;
            }

            User user = new User();

            user.UserId = userServices.GetLastUser().UserId + 1;
            user.Username = txtUsername.Text;
            user.Password = txtPassword.Password.ToString();
            user.Email = txtEmail.Text;
            user.RoleId = 2;

            if (userServices.AddUser(user))
            {
                MessageBox.Show("User added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Failed to add user. Username may already exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!validateForm())
            {
                return;
            }

            User user = dtgAllStaff.SelectedItem as User;
            if (user == null)
            {
                MessageBox.Show("Please select a user to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (userServices.DeleteUser(user.UserId))
                {
                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    loadDataInit();
                }
                else
                {
                    MessageBox.Show("Failed to delete user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!validateForm())
            {
                return;
            }

            User user = dtgAllStaff.SelectedItem as User;
            if (user == null)
            {
                MessageBox.Show("Please select a user to update.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            user.Username = txtUsername.Text.Trim();
            user.Password = txtPassword.Password.Trim();
            user.Email = txtEmail.Text.Trim();

            try
            {
                if (userServices.UpdateUser(user))
                {
                    MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    loadDataInit();
                }
                else
                {
                    MessageBox.Show("Failed to update user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }
    }
}