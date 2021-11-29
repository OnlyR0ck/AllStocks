using ReactiveUI;
using Splat;

namespace AllStocks.ViewModels
{
    public class StatViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "StatView";

        public IScreen HostScreen { get; }

        public StatViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}