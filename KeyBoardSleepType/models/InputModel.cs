using Microsoft.AspNetCore.Mvc;

namespace KeyBoardSleepType.models
{
    public class InputModel
    {
        public int CountWords { get; set; } = 0;
        public int Mistake { get; set; } = 0;
        public int CountBaclLigtKey { get; set; } = 0;
        public int CountLettersInInputData { get; set; } = 0;
        public string StringComparativeSymbols { get; set; } = "";
        public int ErrorCount { get; set; } = 0;
        public int Time { get; set; } = 0;

        [BindProperty]
        public string KeyForReturn { get; set; } = "key_BackSpace";
    }
}
