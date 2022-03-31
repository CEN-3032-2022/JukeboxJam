using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class handleClient
    {
        protected TcpClient clientSocket;
        protected List<string> messages = new List<string>();
        protected NetworkStream networkStream = null;
        public void startClient(TcpClient inClientSocket)
        {
            this.clientSocket = inClientSocket;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        public string getNextMessage()
        {
            if (messages.Count <= 0)
            {
                return null;
            }
            string message = messages[0];
            messages.RemoveAt(0);
            return message;
        }

        private void doChat()
        {
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;

            networkStream = clientSocket.GetStream();

            while ((clientSocket.Connected))
            {
                try
                {
                    if (clientSocket.Available <= 0)
                    {
                        Thread.Sleep(0);
                        continue;
                    }

                    networkStream.Read(bytesFrom, 0, clientSocket.Available);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    messages.Add(dataFromClient);


                    //Console.WriteLine(" >> " + "From client- " + clNo + " " + dataFromClient);

                    //if (dataFromClient != "start" && dataFromClient != "restart")
                    //{
                    //    Console.WriteLine("'" + dataFromClient + "'");
                    //    requestCount = requestCount + 1;

                    //}
                    //else
                    //{
                    //    requestCount = 0;
                    //}


                    //Console.WriteLine(requestCount + "");
                    //rCount = Convert.ToString(requestCount);
                    //serverResponse = "Server to client(" + clNo + ") " + rCount;
                    //sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    //networkStream.Write(sendBytes, 0, sendBytes.Length);
                    //networkStream.Flush();
                    //Console.WriteLine(" >> " + serverResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ClientLeft");
                    //Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }

        public TcpClient getClient()
        {
            return clientSocket;
        }

        public void sendMessage(string message)
        {
            try
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
                Console.WriteLine(" >> " + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" >> " + ex.ToString());
            }
        }
    }
}
