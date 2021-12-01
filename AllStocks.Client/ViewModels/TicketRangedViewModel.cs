using ReactiveUI;
using Splat;

namespace AllStocks.Client.ViewModels
{
    public class TicketRangedViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "TicketRangedView";

        public IScreen HostScreen { get; }

        public TicketRangedViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}