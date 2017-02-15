using Core.Models;

namespace Core.Interface
{
    public interface IQuoteORMService
    {
        Quote ExtractAndSaveQuoteFromOptionChain(JsonResult optionsChain);
    }
}
