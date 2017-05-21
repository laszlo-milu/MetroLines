using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using ConsoleApplication.ImprovedGraph;
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

        private static readonly List<string> _allStations = RemoveDuplicates(ImprovedGraph.ImprovedGraph.M1Stations
                                                                         .Concat(ImprovedGraph.ImprovedGraph.M2Stations)
                                                                         .Concat(ImprovedGraph.ImprovedGraph.M3Stations)
                                                                         .Concat(ImprovedGraph.ImprovedGraph.M4Stations)
                                                                         .Concat(ImprovedGraph.ImprovedGraph.M5Stations)
                                                                         .ToList());

        public static void Main(string[] args)
        {
            while (true)
            {
                var graph = new ImprovedGraph.ImprovedGraph().createGraph();


                Console.WriteLine();
                Console.WriteLine("Enter a starting point:");
                var start = graph.findStation(Autocomplete());
                if (start == null)
                {
                    break;
                }
                Station destination;
                bool first = true;
                do
                {
                    Console.WriteLine();
                    Console.WriteLine(first
                        ? "Enter a destination:"
                        : "Enter a destination different from the starting point:");
                    destination = graph.findStation(Autocomplete());
                    if (destination == null)
                    {
                        break;
                    }
                    first = false;

                } while (start.Equals(destination));
                if (destination == null)
                {
                    break;
                }
                Console.WriteLine();
                var watch = System.Diagnostics.Stopwatch.StartNew();
                graph.findCombinations(start, destination);
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds + "ms");
//                PrintAllStationsAllRoutes(graph);
                Console.WriteLine(String.Format("All Combinations: {0}", graph.PossibleStations.Count));
                foreach (var stations in graph.PossibleStations)
                {
                    PrintStations(stations);
                    Console.WriteLine(new String('_', Console.BufferWidth));
                }
                Console.Write("Fewest Exchanges:");
                PrintStations(graph.findFewestExchangeRoute());
                Console.WriteLine();
                Console.Write("Fastest Route:");
                PrintStations(graph.findFastestRoute());
                Console.WriteLine();
                Console.Write("Shortest Distance:");
                PrintStations(graph.findShortestRoute());
            }
        }

        private static void PrintStations(List<Station> stations)
        {
            for (var i = 0; i < stations.Count; i++)
            {
                if (0 < i && i < stations.Count-1)
                {
                    ColorStation(stations[i], stations[i-1], stations[i+1]);
                    Console.Write(" > ");
                }
                else if (i > 0)
                {
                    var c = stations[i].MetroLines.ToList().Intersect(stations[i-1].MetroLines.ToList()).ToArray()[0].BackgroundColor;
                    ColorStation(stations[i], c);
                } else if (i < stations.Count - 1)
                {
                    var c = stations[i].MetroLines.ToList().Intersect(stations[i+1].MetroLines.ToList()).ToArray()[0].BackgroundColor;
                    ColorStation(stations[i], c);
                    Console.Write(" > ");

                }
            }
            Console.WriteLine();
        }

        private static void ColorStation(Station current, Station previous, Station next)
        {
            ConsoleColor color1 = current.MetroLines.ToList().Intersect(previous.MetroLines.ToList()).ToArray()[0].BackgroundColor;
            ConsoleColor color2 = current.MetroLines.ToList().Intersect(next.MetroLines.ToList()).ToArray()[0].BackgroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = color1;
            Console.Write(" " + current.Name.Substring(0, current.Name.Length/2));
            Console.BackgroundColor = color2;
            Console.Write(current.Name.Substring(current.Name.Length/2) + " ");
            Console.ResetColor();
        }

        private static void ColorStation(Station station, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = color;
            Console.Write(String.Format(" {0} ", station.Name));
            Console.ResetColor();
        }


        private static void PrintAllStationsAllRoutes(ImprovedGraph.ImprovedGraph graph)
        {
            for (int i = 0; i < graph.PossibleStations.Count; i++)
            {
                Console.WriteLine(graph.PossibleStations[i][0].Name);
                for (int j = 1; j < graph.PossibleStations[i].Count; j++)
                {
                    Console.WriteLine(graph.PossibleRoutes[i][j-1].Stations[0].Name + " - " + graph.PossibleRoutes[i][j-1].Stations[1].Name);
                    Console.WriteLine(graph.PossibleStations[i][j].Name);

                }
                Console.WriteLine("_______________________________________");
            }
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
                        output = output.Equals(retString)
                            ? StringSearch.AutoComplete(retString, _allStations, true)
                            : StringSearch.AutoComplete(retString, _allStations);
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.WriteLine();
                        ClearCurrentConsoleLine();
                        Console.Write(output);
                        retString = output;
                        curIndex = output.Length;
                        break;

                    case ConsoleKey.Escape:
                        return null;

                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        if (_allStations.Contains(retString))
                        {
                            return retString;
                        }
                        else
                        {
                            ClearCurrentConsoleLine(1);
                            Console.Write("That's not a valid metro station." + Environment.NewLine);
                            Console.Write(retString);
                        }
                        break;

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
                            if (retString.Length < Console.WindowWidth - 1)
                            {
                                retString += readKeyResult.KeyChar;
                                Console.Write(readKeyResult.KeyChar);
                                curIndex++;
                            }
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

        public static List<T> RemoveDuplicates<T>(List<T> list)
        {
            HashSet<T> set = new HashSet<T>(list);
            List<T> result = list.Distinct().ToList();
            return result;
        }
        public static void ClearCurrentConsoleLine(int i=0)
        {
            int currentLineCursor = Console.CursorTop - i;
            Console.SetCursorPosition(0, Console.CursorTop - i);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}