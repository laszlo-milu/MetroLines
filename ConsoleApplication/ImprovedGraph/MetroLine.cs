using System.Collections.Generic;

namespace ConsoleApplication.ImprovedGraph
{
    public class MetroLine
    {
        private readonly List<string> _stations;

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

        private readonly int _averageDistance;
        private readonly int _maxSpeed;

        public MetroLine(List<string> stations, int averageDistance, int maxSpeed)
        {
            _stations = stations;
            _averageDistance = averageDistance;
            _maxSpeed = maxSpeed;
        }
    }
}