using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AllStocks.ViewModels;
using ReactiveUI;

namespace AllStocks.Views
{
    /// <summary>
    /// Interaction logic for ServerParamsView.xaml
    /// </summary>
    public partial class ServerParamsView : ReactiveUserControl<ServerParamsViewModel>
    {
        public ServerParamsView()
        {
            InitializeComponent();

           /* this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.UrlPathSegment, x => x.MainTextBlock)
                    .DisposeWith(disposables);
            });*/
        }
    }
}
