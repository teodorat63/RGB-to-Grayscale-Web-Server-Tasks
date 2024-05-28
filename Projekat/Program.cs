using System;
using System.Net;
using System.Threading.Tasks;

namespace Projekat
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 5050;

            TcpServer mojServer = new TcpServer(ipAddress, port);
            await mojServer.StartServer();
        }
    }
}
