using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// Represents a TCP/IP sender.
    /// </summary>
    internal class TcpIpSender
    {
        /// <summary>
        /// Gets or sets the port for the TCP/IP sender.
        /// </summary>
        public int port;

        /// <summary>
        /// Gets or sets the IP address for the TCP/IP sender.
        /// </summary>
        public IPAddress ipAddress;

        /// <summary>
        /// Gets the IP end point for the TCP/IP sender.
        /// </summary>
        public IPEndPoint ipEndPoint;

        /// <summary>
        /// Initializes a new instance of the TcpIpSender class with the specified IP address and port.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="port">The port.</param>
        public TcpIpSender(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            ipEndPoint = new IPEndPoint(ipAddress, port);
        }


        /// <summary>
        /// Sends the specified message asynchronously.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendenAsync(string message)
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(ipEndPoint);
            await using NetworkStream stream = client.GetStream();

            // Send message
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(messageBytes);
        }
    }
}
