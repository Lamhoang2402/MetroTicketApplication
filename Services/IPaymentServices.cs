using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IPaymentServices
    {
        string CreatePaymentRequest(string orderId, decimal amount, string ipAddress = "127.0.0.1", string bankCode = "");
        PaymentResponseModel GetFullResponseData(System.Collections.Specialized.NameValueCollection queryParams);
    }
}