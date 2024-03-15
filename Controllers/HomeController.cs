using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _options;

        public HomeController(FinnhubService finnhubService,
            IOptions<TradingOptions> options)
        {
            _finnhubService = finnhubService;
            _options = options;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            string stockSymbol = _options.Value.DefaultStockSymbol ?? "MSFT";
            Dictionary<string, object>? stockQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);
            Dictionary<string, object>? stockProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
            
            Stock stock = new Stock
            {
                StockSymbol = stockSymbol,
                CompanyName = stockProfile["name"].ToString(),
                CurrentPrice = Convert.ToDouble(stockQuote["c"].ToString()),
                LowestPrice = Convert.ToDouble(stockQuote["l"].ToString()),
                HighestPrice = Convert.ToDouble(stockQuote["h"].ToString()),
                OpenPrice = Convert.ToDouble(stockQuote["o"].ToString())
            };
            return View(stock);
        }
    }
}
