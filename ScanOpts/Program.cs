using System;
using System.Timers;
using Ninject;
using Core.Interface;
using DIContainer;
using System.Collections.Generic;
using Core.ORMModels;
using Core.JsonQuoteSummary;
using Newtonsoft.Json;
using Core.JsonKeyStatistics;
using ScanOpts.DIModule;

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

        static void Main(string[] args)
        {
            var start = DateTime.Now;
            Console.WriteLine("Start: {0}", start);

            Console.WriteLine(String.Format("{0}{1} Initialize DIContainer{0}", Environment.NewLine, DateTime.Now));
            InitializeDiContainer();
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************", Environment.NewLine);

            IOCContainer.Instance.Get<ILogger>().Info("DIContainer initialized");

            /*********************************************************************************/

            // holders
            //string url = "https://query2.finance.yahoo.com/v10/finance/quoteSummary/PLCE?formatted=true&crumb=0xiMyBSKbKe&lang=en-US&region=US&modules=institutionOwnership%2CfundOwnership%2CmajorDirectHolders%2CmajorHoldersBreakdown%2CinsiderTransactions%2CinsiderHolders%2CnetSharePurchaseActivity&corsDomain=finance.yahoo.com";
            //string sPage = WebPage.Get(url);
            //RootObject quoteSummary = JsonConvert.DeserializeObject<RootObject>(sPage);

            //// key statistics
            //string url1 = "https://query1.finance.yahoo.com/v10/finance/quoteSummary/CAT?formatted=true&crumb=0xiMyBSKbKe&lang=en-US&region=US&modules=defaultKeyStatistics%2CfinancialData%2CcalendarEvents&corsDomain=finance.yahoo.com";
            //string sPage1 = WebPage.Get(url1);
            //BaseObject.RootObject bo = JsonConvert.DeserializeObject<BaseObject.RootObject>(sPage1);

            //var wtf = quoteSummary;

            /*********************************************************************************/
            
            IOCContainer.Instance.Get<ISymbolService>().LoadAllSymbolsFromUSExchangesNoSave();

            List<Symbols> symbols = IOCContainer.Instance.Get<ISymbolService>().GetSymbols();

            IOCContainer.Instance.Get<IBollingerBandService>().RunBollingerBandsCheck(symbols);

            //IOCContainer.Instance.Get<ISymbolService>().LoadAllSymbolsFromAllExchanges();

            //List<Symbols> symbols = IOCContainer.Instance.Get<ISymbolORMService>().GetFromDBSymbolsFromTheseExchanges(null);

            //IOCContainer.Instance.Get<IHistoryService>().RunHistoryCollection(symbols);

            //IOCContainer.Instance.Get<IOptionService>().RunOptionsCollection(symbols);

            //IOCContainer.Instance.Get<IHistoryService>().RunHistoryCollection();

            //IOCContainer.Instance.Get<ILogger>().Info("Start timer...");
            //Timer t1 = new Timer();
            //t1.Interval = (1000 * 60); // 1 minute
            //t1.Elapsed += new ElapsedEventHandler(t1_Elapsed);
            //t1.AutoReset = true;
            //t1.Start();

            var end = DateTime.Now;
            Console.WriteLine("Start: {0}", start);
            Console.WriteLine("End: {0}", end);

            Console.ReadKey();
        }

        static void t1_Elapsed(object sender, ElapsedEventArgs e)
        {
            return;

            DateTime scheduledRun1 = DateTime.Today.AddHours(9).AddMinutes(30);  // runs today at 9:30m.
            DateTime scheduledRun2 = DateTime.Today.AddHours(16).AddMinutes(00);  // runs today at 4:00pm.

            bool run = (DateTime.Now.Hour == scheduledRun1.Hour && DateTime.Now.Minute == scheduledRun1.Minute)
                || (DateTime.Now.Hour == scheduledRun2.Hour && DateTime.Now.Minute == scheduledRun2.Minute);

            if (run)
            {
                IOCContainer.Instance.Get<IOptionService>().RunOptionsCollection();
            }
        }
    }
}
