using System;
using System.Collections.Generic;

namespace Brain3D
{
    class Response : TimeLine
    {
        Dictionary<QuerySequence, Tree> forest;
        Dictionary<QuerySequence, List<Leaf>> map;

        TreeLayout layout;
        Tree tree;

        Dictionary<QuerySequence, ResponseSequence> sequence;

        public Response()
        {
            layout = new TreeLayout();
            forest = new Dictionary <QuerySequence, Tree>();
            sequence = new Dictionary<QuerySequence, ResponseSequence>();

            map = new Dictionary<QuerySequence, List<Leaf>>();
            SimulatedNeuron.ActivateResponse += new EventHandler(Activate);
        }

        public void Play(Player player)
        {
            sequence[CurrentQuery].Play(player);
        }

        void Activate(object sender, EventArgs e)
        {
            Tuple<QuerySequence, Neuron, int> tuple = (Tuple<QuerySequence, Neuron, int>)sender;

            if (!map.ContainsKey(tuple.Item1))
            {
                map.Add(tuple.Item1, new List<Leaf>());
            }

            map[tuple.Item1].Add(new Leaf(tuple.Item2, tuple.Item3));
        }

        public void Load()
        {
            HashSet<Synapse> synapses = new HashSet<Synapse>(Brain.Synapses.Keys);
            sequence.Clear();
            forest.Clear();

            foreach (QuerySequence query in map.Keys)
            {
                forest.Add(query, new Tree(new HashSet<Leaf>(map[query]), synapses));
                sequence.Add(query, new ResponseSequence());

                foreach (Leaf leaf in map[query])
                {
                    sequence[query].Add(new SequenceTile(leaf.Neuron.Word), false);
                }

                sequence[query].Reload();
            }

            Refresh();
        }

        public void Clear()
        {
            sequence[QueryContainer.Query].Clear();
            map.Clear();
        }

        public override void Show()
        {
            if (!visible)
            {
                tree.Show();
                layout.Show();
                base.Show();
            }
        }

        public override void Hide()
        {
            if (visible)
            {
                tree.Hide();
                layout.Hide();
                base.Hide();
            }
        }

        public override void Refresh()
        {
            if (visible && tree != forest[CurrentQuery])
            {
                if (tree != null)
                {
                    tree.Hide();
                }

                mouses.Clear();

                tree = forest[CurrentQuery];
                tree.Show();

                mouses.AddRange(tree.Leafs);
            }
            else
            {
                tree = forest[CurrentQuery];
            }
        }

        protected override void Rescale()
        {
            tree.Scale = scale;
            layout.Scale = scale;
        }
    }
}