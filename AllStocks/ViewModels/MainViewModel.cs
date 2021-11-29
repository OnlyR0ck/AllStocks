using System;
using System.Reactive;
using System.Reflection;
using System.Windows.Input;
using AllStocks.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AllStocks.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {

        [Reactive]
        public RoutingState Router { get; set; }


        public ReactiveCommand<Unit, Unit> GoToParamsView { get; set; }
        public ReactiveCommand<Unit, Unit> GoToLogView { get; set; }
        public ReactiveCommand<Unit, Unit> GoToStatView { get; set; }
        public ReactiveCommand<Unit, Unit> GoToDataBaseView;


        public ServerParamsViewModel ServerParamsViewModel { get; set; }
        public LoggerViewModel LoggerViewModel { get; set; }
        public DatabaseViewModel DatabaseViewModel { get; set; }
        public StatViewModel StatViewModel { get; set; }


        public MainViewModel()
        {
            Router = new RoutingState();

            ServerParamsViewModel = new ServerParamsViewModel();
            LoggerViewModel = new LoggerViewModel();
            DatabaseViewModel = new DatabaseViewModel();
            StatViewModel = new StatViewModel();

            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());


           /* Locator.CurrentMutable.Register(() => new ServerParamsView(), typeof(IViewFor<ServerParamsViewModel>));
            Locator.CurrentMutable.Register(() => new LoggerView(), typeof(IViewFor<LoggerViewModel>));*/


            Router.Navigate.Execute(ServerParamsViewModel);



            GoToParamsView = ReactiveCommand.Create(() =>
            {
                Router.Navigate.Execute(ServerParamsViewModel);
            });

            GoToLogView = ReactiveCommand.Create(() =>
            {
                Router.Navigate.Execute(LoggerViewModel);
            });

            GoToDataBaseView = ReactiveCommand.Create(() =>
            {
                Router.Navigate.Execute(DatabaseViewModel);
            });

            GoToStatView = ReactiveCommand.Create(() =>
            {
                Router.Navigate.Execute(StatViewModel);
            });

        }

    }
}
