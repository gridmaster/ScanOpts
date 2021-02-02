using BollingerBandService;
using SMA60CycleService;
using Core.Interface;
using DailySymbolService;
using Logger;
using Ninject.Modules;
using OptonService;
using ORMService;
using SymbolHistoryService;
using DataAnalyticsService;

namespace ScanOpts.DIModule
{
    class DebugModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Log4NetLogger>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Debug);

            Bind<IStatisticORMService>().To<StatisticORMService>().InSingletonScope();
            Bind<IOptionORMService>().To<OptionORMService>().InSingletonScope();
            Bind<ICallPutORMService>().To<CallPutORMService>().InSingletonScope();
            Bind<ISymbolORMService>().To<SymbolsORMService>().InSingletonScope();
            Bind<IOptionService>().To<OptionService>().InSingletonScope();
            Bind<IDailyQuotesORMService>().To<DailyQuotesORMService>().InSingletonScope();
            Bind<IHistoryService>().To<HistoryService>().InSingletonScope();
            Bind<ISymbolService>().To<SymbolService>().InSingletonScope();
            Bind<IExchangeORMService>().To<ExchangeORMService>().InSingletonScope();

            Bind<ISMA60CycleService>().To<SMA60CyclesService>().InSingletonScope();
            Bind<IBollingerBandService>().To<BollingerBandsService>().InSingletonScope();

            Bind<IBollingerBandORMService>().To<BollingerBandORMService>().InSingletonScope();
            Bind<IAnalyticsService>().To<AnalyticsService>().InSingletonScope();
        }
    }
}
