using System;
using System.Reactive.Disposables;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using AllStocks.Client.ViewModels;
using ReactiveUI;
namespace AllStocks.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        #region BorderShit

        // The enum flag for DwmSetWindowAttribute's second parameter, which tells the function what attribute to set.
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_WINDOW_CORNER_PREFERENCE = 33,
            DWMWA_VISIBLE_FRAME_BORDER_THICKNESS = 37
        }

        // The DWM_WINDOW_CORNER_PREFERENCE enum for DwmSetWindowAttribute's third parameter, which tells the function
        // what value of the enum to set.
        public enum DWM_WINDOW_CORNER_PREFERENCE
        {
            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND = 1,
            DWMWCP_ROUND = 2,
            DWMWCP_ROUNDSMALL = 3
        }

        // Import dwmapi.dll and define DwmSetWindowAttribute in C# corresponding to the native function.
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern long DwmSetWindowAttribute(IntPtr hwnd,
            DWMWINDOWATTRIBUTE attribute,
            ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
            uint cbAttribute);

        #endregion


        
        public MainWindow()
        {
            InitializeComponent();

            SetRoundCorners();

            ViewModel = new MainViewModel();


            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, mvm => mvm.Router, vm => vm.RoutedViewHost.Router)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoToCompanyInfoView, v => v.ButtonCompanyInfo)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoToTicketDailyView, v => v.ButtonTodayTicket)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoToTicketRangedView, v => v.ButtonTicketPriceFromTo)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoToTicketInfoNDaysView, v => v.ButtonTicketPrice)
                    .DisposeWith(disposables);
            });
        }

        private void SetRoundCorners()
        {
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
        }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow == null)
            {
                return;
            }
            Application.Current.MainWindow.WindowState =
                Application.Current.MainWindow.WindowState == WindowState.Maximized ?
                    WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow == null)
            {
                return;
            }

            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Boder_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
