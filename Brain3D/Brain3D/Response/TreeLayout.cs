using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class TreeLayout : DrawableComposite
    {
        Line horizontal;
        List<Line> legend;
        List<LabelTL> labels;

        public TreeLayout()
        {
            legend = new List<Line>();
            labels = new List<LabelTL>();
            horizontal = new Line(new Vector3(0, -1, 0), new Vector3(25, -1, 0), Color.Purple, 0.005f);

            for (int i = 0; i < 25; i++)
                legend.Add(new Line(new Vector3(i, -1.05f, 0), new Vector3(i, -1, 0), Color.Purple, 0.005f));

            drawables.Add(horizontal);
            drawables.AddRange(legend);
        }

        public override void Show()
        {
            if (!initialized)
            {
                for (int i = 0; i < 25; i++)
                    labels.Add(new LabelTL(i * 10));

                drawables.AddRange(labels);
                initialized = true;
            }

            base.Show();
        }

        public override float Scale
        {
            set
            {
                float x = value * 25;
                scale = value;

                for (int i = 0; i < 25; i++)
                {
                    x = scale * i;
                    legend[i].shift(new Vector3(x, -1.05f, 0), new Vector3(x, -1, 0));
                    labels[i].moveX(x);
                }
            }
        }
    }
}
