using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;

namespace ConsoleApplication.Graph
{
    public class Node
    {
        private readonly string _name;
        private List<Node> _neighbors = new List<Node>();

        public Node(string name, Node neighbor=null, bool visited=false)
        {
            _name = name;
            if (neighbor != null)
            {
                _neighbors.Add(neighbor);
//                Console.WriteLine(Name + " ADDED TO " + neighbor.Name);
                neighbor.Neighbors.Add(this);
//                Console.WriteLine(neighbor.Name + " ADDED TO " + Name);
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public List<Node> Neighbors
        {
            get { return _neighbors; }
            set { _neighbors = value; }
        }

        public bool Visited { get; set; }

        public Node Find(string target, bool log=false)
        {
            Node ret = null;
            for (int i = 0; i < _neighbors.Count; i++)
            {
                if (!Visited)
                {
                    Visited = true;
                    Program.VisitedStations.Add(this);
                }

                if (_neighbors[i].Name == target)
                {
                    Program.Matches.Add(this);
                    Program.Matches.Add(Neighbors[i]);
                    return _neighbors[i];
                }
                else if (!_neighbors[i].Visited && ret == null)
                {
                    if (log)
                    {
                        Program.Matches.Add(this);
                    }
                    ret = log ? _neighbors[i].Find(target, true) : _neighbors[i].Find(target);
                }
            }

            return ret;
        }
    }
}