using System.Collections.Generic;

namespace Core.Interface
{
    public interface IOptionService
    {
        void RunOptionsCollection();

        void RunOptionsCollection(List<string> symbols);
    }
}
