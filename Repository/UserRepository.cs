using BusinessObjects;
using Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public bool AddUser(User user) => UserDAO.Instance.AddUser(user);

        public User GetLastUser() => UserDAO.Instance.GetLastUser();

        public bool DeleteUser(int id) => UserDAO.Instance.DeleteUser(id);

        public User GetUser(string username) => UserDAO.Instance.GetUser(username);

        public List<User> GetUsers() => UserDAO.Instance.GetUsers();

        public bool UpdateUser(User user) => UserDAO.Instance.UpdateUser(user);
    }
}
