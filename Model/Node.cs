using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffman.Model
{
    public class Node
    {
        public Node()
        {
            Order = 0;
            Completed = false;
            Placed = false;
            succesors = new List<Node>();
            predecessors = new List<Node>();
            nodeOrders = new List<Node>();
            orders = new List<int>();
        }

        public Node(int id)
        {
            Id = id;
            Order = 0;
            Completed = false;
            Placed = false;
            succesors = new List<Node>();
            predecessors = new List<Node>();
            nodeOrders = new List<Node>();
            orders = new List<int>();
        }

        public int Id { get; set; }
        public int Order { get; set; }
        public bool Completed { get; set; }
        public bool Placed { get; set; }
        public List<Node> succesors;
        public List<Node> predecessors;
        public List<Node> nodeOrders;
        public List<int> orders;
    }
}
