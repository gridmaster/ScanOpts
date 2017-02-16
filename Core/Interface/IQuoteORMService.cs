using Core.JsonModels;

namespace Core.Interface
{
    public interface IQuoteORMService
    {
        Quote ExtractAndSaveQuoteFromOptionChain(JsonResult optionsChain);
    }
}
