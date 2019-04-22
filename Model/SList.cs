using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffman.Model
{
    public class SList
    {
        public SList()
        {
            orders = new List<int>();
        }

        public SList(int key)
        {
            Key = key;
            orders = new List<int>();
        }

        public int Key { get; set; }
        public List<int> orders;
    }
}
