using KeyBoardSleepType.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace KeyBoardSleepType.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly InputModel _inputModel;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Random random = new Random();
        public string[] LinesToSession = new string[5];

        public List<string> Lines { get; set; } = new List<string> { "", "", "", "", "", "", "" };
 

        private Dictionary<char, string> _RU_Keys = new Dictionary<char, string> {
            {'й', "key_14" }, {'ц', "key_15"}, {'у', "key_16"}, {'к', "key_17"}, {'е', "key_18"}, {'н', "key_19"}, {'г', "key_20"}, {'ш', "key_21"}, {'щ', "key_22"}, {'з', "key_23"}, {'х', "key_24"}, {'ъ', "key_25"},
            {'ф', "key_28" }, {'ы', "key_29"}, {'в', "key_30"}, {'а', "key_31"}, {'п', "key_32"}, {'р', "key_33"}, {'о', "key_34"}, {'л', "key_35"}, {'д', "key_36"}, {'ж', "key_37"}, {'э', "key_38"},
            {'я', "key_39" }, {'ч', "key_40"}, {'с', "key_41" }, {'м', "key_42" }, {'и', "key_43" }, {'т', "key_44" }, {'ь', "key_45" }, {'б', "key_46" }, {'ю', "key_47" }, {'ё', "key_1"},
            {',', "key_48" }, {' ', "key_50"}, {'.', "key_48"}, {'-', "key_12"}, {'¥' , "Ent"},
        };



        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context, InputModel inputModel, IHttpContextAccessor HttpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _inputModel = inputModel;
            _httpContextAccessor = HttpContextAccessor;
        }

        public async void OnGet(string wordPart)
            {
                if (HttpContext.Request.Path == "/")
            {
                HttpContext.Session.SetInt32("CountWords", _inputModel.CountWords);
                HttpContext.Session.SetInt32("ErrorCount", _inputModel.ErrorCount);

                await ElementsFromDb();
            }
            else
                await ElementsFromDbWordsAsync();
        }
        public async void OnGetWord()
        {
            await ElementsFromDbWordsAsync();
        }

        public ActionResult OnPostCheackLetter([FromForm] string inputData = "", [FromForm] int LengthWord = 0)
         {
            var str = HttpContext.Session.GetString("Lines_0");
            var err = HttpContext.Session.GetInt32("ErrorCount");
            List<string> answer = new List<string> { "", "" };

            try
            {
                if (str != null)
                {
                    if (inputData == null)
                        inputData = " ";

                    if (inputData.ToLower() != str[LengthWord].ToString().ToLower())
                    {
                        if (err == 0)
                            err = LengthWord;

                        else if (inputData.ToLower() == str[err.Value].ToString().ToLower())
                            err = 0;

                        answer[0] = "key_BackSpace";
                    }

                    else if (err == 0 || err > LengthWord)
                    {
                        answer[0] = _RU_Keys[str.ToLower()[LengthWord + 1]];
                    }

                    else
                        answer[0] = "key_BackSpace";

                    HttpContext.Session.SetInt32("ErrorCount", err.Value);

                   if (LengthWord >= 1)
                        answer[1] = _RU_Keys[str.ToLower()[LengthWord]];
                    else
                        answer[1] = "key_BackSpace";
                    return new JsonResult(answer);
                }

            }catch
            {
                throw new Exception();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCheackEnter([FromForm]string page)
        {
            Console.WriteLine(page);
            try
            {
                if (HttpContext.Session.GetString("Lines_1") != "" && HttpContext.Session.GetString("Lines_1") != null)
                {

                    for (int i = 0; i < 4; i++)
                    {
                        LinesToSession[i] = HttpContext.Session.GetString("Lines_" + (i + 1));

                        if (LinesToSession[i] != null && LinesToSession[i] != "")
                            HttpContext.Session.SetString("Lines_" + i, LinesToSession[i]);
                        else
                            HttpContext.Session.SetString("Lines_" + i, "");
                    }
                }
                else
                    switch (page)
                    {
                        case "WordsInput":
                            await ElementsFromDbWordsAsync();
                            break;
                        case "CountWordsInput":
                            await ElementsFromDbWordsAsync();
                            break;
                        default:
                            await ElementsFromDb();
                            break;

                    }
                    
            }catch
            {
                throw new Exception();
            }

            return new JsonResult(LinesToSession);
            
        }

        public IActionResult OnPostCheackBackSpace([FromForm] string inputData = "", [FromForm] int LengthWord = 0)
        {
           var str = HttpContext.Session.GetString("Lines_0");
            List<string> answer = new List<string> { "", "" };

            answer[0] = _RU_Keys[str.ToLower()[LengthWord-1]];
            answer[1] = _RU_Keys[str.ToLower()[LengthWord]];

            return new JsonResult(answer);
        }


        private async Task ElementsFromDb()
        {
            try
            {
                EmptySessionStrings();
                var countW = (HttpContext.Session.GetInt32("CountWords") ?? 0) + 1;
                HttpContext.Session.SetInt32("CountWords", countW);

                var TextForCompare = await _context.Texts
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(t => t.id == countW);

                //#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                List<string> strings = TextForCompare.MainText.Split(" ").ToList();
                //#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

                int currentLine = 0;
                WorkingTextFromDatabase(strings, ref currentLine);


                // Сохраняем строки в сессию
                for (int j = 0; j < LinesToSession.Length && j <= currentLine; j++)
                {
                    if (!string.IsNullOrEmpty(LinesToSession[j]))
                    {
                        HttpContext.Session.SetString($"Lines_{j}", LinesToSession[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку вместо пустого catch
                _logger.LogError(ex, "Ошибка в ElementsFromDbWordsAsync");
                throw; // Перебрасываем исключение дальше
            }
        }


        public async Task ElementsFromDbWordsAsync()  // Изменили возвращаемый тип на Task
        {
            try
            {
                EmptySessionStrings();
                // Получаем значение из сессии или используем 0 по умолчанию
                var countW = (HttpContext.Session.GetInt32("CountWords") ?? 0) + 1;
                HttpContext.Session.SetInt32("CountWords", countW);


                 var allWords = await _context.Words.AsNoTracking().ToListAsync();

                List<string> textForCompare = allWords
                    .OrderBy(x => random.Next())
                    .Take(10)
                    .Select(x => x.word)
                    .ToList();

                int currentLine = 0;
                WorkingTextFromDatabase(textForCompare, ref currentLine);

                // Сохраняем строки в сессию
                for (int j = 0; j < LinesToSession.Length && j <= currentLine; j++)
                {
                    if (!string.IsNullOrEmpty(LinesToSession[j]))
                    {
                        HttpContext.Session.SetString($"Lines_{j}", LinesToSession[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку вместо пустого catch
                _logger.LogError(ex, "Ошибка в ElementsFromDbWordsAsync");
                throw; // Перебрасываем исключение дальше
            }
        }

        private void WorkingTextFromDatabase(List<string> textForCompare, ref int currentLine)
        {
            var context = _httpContextAccessor.HttpContext;

            if (textForCompare != null && textForCompare.Count > 0)
            {
                foreach (string word in textForCompare)
                {
                    // Проверяем, чтобы не выйти за границы массива
                    if (currentLine >= LinesToSession.Length)
                        break;

                    // Проверяем длину строки
                    if ((LinesToSession[currentLine].Length + word.Length + 1) <= 80) // +1 для пробела
                    {
                        LinesToSession[currentLine] += word + " ";
                    }
                    else
                    {
                        // Удаляем последний пробел, если он есть
                        if (!string.IsNullOrEmpty(LinesToSession[currentLine]))
                        {
                            LinesToSession[currentLine] = LinesToSession[currentLine].TrimEnd();
                        }

                        currentLine++;

                        // Проверяем границы массива перед добавлением нового слова
                        if (currentLine < LinesToSession.Length)
                        {
                            LinesToSession[currentLine] = word + " ";
                        }
                    }
                }

                // Обработка последней строки
                if (currentLine < LinesToSession.Length && !string.IsNullOrEmpty(LinesToSession[currentLine]))
                {
                    // Удаляем последний пробел и добавляем точку
                    LinesToSession[currentLine] = LinesToSession[currentLine].TrimEnd() + ". ";
                }
                else if (currentLine > 0 && currentLine < LinesToSession.Length && string.IsNullOrEmpty(LinesToSession[currentLine]))
                {
                    // Обработка случая, когда последняя строка пустая
                    LinesToSession[currentLine - 1] = LinesToSession[currentLine - 1].TrimEnd() + ". ";
                }
            }
        }

        void EmptySessionStrings()
        {
            // Очищаем строки в сессии и массиве
            for (int i = 0; i < LinesToSession.Length; i++)
            {
                LinesToSession[i] = "";
                HttpContext.Session.SetString($"Lines_{i}", string.Empty);
            }
        }
    } 
}