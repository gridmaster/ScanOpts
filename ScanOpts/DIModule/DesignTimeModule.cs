using Logger;
using LogWriter4.Core.Interface;
//using Services.Interface;
//using Services.Service;
using Ninject.Modules;

namespace LogWriter4.DIModule
{
    public class DesignTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Log4NetWrapper>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Info);

            //Bind<IMyFakeService>().To<MyFakeService>().InSingletonScope();
            //Bind<IMyOtherService>().To<MyOtherService>().InSingletonScope();
        }
    }
}
