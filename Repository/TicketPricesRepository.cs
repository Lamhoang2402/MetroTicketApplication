using BusinessObjects;
using Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TicketPricesRepository : ITicketPricesRepository
    {
        public TicketPrice GetTicketPriceById(int ticketPriceId) => TicketPricesDAO.Instance.GetTicketPriceById(ticketPriceId);

        public List<TicketPrice> GetTicketPrices() => TicketPricesDAO.Instance.GetTicketPrices();

        public List<TicketType> GetTicketTypes() => TicketTypesDAO.Instance.GetTicketTypes();

        public List<CustomerType> GetCustomerTypes() => CustomerTypeDAO.Instance.GetCustomerTypes();

        public bool Delete(int id) => TicketPricesDAO.Instance.Delete(id);

        public bool AddTicketPrice(TicketPrice ticketPrice) => TicketPricesDAO.Instance.AddTicketPrice(ticketPrice);

        public bool UpdateTicketPrice(TicketPrice ticketPrice) => TicketPricesDAO.Instance.UpdateTicketPrice(ticketPrice);
    }
}

