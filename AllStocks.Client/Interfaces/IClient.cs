using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllStocks.Client.Classes;
using AllStocks.Client.Enums;

namespace AllStocks.Client.Interfaces
{
    public interface IClient
    {
        //void Connect();
        //Task<string> GetInfoAsync(ServerCommandType type);
        string GetResponse();

        Task<string> StartClient();

        public AsynchronousClient Type(ServerCommandType type);
        public AsynchronousClient Symbol(string symbol);
        public AsynchronousClient LastDays(int lastDays);
        public AsynchronousClient DateFrom(DateTime dateFrom);
        public AsynchronousClient DateTo(DateTime dateTo);

    }
}
