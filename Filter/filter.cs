using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Filters
{
    class Filter
    {
        private string sep = "";

        public Filter(string Seperator)
        {
            sep = Seperator;
        }

        public int CountFilt(string From)
        {
            return CountFilt(From, sep, false);
        }

        public int CountFilt(string From, string Seperator)
        {
            return CountFilt(From, Seperator, false);
        }

        public int CountFilt(string From, string Seperator, bool CaseSensitive)
        {
            int res = 0;

            if (Seperator == "") Seperator = sep;

            if (!CaseSensitive)
            {
                From = From.ToUpper();
                Seperator = Seperator.ToUpper();
            }

            if (From.Length > 0)
            {
                res = 1;
                for (int ic = 0; ic < From.Length - Seperator.Length; ic++)
                {
                    if (From.Substring(ic, Seperator.Length) == Seperator)
                    {
                        res++;
                    }
                }

            }
            return res;
        }

        public string GetFilt(string From, int Number)
        {
            return GetFilt(From, Number, sep, false);
        }

        public string GetFilt(string From, int Number, string Seperator)
        {
            return GetFilt(From, Number, Seperator, false);
        }

        public string GetFilt(string From, int Number, string Seperator, bool CaseSensitive)
        {
            string Res = "";
            string TFrom = From;
            if (!CaseSensitive)
            {
                TFrom = TFrom.ToUpper();
                Seperator = Seperator.ToUpper();
            }

            if (Number > CountFilt(TFrom, Seperator, CaseSensitive))
            {
                return Res;
            }

            if (TFrom.Length > 0)
            {
                if ((TFrom.Substring(0, Seperator.Length) == Seperator) && (Number == 1))
                {
                    return Res;
                }

                int CStart = 0;
                int CSlut = 0;
                int ATF = 1;
                int LastRecorded = 0;
                if (Number == 1)
                {
                    CStart = 0;
                }

                for (int IC = 0; IC < TFrom.Length - Seperator.Length; IC++)
                {
                    LastRecorded = IC;
                    if (TFrom.Substring(IC, Seperator.Length) == Seperator)
                    {
                        ATF++;
                        if (ATF == Number)
                        {
                            CStart = IC + Seperator.Length;
                        }
                        if (ATF == Number + 1)
                        {
                            CSlut = IC - 1;
                        }
                    }
                }
                if ((CStart != 0) && (CSlut == 0))
                {
                    CSlut = LastRecorded + Seperator.Length;
                }
                Res = From.Substring(CStart, CSlut - CStart + 1);
            }
            return Res;
        }

        public string DelFilt(string From, int Number)
        {
            return DelFilt(From, Number, sep, false);
        }

        public string DelFilt(string From, int Number, string Seperator)
        {
            return DelFilt(From, Number, Seperator, false);
        }

        public string DelFilt(string From, int Number, string Seperator, bool CaseSensitive)
        {
            string Finish = "";
            string TFrom = From;
            if (From.Length == 0) return Finish;
            if (!CaseSensitive)
            {
                TFrom = TFrom.ToUpper();
                Seperator = Seperator.ToUpper();
            }
            int At = 1;
            int Start = 1;
            if (At == Number) Start = 0;
            int Count = 0;
            int Mat = 0;
            if ((CountFilt(TFrom, Seperator) - 1) == 0)
            {
                return Finish;
            }
            for (int ICount = 0; ICount < TFrom.Length - Seperator.Length; ICount++)
            {
                if (TFrom.Substring(ICount, Seperator.Length) == Seperator)
                {
                    At++;
                    if (At == Number) Start = ICount;
                    if (At == Number + 1) Count = ICount;
                }
                Mat = ICount;
            }
            if (At == Number) Count = Mat;
            if (Count == 0) Finish = From;
            if (Number == 1)
            {
                Count = Count + Seperator.Length;
                Finish = From.Substring(Count, From.Length - Count);
            }
            if (Number > 1)
            {
                Finish = From.Substring(0, Start) + From.Substring(Count, From.Length - Count);
            }
            if (Number == CountFilt(From, Seperator))
            {
                Finish = From.Substring(0, Start);
            }
            return Finish;
        }

        public string ReplaceFilt(string From, int Number, string ReplaceWith)
        {
            return ReplaceFilt(From, Number, ReplaceWith, sep, false);
        }

        public string ReplaceFilt(string From, int Number, string ReplaceWith, string Seperator)
        {
            return ReplaceFilt(From, Number, ReplaceWith, Seperator, false);
        }

        public string ReplaceFilt(string From, int Number, string ReplaceWith, string Seperator, bool CaseSensitive)
        {
            string preStr = "";
            string postStr = "";
            string TFrom = From;
            if (!CaseSensitive)
            {
                TFrom = TFrom.ToUpper();
                Seperator = Seperator.ToUpper();
            }
            if (Number > CountFilt(TFrom, Seperator, CaseSensitive))
            {
                return From;
            }

            for (int i = 1; i != Number; i++)
            {
                preStr = preStr + Seperator + GetFilt(From, i, Seperator, CaseSensitive);
            }
            for (int i = (Number + 1); i <= CountFilt(TFrom, Seperator, CaseSensitive); i++)
            {
                postStr = postStr + Seperator + GetFilt(From, i, Seperator, CaseSensitive);
            }

            string TempStr = preStr + Seperator + ReplaceWith + postStr;
            if (TempStr.Length > 0)
            {
                if (TempStr.Substring(0, Seperator.Length) == Seperator)
                {
                    TempStr = TempStr.Substring(Seperator.Length, TempStr.Length - Seperator.Length);
                }
            }
            return TempStr;
        }

//  For IC := Number+1 to CountFilt(From, Seperator) do begin
//   TempStr := TempStr + Seperator + GetFilt(From, IC, Seperator);
//  End;
//  If Length(TempStr) > 0 Then
//   If copy(TempStr, 1, length(Seperator)) = Seperator Then TempStr := Copy(TempStr, length(seperator) + 1, Length(TempStr));
//  Result := TempStr;
// End;
//End;

    }


    class program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Filter test");

            string str = "1234567890";
            for (int i = 0; i < str.Length; i++)
            {
                Console.WriteLine(str[i]);
            }

            Console.WriteLine(str.Substring(1, 5));

            str = "item1||item2||item3||item4||item5";
            Filter filter = new Filter("||");

            Console.WriteLine(str + " --- " + filter.CountFilt(str).ToString());
            Console.WriteLine("Filt #1: " + filter.GetFilt(str, 1)); 
            Console.WriteLine("Filt #2: " + filter.GetFilt(str, 2));
            Console.WriteLine("Filt #3: " + filter.GetFilt(str, 3));
            Console.WriteLine("Filt #4: " + filter.GetFilt(str, 4));
            Console.WriteLine("Filt #5: " + filter.GetFilt(str, 5));
            Console.WriteLine("DelFilt #1: " + filter.DelFilt(str, 1));
            Console.WriteLine("DelFilt #2: " + filter.DelFilt(str, 2));
            Console.WriteLine("DelFilt #3: " + filter.DelFilt(str, 3));
            Console.WriteLine("DelFilt #4: " + filter.DelFilt(str, 4));
            Console.WriteLine("DelFilt #5: " + filter.DelFilt(str, 5));
            Console.WriteLine("ReplaceFilt #1: " + filter.ReplaceFilt(str, 1, "REMOVED"));
            Console.WriteLine("ReplaceFilt #2: " + filter.ReplaceFilt(str, 2, "REMOVED"));
            Console.WriteLine("ReplaceFilt #3: " + filter.ReplaceFilt(str, 3, "REMOVED"));
            Console.WriteLine("ReplaceFilt #4: " + filter.ReplaceFilt(str, 4, "REMOVED"));
            Console.WriteLine("ReplaceFilt #5: " + filter.ReplaceFilt(str, 5, "REMOVED"));

            //    from, number, replace, seperator, sensitive

            /*
            str = "1234567890";
            str = str.Substring(0, 10);
            Console.WriteLine(str);
            */
        }
    }
}