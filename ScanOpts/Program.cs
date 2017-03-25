using System;
using System.Timers;
using Ninject;
using Core.DIModule;
using Core.Interface;
using DIContainer;
using System.Collections.Generic;
using Core.ORMModels;

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

            Console.WriteLine("Start: {0}", DateTime.Now);

            Console.WriteLine(String.Format("{0}{1} Initialize DIContainer{0}", Environment.NewLine, DateTime.Now));
            InitializeDiContainer();
            IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************", Environment.NewLine);

            IOCContainer.Instance.Get<ILogger>().Info("DIContainer initialized");

            //IOCContainer.Instance.Get<ISymbolService>().LoadAllSymbolsFromAllExchanges();

            List<Symbols> symbols = IOCContainer.Instance.Get<ISymbolORMService>().GetFromDBSymbolsFromTheseExchanges(null);
            
            //IOCContainer.Instance.Get<IOptionService>().RunOptionsCollection();

            //IOCContainer.Instance.Get<IHistoryService>().RunHistoryCollection();

            IOCContainer.Instance.Get<ILogger>().Info("Start timer...");
            Timer t1 = new Timer();
            t1.Interval = (1000 * 60); // 1 minute
            t1.Elapsed += new ElapsedEventHandler(t1_Elapsed);
            t1.AutoReset = true;
            //t1.Start();

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
