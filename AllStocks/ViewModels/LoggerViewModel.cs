using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;

namespace AllStocks.ViewModels
{
    public class LoggerViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "LoggerView";

        public IScreen HostScreen { get; }

        public LoggerViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
