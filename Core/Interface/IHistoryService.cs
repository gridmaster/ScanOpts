using System.Collections.Generic;

namespace Core.Interface
{
    public interface IHistoryService
    {
        void RunHistoryCollection();

        void RunHistoryCollection(List<string> symbols);
    }
}
