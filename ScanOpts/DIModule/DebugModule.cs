using Core.Interface;
using Logger;
using Ninject.Modules;
using OptonService;
using ORMService;

namespace Core.DIModule
{
    class DebugModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Log4NetLogger>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Debug);

            Bind<IQuoteORMService>().To<QuoteORMService>().InSingletonScope();
            Bind<IOptionORMService>().To<OptionORMService>().InSingletonScope();
            Bind<ICallPutORMService>().To<CallPutORMService>().InSingletonScope();
            Bind<ISymbolORMService>().To<SymbolsORMService>().InSingletonScope();
            Bind<IOptionService>().To<OptioinService>().InSingletonScope();
        }
    }
}
