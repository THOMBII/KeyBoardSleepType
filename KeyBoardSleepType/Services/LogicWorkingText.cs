//using Microsoft.EntityFrameworkCore;

//namespace KeyBoardSleepType.Services
//{
//    public class LogicWorkingText
//    {
//        private void ElementsFromDb()
//        {

//            //var CountW = HttpContext.Session.GetInt32("CountWords") + 1;
//            //var TextForCompare = _context.Texts.Find(CountW);

//            //for (int i = 0; i < 5; i++)
//            //{
//            //    HttpContext.Session.SetString("Lines_" + i, "");
//            //}

//            //if (TextForCompare != null)
//            //{
//            //    //#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
//            //    string[] strings = TextForCompare.MainText.Split(" ");
//            //    //#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

//            //    int i = 0;
//            //    foreach (string s in strings)
//            //    {
//            //        if (Lines[i].Length <= 80)
//            //            Lines[i] += s + " ";
//            //        else
//            //        {
//            //            i++;
//            //            Lines[i] += s + " ";
//            //        }
//            //    }

//            //    if (Lines[i] != null && Lines[i] != "")
//            //    {

//            //        Lines[i] = Lines[i].Remove(Lines[i].Length - 1) + ". ";
//            //    }
//            //    else
//            //        Lines[i - 1] = Lines[i].Remove(Lines[i].Length - 1) + ". ";

//            //    for (int j = 0; j <= i; j++)
//            //    {
//            //        HttpContext.Session.SetString("Lines_" + j, Lines[j]);
//            //    }
//            //}
//            //HttpContext.Session.SetInt32("CountWords", CountW.Value);   //Нужно как то избавится от предупреждения 
//        }

//        private public List<string> ElementsFromDb(int CountW)
//        {
//            List<string> Lines = new List<string> { "", "", "", "", "", "", "" };

//            var TextForCompare = _context.Texts.Find(CountW);

//            for (int i = 0; i < 5; i++)
//            {
//                HttpContext.Session.SetString("Lines_" + i, "");
//            }

//            if (TextForCompare != null)
//            {
//                //#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
//                string[] strings = TextForCompare.MainText.Split(" ");
//                //#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

//                int i = 0;
//                foreach (string s in strings)
//                {
//                    if (Lines[i].Length <= 80)
//                        Lines[i] += s + " ";
//                    else
//                    {
//                        i++;
//                        Lines[i] += s + " ";
//                    }
//                }

//                if (Lines[i] != null && Lines[i] != "")
//                {

//                    Lines[i] = Lines[i].Remove(Lines[i].Length - 1) + ". ";
//                }
//                else
//                    Lines[i - 1] = Lines[i].Remove(Lines[i].Length - 1) + ". ";

//                for (int j = 0; j <= i; j++)
//                {
//                    HttpContext.Session.SetString("Lines_" + j, Lines[j]);
//                }
//            }
//            HttpContext.Session.SetInt32("CountWords", CountW.Value);   //Нужно как то избавится от предупреждения 
//        }
//    }
//}
