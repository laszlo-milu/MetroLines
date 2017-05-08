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
            ConsoleKey.OemPeriod, ConsoleKey.OemMinus, ConsoleKey.OemComma, ConsoleKey.Spacebar
        };

        private static readonly List<string> _allStations = RemoveDuplicates(Graph.Graph.m1Stations
                                                                         .Concat(Graph.Graph.m2Stations)
                                                                         .Concat(Graph.Graph.m3Stations)
                                                                         .Concat(Graph.Graph.m4Stations)
                                                                         .ToList());

        public static void Main(string[] args)
        {
            var graph = new Graph.Graph();
            var station = graph.CreateGraph();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Enter a starting point:");
                var start = station.Find(Autocomplete());
                if (start == null)
                {
                    break;
                }
                Console.WriteLine();
                graph.ResetGraph();
                Console.WriteLine("Enter a destination:");
                var destination = station.Find(Autocomplete());
                if (destination == null)
                {
                    break;
                }
                Console.WriteLine();
                graph.ResetGraph();
                start.Find(destination.Name, true);
                graph.ResetGraph();

                var matches = Graph.Graph.Matches;

                matches = Graph.Graph.Optimize(start, destination);
                for (int i = matches.Count - 1; i >= 0; i--)
                {
                    var mLine = String.Format(" ({0})", matches[i].MetroLines[0]);
                    if (matches[i].MetroLines.Length != 1)
                    {
                        mLine = mLine.Substring(0, 4);
                        for (int j = 1; j < matches[i].MetroLines.Length; j++)
                        {
                            mLine += String.Format(", {0}", matches[i].MetroLines[j]);
                        }
                        mLine += ")";
                    }
                    Console.WriteLine("(" + ((i - matches.Count) * -1) + ") " + matches[i].Name + mLine);
                }
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