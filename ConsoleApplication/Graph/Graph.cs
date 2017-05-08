using System.Collections.Generic;

namespace ConsoleApplication.Graph
{
    public class Graph
    {
        public static readonly List<string> m1Stations = new List<string>()
        {
            "Vörösmarty tér", "Deák Ferenc tér", "Bajcsy-Zsilinszky út", "Opera", "Oktogon", "Vörösmarty utca",
            "Kodály körönd", "Bajza utca", "Hősök tere", "Széchenyi Fürdő", "Mexikói út"
        };
        public static readonly List<string> m2Stations = new List<string>()
        {
            "Déli pályaudvar", "Széll Kálmán tér", "Batthyány tér", "Kossuth Lajos tér", "Deák Ferenc tér",
            "Astoria", "Blaha Lujza tér", "Keleti pályaudvar", "Puskás Ferenc Stadion", "Pillangó utca", "Örs vezér tér"
        };
        public static readonly List<string> m3Stations = new List<string>()
        {
            "Újpest központ", "Újpest városkapu", "Gyöngyösi utca", "Forgách utca", "Árpád híd", "Dózsa György út",
            "Lehel tér", "Nyugati pályaudvar", "Arany János utca", "Deák Ferenc tér", "Ferenciek tere", "Kálvin tér",
            "Corvin-negyed", "Klinikák", "Nagyvárad tér", "Népliget", "Ecseri út", "Pöttyös utca", "Határ út", "Kőbánya-Kispest"
        };
        public static readonly List<string> m4Stations = new List<string>()
        {
            "Keleti pályaudvar", "II. János Pál pápa tér", "Rákóczi tér", "Kálvin tér", "Fővám tér", "Szent Gellért tér",
            "Móricz Zsigmond körtér", "Újbuda központ", "Bikás park", "Kelenföldi pályaudvar"
        };

        private static List<Node> _matches = new List<Node>();

        public static List<Node> Matches
        {
            get { return _matches; }
            set { _matches = value; }
        }

        private static List<Node> _visitedStations = new List<Node>();

        public static List<Node> VisitedStations
        {
            get { return _visitedStations; }
            set { _visitedStations = value; }
        }

        public void ResetGraph()
        {
            for (int i = 0; i < VisitedStations.Count; i++)
            {
                VisitedStations[i].Visited = false;
            }
            VisitedStations.Clear();
        }

        public Node CreateGraph()
        {
            var m = new[] {"M1"};
            var prev = new Node(m1Stations[0], m);
            Node deak = null;

            for (int i = 1; i < m1Stations.Count; i++)
            {
                Node node = new Node(m1Stations[i], m, prev);
                prev = node;
                if (m1Stations[i] == "Deák Ferenc tér")
                {
                    node = new Node(m1Stations[i], new[] {"M1", "M2", "M3"}, prev);
                    deak = node;
                }
            }
            Node keleti = null;
            m = new[] {"M2"};

            prev = new Node(m2Stations[0], m);
            for (int i = 1; i < m2Stations.Count; i++)
            {
                Node node;
                if (m2Stations[i] == "Deák Ferenc tér")
                {
                    node = deak;
                    deak.Neighbors.Add(prev);
                    prev.Neighbors.Add(deak);
                }
                else
                {
                    node = new Node(m2Stations[i], m, prev);
                    if (m3Stations[i] == "Keleti pályaudvar")
                    {
                        node = new Node(m3Stations[i], new[] {"M2", "M4"}, prev);
                        keleti = node;
                    }
                }
                prev = node;
            }
            m = new[] {"M3"};
            Node kalvin = null;
            prev = new Node(m3Stations[0], m);
            for (int i = 1; i < m3Stations.Count; i++)
            {
                Node node;
                if (m3Stations[i] == "Deák Ferenc tér")
                {
                    node = deak;
                    deak.Neighbors.Add(prev);
                    prev.Neighbors.Add(deak);
                }
                else
                {
                    node = new Node(m3Stations[i], m, prev);
                    if (m3Stations[i] == "Kálvin tér")
                    {
                        node = new Node(m3Stations[i], new []{"M3", "M4"}, prev);
                        kalvin = node;
                    }
                }
                prev = node;
            }
            m = new[] {"M4"};
            prev = new Node(m4Stations[0], m);
            for (int i = 1; i < m4Stations.Count; i++)
            {
                Node node;
                if (m4Stations[i] == "Kálvin tér")
                {
                    node = kalvin;
                    kalvin.Neighbors.Add(prev);
                    prev.Neighbors.Add(kalvin);
                }
                else if (m4Stations[i] == "Keleti pályaudvar")
                {
                    node = keleti;
                    keleti.Neighbors.Add(prev);
                    prev.Neighbors.Add(keleti);
                }
                else
                {
                    node = new Node(m4Stations[i], m, prev);
                }
                prev = node;
            }
            return prev;
        }

        public static List<Node> Optimize(Node start, Node target)
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
                List<Node> noDuplicates = Program.RemoveDuplicates(optimized);
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
    }
}