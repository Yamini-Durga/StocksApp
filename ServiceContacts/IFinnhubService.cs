﻿namespace StocksApp.ServiceContacts
{
    public interface IFinnhubService
    {
        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    }
}
