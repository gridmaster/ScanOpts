using System;
using Newtonsoft.Json;
using Core.JsonModels;
using Core.Web;
using Ninject;
using Core.DIModule;
using DIContainer;
using Core.Interface;
using System.Collections.Generic;
using System.Timers;
using ORMService;
using ScanOpts.Utility;

namespace Core
{
    class Program
    {

        #region initialize DI container
        // This initializes the IOC Container and implements
        // the singleton pattern.
        private static void InitializeDiContainer()
        {
            NinjectSettings settings = new NinjectSettings
            {
                LoadExtensions = false
            };

            // change DesignTimeModule to run other implementation ProductionModule || DebugModule
            IOCContainer.Instance.Initialize(settings, new DebugModule());
        }
        #endregion Initialize DI Container

        #region tunable resources
        public static int multipliar = 100;
        public static decimal maxLoss = -0.04m;
        public static decimal maxPain = 0.92m;
        public static decimal profitSellStop = 0.99m;
        public static decimal splitTest = 200m;
        public static int numberOfMonths = -1;
        public static int maxAge = 30;

        public static DateTime startDate = new DateTime(2013, 11, 01);

        private static string dividends = "http://real-chart.finance.yahoo.com/table.csv?s=WFC&a=05&b=1&c=1972&d=04&e=29&f=2015&g=v&ignore=.csv";

        #region buySellMatrix
        public static bool buyOnOpen;
        public static bool buyOnTrigger;
        public static bool buyOnClose;
        public static bool sellOnOpen;
        public static bool sellOnTrigger;
        public static bool sellOnClose;
        #endregion buySellMatrix

        #endregion tunable resources

        static void Main(string[] args)
        {

            Console.WriteLine("Start: {0}", DateTime.Now);

            Console.WriteLine(String.Format("{0}{1} Initialize DIContainer{0}", Environment.NewLine, DateTime.Now));
            InitializeDiContainer();
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************", Environment.NewLine);
            IOCContainer.Instance.Get<ILogger>().InfoFormat("DIContainer initialized{0}", Environment.NewLine);

            IOCContainer.Instance.Get<ILogger>().InfoFormat("Start timer...", Environment.NewLine);
            Timer t1 = new Timer();
            t1.Interval = (1000 * 60); // 1 minute
            t1.Elapsed += new ElapsedEventHandler(t1_Elapsed);
            t1.AutoReset = true;
            t1.Start();

            //RunOptionsCollection(symbols);

            Console.ReadKey();
        }

        static void t1_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime scheduledRun1 = DateTime.Today.AddHours(9).AddMinutes(30);  // runs today at 9:30m.
            DateTime scheduledRun2 = DateTime.Today.AddHours(16).AddMinutes(00);  // runs today at 4:00pm.

            bool run = (DateTime.Now.Hour == scheduledRun1.Hour && DateTime.Now.Minute == scheduledRun1.Minute)
                || (DateTime.Now.Hour == scheduledRun2.Hour && DateTime.Now.Minute == scheduledRun2.Minute);

            if (run)
            {
                RunOptionsCollection();
            }
        }

        private static void RunOptionsCollection()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("RunOptionsCollection - GetSymbols");
            List<string> symbols = IOCContainer.Instance.Get<SymbolsORMService>().GetSymbols();
            RunOptionsCollection(symbols);
        }

        private static void RunOptionsCollection(List<string> symbols)
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("Start - RunOptionsCollection");
            decimal date = 1487200000; //  1487289600;

            try
            {
                foreach (string symbol in symbols) 
                {
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Get {1} page {0}", Environment.NewLine, symbol);
                    // this gets the options chain ... need the dates.
                    string uriString = "https://query2.finance.yahoo.com/v7/finance/options/{0}?formatted=true&crumb=bE4Li32tCWR&lang=en-US&region=US&straddle=true&date={1}&corsDomain=";
                    string sPage = WebPage.Get(String.Format(uriString, symbol, date));
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Page captured{1}", symbol, Environment.NewLine);

                    IOCContainer.Instance.Get<ILogger>().InfoFormat("Deserialize {0}{1}", symbol, Environment.NewLine);
                    JsonResult optionChain = JsonConvert.DeserializeObject<JsonResult>(sPage);
                    IOCContainer.Instance.Get<ILogger>().InfoFormat("{0} deserialized{1}", symbol, Environment.NewLine);

                    List<decimal> expireDates = optionChain.OptionChain.Result[0].ExpirationDates;

                    Quote quote = new Quote();
                    int newId = 0;

                    foreach (decimal eDate in expireDates)
                    {
                        IOCContainer.Instance.Get<ILogger>().InfoFormat("Get {1} page for expiration date {2}{0}", Environment.NewLine, symbol, eDate);
                        sPage = WebPage.Get(String.Format(uriString, symbol, eDate));
                        IOCContainer.Instance.Get<ILogger>().InfoFormat("Page captured{1}", symbol, Environment.NewLine);

                        IOCContainer.Instance.Get<ILogger>().InfoFormat("Deserialize {0}{1}", symbol, Environment.NewLine);
                        optionChain = JsonConvert.DeserializeObject<JsonResult>(sPage);
                        IOCContainer.Instance.Get<ILogger>().InfoFormat("{0} deserialized{1}", symbol, Environment.NewLine);

                        if (String.IsNullOrEmpty(quote.Symbol))
                        {
                            quote = IOCContainer.Instance.Get<IQuoteORMService>().ExtractAndSaveQuoteFromOptionChain(optionChain);

                            newId = IOCContainer.Instance.Get<IQuoteORMService>().Add(quote);
                        }

                        List<Straddles> wtf = optionChain.OptionChain.Result[0].Options[0].Straddles;

                        List<CallPut> callputs = IOCContainer.Instance.Get<IOptionORMService>().ExtractCallsAndPutsFromOptionChain(quote.Symbol, newId, optionChain.OptionChain.Result[0].Options[0].Straddles);

                        IOCContainer.Instance.Get<ICallPutORMService>().AddMany(callputs);
                    }
                }
            }
            catch (Exception exc)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("Sucker blew up: {0}", exc);
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().InfoFormat("End - RunOptionsCollection");
                //IOCContainer.Instance.Get<ILogger>().InfoFormat("We're done'...{0}", Environment.NewLine);
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }

        }
    }
}
