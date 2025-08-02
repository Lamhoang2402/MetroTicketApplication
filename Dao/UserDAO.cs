using BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao
{
    public class UserDAO
    {
        private static MetroTicketContext dbContext = null;
        private static UserDAO instance = null;

        public static UserDAO Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new UserDAO();
                }
                return instance;
            }
        }

        private UserDAO()
        {
            dbContext = new MetroTicketContext();
        }

        public User GetUser(string username) => dbContext.Users.FirstOrDefault(u => u.Username.Equals(username));

        public User GetLastUser() => dbContext.Users.OrderByDescending(u => u.UserId).FirstOrDefault();

        public List<User> GetUsers() => dbContext.Users.ToList();

        public bool AddUser(User user)
        {
            bool success = false;
            User dbUser = GetUser(user.Username);

            try
            {
                if (dbUser == null)
                {
                    dbContext.Add(user);
                    dbContext.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding user: " + ex.Message);
            }
            return success;
        }

        public bool DeleteUser(int id)
        {
            bool success = false;
            User dbUser = dbContext.Users.SingleOrDefault(u => u.UserId.Equals(id));

            try
            {
                if (dbUser != null)
                {
                    dbContext.Users.Remove(dbUser);
                    dbContext.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting user: " + ex.Message);
            }
            return success;
        }

        public bool UpdateUser(User user)
        {
            bool success = false;
            User dbUser = dbContext.Users.SingleOrDefault(u => u.UserId.Equals(user.UserId));

            try
            {
                if (dbUser != null)
                {
                    dbUser.UserId = user.UserId;
                    dbUser.Username = user.Username;
                    dbUser.RoleId = user.RoleId;
                    dbUser.Password = user.Password;
                    dbUser.Email = user.Email;

                    dbContext.Entry(dbUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    dbContext.SaveChanges();
                    dbContext.Entry(dbUser).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user: " + ex.Message);
            }
            return success;
        }
    }
}
