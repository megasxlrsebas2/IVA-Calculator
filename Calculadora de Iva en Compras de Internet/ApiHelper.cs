using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Calculadora_de_Iva_en_Compras_de_Internet
{
    public static class ApiHelper
    {
        // Static because we run it once per application, its like opening one browser
        public static HttpClient ApiClient { get; set; }

        //This basically sets up the interneting our program will do
        public static void InitializeClient()
        {
            ApiClient =  new HttpClient();
            ApiClient.BaseAddress = new Uri("https://api.currencyfreaks.com/latest?apikey=5509a6a67c7945fd978259cf877a401b");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
