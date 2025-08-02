using BusinessObjects;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TicketServices : ITicketServices
    {
        private IticketRepository repo = null;

        public TicketServices()
        {
            repo = new TicketRepository();
        }

        public bool AddTicket(Ticket ticket) => repo.AddTicket(ticket);

        public Ticket GetLastTicket() => repo.GetLastTicket();

        public bool DeleteTicket(string id) => repo.DeleteTicket(id);

        public Ticket GetTicketByID(string id) => repo.GetTicketByID(id);

        public List<Ticket> GetTickets() => repo.GetTickets();

        public bool UpdateTicket(Ticket ticket) => repo.UpdateTicket(ticket);
    }
}
