using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Projekat
{
    internal class TcpServer
    {
        private readonly TcpListener tcpListener;
        private readonly ConcurrentBag<RequestInfo> receivedRequests;
        private readonly HttpRequestHandler requestHandler;

        public TcpServer(IPAddress ipAddress, int port)
        {
            tcpListener = new TcpListener(ipAddress, port);
            receivedRequests = new ConcurrentBag<RequestInfo>();
            requestHandler = new HttpRequestHandler();
        }

        public async Task StartServer()
        {
            tcpListener.Start();
            Console.WriteLine("Server started...");

            try
            {
                while (true)
                {
                    var client = await tcpListener.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected...");

                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accepting client: {ex.Message}");
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (client)
            {
                using (NetworkStream stream = client.GetStream())
                {
                    try
                    {
                        string request = await requestHandler.ReadRequestAsync(stream);
                        Console.WriteLine(request);

                        var requestInfo = new RequestInfo { request = request };
                        receivedRequests.Add(requestInfo);

                        Console.WriteLine($"Request {requestInfo.myNumber} received:\n{requestInfo}");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error handling client request: {ex.Message}");
                    }
                }
            }
        }
    }
}
