using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AllStocks.Client.Enums;
using AllStocks.Client.Interfaces;
using AllStocks.Client.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using DynamicData.Alias;

namespace AllStocks.Client.ViewModels
{
    public class CompanyInfoViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ObservableAsPropertyHelper<IEnumerable<SearchResultViewModel>> _autocompleteResults;
        public IEnumerable<SearchResultViewModel> AutocompleteResults => _autocompleteResults.Value;
        
        public bool IsSearchStarted = true;


        private readonly ObservableAsPropertyHelper<CompanyKeyMetricsModel> _keyMetrics;
        public CompanyKeyMetricsModel CompanyKeyMetrics => _keyMetrics.Value;

        [Reactive] public string SearchTerm { get; set; }


        private readonly ObservableAsPropertyHelper<bool> _isAvailable;
        public bool IsAvailable => _isAvailable.Value;


        [Reactive]
        public string ServerResponse { get; set; }

        public IClient Client { get; set; }
    

        public ReactiveCommand<Unit, string> GetResponse { get; set; }
        public ReactiveCommand<Unit, Unit> BlockSearch { get; set; }


        public string? UrlPathSegment => "CompanyInfoView";

        public IScreen HostScreen { get; }

        public CompanyInfoViewModel(IScreen screen = null, IClient client = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            Client = client ?? Locator.Current.GetService<IClient>();

            GetResponse = ReactiveCommand.CreateFromTask(() =>
            {
                return Task.Run(() =>
                {
                    return Client.StartClient();
                });
            });

            GetResponse.Subscribe(x => ServerResponse = x);
           
            GetResponse.ThrownExceptions.Subscribe(exception =>
            {
                this.Log().Warn($"Exception: {exception}");
            });

            _autocompleteResults = this
                .WhenAnyValue(x => x.SearchTerm)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Select(term => term?.Trim())
                .DistinctUntilChanged()
                .Where(term => !string.IsNullOrWhiteSpace(term))
                .SelectMany(GetSearchHints)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.AutocompleteResults);

            _autocompleteResults.ThrownExceptions.Subscribe(error =>
            {
                this.Log().Error(error.Message);
            });

            _isAvailable = this
                .WhenAnyValue(x => x.AutocompleteResults)
                .Select(autocompleteSearchResults => autocompleteSearchResults != null)
                .ToProperty(this, x => x.IsAvailable);

            _keyMetrics = this
                .WhenAnyValue(
                    vm => vm.IsSearchStarted,
                    vm => vm.SearchTerm)
                .Where(x => x.Item1 == true)
                .Select(x => x.Item2)
                .DistinctUntilChanged()
                .Where(term => !string.IsNullOrWhiteSpace(term))
                .SelectMany(x => GetKeyMetrics(x, CancellationToken.None))
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.CompanyKeyMetrics);
        }

        private async Task<IEnumerable<SearchResultViewModel>> GetSearchHints(string term, CancellationToken token)
        {
            if (!IsSearchStarted)
            {
                IsSearchStarted = true;
                return null;
            }

            Client
                .Symbol(term)
                .Type(ServerCommandType.Search);
            string result = await Client.StartClient();
            //_isSearchAvailable = true;

            return result.Split(' ').Select(x => new SearchResultViewModel(x));
        }

        private async Task<CompanyKeyMetricsModel> GetKeyMetrics(string term, CancellationToken token)
        {
            Client
                .Symbol(term)
                .Type(ServerCommandType.KeyMetrics);
            string jsonResult = await Client.StartClient();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };

            CompanyKeyMetricsModel keyMetrics = JsonSerializer.Deserialize<CompanyKeyMetricsModel>(jsonResult, options);


            return keyMetrics;
        }
    }
}
