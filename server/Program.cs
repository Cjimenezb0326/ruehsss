using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server("localhost", 4440);
            s.Start();  // Inicia el servidor
        }
    }

    class Server
    {
        IPAddress ipAddr;
        IPEndPoint endPoint;
        Socket s_Server;
        Socket s_Client;

        public Server(string ip, int port)
        {
            ipAddr = Dns.GetHostEntry(ip).AddressList[0];
            endPoint = new IPEndPoint(ipAddr, port);

            s_Server = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            s_Server.Bind(endPoint);
            s_Server.Listen(10);
            Console.WriteLine("Servidor iniciado. Esperando conexiones...");
        }

        public void Start()
        {
            s_Client = s_Server.Accept();
            Console.WriteLine("Cliente conectado.");

            try
            {
                while (true)
                {
                    // Recibir mensaje
                    byte[] buffer = new byte[1024];
                    int bytesRec = s_Client.Receive(buffer);
                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRec);
                    Console.WriteLine("Mensaje recibido del cliente: " + receivedMessage);

                    // Enviar mensaje
                    Console.WriteLine("Ingrese el mensaje a enviar de vuelta al cliente:");
                    string responseMessage = Console.ReadLine();
                    byte[] byteMsg = Encoding.ASCII.GetBytes(responseMessage);
                    s_Client.Send(byteMsg);
                    Console.WriteLine("Mensaje enviado al cliente.");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                s_Client.Shutdown(SocketShutdown.Both);
                s_Client.Close();
            }
        }
    }
}

