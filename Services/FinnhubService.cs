using StocksApp.ServiceContacts;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using(HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
                };
                HttpResponseMessage response = await httpClient.SendAsync(request);
                Stream stream = response.Content.ReadAsStream();
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                Dictionary<string, object>? responseDict = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData);

                if (responseDict == null)
                    throw new InvalidOperationException("No response from Finnhub server");
                if (responseDict.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDict["error"]));

                return responseDict;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(){
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
                };
                HttpResponseMessage response = await httpClient.SendAsync(request);
                Stream stream = response.Content.ReadAsStream();
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                Dictionary<string, object>? responseDict = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData);

                if (responseDict == null)
                    throw new InvalidOperationException("No response from Finnhub server");
                if (responseDict.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDict["error"]));

                return responseDict;
            }
        }
    }
}
