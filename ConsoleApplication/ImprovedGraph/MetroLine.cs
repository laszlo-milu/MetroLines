using System;
using System.Collections.Generic;

namespace ConsoleApplication.ImprovedGraph
{
    public class MetroLine
    {
        private readonly string _name;
        private readonly List<string> _stationNames;
        private readonly int _averageDistance;
        private readonly int _maxSpeed;
        private readonly ConsoleColor _backgroundColor;

        public ConsoleColor BackgroundColor
        {
            get { return _backgroundColor; }
        }

        public List<string> StationNames
        {
            get { return _stationNames; }
        }

        public int AverageDistance
        {
            get { return _averageDistance; }
        }

        public int MaxSpeed
        {
            get { return _maxSpeed; }
        }

        public string Name
        {
            get { return _name; }
        }

        public MetroLine(string name, List<string> stationNames, int averageDistance, int maxSpeed, ConsoleColor backgroundColor)
        {
            _name = name;
            _stationNames = stationNames;
            _averageDistance = averageDistance;
            _maxSpeed = maxSpeed;
            _backgroundColor = backgroundColor;

        }
    }
}