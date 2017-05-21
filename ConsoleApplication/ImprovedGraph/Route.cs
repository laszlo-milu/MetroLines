using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace ConsoleApplication.ImprovedGraph
{
    public class Route
    {
        private readonly Station[] _stations;
        private readonly int _distance; // in meters
        private readonly int _time; // in seconds
        private readonly MetroLine _metroLine;
        public Route(Station[] stations, int distance, int time)
        {
            _stations = stations;
            _distance = distance;
            _time = time;
            _metroLine = _stations[0].MetroLines.ToList().Intersect(_stations[1].MetroLines.ToList()).ToArray()[0];

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

        public MetroLine MetroLine
        {
            get { return _metroLine; }
        }
    }
}