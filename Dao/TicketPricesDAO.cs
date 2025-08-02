using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
namespace Dao
{
    public class TicketPricesDAO
    {
        private MetroTicketContext _context;
        private static TicketPricesDAO instance;

        public TicketPricesDAO()
        {
            _context = new MetroTicketContext();
        }

        public static TicketPricesDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TicketPricesDAO();
                }
                return instance;
            }
        }

        public TicketPrice GetTicketPriceById(int ticketPriceId)
        {
            return _context.TicketPrices.FirstOrDefault(tp => tp.TicketPriceId.Equals(ticketPriceId));
        }

        public List<TicketPrice> GetTicketPrices()
        {
            return _context.TicketPrices.Include(i => i.TicketType).Include(ic => ic.CustomerType).ToList();
        }

        public bool Delete(int id)
        {
            TicketPrice ticketPriceRemove = _context.TicketPrices.Find(id);
            if (ticketPriceRemove == null)
            {
                return false;
            }
            _context.Remove(ticketPriceRemove);
            _context.SaveChanges();
            return true;
        }

        public bool AddTicketPrice(TicketPrice ticketPrice)
        {
            bool isSuccess = false;
            TicketPrice ticket = this.GetTicketPriceById(ticketPrice.TicketPriceId);
            try
            {
                if (ticket == null)
                {
                    _context.TicketPrices.Add(ticketPrice);
                    _context.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddTicketPrice: " +  ex);
            }
            return isSuccess;
        }

        public bool UpdateTicketPrice(TicketPrice ticketPrice)
        {
            bool isSuccess = false;
            TicketPrice ticket = this.GetTicketPriceById(ticketPrice.TicketPriceId);
            try
            {
                if (ticket != null)
                {
                    ticket.TicketTypeId = ticketPrice.TicketTypeId;
                    ticket.CustomerTypeId = ticketPrice.CustomerTypeId;
                    ticket.TicketTypeId = ticketPrice.TicketTypeId;
                    ticket.Price = ticketPrice.Price;
                    _context.Entry<TicketPrice>(ticket).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    _context.Entry<TicketPrice>(ticket).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddTicketPrice: " + ex);
            }
            return isSuccess;
        }
    }
}
