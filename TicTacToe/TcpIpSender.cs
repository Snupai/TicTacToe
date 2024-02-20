using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class TcpIpSender
    {
        public int port;
        public IPAddress ipAddress;
        public IPEndPoint ipEndPoint;

        public TcpIpSender(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            ipEndPoint = new IPEndPoint(ipAddress, port);
        }


        public async Task SendenAsync(string message)
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(ipEndPoint);
            await using NetworkStream stream = client.GetStream();

            // Nachricht senden
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(messageBytes);
        }
    }
}
