using Core.Interface;
using Logger;
using Ninject.Modules;
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

            //    Bind<IMyFakeService>().To<MyFakeService>().InSingletonScope();
            //    Bind<IMyOtherService>().To<MyOtherService>().InSingletonScope();
        }
    }
}
