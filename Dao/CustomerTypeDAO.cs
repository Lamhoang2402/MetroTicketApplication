using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao
{
    public class CustomerTypeDAO
    {
        private MetroTicketContext _context;
        private static CustomerTypeDAO instance;

        public CustomerTypeDAO()
        {
            _context = new MetroTicketContext();
        }

        public static CustomerTypeDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerTypeDAO();
                }
                return instance;
            }
        }

        public CustomerType GetTicketPriceById(string customerTypeId)
        {
            return _context.CustomerTypes.FirstOrDefault(ct => ct.CustomerTypeId.Equals(customerTypeId));
        }

        public List<CustomerType> GetCustomerTypes()
        {
            return _context.CustomerTypes.ToList();
        }
    }
}
