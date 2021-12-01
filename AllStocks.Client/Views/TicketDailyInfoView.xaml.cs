using System.Windows.Controls;
using AllStocks.Client.ViewModels;
using ReactiveUI;

namespace AllStocks.Client.Views
{
    /// <summary>
    /// Interaction logic for TicketDailyInfoView.xaml
    /// </summary>
    public partial class TicketDailyInfoView : ReactiveUserControl<TicketDailyInfoViewModel>
    {
        public TicketDailyInfoView()
        {
            InitializeComponent();
        }
    }
}
