using BusinessObjects;
using Dao;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for TicketManagement.xaml
    /// </summary>
    public partial class TicketCheckoutWindow : Window
    {
        User _user;
        ITicketPricesServices _ticketPricesServices;
        ITicketServices ticketServices;

        public TicketCheckoutWindow(User user)
        {
            InitializeComponent();
            _ticketPricesServices = new TicketPricesService();
            ticketServices = new TicketServices();
            QuestPDF.Settings.License = LicenseType.Community;
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
        }


        private void dtgAllTicket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTicket = dtgAllTicket.SelectedItem as TicketPrice;
            if (selectedTicket != null)
            {
                cmbTicketType.SelectedValue = selectedTicket.TicketTypeId;
                cmbCustomerType.SelectedValue = selectedTicket.CustomerTypeId;
                lblTotalPrice.Content = "Total Price: " + selectedTicket.Price.ToString("C");
            }
        }

        private bool validateForm()
        {
            if (cmbCustomerType.SelectedItem == null)
            {
                MessageBox.Show("Please select a Customer Type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (cmbTicketType.SelectedItem == null)
            {
                MessageBox.Show("Please select a Ticket Type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if(!validateForm())
            {
                return;
            }

            var selectedTicket = dtgAllTicket.SelectedItem as TicketPrice;

            string bank = "NCB";
            string cardNumber = "9704198526191432198";
            string cardHolderName = "NGUYEN VAN A";
            decimal totalPrice = decimal.Parse(selectedTicket.Price.ToString());

            var paymentWindow = new PaymentConfirmationWindow(bank, cardNumber, cardHolderName, totalPrice);
            bool? result = paymentWindow.ShowDialog();

            if (result == true)
            {
                MessageBox.Show("Payment confirmed!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);

                Ticket ticket = new Ticket();
                ticket.TicketId = Guid.NewGuid().ToString();
                ticket.PurchasedAt = DateTime.Now;
                ticket.Status = "Active";
                ticket.TicketPriceId = (dtgAllTicket.SelectedItem as TicketPrice).TicketPriceId;
              
                ticketServices.AddTicket(ticket);
            }
            else
            {
                MessageBox.Show("Payment was canceled.", "Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnGenerateTicket_Click(object sender, RoutedEventArgs e)
        {
            Ticket lastTicket = ticketServices.GetLastTicket();

            if (lastTicket == null)
            {
                MessageBox.Show("No tickets found to generate.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            TicketPrice ticketPrice = _ticketPricesServices.GetTicketPriceById(lastTicket.TicketPriceId);

            if (ticketPrice == null)
            {
                MessageBox.Show("Ticket price information not found for the last ticket.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string pdfFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"metro_ticket_{lastTicket.TicketId}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(20);
                    page.Content().Padding(10).Column(column =>
                    {
                        column.Spacing(10);
                        column.Item().Text("Metro Ticket").FontSize(24).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                        column.Item().LineHorizontal(1);

                        column.Item().Text($"Ticket ID: {lastTicket.TicketId}").FontSize(12);
                        column.Item().Text($"Ticket Type: {ticketPrice.TicketType.TicketTypeName}").FontSize(12);
                        column.Item().Text($"Customer Type: {ticketPrice.CustomerType.TypeName}").FontSize(12);
                        column.Item().Text($"Price: {ticketPrice.Price.ToString("C")}").FontSize(16).Bold().FontColor(QuestPDF.Helpers.Colors.Red.Darken2);
                        column.Item().Text($"Purchase Date: {lastTicket.PurchasedAt:g}").FontSize(12);

                        column.Item().PaddingTop(20).Text("Thank you for your purchase!").FontSize(12).Italic();
                    });
                });
            })
            .GeneratePdf(pdfFilePath);

            Process.Start(new ProcessStartInfo(pdfFilePath) { UseShellExecute = true });

            MessageBox.Show("Ticket PDF generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Closed += (sender, args) =>
            {
                if (File.Exists(pdfFilePath))
                {
                    try
                    {
                        File.Delete(pdfFilePath);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Could not delete the temporary PDF file. Please close it manually. Error: {ex.Message}", "File Deletion Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            };
        }
    }
}
