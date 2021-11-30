using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AllStocks.Classes
{
    public class FMPClient
    {
        private string _symbol;
        private const string _apiKey = "e9f1e718d3f97c8d9ee5257bae15bd6a";


        public string ApiKey => _apiKey;

        public string Symbol
        {
            get => _symbol;
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    _symbol = Strings.Trim(value);
                }
            }
        }



        public async Task<List<string>> GetCompanyInfo(int years)
        {
            List<string> responseList = new List<string>();
            using (HttpClient httpClient = new HttpClient())

            {
                using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"),
                    $"https://financialmodelingprep.com/api/v3/income-statement/{Symbol}?limit={years}&apikey={ApiKey}"))

                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    string body = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true
                    };
                    using (JsonDocument document = JsonDocument.Parse(body))
                    {
                        foreach (JsonElement element in document.RootElement.EnumerateArray())
                        {
                            responseList.Add(element.ToString());
                        }
                    }
                }
            }

            return responseList;
        }

        public async Task<List<string>> SearchTickets(string query, int limit = 10, string exchange = "NASDAQ")
        {

            List<string> responseList = new List<string>();
            using (HttpClient httpClient = new HttpClient())

            {
                using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"),
                    $"https://financialmodelingprep.com/api/v3/search-ticker?query={query}&limit={limit}&exchange={exchange}&apikey={ApiKey}"))

                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    string body = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true
                    };
                    using (JsonDocument document = JsonDocument.Parse(body))
                    {
                        foreach (JsonElement element in document.RootElement.EnumerateArray())
                        {
                            responseList.Add(element.GetProperty("symbol").ToString());
                        }
                    }
                }
            }

            return responseList;
        }

        //<param > YYYY-MM-DD
        public async Task<List<string>> GetStockPriceFromTo(string from, string to)
        {
            string query = $"https://financialmodelingprep.com/api/v3/historical-price-full/{Symbol}?from={{from}}&to={{to}}&apikey={ApiKey}";

            List<string> responseList = new List<string>();
            using (HttpClient httpClient = new HttpClient())

            {
                using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), query))
                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    string body = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true
                    };
                    using (JsonDocument document = JsonDocument.Parse(body))
                    {
                        foreach (JsonElement element in document.RootElement.EnumerateArray())
                        {
                            responseList.Add(element.GetProperty("symbol").ToString());
                        }
                    }
                }
            }

            return responseList;
        }

        public async Task<List<string>> GetStockPriceToday(string from, string to)
        {
            string date = $"{DateTime.Today:yyyy-MM-dd}";
            string query = $"https://financialmodelingprep.com/api/v3/historical-chart/1hour/AAPL?from={date}&apikey={ApiKey}";

            List<string> responseList = new List<string>();
            using (HttpClient httpClient = new HttpClient())

            {
                using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), query))
                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    string body = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true
                    };
                    using (JsonDocument document = JsonDocument.Parse(body))
                    {
                        foreach (JsonElement element in document.RootElement.EnumerateArray())
                        {
                            responseList.Add(element.ToString());
                        }
                    }
                }
            }

            return responseList;
        }


        public async Task<List<string>> GetStockPriceLastNTrades(int trades)
        {
            string query = $"https://financialmodelingprep.com/api/v3/historical-price-full/{Symbol}?timeseries={trades}&apikey={ApiKey}";

            List<string> responseList = new List<string>();
            using (HttpClient httpClient = new HttpClient())

            {
                using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), query))
                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    string body = await response.Content.ReadAsStringAsync();

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true
                    };
                    using (JsonDocument document = JsonDocument.Parse(body))
                    {
                        foreach (JsonElement element in document.RootElement.EnumerateArray())
                        {
                            responseList.Add(element.ToString());
                        }
                    }
                }
            }

            return responseList;
        }


    }

}