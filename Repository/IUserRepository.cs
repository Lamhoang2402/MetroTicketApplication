using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRepository
    {
        public User GetUser(string username);

        public User GetLastUser();

        public bool AddUser(User user);

        public List<User> GetUsers();

        public bool DeleteUser(int id);
        
        public bool UpdateUser(User user);
    }
}
