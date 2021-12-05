using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Input;
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

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.IsAvailable,
                        v => v.SearchResultListBox.Visibility)
                    .DisposeWith(disposables);
                this.Bind(ViewModel,
                        vm => vm.SearchTerm,
                        v => v.SearchTextBox.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.AutocompleteResults,
                        v => v.SearchResultListBox.ItemsSource)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].marketCapTTM,
                        v => v.MarketCapValue)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].enterpriseValueTTM,
                        v => v.EnterpriseValue)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].peRatioTTM,
                        v => v.PERatio)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].roeTTM,
                        v => v.ROERatio)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].roicTTM,
                        v => v.ROICRatio)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].priceToSalesRatioTTM,
                        v => v.PtoS)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].ptbRatioTTM,
                        v => v.PtoB)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].evToSalesTTM,
                        v => v.EVtoR)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel,
                        vm => vm.CompanyKeyMetrics.Metrics[0].enterpriseValueOverEBITDATTM,
                        v => v.EVtoEbitda)
                    .DisposeWith(disposables);
            });
        }

        private void SearchResultListBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (ReferenceEquals(sender, SearchResultListBox))
            {
                if (SearchResultListBox.Items.Count > 0)
                {
                    switch (e.Key)
                    {
                        case Key.Down:
                            if (!SearchResultListBox.Items.MoveCurrentToNext())
                            {
                                SearchResultListBox.Items.MoveCurrentToLast();
                            }

                            break;

                        case Key.Up:
                            if (!SearchResultListBox.Items.MoveCurrentToPrevious())
                            {
                                SearchResultListBox.Items.MoveCurrentToFirst();
                            }

                            break;
                        case Key.Enter:
                            if (SearchResultListBox.SelectedItem != null)
                            {
                                ViewModel.SearchTerm = ((SearchResultViewModel)SearchResultListBox.SelectedItem).SearchResult;
                                ViewModel.IsSearchStarted = false;
                                SearchResultListBox.ItemsSource = null;
                                return;
                            }

                            break;
                        default:
                            return;
                    }

                    e.Handled = true;
                    ListBoxItem listBoxItem =
                        (ListBoxItem) SearchResultListBox.ItemContainerGenerator.ContainerFromItem(SearchResultListBox
                            .SelectedItem);
                    listBoxItem.Focus();
                }
            }
        }
    }
}
