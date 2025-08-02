using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITicketServices
    {
        public Ticket GetTicketByID(string id);

        public Ticket GetLastTicket();

        public List<Ticket> GetTickets();

        public bool AddTicket(Ticket ticket);

        public bool DeleteTicket(string id);

        public bool UpdateTicket(Ticket ticket);
    }
}
