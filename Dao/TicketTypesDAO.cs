using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao
{
    public class TicketTypesDAO
    {
        private MetroTicketContext _context;
        private static TicketTypesDAO instance;

        public TicketTypesDAO()
        {
            _context = new MetroTicketContext();
        }

        public static TicketTypesDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TicketTypesDAO();
                }
                return instance;
            }
        }

        public TicketType GetTicketTypeId(string ticketTpyeId)
        {
            return _context.TicketTypes.FirstOrDefault(tt => tt.TicketTypeId.Equals(ticketTpyeId));
        }

        public List<TicketType> GetTicketTypes()
        {
            return _context.TicketTypes.ToList();
        }
    }
}
