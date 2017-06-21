﻿using System.Collections.Generic;
using Core.ORMModels;

namespace Core.Interface
{
    public interface IBollingerBandService
    {
        void RunBollingerBandsCheck();

        void RunBollingerBandsCheck(List<Symbols> symbols);

        void RunBollingerBandsCheck(List<string> symbols);
    }
}
