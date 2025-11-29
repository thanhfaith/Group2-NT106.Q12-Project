using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCoCaNgua
{
    public class AuthService
    {
        private readonly IUserRepository _repo;

        public AuthService(IUserRepository repo)
        {
            _repo = repo;
        }

        public string HandleRequest(string request)
        {
            var parts = request.Split('|');
            if (parts.Length < 3)
                return "ERROR|Request không hợp lệ";

            string command = parts[0];
            string username = parts[1];
            string password = parts[2];

            switch (command)
            {
                case "REGISTER":
                    return HandleRegister(username, password);
                case "LOGIN":
                    return HandleLogin(username, password);
                default:
                    return "ERROR|Lệnh không hỗ trợ";
            }
        }

        private string HandleRegister(string username, string password)
        {
            if (_repo.UsernameExists(username))
                return "ERROR|Tài khoản đã tồn tại";

            var user = new User
            {
                Username = username,
                Password = password
            };

            _repo.Register(user);
            return "OK|Đăng ký thành công";
        }

        private string HandleLogin(string username, string password)
        {
            var user = _repo.Login(username, password);

            if (user == null)
                return "ERROR|Sai tài khoản hoặc mật khẩu";

            return "OK|Đăng nhập thành công";
        }
    }
}
