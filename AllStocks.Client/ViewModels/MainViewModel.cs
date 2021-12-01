using System.Reactive;
using System.Reflection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AllStocks.Client.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        [Reactive]
        public RoutingState Router { get; set; }


        public ReactiveCommand<Unit, Unit> GoToCompanyInfoView { get; set; }
        public ReactiveCommand<Unit, Unit> GoToTicketDailyView { get; set; }
        public ReactiveCommand<Unit, Unit> GoToTicketInfoNDaysView { get; set; }
        public ReactiveCommand<Unit, Unit> GoToTicketRangedView { get; set; }  


        public CompanyInfoViewModel CompanyInfoViewModel { get; set; }
        public TicketDailyInfoViewModel TicketDailyInfoViewModel { get; set; }
        public TicketInfoForNDaysViewModel TicketInfoForNDaysViewModel{ get; set; }
        public TicketRangedViewModel TicketRangedViewModel { get; set; }


        public MainViewModel()
        {
            Router = new RoutingState();

            CompanyInfoViewModel = new CompanyInfoViewModel();
            TicketDailyInfoViewModel = new TicketDailyInfoViewModel();
            TicketInfoForNDaysViewModel = new TicketInfoForNDaysViewModel();
            TicketRangedViewModel = new TicketRangedViewModel();

            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

            Router.Navigate.Execute(CompanyInfoViewModel);


            GoToCompanyInfoView = ReactiveCommand.Create(() =>
            {
                Router.Navigate.Execute(CompanyInfoViewModel);
            });

            GoToTicketDailyView = ReactiveCommand.Create(() =>
            {
                Router.Navigate.Execute(TicketDailyInfoViewModel);
            });

            GoToTicketInfoNDaysView = ReactiveCommand.Create(() =>
            {
                Router.Navigate.Execute(TicketInfoForNDaysViewModel);
            });

            GoToTicketRangedView = ReactiveCommand.Create(() =>
            {
                Router.Navigate.Execute(TicketRangedViewModel);
            });
        }
    }
}
