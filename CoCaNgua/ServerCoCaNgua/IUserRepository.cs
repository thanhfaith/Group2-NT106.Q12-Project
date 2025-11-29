using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCoCaNgua
{
    public interface IUserRepository
    {
        bool UsernameExists(string username);
        bool Register(User user);
        User Login(string username, string password);
    }
}
