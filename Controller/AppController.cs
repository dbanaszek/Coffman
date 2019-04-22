using Coffman.Model;
using Coffman.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffman.Controller
{
    public class AppController
    {
        Interaction interaction = new Interaction();
        private SortedDictionary<int, Node> nodes;
        private SortedDictionary<int, Node> orderedNodes;
        ShowData data;
        List<Machine> machines;
        private static int i = 1;
        private int machine = 0;


        public void Begin()
        {
            SetMachines();
            GetNodes(interaction.GetFileName());
        }

        private void GetNodes(string filename)
        {
            nodes = interaction.ReadXML(filename);
        }

        public void WriteNodes()
        {
            interaction.WriteTasks(nodes);
        }

        public void WriteOrder()
        {
            interaction.WriteOrder(orderedNodes);
        }

        public void BeginOrder()
        {
            List<int> keys = nodes.Keys.ToList();

            foreach(int id in keys)
            {
                if (nodes[id].succesors.Count() == 0)
                {
                    nodes[id].Order = i;
                    ++i;
                }
            }
        }

        private void SetMachines()
        {
            machine = interaction.GetMachines();
        }

        private void SortOrders()
        {
            List<int> keys = nodes.Keys.ToList();
            foreach(int key in keys)
            {
                if (nodes[key].orders.Count() == 0 && nodes[key].nodeOrders.Count() > 0) {
                    foreach (Node order in nodes[key].nodeOrders)
                    {

                        nodes[key].orders.Add(order.Order);
                    }
                    nodes[key].orders.Sort((a, b) => -1 * a.CompareTo(b));
                }
            }
        }

        private List<int> SortKeys(List<int> keys)
        {

            keys.Sort((a, b) => -1 * a.CompareTo(b));
            return keys;
        }

        public void ContinueOrder()
        {
            bool ready = true;
            List<int> keys = nodes.Keys.ToList();
            List<SList> lists = new List<SList>();
            List<Node> areReady = new List<Node>();
            Node smallest;
            int max, j;


            foreach (int id in keys)
            {
                foreach(Node succesor in nodes[id].succesors)
                {
                    if (succesor.Order == 0)
                        ready = false;
                }
                
                if (ready && nodes[id].Order == 0)
                {
                    foreach (Node succesor in nodes[id].succesors)
                    {
                        if(!nodes[id].nodeOrders.Contains(succesor))
                            nodes[id].nodeOrders.Add(succesor);
                    }
                    areReady.Add(nodes[id]);
                }
                ready = true;
            }

            if (areReady.Count() > 0)

                smallest = areReady[0];
            else
                throw new ArgumentNullException("Slist is empty");
            SortOrders();

            foreach (Node node in areReady)
            {
                if (smallest.orders.Count() > node.orders.Count())
                {

                    while (node.orders.Count() < smallest.orders.Count())
                        node.orders.Add(0);

                    max = node.orders.Count();
                }
                else
                {
                    while (smallest.orders.Count() < node.orders.Count())
                        smallest.orders.Add(0);

                    max = smallest.orders.Count();
                }

                for(j = 0; j < max; j++)
                {

                    if (smallest.orders[j] > node.orders[j])
                    {
                        smallest = node;
                        break;
                    }
                    else
                    {
                        if (smallest.orders[j] < node.orders[j])
                            break;
                    }
                        
                }
            }

            nodes[smallest.Id].Order = i;
            i++;
        }

        public void OrderAll()
        {
            BeginOrder();
            while (i <= nodes.Count())
            {
                try
                {
                    ContinueOrder();
                }catch(Exception e)
                {
                    continue;
                }
                
            }
        }

        private void ConvertDictionary()
        {
            List<int> keys = nodes.Keys.ToList();
            orderedNodes = new SortedDictionary<int, Node>();
            foreach(int key in keys)
            {
                orderedNodes.Add(nodes[key].Order, nodes[key]);
            }
        }

        private int FindNode()
        {
            List<int> keys = orderedNodes.Keys.ToList();
            keys.Reverse();
            bool isValid = true;

            foreach(int key in keys)
            {
                if (orderedNodes[key].predecessors.Count() == 0)
                {
                    if (orderedNodes[key].Placed == false)
                        return key;
                }
                else
                {
                    if (orderedNodes[key].Placed == false)
                    {
                        foreach (Node node in orderedNodes[key].predecessors)
                            if (node.Completed == false)
                                isValid = false;
                        if (isValid)
                            return key;
                    }
                }
            }

            throw new IndexOutOfRangeException("No node has been found");

        }

        public void DoWork()
        {
            machines = new List<Machine>();
            List<int> keys = new List<int>();
            ConvertDictionary();
            int j, temp;
            if(machine <= 0)
                throw new ArgumentNullException("No machines to operate");
            else
                for (j = 0; j < machine; j++)
                {
                    machines.Add(new Machine());
                }

            i--;
            while(i >= 0)
            {
                for(j = 0; j < machine; j++)
                {
                    try
                    {
                        temp = FindNode();
                    }catch(Exception e)
                    {
                        continue;
                    }
                    machines[j].execution.Add(orderedNodes[temp]);
                    orderedNodes[temp].Placed = true;
                    keys.Add(temp);
                }
                for (j = 0; j < keys.Count(); j++)
                {
                    orderedNodes[keys[j]].Completed = true;
                }
                keys.Clear();
                i -= machine;
            }
        }

        public void WriteMachines()
        {
            interaction.WriteMachines(machines);
        }

        public void CreateGraph()
        {
            data = new ShowData();
            data.CreateGraph(nodes);
        }
    }
}
