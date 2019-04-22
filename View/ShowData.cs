using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;
using Coffman.Model;
using System.Threading;

namespace Coffman.View
{
    public class ShowData
    {
        List<int> keys;
        List<GraphHelper> helpers;
        SortedDictionary<int, Model.Node> nodes;

        private void CreateHelpers()
        {
            helpers = new List<GraphHelper>();
            GraphHelper helper = new GraphHelper();

            foreach(int key in keys)
            {
                foreach (Model.Node node in nodes[key].succesors) {
                    helper.Id = nodes[key].Id;
                    helper.Connect = node.Id;
                    if (!helpers.Contains(helper))
                    {
                        helpers.Add(helper);
                    }
                    helper = new GraphHelper();
                        
                }
            }
        }
        public void CreateGraph(SortedDictionary<int, Model.Node> nodes)
        {
            //form
            Form form = new Form();
            //view
            GViewer viewer = new GViewer();
            //graph
            Graph graph = new Graph("graph");

            graph.Attr.LayerDirection = LayerDirection.LR;

            this.nodes = nodes;
            keys = nodes.Keys.ToList();
            CreateHelpers();


            foreach (GraphHelper helper in helpers)
            {
                graph.AddEdge(helper.Id.ToString(), helper.Connect.ToString());
            }

            viewer.Graph = graph;

            form.SuspendLayout();
            viewer.Dock = DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();

            Thread thread = new Thread(() => form.ShowDialog());
            thread.Start();
        }
    }
}
