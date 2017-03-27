using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IHistoryService
    {
        void RunHistoryCollection();

        void RunHistoryCollection(List<string> symbols);

        void RunHistoryCollection(List<Symbols> symbols);
    }
}
