using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskCalculator
{
    public class Calculator
    {
        class FxPair
        {
            internal string Pair;
            internal double Rate;
        }

        List<FxPair> fxPairs = new List<FxPair>();

        public Calculator()
        {
            //String[] pairs = File.ReadAllLines(@"C:\Users\mtaylor\Dropbox\MT4\FxRates.txt");

            //foreach (string p in pairs)
            //{
            //    string[] columnData = p.Split('\t');
            //    FxPair pair = new FxPair() { Pair = columnData[0], Rate = double.Parse(columnData[1]) };
            //    fxPairs.Add(pair);
            //}

            ReadExchangeRatesFromAzure();
        }

        double GetExchangeRateForAccountCurrency(string accountCurrency, string pair)
        {
            double rate = 1;
            string quote = GetQuote(pair);
            string quoteBase = (quote + accountCurrency).ToUpper();

            if (accountCurrency != quote)
            {
                FxPair foundPair = (from fx in fxPairs
                                    where fx.Pair == quoteBase
                                    select fx).FirstOrDefault();

                rate = foundPair.Rate;
            }

            return rate;
        }

        string GetQuote(string pair)
        {
            string quote = pair.Substring(3, 3);

            if (quote == "CNH")
                quote = "CNY";

            return quote;
        }

        byte GetNumberOfDigits(string quote)
        {
            if ((quote == "JPY") || (quote == "HUF") || (quote == "THB"))
                return 3;
            else
                return 5;
        }

        public double CalcRisk(string accountCurrency,
            string pair,
            double pips,
            double lotSizeInUnits)
        {
            double rate = GetExchangeRateForAccountCurrency(accountCurrency, pair);
            string quote = GetQuote(pair);
            byte digits = GetNumberOfDigits(quote);

            if (digits == 5)
                pips = (pips * 0.0001);
            else
                pips = (pips * 0.01);

            double risk = pips * rate * lotSizeInUnits;
            return Math.Round(risk, 2);
        }

        //public void UploadExchangeRatesToAzure()
        //{
        //    CloudStorageAccount storageAccount = Common
        //        .CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        //    CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();
        //    CloudFileShare cloudFileShare = cloudFileClient.GetShareReference("exchangerates");
        //    CloudFileDirectory rootDirectory = cloudFileShare.GetRootDirectoryReference();
        //    string ratesFile = "FxRates.txt";
        //    CloudFile cloudFile = rootDirectory.GetFileReference(ratesFile);
        //    cloudFile.UploadFromFile(@"C:\Users\mtaylor\Dropbox\MT4\FxRates.txt");
        //}

        string ReadExchangeRatesFromAzure()
        {
            //CloudStorageAccount storageAccount = Common
            //    .CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            //CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();
            //CloudFileShare cloudFileShare = cloudFileClient.GetShareReference("exchangerates");
            //CloudFileDirectory rootDirectory = cloudFileShare.GetRootDirectoryReference();
            //string ratesFile = "FxRates.txt";
            //CloudFile cloudFile = rootDirectory.GetFileReference(ratesFile);

            string contents = File.ReadAllText(@"FxRates.txt");

            //using (StreamReader sr = new StreamReader(cloudFile.OpenRead(), Encoding.ASCII))
            //{
            //    contents = sr.ReadToEnd();
            //}

            string[] pairs = contents.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string p in pairs)
            {
                string[] columnData = p.Split('\t');
                FxPair pair = new FxPair() { Pair = columnData[0], Rate = double.Parse(columnData[1]) };
                fxPairs.Add(pair);
            }

            return contents;
        }
    }
}
