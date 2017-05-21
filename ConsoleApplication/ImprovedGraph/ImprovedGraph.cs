using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication.ImprovedGraph
{
    public class ImprovedGraph
    {
        public static readonly List<string> M1Stations = new List<string>()
        {
            "Vörösmarty tér", "Deák Ferenc tér", "Bajcsy-Zsilinszky út", "Opera", "Oktogon", "Vörösmarty utca",
            "Kodály körönd", "Bajza utca", "Hősök tere", "Széchenyi Fürdő", "Mexikói út"
        };

        public static readonly List<string> M2Stations = new List<string>()
        {
            "Déli pályaudvar", "Széll Kálmán tér", "Batthyány tér", "Kossuth Lajos tér", "Deák Ferenc tér",
            "Astoria", "Blaha Lujza tér", "Keleti pályaudvar", "Puskás Ferenc Stadion", "Pillangó utca", "Örs vezér tér"
        };

        public static readonly List<string> M3Stations = new List<string>()
        {
            "Újpest központ", "Újpest városkapu", "Gyöngyösi utca", "Forgách utca", "Árpád híd", "Dózsa György út",
            "Lehel tér", "Nyugati pályaudvar", "Arany János utca", "Deák Ferenc tér", "Ferenciek tere", "Kálvin tér",
            "Corvin-negyed", "Klinikák", "Nagyvárad tér", "Népliget", "Ecseri út", "Pöttyös utca", "Határ út", "Kőbánya-Kispest"
        };

        public static readonly List<string> M4Stations = new List<string>()
        {
            "Keleti pályaudvar", "II. János Pál pápa tér", "Rákóczi tér", "Kálvin tér", "Fővám tér", "Szent Gellért tér",
            "Móricz Zsigmond körtér", "Újbuda központ", "Bikás park", "Kelenföldi pályaudvar"
        };

        public static readonly List<string> M5Stations = new List<string>()
        {
            "Flórián tér", "Anfiteátrum", "Szépvölgyi út", "Margitsziget-Szent István park", "Lehel tér", "Oktogon",
            "Klauzál tér", "Astoria", "Kálvin tér", "Boráros tér", "Könyves Kálmán körút", "Beöthy utca", "Kén utca",
            "Timót utca", "Határ út"
        };

        private List<Station> _allStations = new List<Station>();
        private List<Route> _allRoutes = new List<Route>();

        private List<List<Station>> _possibleStations = new List<List<Station>>();
        private List<List<Route>> _possibleRoutes = new List<List<Route>>();

        public List<List<Station>> PossibleStations
        {
            get { return _possibleStations; }
        }

        public List<List<Route>> PossibleRoutes
        {
            get { return _possibleRoutes; }
        }

        private static MetroLine _m1 = new MetroLine("M1", M1Stations, 400, 50, ConsoleColor.Yellow);
        private static MetroLine _m2 = new MetroLine("M2", M2Stations, 936, 70, ConsoleColor.Red);
        private static MetroLine _m3 = new MetroLine("M3", M3Stations, 825, 60, ConsoleColor.Blue);
        private static MetroLine _m4 = new MetroLine("M4", M4Stations, 740, 80, ConsoleColor.DarkGreen);
        private static MetroLine _m5 = new MetroLine("M5", M5Stations, 943, 80, ConsoleColor.DarkMagenta);
        private static MetroLine[] _allMetroLines = {_m1, _m2, _m3, _m4, _m5};


        public ImprovedGraph createGraph()
        {
            Station previous;
            Station current;
            foreach (var metroLine in _allMetroLines)
            {
                previous = null;
                current = null;
                for (int i = 0; i < metroLine.StationNames.Count; i++)
                {
                    if (current != null)
                    {
                        previous = current;
                    }
                    current = createStation(metroLine.StationNames[i]);
                    if (previous != null)
                    {
                        var route = createRoute(new[] {previous, current}, metroLine.AverageDistance, metroLine.MaxSpeed);
                    }
                }
            }
            return this;
        }

        public Station findStation(string name)
        {
            foreach (var station in _allStations)
            {
                if (station.Name == name)
                {
                    return station;
                }
            }
            return null;
        }

        private Station createStation(string name)
        {
            var station = findStation(name);
            if (station != null) return station;
            var exchangeCounter = 0;
            var metroLines = new List<MetroLine>();
            foreach (MetroLine metroLine in _allMetroLines)
            {
                if (metroLine.StationNames.Contains(name))
                {
                    exchangeCounter++;
                    metroLines.Add(metroLine);
                }
            }
            station = new Station(name, (exchangeCounter > 1), metroLines.ToArray());
            _allStations.Add(station);
            return station;
        }

        private Route createRoute(Station[] stations, int distance, int maxSpeed)
        {
            var route = new Route(stations, distance, (int)(distance/(maxSpeed*3.6)*25));
            foreach (var station in stations)
            {
                station.AddRoute(route);
            }
            _allRoutes.Add(route);
            return route;
        }

        public void findCombinations(Station start, Station target, List<Station> stations = null, List<Route> routes = null)
        {
            if (stations == null) { stations = new List<Station>(){start};}
            if (routes == null) { routes = new List<Route>();}
            if (start.Equals(target))
            {
                _possibleStations.Add(stations);
                _possibleRoutes.Add(routes);
            }
            else
            {
                foreach (var route in start.Routes)
                {
                    var next = start.otherStation(route);
                    var nextStations = new List<Station>(stations);
                    var nextRoutes = new List<Route>(routes);

                    if (!stations.Contains(next))
                    {
                        if (!nextStations.Contains(next))
                        {
                            nextStations.Add(next);
                            nextRoutes.Add(route);
                        }
                        findCombinations(next, target, nextStations, nextRoutes);
                    }
                }
            }
        }

        public List<Station> findFastestRoute()
        {
            int fastestTime = -1;
            int fastestIndex = 0;
            for (int i = 0; i < _possibleRoutes.Count; i++)
                {
                    var currentTime = 0;
                    var routes = _possibleRoutes[i];
                    for (int j = 0; j < routes.Count; j++)
                    {
                        currentTime += routes[j].Time;
                    }
                    if (fastestTime == -1)
                    {
                        fastestTime = currentTime;
                        fastestIndex = i;
                    }
                    else if (currentTime < fastestTime)
                    {
                        fastestTime = currentTime;
                        fastestIndex = i;
                    }

                }
            Console.Write(String.Format(" {0} seconds", fastestTime) + Environment.NewLine);
            return _possibleStations[fastestIndex];
        }

        public List<Station> findShortestRoute()
        {
            int shortest = -1;
            int shortestIndex = 0;
            for (int i = 0; i < _possibleRoutes.Count; i++)
            {
                var currentDistance = 0;
                var routes = _possibleRoutes[i];
                for (int j = 0; j < routes.Count; j++)
                {
                    currentDistance += routes[j].Distance;
                }
                if (shortest == -1)
                {
                    shortest = currentDistance;
                    shortestIndex = i;
                }
                else if (currentDistance < shortest)
                {
                    shortest = currentDistance;
                    shortestIndex = i;
                }

            }
            Console.Write(String.Format(" {0} meters", shortest) + Environment.NewLine);
            return _possibleStations[shortestIndex];
        }

        public List<Station> findFewestExchangeRoute()
        {
            int fewestExchange = 999;
            int fewestExchangeIndex = 0;
            for (int i = 0; i < _possibleRoutes.Count; i++)
            {
                var currentExchanges = 0;
                var routes = _possibleRoutes[i];
                for (int j = 0; j < routes.Count; j++)
                {
                    if (!routes[j].Equals(routes.Last()))
                    {
                        if (!routes[j].MetroLine.Equals(routes[j+1].MetroLine))
                        {
                            currentExchanges++;
                        }
                    }
                }
                if (currentExchanges < fewestExchange)
                {
                    fewestExchange = currentExchanges;
                    fewestExchangeIndex = i;
                }
            }
            Console.Write(String.Format(" {0}", fewestExchange) + Environment.NewLine);
            return _possibleStations[fewestExchangeIndex];
        }
    }
}