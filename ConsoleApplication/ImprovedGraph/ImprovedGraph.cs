using System.Collections.Generic;

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

        private List<Station> _allStations = new List<Station>();
        private List<Route> _allRoutes = new List<Route>();

        public ImprovedGraph createGraph()
        {
            var m1 = new MetroLine(M1Stations, 400, 50);
            var m2 = new MetroLine(M2Stations, 936, 70);
            var m3 = new MetroLine(M3Stations, 825, 60);
            var m4 = new MetroLine(M4Stations, 740, 80);
            var allMetroLines = new[] {m1, m2, m3, m4};
            Station previous = null;
            Station current = null;
            foreach (var metroLine in allMetroLines)
            {
                for (int i = 0; i < metroLine.Stations.Count; i++)
                {
                    if (current != null)
                    {
                        previous = current;
                    }
                    current = createStation(metroLine.Stations[i]);
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
            var metroLines = new List<string>();
            var allStations = new[] {M1Stations, M2Stations, M3Stations, M4Stations};
            for (var i = 0; i < allStations.Length; i++)
            {
                if (allStations[i].Contains(name))
                {
                    exchangeCounter++;
                    metroLines.Add("M" + (i+1));
                }
            }
            station = new Station(name, (exchangeCounter > 1), metroLines.ToArray());
            _allStations.Add(station);
            return station;
        }

        private Route createRoute(Station[] stations, int distance, int maxSpeed)
        {
            var route = new Route(stations, distance, (int)(maxSpeed*3.6)/distance);
            foreach (var station in stations)
            {
                station.AddRoute(route);
            }
            _allRoutes.Add(route);
            return route;
        }
    }
}