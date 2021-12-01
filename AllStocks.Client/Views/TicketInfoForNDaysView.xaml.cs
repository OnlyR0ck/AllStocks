using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AllStocks.Client.ViewModels;
using ReactiveUI;

namespace AllStocks.Client.Views
{
    /// <summary>
    /// Interaction logic for TicketInfoForNDaysView.xaml
    /// </summary>
    public partial class TicketInfoForNDaysView : ReactiveUserControl<TicketInfoForNDaysViewModel>
    {
        public TicketInfoForNDaysView()
        {
            InitializeComponent();
        }
    }
}
