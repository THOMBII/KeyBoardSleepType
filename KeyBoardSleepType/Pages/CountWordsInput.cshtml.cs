using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeyBoardSleepType.Pages
{
    public class CountWordsInputModel : PageModel
    {
        private readonly IndexModel _index;

        public CountWordsInputModel(IndexModel indexModel)
        {
            _index = indexModel;
        }
        public void OnGet()
        {
            _index.ElementsFromDbWordsAsync();
        }
    }
}
