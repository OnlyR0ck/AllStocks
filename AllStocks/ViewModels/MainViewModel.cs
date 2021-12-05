using System;
using System.Reactive;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using AllStocks.Classes;
using AllStocks.Interfaces;
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

        public IServer Server { get; }


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
            Locator.CurrentMutable.Register(() => new AsynchronousSocketListener("127.0.0.1", 12347), typeof(IServer));

            Server = Locator.Current.GetService<IServer>();


           /* Locator.CurrentMutable.Register(() => new ServerParamsView(), typeof(IViewFor<ServerParamsViewModel>));
            Locator.CurrentMutable.Register(() => new LoggerView(), typeof(IViewFor<LoggerViewModel>));*/


            Router.Navigate.Execute(ServerParamsViewModel);

            CommandsInit();

            Thread serverThread = new Thread(Server.StartServer);
            serverThread.Start();
        }

        private void CommandsInit()
        {
            GoToParamsView = ReactiveCommand.Create(() => { Router.Navigate.Execute(ServerParamsViewModel); });

            GoToLogView = ReactiveCommand.Create(() => { Router.Navigate.Execute(LoggerViewModel); });

            GoToDataBaseView = ReactiveCommand.Create(() => { Router.Navigate.Execute(DatabaseViewModel); });

            GoToStatView = ReactiveCommand.Create(() => { Router.Navigate.Execute(StatViewModel); });
        }
    }
}
