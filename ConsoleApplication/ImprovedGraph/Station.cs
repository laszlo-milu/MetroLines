using System.Collections.Generic;
using ConsoleApplication.Graph;

namespace ConsoleApplication.ImprovedGraph
{
    public class Station
    {
        private readonly string _name;
        private readonly bool _exchange;
        private readonly MetroLine[] _metroLines;
        private List<Route> _routes = new List<Route>();
        private bool _visited;

        public bool Visited
        {
            get { return _visited; }
            set { _visited = value; }
        }

        public MetroLine[] MetroLines
        {
            get { return _metroLines; }
        }

        public List<Route> Routes
        {
            get { return _routes; }
        }

        public Station(string name, bool exchange, MetroLine[] metroLines)
        {
            _name = name;
            _exchange = exchange;
            _metroLines = metroLines;
            _visited = false;
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