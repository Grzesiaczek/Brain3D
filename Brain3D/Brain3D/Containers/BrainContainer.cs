using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Brain3D
{
    class BrainContainer
    {
        List<Brain> data;
        Brain brain;

        int index;

        public BrainContainer()
        {
            data = new List<Brain>();
            brain = new Brain();
            data.Add(brain);
            index = 0;
        }

        public void Load()
        {
            StreamReader reader;

            try
            {
                data.Clear();
                index = 0;

                reader = new StreamReader(File.Open("Files\\data.xml", FileMode.Open));
                XmlDocument xml = new XmlDocument();
                xml.Load(reader);
                reader.Close();

                XmlNode parent = xml.FirstChild.NextSibling;

                foreach (XmlNode node in parent)
                {
                    Brain brain = new Brain();
                    data.Add(brain);
                    brain.Load(node, Presentation.Display);
                }

                this.brain = data[0];
                Reload();
            }
            catch (Exception) { }
        }

        public void Simulate(int length)
        {
            foreach (Brain brain in data)
            {
                brain.QueryContainer.Simulate(length);
            }
        }

        public void NextBrain()
        {
            if (++index == data.Count)
            {
                index = 0;
            }

            brain = data[index];
            Reload();
        }

        public void PreviousBrain()
        {
            if (index == 0)
            {
                index = data.Count;
            }

            brain = data[--index];
            Reload();
        }

        private void Reload()
        {
            Presentation.Controller.UpdateBrains(index + 1, data.Count);
            Presentation.Reload();
        }

        public Brain Brain
        {
            get
            {
                return brain;
            }
        }

        public QueryContainer QueryContainer
        {
            get
            {
                return brain.QueryContainer;
            }
        }
    }
}
