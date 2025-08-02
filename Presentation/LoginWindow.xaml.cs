using BusinessObjects;
using Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private IUserServices userServices = null;

        public LoginWindow()
        {
            InitializeComponent();
            userServices = new UserServices();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            User user = userServices.Login(txtUsername.Text, txtPassword.Password);
            if(user != null)
            {
                if(user.RoleId == 1)
                {
                    MessageBox.Show("Welcome Manager!");
                    TicketManagement ticketManagement = new TicketManagement(user);
                    ticketManagement.Show();
                    this.Close();
                }
                else if(user.RoleId == 2)
                {
                    MessageBox.Show("Welcome Staff!");
                    TicketCheckoutWindow ticketCheckoutWindow = new TicketCheckoutWindow(user);
                    ticketCheckoutWindow.Show();
                    this.Close();
                }
                else if (user.RoleId == 3)
                {
                    MessageBox.Show("Welcome Admin!");
                    StaffManagementWindow staffManagementWindow = new StaffManagementWindow(user);
                    staffManagementWindow.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
    }
}