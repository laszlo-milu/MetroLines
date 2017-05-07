using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication.Graph;
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
            ConsoleKey.OemPeriod, ConsoleKey.OemMinus, ConsoleKey.OemComma, ConsoleKey.Spacebar
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

        private static List<Node> _visitedStations = new List<Node>();

        public static List<Node> VisitedStations
        {
            get { return _visitedStations; }
            set { _visitedStations = value; }
        }

        private static List<Node> _matches = new List<Node>();

        public static List<Node> Matches
        {
            get { return _matches; }
            set { _matches = value; }
        }

        public static void Main(string[] args)
        {
            Array.Copy(_m1Stations, _allStations, _m1Stations.Length);
            Array.Copy(_m2Stations, 0, _allStations, _m1Stations.Length, _m2Stations.Length);
            Array.Copy(_m3Stations, 0, _allStations, _m1Stations.Length + _m2Stations.Length, _m3Stations.Length);
            Array.Copy(_m4Stations, 0, _allStations, _m1Stations.Length + _m2Stations.Length + _m3Stations.Length, _m4Stations.Length);
            _allStations = RemoveDuplicates(_allStations);

            var station = CreateGraph();

            Console.WriteLine();
//            Console.WriteLine(prev.Find(Autocomplete(),true).Name);
            Console.WriteLine("Enter a starting point!");
            Node start = station.Find(Autocomplete());
            Console.WriteLine();
            ResetGraph();
            Console.WriteLine("Enter a destination!");
            Node destination = station.Find(Autocomplete());
            Console.WriteLine();
            ResetGraph();
            start.Find(destination.Name, true);

            Matches = Optimize(start, destination);
            for (int i = Matches.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("(" + ((i - Matches.Count) * -1) + ") " + Matches[i].Name);
            }

            Autocomplete();

        }

        private static List<Node> Optimize(Node start, Node target)
        {
            List<Node> optimized = new List<Node>(){};
            for (int i = Matches.Count - 1; i >= 0; i--)
            {
                if (optimized.Count == 0 && Matches[i] == target)
                {
                    optimized.Add(Matches[i]);
                }
                else if (optimized.Count > 0)
                {
                    if (optimized[optimized.Count-1].Neighbors.Contains(Matches[i]))
                    {
                        optimized.Add(Matches[i]);
                    }
                    if (optimized[optimized.Count-1] == start)
                    {
                        break;
                    }
                }
            }
            List<Node> noDuplicates = RemoveDuplicates(optimized);
            optimized.Clear();
            for (int i = 0; i < noDuplicates.Count; i++)
            {
                if (optimized.Count == 0)
                {
                    optimized.Add(noDuplicates[i]);
                }
                else
                {
                    if (optimized[optimized.Count-1].Neighbors.Contains(noDuplicates[i]))
                    {
                        optimized.Add(noDuplicates[i]);
                    }
                    else
                    {
                        optimized.Remove(optimized[optimized.Count - 1]);
                        i--;
                    }
                }
            }
            return optimized;
        }

        private static void ResetGraph()
        {
            for (int i = 0; i < VisitedStations.Count; i++)
            {
                VisitedStations[i].Visited = false;
            }
            VisitedStations.Clear();
        }

        private static Node CreateGraph()
        {
            Node prev = new Node(_m1Stations[0]);
            Node deak = null;

            for (int i = 1; i < _m1Stations.Length; i++)
            {
                Node node = new Node(_m1Stations[i], prev);
                prev = node;
                if (_m1Stations[i] == "Deák Ferenc tér")
                {
                    deak = node;
                }
            }
            Node keleti = null;
            prev = new Node(_m2Stations[0]);
            for (int i = 1; i < _m2Stations.Length; i++)
            {
                Node node;
                if (_m2Stations[i] == "Deák Ferenc tér")
                {
                    node = deak;
                    deak.Neighbors.Add(prev);
//                    Console.WriteLine(prev.Name+ " ADDED TO " + deak.Name);
                    prev.Neighbors.Add(deak);
//                    Console.WriteLine(deak.Name+ " ADDED TO " + prev.Name);

                }
                else
                {
                    node = new Node(_m2Stations[i], prev);
                    if (_m3Stations[i] == "Keleti pályaudvar")
                    {
                        keleti = new Node(_m3Stations[i], prev);
                    }
                }
                prev = node;
            }
            Node kalvin = null;
            prev = new Node(_m3Stations[0]);
            for (int i = 1; i < _m3Stations.Length; i++)
            {
                Node node;
                if (_m3Stations[i] == "Deák Ferenc tér")
                {
                    node = deak;
                    deak.Neighbors.Add(prev);
//                    Console.WriteLine(prev.Name+ " ADDED TO " + deak.Name);
                    prev.Neighbors.Add(deak);
//                    Console.WriteLine(deak.Name+ " ADDED TO " + prev.Name);


                }
                else
                {
                    node = new Node(_m3Stations[i], prev);
                    if (_m3Stations[i] == "Kálvin tér")
                    {
                        kalvin = new Node(_m3Stations[i], prev);
                    }
                }
                prev = node;
            }
            prev = new Node(_m4Stations[0]);
            for (int i = 1; i < _m4Stations.Length; i++)
            {
                Node node;
                if (_m4Stations[i] == "Kálvin tér")
                {
                    node = kalvin;
                    kalvin.Neighbors.Add(prev);
                    prev.Neighbors.Add(kalvin);


                }
                else if (_m4Stations[i] == "Keleti pályaudvar")
                {
                    node = keleti;
                    keleti.Neighbors.Add(prev);
                    prev.Neighbors.Add(keleti);


                }
                else
                {
                    node = new Node(_m4Stations[i], prev);
                }
                prev = node;
            }
            return prev;
        }


        private static string Autocomplete()
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
//                        else
//                        {
//                            Console.WriteLine(readKeyResult.Key.ToString());
//                        }
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

        public static List<Node> RemoveDuplicates(List<Node> l)
        {
            HashSet<Node> set = new HashSet<Node>(l);
            List<Node> result = l.Distinct().ToList();
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