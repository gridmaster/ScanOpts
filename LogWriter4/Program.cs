using System;
using LogWriter4.Core.Interface;
using LogWriter4.DIModule;
using Ninject;
using DIContainer;
//using Services.Interface;

namespace LogWriter4
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
            try
            {
                InitializeDiContainer();
                DIContainer.IOCContainer.Instance.Get<ILogger>()
                    .InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
                DIContainer.IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Main's runnin'...{0}", Environment.NewLine);
                //DIContainer.IOCContainer.Instance.Get<IMyFakeService>().DoSomething(123);
                //DIContainer.IOCContainer.Instance.Get<IMyOtherService>().DoSomethingElse(456);
            }
            catch (Exception exc)
            {
                DIContainer.IOCContainer.Instance.Get<ILogger>().Fatal("Sucker blew up: {0}", exc);
            }
            finally
            {
                DIContainer.IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Main's done'...{0}", Environment.NewLine);
                DIContainer.IOCContainer.Instance.Get<ILogger>()
                    .InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);

                Console.ReadKey();
            }
        }
    }
}
