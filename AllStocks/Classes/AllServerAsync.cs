using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AllStocks.Client.Enums;
using AllStocks.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AllStocks.Classes
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }

    public class AsynchronousSocketListener : IServer
    {
        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private IPAddress _ip;
        private int _port;

        public AsynchronousSocketListener(string ip, int port)
        {
            _ip = IPAddress.Parse(ip);
            _port = port;
        }

        public void StartServer()
        {
            // Establish the local endpoint for the socket.  
              
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(_ip, _port);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    listener.BeginAccept(AcceptCallback, listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("\0", StringComparison.Ordinal) > -1)
                //if (content.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
                //if(handler.)
                {
                    // All the data has been read from the
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.  
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static string GetApiResponse(string[] clientCommand)
        {
            ServerCommandType type = RecognizeCommand(clientCommand[0]);

            FMPClient client = new FMPClient();
            client.Symbol = clientCommand[1].ToUpper();

            StringBuilder builder = new StringBuilder();

            switch (type)
            {
                case ServerCommandType.Login:
                    break;
                case ServerCommandType.Register:
                    break;
                case ServerCommandType.CompanyInfo:
                    Task<List<string>> companyInfo = client.GetCompanyInfo();
                    foreach (string info in companyInfo.Result)
                    {
                        builder.Append(info);
                    }
                    break;
                case ServerCommandType.TicketDaily:
                    Task<List<string>> ticketDailyInfo = client.GetStockPriceToday();
                    foreach (string info in ticketDailyInfo.Result)
                    {
                        builder.Append(info);
                    }
                    break;
                case ServerCommandType.TicketForNDays:
                    int trades = int.Parse(clientCommand[2]);
                    Task<List<string>> ticketLastNInfo = client.GetStockPriceLastNTrades(trades);
                    foreach (string info in ticketLastNInfo.Result)
                    {
                        builder.Append(info);
                    }
                    break;
                //TODO: Fix date 
                case ServerCommandType.TicketRanged:
                    string from = $"{DateTime.Parse(clientCommand[3]):yyyy-MM-dd}";
                    string to = $"{DateTime.Parse(clientCommand[4]):yyyy-MM-dd}";

                    Task<List<string>> ticketRangedInfo =
                        client.GetStockPriceFromTo(from, to);
                    foreach (string info in ticketRangedInfo.Result)
                    {
                        builder.Append(info);
                    }
                    break;
                case ServerCommandType.Search:
                    List<string> tickets = client.SearchTickets(client.Symbol).Result;
                    foreach (string searchTicket in tickets)
                    {
                        builder.Append($"{searchTicket} ");
                    }

                    string result = builder.ToString().Trim();
                    builder.Clear();
                    builder.Append(result);
                    break;

                case ServerCommandType.KeyMetrics:
                    List<string> metrics = client.GetKeyMetrics().Result;

                    foreach (string keyMetrics in metrics)
                    {
                        builder.Append(keyMetrics);
                    }
                    break;
                    
            }

            return builder.ToString();
        }

        private static ServerCommandType RecognizeCommand(string command) =>
            Enum.Parse<ServerCommandType>(command);

        private static void Send(Socket handler, string data)
        {
            string[] clientCommands = data.Trim().Trim('\0').Split(' ');

            string serverResponse = GetApiResponse(clientCommands);

            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(serverResponse);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
