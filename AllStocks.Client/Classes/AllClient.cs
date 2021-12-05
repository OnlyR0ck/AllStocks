using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AllStocks.Client.Enums;
using AllStocks.Client.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace AllStocks.Client.Classes
{
    public class AllClient 
    {
        private IPAddress _ipAddress;
        private int _port;
        private StringBuilder _serverResponse = new();
        private TcpClient _clientSocket;

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

        [Reactive]
        public StringBuilder ServerResponse => _serverResponse;

        public AllClient(string ip, int port)
        {
            IpAddress = IPAddress.Parse(ip);
            Port = port;
        }


        public void Connect()
        {
            _clientSocket = new TcpClient();
            _clientSocket.Connect(IpAddress.ToString(), Port);
        }

        public async Task<string> GetInfoAsync(ServerCommandType type)
        {
            return await Task.Run(() =>
            {
                NetworkStream serverStream = _clientSocket.GetStream();
                //BinaryWriter writer = new BinaryWriter(serverStream);

                byte[] outStream = Encoding.UTF8.GetBytes($"{type} AAPL");
                /*writer.Write(outStream);
                writer.Flush();*/
                serverStream.Write(outStream, 0, outStream.Length);

                byte[] inputStream = new byte[1100000];
                while (true)
                {
                    if (_clientSocket.Available == 0)
                    {
                        continue;
                    }

                    serverStream.Read(inputStream, 0, _clientSocket.ReceiveBufferSize);
                    break;
                }
                ServerResponse.Append(Encoding.UTF8.GetString(inputStream));
                return ServerResponse.ToString();

            });
        }
    }
}
