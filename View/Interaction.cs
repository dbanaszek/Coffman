using Coffman.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Coffman.View
{
    public class Interaction
    {
        private bool IsValid = true;
        private Node node = new Node();
        private Helper helper;
        private SortedDictionary<int, Node> nodes = new SortedDictionary<int, Node>();
        private List<Helper> helpers = new List<Helper>();
        private List<int> nodeIds = new List<int>(); 

        private void CheckId(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("ID lower or eqaul 0");
                IsValid = false;
            }
        }

        private void AssignConnections()
        {
            foreach (Helper helper in helpers)
            {
                nodes[helper.NodeId].succesors.Add(nodes[helper.SuccesorId]);
                nodes[helper.SuccesorId].predecessors.Add(nodes[helper.NodeId]);
            }

        }

        private void ValueHandel(string name, string value)
        {
            switch (name)
            {
                case "id":
                    CheckId(Int32.Parse(value));
                    node.Id = Int32.Parse(value);
                    nodeIds.Add(Int32.Parse(value));
                    break;

                case "succesor-id":
                    try
                    {
                        CheckId(Int32.Parse(value));
                        helper = new Helper
                        {
                            NodeId = node.Id,
                            SuccesorId = Int32.Parse(value)
                        };
                        helpers.Add(helper);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Succesor ID is lower or equal 0");
                        IsValid = false;
                    }
                    break;
            }
        }

        private void CloseHandler(string value)
        {
            switch (value)
            {
                case "node":
                    nodes.Add(node.Id, node);
                    node = new Node();
                    break;
            }
        }

        public SortedDictionary<int, Node> ReadXML(string filename)
        {

            string name = "";
            StringBuilder sb = new StringBuilder(@"E:\DotNet\Coffman\Coffman\Resources\");
            sb.Append(filename);

            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse
            };
            XmlReader reader = XmlReader.Create(sb.ToString(), settings);

            reader.MoveToContent();

            while (reader.Read() && IsValid)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        name = reader.Name;
                        break;
                    case XmlNodeType.Text:
                        ValueHandel(name, reader.Value);
                        break;
                    case XmlNodeType.EndElement:
                        CloseHandler(reader.Name);
                        break;
                }
            }

            AssignConnections();
            return nodes;
        }

        public int GetMachines()
        {
            int i;
            Console.Write("Podaj ilość maszyn: ");
            i = int.Parse(Console.ReadLine());

            return i;
        }

        public string GetFileName()
        {
            string filename;
            Console.Write("Podaj nazwę pliku XML: ");
            filename = Console.ReadLine();

            return filename;
        }

        private void WriteNodes(Machine machine)
        {
            foreach(Node node in machine.execution)
            {
                if (node.Id < 10)
                    Console.Write("  Z{0} |", node.Id);
                else
                    Console.Write(" Z{0} |", node.Id);
            }
            Console.WriteLine("");

        }

        private void WriteLists(SortedDictionary<int, Node> nodes)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" SListy");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================\n");

        }

        public void WriteTasks(SortedDictionary<int, Node> nodes)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Wczytane zadania");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================\n");
            
            List<int> keys = nodes.Keys.ToList();
            foreach(int key in keys)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Zadanie nr {0}", nodes[key].Id);
                Console.ForegroundColor = ConsoleColor.White;
                if(nodes[key].succesors.Count() > 0)
                    Console.WriteLine("Następniki: ");
                foreach (Node node in nodes[key].succesors)
                {
                    Console.WriteLine("     Zadanie nr {0}", node.Id);
                }
                Console.WriteLine();
            }
        }

        public void WriteOrder(SortedDictionary<int, Node> nodes)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("======================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Uporządkowanie zadań");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("======================\n");

            List<int> keys = nodes.Keys.ToList();
            foreach (int key in keys)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Porządek: {0}", nodes[key].Order);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Zadanie nr {0}", nodes[key].Id);
                Console.Write("SList: (", nodes[key].Id);
                foreach (int order in nodes[key].orders)
                {
                    if(order != 0)
                        Console.Write("{0} ", order);
                }
                Console.WriteLine(")");
                Console.WriteLine();
            }
        }

        public void WriteCMax(int max)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n C max = {0}", max);
        }

        public void WriteMachines(List<Machine> machines)
        {
            int i = 0;
            int max = machines[0].execution.Count();
            machines.Reverse();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Uszeregowanie");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============\n");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (Machine machine in machines)
            {
                if (machine.execution.Count() > max)
                    max = machine.execution.Count();
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("M{0}", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" |");
                WriteNodes(machine);
                i++;
            }
            Console.Write("|");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Czas");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");
            for (i = 1; i<=max; i++)
            {
                if(i < 10)
                    Console.Write("   {0} |", i);
                else
                    Console.Write("  {0} |", i);
            }
            WriteCMax(max);
            Console.ReadLine();
        }
    }
}
