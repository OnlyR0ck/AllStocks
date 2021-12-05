using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AllStocks.Client.ViewModels
{
    public class SearchResultViewModel : ReactiveObject
    {
        [Reactive] public string SearchResult { get; set; }


        public SearchResultViewModel(string result)
        {
            SearchResult = result;
        }

    }
}
