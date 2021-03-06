﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace __ClientSocket__
{
    class ClientSocket
    {
        private string host = "";
        private int port = 0;
        private Socket socket = null;
        private System.Net.IPEndPoint ep = null;
        IPHostEntry hostInfo = null;

        public ClientSocket(string host, int port)
        {
            hostInfo = Dns.GetHostByName(host);
            ep = new System.Net.IPEndPoint(hostInfo.AddressList[0], port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public ClientSocket(Socket socket)
        {
            this.socket = socket;
        }
        public bool connect()
        {
            try
            {
                socket.Connect(ep);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }
        public int dataAvailable()
        {
            return socket.Available;
        }
        public int write(int b)
        {
            byte[] msg = new byte[1];
            msg[0] = (byte)b;
            return socket.Send(msg);
        }
		public int write(byte[] b)
		{
			return socket.Send(b);
		}
        public int write(string s)
        {
            byte[] msg = Encoding.GetEncoding(1252).GetBytes(s);
            return socket.Send(msg);
        }
        public int read()
        {
            byte[] rcvbuffer = new byte[1];
            socket.Receive(rcvbuffer);
            return rcvbuffer[0];
        }
        public int read(byte[] b, int len)
        {
            return socket.Receive(b, len, SocketFlags.None);
        }
        public string readLine()
        {
            string rcv = "";
            byte[] rcvbuffer = new byte[1];
            rcvbuffer[0] = 0;

            //System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Encoding enc = Encoding.GetEncoding(1252);
            while (rcvbuffer[0] != '\n')
            {
                socket.Receive(rcvbuffer);
                rcv += enc.GetString(rcvbuffer);
            }

            return rcv.Substring(0, rcv.Length - 1);
        }
        public void close()
        {
            socket.Close();
            socket = null;
        }
    }
}
