using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AllStocks.Client.Enums;
using AllStocks.Interfaces;

namespace AllStocks.Classes
{
    class AllServer : IServer
    {
        private IPAddress _ipAddress;
        private int _port;
        private static int _clientCounter;

        public int Clients
        {
            get => _clientCounter;
            set
            {
                if (value >= 0)
                {
                    _clientCounter = value;
                }
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                if (value > 0)
                {
                    _port = value;
                }
            }
        }

        public IPAddress IpAddress
        {
            get => _ipAddress;
            set => _ipAddress = value;
        }

        public AllServer(string ip, int port)
        {
            IpAddress = IPAddress.Parse(ip);
            Port = port;
        }

        public void StartServer()
        {
            TcpListener serverSocket = new TcpListener(IpAddress,Port);
            TcpClient clientSocket;

            serverSocket.Start();
            //Console.WriteLine(" >> " + "Server Started");

            Clients = 0;
            while (true)
            {
                Clients += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                ClientHandler clientHandler = new ClientHandler();
                clientHandler.StartClient(clientSocket);
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
    }

    public class ClientHandler
    {
        TcpClient _clientSocket;
        public void StartClient(TcpClient inClientSocket)
        {
            _clientSocket = inClientSocket;
            Thread clientThread = new Thread(GetServerResponse);
            clientThread.Start();
        }

        private void GetServerResponse()
        {
            while (_clientSocket.Available != 0)
            {
                try
                {
                    //BinaryReader reader = new BinaryReader(_clientSocket.GetStream());
                    NetworkStream networkStream = _clientSocket.GetStream();
                    StringBuilder builder = new StringBuilder();

                    byte[] bytesFrom = new byte[_clientSocket.ReceiveBufferSize];

                    while (networkStream.CanRead)
                    {
                        networkStream.Read(bytesFrom, 0, _clientSocket.ReceiveBufferSize);
                        builder.Append(Encoding.UTF8.GetString(bytesFrom));
                    }
                    //string dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                    string dataFromClient = builder.ToString();

                    dataFromClient = dataFromClient.Trim();
                    dataFromClient = dataFromClient.Trim('\0');
                    string[] clientCommands = dataFromClient.Split(' ');

                    string serverResponse = GetApiResponse(clientCommands);
                    byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex);
                }
            }
        }

        private string GetApiResponse(string[] clientCommand)
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

            }

            return builder.ToString();
        }

        private ServerCommandType RecognizeCommand(string command) => 
            Enum.Parse<ServerCommandType>(command);
    }
}
