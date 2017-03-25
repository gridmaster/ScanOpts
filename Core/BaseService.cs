using System;
using System.Runtime.CompilerServices;
using Core.Interface;

namespace Core
{
    public abstract class BaseService
    {
        #region Fields
        protected readonly ILogger logger;
        #endregion

        #region Properties

        public bool IsInitialized { get; protected set; }

        #endregion

        #region Protected/Private Methods
        protected BaseService(ILogger logger)
        {
            this.logger = logger;
        }

        protected void ThrowIfNotInitialized([CallerMemberName] string methodName = null)
        {
            if (IsInitialized)
            {
                return;
            }
            string errorMessage = string.Format(
                "The {0}() method cannot be executed because the {1} has not yet been initialized.",
                methodName,
                GetType().Name);

            logger.Error(errorMessage);
            throw new Exception(errorMessage);
        }

        protected void ThrowIfIsInitialized([CallerMemberName] string methodName = null)
        {
            if (!IsInitialized)
            {
                return;
            }

            string errorMessage = string.Format(
                "The {0}() method cannot be executed because the {1} has already been initialized.",
                methodName,
                GetType().Name);

            logger.Error(errorMessage);
            throw new Exception(errorMessage);
        }
        #endregion

        // was protected as a base class
        public string GetThisMethodName([CallerMemberName] string methodName = null)
        {
            return string.Format("{0}.{1}", GetType().Name, (methodName ?? "?"));
        }
    }
}

