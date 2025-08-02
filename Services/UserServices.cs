using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Services
{
    public class UserServices : IUserServices
    {
        private static UserRepository repo = null;

        public UserServices() { 
            repo = new UserRepository();
        }

        public User Login(string username, string password)
        {
            User user = repo.GetUser(username);
            if(user != null && user.Password.Equals(password))
            {
                user.Password = null; 
                return user;
            }
            return null;
        }

        public bool AddUser(User user) => repo.AddUser(user);

        public List<User> GetUsers() => repo.GetUsers();

        public bool DeleteUser(int id) => repo.DeleteUser(id);

        public bool UpdateUser(User user) => repo.UpdateUser(user);

        public User GetLastUser() => repo.GetLastUser();
    }
}
