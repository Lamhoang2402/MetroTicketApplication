using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services
{
    public class VNPayHttpListener
    {
        private HttpListener _listener;
        private TaskCompletionSource<string> _tcs;

        public VNPayHttpListener(string listenerUrl)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(listenerUrl);
            _tcs = new TaskCompletionSource<string>();
        }

        public async Task<string> Start()
        {
            _listener.Start();
            var context = await _listener.GetContextAsync();

            var request = context.Request;
            var response = context.Response;

            var vnpayResponse = request.QueryString;

            string responseString = "<html><body><h1>Payment received. You can now close this window.</h1></body></html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var output = response.OutputStream;
            await output.WriteAsync(buffer, 0, buffer.Length);
            output.Close();

            _listener.Stop();
            _tcs.SetResult("Payment processed");

            return vnpayResponse.ToString();
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}
