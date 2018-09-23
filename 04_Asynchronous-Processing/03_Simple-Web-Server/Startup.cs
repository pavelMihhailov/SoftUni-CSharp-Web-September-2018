using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _03_Simple_Web_Server
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 1300;
            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();

            Console.WriteLine("Server started.");
            Console.WriteLine("Listening to TCP Clients at 127.0.0.1:{0}", port);

            var task = Task.Run(() => ConnectWithTcpClient(listener));
            task.Wait();
        }

        public static async Task ConnectWithTcpClient(TcpListener tcpListener)
        {
            while (true)
            {
                Console.WriteLine("Waiting for client...");
                var client = await tcpListener.AcceptTcpClientAsync();

                Console.WriteLine("Client connected.");

                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, buffer.Length);

                var message = Encoding.ASCII.GetString(buffer);
                Console.WriteLine(message);

                byte[] data = Encoding.ASCII.GetBytes("Hello from server!");
                client.GetStream().Write(data, 0, data.Length);

                Console.WriteLine("Closing connection.");
                client.GetStream().Dispose();
            }
        }
    }
}
