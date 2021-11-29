using ReactiveUI;
using Splat;

namespace AllStocks.ViewModels
{
    public class DatabaseViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "DatabaseView";

        public IScreen HostScreen { get; }

        public DatabaseViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
