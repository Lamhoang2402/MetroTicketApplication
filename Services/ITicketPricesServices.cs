using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITicketPricesServices
    {
        public TicketPrice GetTicketPriceById(int id);

        public List<TicketPrice> GetTicketPrices();

        public List<TicketType> GetTicketTypes();

        public List<CustomerType> GetCustomerTypes();

        public bool Delete(int id);

        public bool AddTicketPrice(TicketPrice ticketPrice);

        public bool UpdateTicketPrice(TicketPrice ticketPrice);
    }
}
