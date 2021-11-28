using System;
using System.IO;
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
            string cacheFile = "C:/Users/megas/source/repos/Calculadora Iva/Calculadora de Iva en Compras de Internet/bin/Debug/data.txt";
            string cacheFileContent = "[Empty]";

            // Check for day works, but now there's no exchange value without calling API. Store value somehow if there isnt any.
            // TO DO:
            // Check if exchange rate updated today with DateTime.Today.ToString()
            // If it did, store value in text file and dont call API
            // If its the same day but theres no value, call API

            string lastUpdated = ""; 
            

            //CheckForCacheFile();
            ApiHelper.InitializeClient();

            CheckForExchangeRateUpdate();



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

            
            ApplicationEndpoint();

            // Make it so application can loop if user wants to calc again

            // Methods
            float InputInitialPrice()
            {
                Console.WriteLine("Please enter the price of your item(s) in USD");
                var userInputedPrice = float.Parse(Console.ReadLine());
                return userInputedPrice;
            }

            void ParseRatesToFloat()
            {
                // This fails if done too quick, have to wait for API response somehow
                pygToUSDexchangeRate = float.Parse(exchangeRates.PYG);
            }

            void CalculateUsdToPYG()
            {
                finalPriceConvertedToPYG = finalUsdPrice * pygToUSDexchangeRate;
            }

            float CalculateIVA(float valueToCalculate)
            {
                calculatedIva = ivaValue * valueToCalculate;
                return calculatedIva;
            }

            // Calls API
            async void RequestExchangeRates()
            {
                exchangeRates = await exchangeRateProcessor.LoadExchangeRates();
            }

            void CheckForExchangeRateUpdate()
            {
                if (lastUpdated == DateTime.Today.ToString()) return;
                else
                {
                    Console.WriteLine("Requesting today's exchange rate...");
                    lastUpdated = DateTime.Today.ToString();
                    RequestExchangeRates();
                    SaveExchangeRateLocally();
                }
            }

            void CheckForCacheFile()
            {
                if (File.Exists(cacheFile))
                {
                    Console.WriteLine("Reading cache file...");
                    cacheFileContent = File.ReadAllText(cacheFile);
                    //Console.WriteLine(cacheFileContent);
                }
                else
                {
                    Console.WriteLine("Cache file not found!");
                    //Environment.Exit(0);
                }
                    

            }

            void SaveExchangeRateLocally()
            {
                if (File.Exists(cacheFile))
                {
                    cacheFileContent = DateTime.Today.ToString();
                    File.WriteAllText(cacheFile, cacheFileContent);
                    Console.WriteLine($"Wrote to the cache: {cacheFileContent}");
                }
                else
                    Console.WriteLine("Cache file not found!");
            }
            
            void ApplicationEndpoint()
            {
                Console.WriteLine("We're all done here! Exit? Y/N");
                string userInput = Console.ReadLine();
                if (userInput.Length > 1) { userInput = userInput.Substring(0, 1); }
                if (userInput == "Y")
                {
                    Environment.Exit(0);
                }
                else if (userInput == "N")
                {
                    // Loop app
                }
                else
                    Console.WriteLine("Not a valid response!");
            }
        }
    }
}
