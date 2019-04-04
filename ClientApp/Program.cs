using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientApp
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
                string message = "";
                socket.Connect(endPoint);

                while (message != "Exit")
                {
                    Console.Write("Write your message: ");
                    message = Console.ReadLine();

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    socket.Send(data);

                    int bytes = 0;
                    byte[] receiveData = new byte[256];
                    StringBuilder receivedMessage = new StringBuilder();

                    do
                    {
                        bytes = socket.Receive(receiveData);
                        receivedMessage.Append(Encoding.UTF8.GetString(receiveData, 0, bytes));
                    } while (socket.Available > 0);

                    Console.WriteLine($"Reply from server: {receivedMessage.ToString()}");
                }
                
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Connection closed...");            
        }
    }
}
