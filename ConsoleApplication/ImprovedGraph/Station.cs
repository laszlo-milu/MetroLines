using System.Collections.Generic;
using ConsoleApplication.Graph;

namespace ConsoleApplication.ImprovedGraph
{
    public class Station
    {
        private readonly string _name;
        private readonly bool _exchange;
        private readonly string[] _metroLines;
        private List<Route> _routes = new List<Route>();

        public List<Route> Routes
        {
            get { return _routes; }
        }

        public Station(string name, bool exchange, string[] metroLines)
        {
            _name = name;
            _exchange = exchange;
            _metroLines = metroLines;
        }

        public string Name
        {
            get { return _name; }
        }

        public void AddRoute(Route route)
        {
            _routes.Add(route);
        }

        public Station otherStation(Route route)
        {
            return route.Stations[0] == this ? route.Stations[1] : route.Stations[0];
        }
    }
}