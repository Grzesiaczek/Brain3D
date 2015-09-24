using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class Response : TimeLine
    {
        List<Tree> wood;

        Dictionary<QuerySequence, Tree> forrest;
        Dictionary<QuerySequence, List<Leaf>> map;

        TreeLayout layout;
        Tree tree;

        Dictionary<QuerySequence, ResponseSequence> sequence;

        int index;

        public Response()
        {
            wood = new List<Tree>();

            layout = new TreeLayout();
            forrest = new Dictionary <QuerySequence, Tree>();
            sequence = new Dictionary<QuerySequence, ResponseSequence>();

            map = new Dictionary<QuerySequence, List<Leaf>>();
            SimulatedNeuron.Activate += new EventHandler(Activate);
        }

        public void Play(Player player)
        {
            sequence[container.Query].Play(player);
        }

        void Activate(object sender, EventArgs e)
        {
            Tuple<Neuron, int> tuple = (Tuple<Neuron, int>)sender;

            if (!map.ContainsKey(Constant.Query))
                map.Add(Constant.Query, new List<Leaf>());

            map[Constant.Query].Add(new Leaf(tuple.Item1, tuple.Item2));
        }

        public void Reload()
        {
            HashSet<Synapse> synapses = new HashSet<Synapse>(brain.Synapses.Keys);
            sequence.Clear();

            foreach (QuerySequence query in map.Keys)
            {
                forrest.Add(query, new Tree(new HashSet<Leaf>(map[query]), synapses));
                sequence.Add(query, new ResponseSequence());

                foreach (Leaf leaf in map[query])
                    sequence[query].Add(new SequenceTile(leaf.Neuron.Word));

                sequence[query].Reload();
            }

            /*
            wood.Clear();
            wood.Add(new Tree(new HashSet<Leaf>(leafs), new HashSet<Synapse>(brain.Synapses.Keys)));
            /*
            foreach(Tuple<Neuron, int> tuple in query.Activations)
            {
                HashSet<Leaf> bush = new HashSet<Leaf>();
                bush.Add(new Leaf(map[tuple]));

                int index = leafs.IndexOf(map[tuple]) + 1;
                int max = tuple.Item2 + query.Interval * 10 + 30;

                for(int i = index; i < leafs.Count; i++)
                {
                    if (leafs[i].Time > max)
                        break;

                    if (leafs[i].Neuron == tuple.Item1)
                        continue;

                    bush.Add(new Leaf(leafs[i]));
                }

                wood.Add(new Tree(bush, new HashSet<Synapse>(brain.Synapses.Keys)));
            }

            sequence.Reload();
            tree = wood[0];*/
        }

        public void Clear()
        {
            sequence[container.Query].Clear();
            map.Clear();
        }

        public override void Show()
        {
            foreach (Tree bush in wood)
                bush.Show();

            layout.Show();
            base.Show();

            foreach (Tree bush in wood)
                bush.Hide();

            tree.Show();
        }

        public override void Hide()
        {
            tree.Hide();
            layout.Hide();
            base.Hide();
        }

        public override void Refresh()
        {
            
        }

        protected override void Rescale()
        {
            foreach (Tree tree in wood)
                tree.Scale = scale;

            layout.Scale = scale;
        }

        public override void Up()
        {
            if(index + 1 < wood.Count)
            {
                tree.Hide();
                tree = wood[++index];
                tree.Show();
            }
        }

        public override void Down()
        {
            if(index > 0)
            {
                tree.Hide();
                tree = wood[--index];
                tree.Show();
            }
        }
    }
}