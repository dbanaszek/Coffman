using Coffman.Controller;
using Coffman.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffman
{
    class Program
    {
        
        static void Main(string[] args)
        {
            AppController controller = new AppController();
            controller.Begin();
            controller.WriteNodes();
            controller.OrderAll();
            controller.CreateGraph();
            controller.DoWork();
            controller.WriteOrder();
            controller.WriteMachines();
        }
    }
}
