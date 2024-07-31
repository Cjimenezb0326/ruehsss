using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Client c = new Client("localhost", 4440);
                c.Start();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    class Client
    {
        private IPAddress ipAddr;
        private IPEndPoint endPoint;
        private Socket s_Client;

        public Client(string ip, int port)
        {
            IPHostEntry host = Dns.GetHostEntry(ip);
            ipAddr = host.AddressList[0];
            endPoint = new IPEndPoint(ipAddr, port);

            s_Client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                s_Client.Connect(endPoint);
                Console.WriteLine("Conexión establecida.");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public void Start()
        {
            try
            {
                while (true)
                {
                    // Enviar mensaje
                    Console.WriteLine("Ingrese el mensaje a enviar:");
                    string mensaje = Console.ReadLine();
                    byte[] byteMsg = Encoding.ASCII.GetBytes(mensaje);
                    s_Client.Send(byteMsg);
                    Console.WriteLine("Mensaje enviado.");

                    // Recibir mensaje
                    byte[] buffer = new byte[1024];
                    int bytesRec = s_Client.Receive(buffer);
                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRec);
                    Console.WriteLine("Mensaje recibido del servidor: " + receivedMessage);
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
