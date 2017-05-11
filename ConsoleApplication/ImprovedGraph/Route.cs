using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace ConsoleApplication.ImprovedGraph
{
    public class Route
    {
        private readonly Station[] _stations;
        private readonly int _distance; // in meters
        private readonly int _time; // in seconds
        public Route(Station[] stations, int distance, int time)
        {
            _stations = stations;
            _distance = distance;
            _time = time;
        }

        public Station[] Stations
        {
            get { return _stations; }
        }

        public int Distance
        {
            get { return _distance; }
        }

        public int Time
        {
            get { return _time; }
        }
    }
}