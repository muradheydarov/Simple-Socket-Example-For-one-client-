using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    class Program
    {
        static string ip = "127.0.0.1";
        static int port = 8088;
        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(endPoint);

                socket.Listen(10);

                Console.WriteLine("Server started... Waiting for connections...");

                Socket handler = socket.Accept();
                Console.WriteLine("Client is connected");

                StringBuilder stringBuilder = new StringBuilder("test");

                while (stringBuilder.ToString() != "Exit")
                {
                    int bytes = 0;
                    stringBuilder = new StringBuilder();
                    byte[] data = new byte[256];

                    do
                    {
                        bytes = handler.Receive(data);
                        stringBuilder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (handler.Available > 0);

                    Console.WriteLine($"{DateTime.Now.ToString()} : {stringBuilder.ToString()}");


                    string returnMessage = "Message delivered";
                    byte[] messageBytes = Encoding.UTF8.GetBytes(returnMessage);

                    handler.Send(messageBytes);                    
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
