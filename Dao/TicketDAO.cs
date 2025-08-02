using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao
{
    public class TicketDAO
    {
        private static MetroTicketContext dbContext = null;
        private static TicketDAO instance = null;

        public static TicketDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TicketDAO();
                }
                return instance;
            }
        }

        public TicketDAO()
        {
            dbContext = new MetroTicketContext();
        }

        public Ticket GetTicketByID(string id) => dbContext.Tickets.FirstOrDefault(t => t.TicketId.Equals(id));

        public Ticket GetLastTicket() => dbContext.Tickets.OrderByDescending(t => t.PurchasedAt).FirstOrDefault();

        public List<Ticket> GetTickets() => dbContext.Tickets.ToList();

        public bool AddTicket(Ticket ticket)
        {
            bool success = false;
            Ticket dbTicket = GetTicketByID(ticket.TicketId);
            try
            {
                if (dbTicket == null)
                {
                    dbContext.Add(ticket);
                    dbContext.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding ticket: " + ex.Message);
            }
            return success;
        }

        public bool DeleteTicket(string id)
        {
            bool success = false;
            Ticket ticketToRemove = GetTicketByID(id);
            try
            {
                if (ticketToRemove != null)
                {
                    dbContext.Remove(ticketToRemove);
                    dbContext.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting ticket: " + ex.Message);
            }
            return success;
        }

        public bool UpdateTicket(Ticket ticket)
        {
            bool success = false;
            Ticket dbTicket = GetTicketByID(ticket.TicketId);
            try
            {
                if (dbTicket != null)
                {
                    dbTicket.TicketId = ticket.TicketId;
                    dbTicket.PurchasedAt = ticket.PurchasedAt;
                    dbTicket.Status = ticket.Status;
                    dbTicket.TicketPriceId = ticket.TicketPriceId;
                    dbContext.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating ticket: " + ex.Message);
            }
            return success;
        }
    }
}
