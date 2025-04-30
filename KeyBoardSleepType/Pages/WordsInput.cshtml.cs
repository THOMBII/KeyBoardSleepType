using KeyBoardSleepType.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

namespace KeyBoardSleepType.Pages
{
    public class WordsInputModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly InputModel _inputModel;
        private readonly IndexModel _index;
        private Random random = new Random();

        public List<string> Lines { get; set; } = new List<string> { "", "", "", "", "", "", "" };

        public WordsInputModel(ApplicationDbContext context, InputModel inputModel, IndexModel index)
        {
            _context = context;
            _inputModel = inputModel;
            _index = index;
        }

        public void OnGet()
        {
            _index.ElementsFromDbWordsAsync();
        }


    }
}
