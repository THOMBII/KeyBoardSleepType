using KeyBoardSleepType.models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KeyBoardSleepType.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly InputModel _inputModel;
        private bool cheack;
        private bool cheackBackSpace = false;

        public List<string> Lines { get; set; } = new List<string> { "", "", "", "", "" };


        private Dictionary<char, string> _RU_Keys = new Dictionary<char, string> {
            { 'й', "key_14" },  {'ц', "key_15"}, {'у', "key_16"}, {'к', "key_17"}, {'е', "key_18"}, {'н', "key_19"}, {'г', "key_20"}, {'ш', "key_21"}, {'щ', "key_22"}, {'з', "key_23"}, {'х', "key_24"}, {'ъ', "key_25"},
            {'ф', "key_28" }, {'ы', "key_29"}, {'в', "key_30"}, {'а', "key_31"}, {'п', "key_32"}, {'р', "key_33"}, {'о', "key_34"}, {'л', "key_35"}, {'д', "key_36"}, {'ж', "key_37"}, {'э', "key_38"},
            {'я', "key_39" }, {'ч', "key_40" }, {'с', "key_41" }, {'м', "key_42" }, {'и', "key_43" }, {'т', "key_44" }, {'ь', "key_45" }, {'б', "key_46" }, {'ю', "key_47" }, {'ё', "key_1"},
            {',', "key_48" }, {' ', "key_50"}, {'.', "key_48"}, {'-', "key_12"}, {'¥' , "Ent"}
        };



        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context, InputModel inputModel)
        {
            _context = context;
            _logger = logger;
            _inputModel = inputModel;
        }

        public void OnGet()
        {
            HttpContext.Session.SetInt32("CountWords", _inputModel.CountWords);
            HttpContext.Session.SetInt32("ErrorCount", _inputModel.ErrorCount);

            ElementsFromDb();
        }


        public ActionResult OnPostCheackLetter([FromForm] string inputData = "", [FromForm] int LengthWord = 0)
        {
            var str = HttpContext.Session.GetString("Lines_0");
            var err = HttpContext.Session.GetInt32("ErrorCount");
            string answer = "";

            try
            {
                if (str != null)
                {
                    if (inputData == null)
                        inputData = " ";

                    if (inputData != str[LengthWord].ToString())
                    {
                        if (err == 0)
                            err = LengthWord;

                        else if (inputData == str[err.Value].ToString())
                            err = 0;

                        answer = "key_BackSpace";
                    }

                    else if (err == 0 || err > LengthWord)
                    {
                        err = 0;
                        answer = "good";
                    }

                    else
                        answer = "key_BackSpace";

                    HttpContext.Session.SetInt32("ErrorCount", err.Value);
                    return new JsonResult(answer);
                }

            }catch
            {
                throw new Exception();
            }

            return Page();
        }

        public IActionResult OnPostCheackEnter()
        {
            //uif(StringComparativeSymbols)

            //string answer = "";

            try
            {
                if (HttpContext.Session.GetString("Lines_1") != "" && HttpContext.Session.GetString("Lines_1") != null)
                {

                    for (int i = 0; i < 4; i++)
                    {
                        Lines[i] = HttpContext.Session.GetString("Lines_" + (i + 1));

                        if (Lines[i] != null && Lines[i] != "")
                            HttpContext.Session.SetString("Lines_" + i, Lines[i]);
                        else
                            HttpContext.Session.SetString("Lines_" + i, "");
                    }
                }
                else
                    ElementsFromDb();
            }catch
            {
                throw new Exception();
            }


            //string[] answer;
            //StringComparativeSymbols = StringComparativeSymbols.Remove(0, CountLettersInInputData + 2);

                //if (StringComparativeSymbols == "" || StringComparativeSymbols == " ")
                //{
                //    //HttpContext.Session.SetInt32("CountWords", CountWords++);
                //    ElementsFromDb();
                //}

                //answer = new string[] { StringComparativeSymbols, ErrorCount.ToString() };
                //ErrorCount = 0;

                //answer = Lines[0].ToString();
            return new JsonResult(Lines);
           
        }


        private void ElementsFromDb()
        {

            var CountW = HttpContext.Session.GetInt32("CountWords") + 1;
            var TextForCompare = _context.Texts.Find(CountW);

            for(int i = 0; i < 5; i++)
            {
                HttpContext.Session.SetString("Lines_" + i, "");
            }

            if (TextForCompare != null)
            {
                //#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                string[] strings = TextForCompare.MainText.Split(" ");
                //#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

                int i = 0;
                foreach(string s in strings)
                {
                    if (Lines[i].Length <= 80)
                        Lines[i] += s + " ";
                    else
                    {
                        i++;
                        Lines[i] += s + " ";                     
                    }
                }

                if (Lines[i] != null && Lines[i] != "")
                    Lines[i] += ".";
                else
                    Lines[i-1] += ".";

                for (int j = 0; j <= i; j++)
                {
                    HttpContext.Session.SetString("Lines_" + j, Lines[j]);
                }
            }
            HttpContext.Session.SetInt32("CountWords", CountW.Value);   //Нужно как то избавится от предупреждения 
        }

        
    }
}