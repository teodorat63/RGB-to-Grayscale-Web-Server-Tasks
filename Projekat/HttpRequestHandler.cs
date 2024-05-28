using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Projekat
{
    internal class HttpRequestHandler
    {
        private readonly ImageService imageService;

        public HttpRequestHandler()
        {
            imageService = new ImageService();
        }

        public async Task<string> ReadRequestAsync(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            string[] parts = request.Split(' ');
            string filename = parts[1].Substring(1);

            if (filename == "")
            {
                await SendResponseAsync("Dobrodosli na server!", stream);
            }
            else if (IsValidImageRequest(filename))
            {
                byte[] outputData = await imageService.ServeImageAsync(filename);
                await SendImageResponseAsync(outputData, stream);
            }
            return request;
        }

        public async Task SendResponseAsync(string request, NetworkStream stream)
        {
            string send = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<h1>" + request + "</h1>";
            byte[] responseData = Encoding.UTF8.GetBytes(send);
            await stream.WriteAsync(responseData, 0, responseData.Length);
        }

        public async Task SendImageResponseAsync(byte[] imageData, NetworkStream stream)
        {
            StringBuilder responseBuilder = new StringBuilder();
            responseBuilder.AppendLine("HTTP/1.1 200 OK");
            responseBuilder.AppendLine("Content-Type: image/jpeg");
            responseBuilder.AppendLine($"Content-Length: {imageData.Length}");
            responseBuilder.AppendLine();
            byte[] responseHeader = Encoding.UTF8.GetBytes(responseBuilder.ToString());

            await stream.WriteAsync(responseHeader, 0, responseHeader.Length);
            await stream.WriteAsync(imageData, 0, imageData.Length);
        }

        public bool IsValidImageRequest(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new ArgumentException("File " + filename + " does not exist");
            }

            string extension = Path.GetExtension(filename).ToLower();
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                extension == ".gif" || extension == ".bmp")
            {
                return true;
            }
            else
            {
                throw new ArgumentException("File " + filename + " is not a valid image file");
            }
        }
    }
}
