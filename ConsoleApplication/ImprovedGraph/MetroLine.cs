using System.Collections.Generic;

namespace ConsoleApplication.ImprovedGraph
{
    public class MetroLine
    {
        private readonly string _name;
        private readonly List<string> _stations;
        private readonly int _averageDistance;
        private readonly int _maxSpeed;

        public List<string> Stations
        {
            get { return _stations; }
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

        public MetroLine(string name, List<string> stations, int averageDistance, int maxSpeed)
        {
            _name = name;
            _stations = stations;
            _averageDistance = averageDistance;
            _maxSpeed = maxSpeed;
        }
    }
}