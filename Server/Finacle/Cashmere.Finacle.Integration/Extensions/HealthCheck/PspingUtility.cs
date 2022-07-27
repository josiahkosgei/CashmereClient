using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Cashmere.Finacle.Integration.Extensions.HealthCheck
{
    internal class PspingUtility
    {
        public static async Task<CustomPingReply> CustomPingAsync(string endPoint)
        {
            // Create a TCP/IP  socket.
            using Socket sender = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                // Connect to Remote EndPoint
                await sender.ConnectAsync(endPoint, 80);

                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                var _psPingReply = new CustomPingReply()
                {
                    Status = IPStatus.Success
                };
                return _psPingReply;

            }
            catch (SocketException se)
            {
                Console.WriteLine($"{se.Message}");
                var _psPingReply = new CustomPingReply()
                {
                    Status = IPStatus.TimedOut
                };
                return _psPingReply;
            }
        }
    }
}
