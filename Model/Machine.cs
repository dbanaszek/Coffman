using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffman.Model
{
    public class Machine
    {
        public Machine()
        {
            execution = new List<Node>();
        }

        public List<Node> execution;
    }
}
