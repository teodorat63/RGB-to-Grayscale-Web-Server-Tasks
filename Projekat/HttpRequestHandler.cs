using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Projekat
{
    internal class HttpRequestHandler
    {

        public string ReadRequest(NetworkStream stream)
        {
            ImageService imageService = new ImageService();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            string[] parts = request.Split(' ');
            string filename = parts[1].Substring(1);

            if (filename == "")
            {
                this.SendResponse("Dobrodosli na server!", stream);
            }
            else if (this.IsValidImageRequest(filename))
            {
                byte[] outputData = imageService.ServeImage(filename, stream);

                this.SendImageResponse(outputData, stream);

            }
            return request;

        }

        public void SendResponse(string request, NetworkStream stream)
        {
            string send = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<h1>" + request + "</ h1 > ";
            byte[] responseData = Encoding.UTF8.GetBytes(send);
            stream.Write(responseData, 0, responseData.Length);
        }

        public void SendImageResponse(byte[] imageData, NetworkStream stream)
        {
            StringBuilder responseBuilder = new StringBuilder();
            responseBuilder.AppendLine("HTTP/1.1 200 OK");
            responseBuilder.AppendLine("Content-Type: image/jpeg");
            responseBuilder.AppendLine($"Content-Length: {imageData.Length}");
            responseBuilder.AppendLine();
            byte[] responseHeader = Encoding.UTF8.GetBytes(responseBuilder.ToString());

            stream.Write(responseHeader, 0, responseHeader.Length);
            stream.Write(imageData, 0, imageData.Length);
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
