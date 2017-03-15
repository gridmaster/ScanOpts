using Core.Interface;
using DailySymbolService;
using Logger;
using Ninject.Modules;
using OptonService;
using ORMService;
using SymbolHistoryService;

namespace Core.DIModule
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
            Bind<IHistoryService>().To<HistoryService>().InSingletonScope();
            Bind<IDailyQuotesORMService>().To<DailyQuotesORMService>().InSingletonScope();
            Bind<ISymbolService>().To<SymbolService>().InSingletonScope();
        }
    }
}
