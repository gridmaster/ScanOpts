using System;
using Ninject;
using Core.DIModule;
using DIContainer;
using Core.Interface;
using System.Timers;
using OptonService;

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
            IOCContainer.Instance.Get<ILogger>().Info("DIContainer initialized");

            IOCContainer.Instance.Get<ILogger>().Info("Start timer...");
            Timer t1 = new Timer();
            t1.Interval = (1000 * 60); // 1 minute
            t1.Elapsed += new ElapsedEventHandler(t1_Elapsed);
            t1.AutoReset = true;
            t1.Start();

            IOCContainer.Instance.Get<IOptionService>().RunOptionsCollection();

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
                IOCContainer.Instance.Get<IOptionService>().RunOptionsCollection();
            }
        }
    }
}
