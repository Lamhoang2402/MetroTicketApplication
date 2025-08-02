using BusinessObjects;
using Dao;
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
    /// Interaction logic for TicketManagement.xaml
    /// </summary>
    public partial class TicketManagement : Window
    {
        User _user;
        ITicketPricesServices _ticketPricesServices;
        public TicketManagement(User user)
        {
            InitializeComponent();
            _ticketPricesServices = new TicketPricesService();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }
        private void loadDataInit()
        {

            _ticketPricesServices = new TicketPricesService();

            dtgAllTicket.ItemsSource = _ticketPricesServices.GetTicketPrices();

            cmbTicketType.ItemsSource = _ticketPricesServices.GetTicketTypes();
            cmbTicketType.DisplayMemberPath = "TicketTypeName";
            cmbTicketType.SelectedValuePath = "TicketTypeId";

            cmbCustomerType.ItemsSource = _ticketPricesServices.GetCustomerTypes();
            cmbCustomerType.DisplayMemberPath = "TypeName";
            cmbCustomerType.SelectedValuePath = "CustomerTypeId";

            txtPriceId.Text = "";
            txtPrice.Text = "";
        }


        private void dtgAllTicket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTicket = dtgAllTicket.SelectedItem as TicketPrice;
            if (selectedTicket != null)
            {
                txtPriceId.Text = selectedTicket.TicketPriceId.ToString();
                txtPrice.Text = selectedTicket.Price.ToString();
                cmbTicketType.SelectedValue = selectedTicket.TicketTypeId;
                cmbCustomerType.SelectedValue = selectedTicket.CustomerTypeId;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!validateForm(false, 0)) return;
            TicketPrice newTicket = new TicketPrice();
            try
            {
                newTicket.TicketPriceId = int.Parse(txtPriceId.Text);
                newTicket.TicketTypeId = int.Parse(cmbTicketType.SelectedValue.ToString());
                newTicket.CustomerTypeId = int.Parse(cmbCustomerType.SelectedValue.ToString());
                newTicket.Price = int.Parse(txtPrice.Text);
                if (_ticketPricesServices.AddTicketPrice(newTicket))
                {
                    MessageBox.Show("Add successfully!");
                    loadDataInit();
                }
                else
                {
                    MessageBox.Show("Add failed! ID already exists or DB error.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var SelectedTicket = dtgAllTicket.SelectedItem as TicketPrice;

            if (SelectedTicket == null)
            {
                MessageBox.Show("Please select a ticket to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var result = MessageBox.Show("Are you sure you want to delete this ticket?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            _ticketPricesServices = new TicketPricesService();

            bool isDeleted = _ticketPricesServices.Delete(SelectedTicket.TicketPriceId);


            if (isDeleted)
            {
                MessageBox.Show("Ticket deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Failed to delete ticket.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool validateForm(bool isUpdating, int currentTicketPriceId)
        {
            if (string.IsNullOrEmpty(txtPriceId.Text))
            {
                MessageBox.Show("Ticket Price ID cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("Ticket Price cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (cmbCustomerType.SelectedItem == null)
            {
                MessageBox.Show("Please select a Customer Type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (cmbTicketType.SelectedItem == null)
            {
                MessageBox.Show("Please select a Ticket Type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            int selectedCustomerTypeId = (int)cmbCustomerType.SelectedValue;
            int selectedTicketTypeId = (int)cmbTicketType.SelectedValue;
            using (var context = new MetroTicketContext())
            {
                var existing = context.TicketPrices.FirstOrDefault(tp =>
                    tp.CustomerTypeId == selectedCustomerTypeId &&
                    tp.TicketTypeId == selectedTicketTypeId
                );

                if (existing != null)
                {
                   
                    if (isUpdating && existing.TicketPriceId != currentTicketPriceId)
                    {
                        MessageBox.Show("Another entry with this Customer Type + Ticket Type already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    
                    else if (!isUpdating)
                    {
                        MessageBox.Show("This Customer Type + Ticket Type combination already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (!int.TryParse(txtPriceId.Text, out id))
            {
                MessageBox.Show("Invalid Ticket Price ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!validateForm(isUpdating: true, currentTicketPriceId: id)) return;
            TicketPrice newTicket = new TicketPrice();
            try
            {
                newTicket.TicketPriceId = int.Parse(txtPriceId.Text);
                newTicket.TicketTypeId = int.Parse(cmbTicketType.SelectedValue.ToString());
                newTicket.CustomerTypeId = int.Parse(cmbCustomerType.SelectedValue.ToString());
                newTicket.Price = int.Parse(txtPrice.Text);
                if (_ticketPricesServices.UpdateTicketPrice(newTicket))
                {
                    MessageBox.Show("Update successfully!");
                    loadDataInit();
                }
                else
                {
                    MessageBox.Show("Update failed! ID does not exist or DB error.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
