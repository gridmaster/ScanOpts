// -----------------------------------------------------------------------
// <copyright file="IOCContainer.cs" company="FF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Ninject;
using Ninject.Modules;

namespace DIContainer
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IOCContainer
    {
        private IKernel kernel = null;
        private static IOCContainer instance = null;

        #region Properties
        public static IOCContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IOCContainer();
                }

                return instance;
            }
        }

        public bool IsInitialized { get { return (kernel != null); } }

        #endregion
        
        public void Initialize(NinjectSettings settings, params INinjectModule[] modules)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("The DI Container is already initialized.");
            }
            kernel = new StandardKernel(settings, modules);
        }

        public T Get<T>()
        {
            VerifyInitialization();

            return kernel.Get<T>();
        }

        public object Get(Type type)
        {
            VerifyInitialization();

            return kernel.Get(type);
        }

        #region Private Methods
        private void VerifyInitialization()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("The DI Container has not yet been initialized.");
            }
        }
        #endregion
    }
}
