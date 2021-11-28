using System;
using Newtonsoft.Json;

namespace Calculadora_de_Iva_en_Compras_de_Internet
{
    class Program
    {
        static void Main(string[] args)
        {

            ExchangeRateProcessor exchangeRateProcessor = new ExchangeRateProcessor();
            ExchangeRateModel exchangeRates = null;
            float initialUsdPrice;
            float ivaValue = 0.10f;
            float calculatedIva;
            float finalUsdPrice;
            float pygToUSDexchangeRate;
            float finalPriceConvertedToPYG;
            string formattedPYGExchangeRate;

            //To do: If application is opened in the same day, store value and dont make API call (Use DateTime), instead use stored value
            
            ApiHelper.InitializeClient();
            RequestExchangeRates();
            
            
            Console.WriteLine("Welcome to SebaSoft's IVA calculator");
            Console.WriteLine("Where we tell you what you ACTUALLY pay online!");
  
            initialUsdPrice = InputInitialPrice();
            CalculateIVA(initialUsdPrice);
            finalUsdPrice = initialUsdPrice + calculatedIva;
            ParseRatesToFloat();
            CalculateUsdToPYG();
          
            // Info to User
            Console.WriteLine($"Your initial price is {initialUsdPrice} USD");
            Console.WriteLine($"Your IVA cost is: {calculatedIva} USD");
            Console.WriteLine($"Your real total cost is {finalUsdPrice} USD");
            // To do: Apply String.Format so that exchange rate looks good, add . between numbers
            // Might need some logic: If the string has more than 3, 6 or 9 digits, format a "." in.
            Console.WriteLine($"Which is {finalPriceConvertedToPYG} PYG");
            
            formattedPYGExchangeRate = exchangeRates.PYG.Substring(0, 4);
            Console.WriteLine($"Today's exchange rate is {formattedPYGExchangeRate} PYG, per {exchangeRates.USD} USD");
            
            // Application endpoint
            Console.ReadLine();

            // Methods
            float InputInitialPrice()
            {
                Console.WriteLine("Please enter the price of your item(s) in USD");
                var userInputedPrice = float.Parse(Console.ReadLine());
                return userInputedPrice;
            }

            void ParseRatesToFloat()
            {
                pygToUSDexchangeRate = float.Parse(exchangeRates.PYG);
            }

            void CalculateUsdToPYG()
            {
                finalPriceConvertedToPYG = finalUsdPrice * pygToUSDexchangeRate;
            }

            float CalculateIVA(float valueToCalculate)
            {
               calculatedIva = ivaValue* valueToCalculate;
               return calculatedIva;
            }

            // Calls API
            async void RequestExchangeRates()
            { 
                exchangeRates = await exchangeRateProcessor.LoadExchangeRates();
            }
        }
    }
}
