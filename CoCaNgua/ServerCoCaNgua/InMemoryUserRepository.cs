using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCoCaNgua
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public bool UsernameExists(string username)
        {
            return _users.Any(u => u.Username == username);
        }

        public bool Register(User user)
        {
            if (UsernameExists(user.Username))
                return false;

            _users.Add(user);
            return true;
        }

        public User Login(string username, string password)
        {
            return _users.FirstOrDefault(
                u => u.Username == username && u.Password == password
            );
        }
    }
}
