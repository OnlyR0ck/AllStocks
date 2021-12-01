using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;

namespace AllStocks.Client.ViewModels
{
    public class CompanyInfoViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "CompanyInfoView";

        public IScreen HostScreen { get; }

        public CompanyInfoViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
