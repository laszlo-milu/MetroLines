using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication.Search;
namespace ConsoleApplication
{
    internal class Program
    {
        private static readonly ConsoleKey[] _characters = new ConsoleKey[]
        {
            ConsoleKey.A, ConsoleKey.Oem7, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.Oem1,
            ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.I, ConsoleKey.Oem102, ConsoleKey.J, ConsoleKey.K,
            ConsoleKey.L, ConsoleKey.M, ConsoleKey.N, ConsoleKey.O, ConsoleKey.OemPlus, ConsoleKey.Oem3, ConsoleKey.Oem4,
            ConsoleKey.P, ConsoleKey.Q, ConsoleKey.R, ConsoleKey.S, ConsoleKey.T, ConsoleKey.U, ConsoleKey.Oem6,
            ConsoleKey.Oem2, ConsoleKey.Oem5, ConsoleKey.V, ConsoleKey.W, ConsoleKey.X, ConsoleKey.Y, ConsoleKey.Z,
            ConsoleKey.OemPeriod, ConsoleKey.OemMinus, ConsoleKey.OemComma
        };
        private static readonly string[] _m1Stations = new string[]
        {
            "Vörösmarty tér", "Deák Ferenc tér", "Bajcsy-Zsilinszky út", "Opera", "Oktogon", "Vörösmarty utca",
            "Kodály körönd", "Bajza utca", "Hősök tere", "Széchenyi Fürdő", "Mexikói út"
        };
        private static readonly string[] _m2Stations = new string[]
        {
            "Déli pályaudvar", "Széll Kálmán tér", "Batthyány tér", "Kossuth Lajos tér", "Deák Ferenc tér",
            "Astoria", "Blaha Lujza tér", "Keleti pályaudvar", "Puskás Ferenc Stadion", "Pillangó utca", "Örs vezér tér"
        };
        private static readonly string[] _m3Stations = new string[]
        {
            "Újpest központ", "Újpest városkapu", "Gyöngyösi utca", "Forgách utca", "Árpád híd", "Dózsa György út",
            "Lehel tér", "Nyugati pályaudvar", "Arany János utca", "Deák Ferenc tér", "Ferenciek tere", "Kálvin tér",
            "Corvin-negyed", "Klinikák", "Nagyvárad tér", "Népliget", "Ecseri út", "Pöttyös utca", "Határ út", "Kőbánya-Kispest"
        };
        private static readonly string[] _m4Stations = new string[]
        {
            "Keleti pályaudvar", "II. János Pál pápa tér", "Rákóczi tér", "Kálvin tér", "Fővám tér", "Szent Gellért tér",
            "Móricz Zsigmond körtér", "Újbuda központ", "Bikás park", "Kelenföldi pályaudvar"
        };
        private static string[] _allStations = new string[_m1Stations.Length + _m2Stations.Length + _m3Stations.Length + _m4Stations.Length];

        public static void Main(string[] args)
        {
            Array.Copy(_m1Stations, _allStations, _m1Stations.Length);
            Array.Copy(_m2Stations, 0, _allStations, _m1Stations.Length, _m2Stations.Length);
            Array.Copy(_m3Stations, 0, _allStations, _m1Stations.Length + _m2Stations.Length, _m3Stations.Length);
            Array.Copy(_m4Stations, 0, _allStations, _m1Stations.Length + _m2Stations.Length + _m3Stations.Length, _m4Stations.Length);
            _allStations = RemoveDuplicates(_allStations);

            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
//                Console.WriteLine(BasicTerminal());
                BasicTerminal();
            }
        }

        private static string BasicTerminal()
        {
            string retString = "";
            string output = "";
            int curIndex = 0;
            do
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);
                switch (readKeyResult.Key)
                {
                    case ConsoleKey.Tab:
                        output = output.Equals(retString) ? StringSearch.AutoComplete(retString, _allStations, true) : StringSearch.AutoComplete(retString, _allStations);
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.WriteLine();
                        ClearCurrentConsoleLine();
//                        Console.WriteLine();
                        Console.Write(output);
                        retString = output;
                        curIndex = output.Length;
                        break;

                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return retString;

                    case ConsoleKey.Backspace:
                         if (curIndex > 0)
                         {
                             retString = retString.Remove(retString.Length - 1);
                             Console.Write(readKeyResult.KeyChar);
                             Console.Write(' ');
                             Console.Write(readKeyResult.KeyChar);
                             curIndex--;
                         }
                         break;
                    default:
                        if (_characters.Contains(readKeyResult.Key))
                        {
                            retString += readKeyResult.KeyChar;
                            Console.Write(readKeyResult.KeyChar);
                            curIndex++;
                        }
                        else
                        {
                            Console.WriteLine(readKeyResult.Key.ToString());
                        }
                        break;
                }
            }
            while (true);
        }
        public static string[] RemoveDuplicates(string[] s)
        {
            HashSet<string> set = new HashSet<string>(s);
            string[] result = new string[set.Count];
            set.CopyTo(result);
            return result;
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}