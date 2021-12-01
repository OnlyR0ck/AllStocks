using AllStocks.Client.ViewModels;
using ReactiveUI;

namespace AllStocks.Client.Views
{
    /// <summary>
    /// Interaction logic for CompanyInfoView.xaml
    /// </summary>
    public partial class CompanyInfoView : ReactiveUserControl<CompanyInfoViewModel>
    {
        public CompanyInfoView()
        {
            InitializeComponent();
        }
    }
}
