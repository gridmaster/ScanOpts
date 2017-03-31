using Core.ORMModels;
using System.Collections.Generic;

namespace Core.Interface
{
    public interface IOptionService
    {
        void RunOptionsCollection();

        void RunOptionsCollection(List<Symbols> symbols);

        void RunOptionsCollection(List<string> symbols);
    }
}
