using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCoCaNgua
{
    public class GameServer
    {
        private readonly AuthService _authService;

        public GameServer()
        {
            IUserRepository repo = new InMemoryUserRepository();
            _authService = new AuthService(repo);
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 9000);
            listener.Start();

            Console.WriteLine("Server started on port 9000");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
            {
                try
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                            break;

                        Console.WriteLine("Received: " + line);

                        string response = _authService.HandleRequest(line);
                        writer.WriteLine(response);

                        Console.WriteLine("Sent: " + response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Client error: " + ex.Message);
                }
            }

            Console.WriteLine("Client disconnected");
        }
    }
}
