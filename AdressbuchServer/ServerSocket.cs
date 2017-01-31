using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using __ClientSocket__;

namespace __ServerSocket__
{
    class ServerSocket
    {
        private int port = 0;
        private Socket serverSocket = null;

        public ServerSocket(int port)
        {
            this.port = port;
            serverSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream,
                                         ProtocolType.Tcp);
            
            IPAddress hostIP = (Dns.Resolve(IPAddress.Any.ToString())).AddressList[0];
            IPEndPoint ep = new IPEndPoint(hostIP, port);

            // MessageBox.Show(ep.ToString());

            try
            {
                serverSocket.Bind(ep);
                serverSocket.Listen(1);
            }
            catch ( Exception ex)
            {
                // MessageBox.Show(ex.Message);
                serverSocket.Close();
                return;
            }
        }

        public Socket accept()
        {
            return serverSocket.Accept();
        }

        public void close()
        {
            serverSocket.Close();
        }

    }
}
