using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AllStocks.ViewModels
{
    public class ServerParamsViewModel : ReactiveObject, IRoutableViewModel
    {
        public IPAddress IpAddress;
        public int Port;

        [Reactive] public int ClientsCount { get; set; }

        public string? UrlPathSegment => "ServerParams";

        public IScreen HostScreen { get; }

        public ServerParamsViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            StartServerAsync();
        }

        private async Task StartServerAsync()
        {
            IpAddress = IPAddress.Parse("127.0.0.1");
            Port = 6000;
            ClientsCount = 0;

        }
    }
}
