using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;

namespace AllStocks.Client.ViewModels
{
    public class TicketInfoForNDaysViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "TicketInfoForLastNDaysView";

        public IScreen HostScreen { get; }

        public TicketInfoForNDaysViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
