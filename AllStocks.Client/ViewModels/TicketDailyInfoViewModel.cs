using ReactiveUI;
using Splat;

namespace AllStocks.Client.ViewModels
{
    public class TicketDailyInfoViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "TicketDailyView";

        public IScreen HostScreen { get; }

        public TicketDailyInfoViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
