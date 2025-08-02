using System.Windows;

namespace Presentation
{
    public partial class PaymentConfirmationWindow : Window
    {
        public PaymentConfirmationWindow(string bank, string cardNumber, string cardHolderName, decimal price)
        {
            InitializeComponent();

            lblBank.Content = bank;
            lblCardNumber.Content = cardNumber;
            lblCardHolderName.Content = cardHolderName;
            lblPrice.Content = price.ToString("C");
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}