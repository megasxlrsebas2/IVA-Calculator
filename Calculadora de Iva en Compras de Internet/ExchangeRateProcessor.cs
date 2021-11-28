using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Calculadora_de_Iva_en_Compras_de_Internet
{
    public class ExchangeRateProcessor
    {
         
        public async Task<ExchangeRateModel> LoadExchangeRates()
        {
            // Request for PYG and USD exchange rates
            string url = "https://api.currencyfreaks.com/latest?apikey=5509a6a67c7945fd978259cf877a401b&symbols=PYG,USD";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url)) // This makes the call to API
            {
                if (response.IsSuccessStatusCode)
                {
                    // ReadAsAsync uses Newtonsoft to try and convert it to the type that you pass it (exchangeRateModel)
                    // It ignores data that it doesnt have properties for
                    // "Rates" is the parent property of PYG and USD in the api json, so we first look trough our RatesModel data type 
                    RatesModel exchange = await response.Content.ReadAsAsync<RatesModel>();

                    return exchange.Rates; //returns the contents of the property Rates, which is PYG and USD
                }
                else
                {
                    throw new Exception(response.ReasonPhrase); // Throws error if theres problem in data
                }
            }
        }
    }
}
