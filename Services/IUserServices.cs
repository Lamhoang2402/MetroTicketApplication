using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserServices
    {
        public User Login(string username, string password);

        public User GetLastUser();

        public List<User> GetUsers();

        public bool DeleteUser(int id);

        public bool UpdateUser(User user);

        public bool AddUser(User user);
    }
}
