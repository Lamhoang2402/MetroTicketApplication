using BusinessObjects;
using Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TicketRepository : IticketRepository
    {
        public bool AddTicket(Ticket ticket) => TicketDAO.Instance.AddTicket(ticket);

        public Ticket GetLastTicket() => TicketDAO.Instance.GetLastTicket();

        public bool DeleteTicket(string id) => TicketDAO.Instance.DeleteTicket(id);

        public Ticket GetTicketByID(string id) => TicketDAO.Instance.GetTicketByID(id);

        public List<Ticket> GetTickets() => TicketDAO.Instance.GetTickets();

        public bool UpdateTicket(Ticket ticket) => TicketDAO.Instance.UpdateTicket(ticket);
    }
}
