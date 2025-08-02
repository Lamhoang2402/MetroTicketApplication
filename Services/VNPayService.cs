using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Services
{
    public class VNPayService : IPaymentServices
    {
        private const string VnpTmnCode = "U684QP4U";
        private const string VnpHashSecret = "G412MC10XXIPJ3F39MN9TMQ2PAQ9HT9L";
        private const string VnpPaymentUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private const string ReturnUrl = "http://localhost:3000/vnpay-return/";

        public string CreatePaymentRequest(string orderId, decimal amount, string ipAddress = "127.0.0.1", string bankCode = "")
        {
            var vnpayParams = new Dictionary<string, string>
            {
                {"vnp_Version", "2.1.0"},
                {"vnp_Command", "pay"},
                {"vnp_TmnCode", VnpTmnCode},
                {"vnp_Amount", ((long)(amount * 100)).ToString()},
                {"vnp_CurrCode", "VND"},
                {"vnp_TxnRef", orderId},
                {"vnp_OrderInfo", "Thanh toan don hang #" + orderId},
                {"vnp_OrderType", "other"},
                {"vnp_Locale", "vn"},
                {"vnp_ReturnUrl", ReturnUrl},
                {"vnp_IpAddr", ipAddress}
            };

            var createDate = DateTime.Now;
            vnpayParams.Add("vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss"));
            vnpayParams.Add("vnp_ExpireDate", createDate.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            if (!string.IsNullOrEmpty(bankCode))
            {
                vnpayParams.Add("vnp_BankCode", bankCode);
            }

            vnpayParams = vnpayParams.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);

            var hashData = string.Join("&",
                vnpayParams.Where(p => !string.IsNullOrEmpty(p.Value))
                           .Select(p => p.Key + "=" + p.Value));

            string secureHash = HmacSha512(VnpHashSecret, hashData);

            var queryString = string.Join("&",
                vnpayParams.Select(p => HttpUtility.UrlEncode(p.Key) + "=" + HttpUtility.UrlEncode(p.Value)));

            return $"{VnpPaymentUrl}?{queryString}&vnp_SecureHash={secureHash}";
        }

        public PaymentResponseModel GetFullResponseData(NameValueCollection queryParams)
        {
            var receivedParams = queryParams.AllKeys
                .Where(k => k != null)
                .ToDictionary(k => k, k => queryParams[k]);

            if (!receivedParams.ContainsKey("vnp_SecureHash"))
            {
                return new PaymentResponseModel { Success = false, Message = "Hash not found in response." };
            }

            string receivedHash = receivedParams["vnp_SecureHash"];
            receivedParams.Remove("vnp_SecureHash");

            var sortedParams = new SortedDictionary<string, string>(receivedParams, StringComparer.Ordinal);

            var hashData = string.Join("&",
                sortedParams.Where(p => !string.IsNullOrEmpty(p.Value))
                            .Select(p => p.Key + "=" + p.Value));

            string localHash = HmacSha512(VnpHashSecret, hashData);

            if (!localHash.Equals(receivedHash, StringComparison.OrdinalIgnoreCase))
            {
                return new PaymentResponseModel { Success = false, Message = "Hash validation failed. Please check your HashSecret." };
            }

            if (receivedParams.TryGetValue("vnp_ResponseCode", out var responseCode) && responseCode != "00")
            {
                return new PaymentResponseModel { Success = false, Message = "Payment was not successful.", VnPayResponseCode = responseCode };
            }

            return new PaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderId = receivedParams.GetValueOrDefault("vnp_TxnRef"),
                TransactionId = receivedParams.GetValueOrDefault("vnp_TransactionNo"),
                OrderDescription = receivedParams.GetValueOrDefault("vnp_OrderInfo"),
                VnPayResponseCode = receivedParams.GetValueOrDefault("vnp_ResponseCode"),
                Message = "Payment successful."
            };
        }

        private string HmacSha512(string key, string data)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var dataBytes = Encoding.UTF8.GetBytes(data);
                var hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }

    public class PaymentResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public string OrderDescription { get; set; }
        public string PaymentMethod { get; set; }
        public string VnPayResponseCode { get; set; }
    }
}