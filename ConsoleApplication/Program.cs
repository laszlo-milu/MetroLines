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

        private static readonly List<string> _allStations = RemoveDuplicates(Graph.Graph.m1Stations.Concat(Graph.Graph.m2Stations)
                                                                         .Concat(Graph.Graph.m3Stations)
                                                                         .Concat(Graph.Graph.m4Stations)
                                                                         .ToList());

        private static List<Node> _matches = new List<Node>();

        public static List<Node> Matches
        {
            get { return _matches; }
            set { _matches = value; }
        }

        public static void Main(string[] args)
        {
            var graph = new Graph.Graph();
            var station = graph.CreateGraph();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Enter a starting point:");
//                Console.WriteLine("Írj be egy kiindulási pontot:");
                var start = station.Find(Autocomplete());
                if (start == null)
                {
                    break;
                }
                Console.WriteLine();
                graph.ResetGraph();
                Console.WriteLine("Enter a destination:");
//                Console.WriteLine("Írj be egy végpontot:");
                var destination = station.Find(Autocomplete());
                if (destination == null)
                {
                    break;
                }
                Console.WriteLine();
                graph.ResetGraph();
                start.Find(destination.Name, true);
                graph.ResetGraph();

                Matches = Optimize(start, destination);
                for (int i = Matches.Count - 1; i >= 0; i--)
                {
                    var mLine = String.Format(" ({0})", Matches[i].MetroLines[0]);
                    if (Matches[i].MetroLines.Length != 1)
                    {
                        mLine = mLine.Substring(0, 4);
                        for (int j = 1; j < Matches[i].MetroLines.Length; j++)
                        {
                            mLine += String.Format(", {0}", Matches[i].MetroLines[j]);
                        }
                        mLine += ")";
                    }
                    Console.WriteLine("(" + ((i - Matches.Count) * -1) + ") " + Matches[i].Name + mLine);
                }
            }
        }

        private static List<Node> Optimize(Node start, Node target)
        {
            if (start == target)
            {
                return new List<Node>(){target};
            }
            else
            {
                List<Node> optimized = new List<Node>() { };
                for (int i = Matches.Count - 1; i >= 0; i--)
                {
                    if (optimized.Count == 0 && Matches[i] == target)
                    {
                        optimized.Add(Matches[i]);
                    }
                    else if (optimized.Count > 0)
                    {
                        if (optimized[optimized.Count - 1].Neighbors.Contains(Matches[i]))
                        {
                            optimized.Add(Matches[i]);
                        }
                        if (optimized[optimized.Count - 1] == start)
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
                        if (optimized[optimized.Count - 1].Neighbors.Contains(noDuplicates[i]))
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
                            ClearPreviousConsoleLine();
                            Console.Write("That's not a valid metro station." + Environment.NewLine);
//                            Console.Write("Ilyen metró állomás nem létezik." + Environment.NewLine);
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

        public static List<T> RemoveDuplicates<T>(List<T> list)
        {
            HashSet<T> set = new HashSet<T>(list);
            List<T> result = list.Distinct().ToList();
            return result;
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void ClearPreviousConsoleLine()
        {
            int currentLineCursor = Console.CursorTop -1;
            Console.SetCursorPosition(0, Console.CursorTop -1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}