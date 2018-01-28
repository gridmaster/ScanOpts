using Core;
using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;

namespace AnalyticsService
{
    public class AnalyticsService : BaseService, IAnalyticsService
    {
        #region Private properties

        #endregion Private properties

        #region Public properties

        #endregion Public properties

        #region Constructors

        public AnalyticsService(ILogger logger) //, BulkLoadAnalyticsService bulkLoadAnalyticsService)
            : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        #endregion Constructors

        public List<string> FindRising50SMATrends(List<Symbols> symbols)
        {
            List<string> syms = new List<string>();

            for (int i = 0; i < symbols.Count; i++)
            {
                syms.Add(symbols[i].Symbol);
            }
            FindRising50SMATrends(syms);
        }

        public List<string> FindRising50SMATrends(List<string> symbols)
        {
            logger.InfoFormat("Start - FindRising50SMATrends");
            List<string> results = new List<string>();


            try
            {
                foreach (string symbol in symbols)
                {
                    logger.InfoFormat("Processing symbol {0}", symbol);


                }
            }
            catch(Exception ex)
            {
                logger.InfoFormat("Error - FindRising50SMATrends {0}", ex.Message);
            }

            logger.InfoFormat("End - FindRising50SMATrends");
            return results;
        }
    }
}
