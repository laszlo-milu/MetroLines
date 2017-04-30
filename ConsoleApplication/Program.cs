using System;
using System.Collections.Generic;
using ConsoleApplication.Search;
namespace ConsoleApplication
{
    internal class Program
    {
        private static readonly string[] _m1Stations = new string[]
        {
            "Vörösmarty tér", "Deák Ferenc tér", "Bajcsy-Zsilinszky út", "Opera", "Oktogon", "Vörösmarty utca",
            "Kodály körönd", "Bajza utca", "Hősök tere", "Széchenyi Fürdő", "Mexikói út"
        };
        private static readonly string[] _m2Stations = new string[]
        {
            "Déli Pályaudvar", "Széll Kálmán tér", "Batthyány tér", "Kossuth Lajos tér", "Deék Ferenc tér",
            "Astoria", "Blaha Lujza tér", "Keleti Pályaudvar", "Puskás Ferenc Stadion", "Pillangó utca", "Örs vezér tér"
        };
        private static readonly string[] _m3Stations = new string[]
        {
            "Újpest központ", "Újpest városkapu", "Gyöngyösi utca", "Forgách utca", "Árpád híd", "Dózsa György út",
            "Lehel tér", "Nyugati Pályaudvar", "Arany János utca", "Deák Ferenc tér", "Ferenciek tere", "Kálvin tér",
            "Corvin-negyed", "Klinikák", "Nagyvárad tér", "Népliget", "Ecseri út", "Pöttyös utca", "Határ út", "Kőbánya-Kispest"
        };
        private static readonly string[] _m4Stations = new string[]
        {
            "Keleti Pályaudvar", "II. János Pál pápa tér", "Rákóczi tér", "Kálvin tér", "Fővám tér", "Szent Gellért tér",
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
            for (int i = 0; i < _allStations.Length; i++)
            {
                Console.WriteLine(_allStations[i]);
            }

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(BasicTerminal());
//                BasicTerminal();
            }
        }

        // returns null if user pressed Escape, or the contents of the line if they pressed Enter.
        private static string BasicTerminal()
        {
            string retString = "";

            int curIndex = 0;
            do
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);

                // handle Tab
                if (readKeyResult.Key == ConsoleKey.Tab)
                {
                    var output = StringSearch.AutoComplete(retString, _allStations);
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentConsoleLine();
//                    Console.WriteLine();
                    Console.Write(output);
                    retString = output;
                    curIndex = output.Length;
                }

                // handle Enter
                else if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return retString;
                }

                // handle backspace
                else if (readKeyResult.Key == ConsoleKey.Backspace)
                {
                    if (curIndex > 0)
                    {
                        retString = retString.Remove(retString.Length - 1);
                        Console.Write(readKeyResult.KeyChar);
                        Console.Write(' ');
                        Console.Write(readKeyResult.KeyChar);
                        curIndex--;
                    }
                }
                else
                    // handle all other keypresses
                {
                    retString += readKeyResult.KeyChar;
                    Console.Write(readKeyResult.KeyChar);
                    curIndex++;
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
            Console.WriteLine();
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}