using BusinessObjects;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TicketPricesService : ITicketPricesServices
    {
        private ITicketPricesRepository repo;

        public TicketPricesService()
        {
            repo = new TicketPricesRepository();
        }

        public TicketPrice GetTicketPriceById(int id)
        {
            return repo.GetTicketPriceById(id);
        }

        public List<TicketPrice> GetTicketPrices()
        {
            return repo.GetTicketPrices();
        }

        public List<TicketType> GetTicketTypes()
        {
            return repo.GetTicketTypes();
        }

        public List<CustomerType> GetCustomerTypes()
        {
            return repo.GetCustomerTypes();
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }

        public bool AddTicketPrice(TicketPrice ticketPrice)
        {
            return repo.AddTicketPrice(ticketPrice);
        }

        public bool UpdateTicketPrice(TicketPrice ticketPrice)
        {
            return repo.UpdateTicketPrice(ticketPrice);
        }
    }
}
