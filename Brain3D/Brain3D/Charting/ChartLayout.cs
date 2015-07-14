using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class ChartLayout : DrawableComposite
    {
        List<Line> horizontals;
        List<Line> legend;
        List<LabelTL> labels;

        public ChartLayout()
        {
            horizontals = new List<Line>();
            legend = new List<Line>();
            labels = new List<LabelTL>();

            horizontals.Add(new Line(new Vector3(0, 1, 0), new Vector3(25, 1, 0), Color.Purple, 0.005f));
            horizontals.Add(new Line(new Vector3(0, 0, 0), new Vector3(25, 0, 0), Color.Purple, 0.005f));
            horizontals.Add(new Line(new Vector3(0, -1, 0), new Vector3(25, -1, 0), Color.Purple, 0.005f));

            drawables.Add(new Line(new Vector3(0, -1.05f, 0), new Vector3(0, 1.2f, 0), Color.Purple, 0.005f));
            drawables.Add(new Line(new Vector3(-0.05f, 1.1f, 0), new Vector3(0, 1.2f, 0), Color.Purple, 0.005f));
            drawables.Add(new Line(new Vector3(0.05f, 1.1f, 0), new Vector3(0, 1.2f, 0), Color.Purple, 0.005f));

            for (int i = 0; i < 25; i++)
                legend.Add(new Line(new Vector3(i, -1.05f, 0), new Vector3(i, -1, 0), Color.Purple, 0.005f));

            drawables.AddRange(horizontals);
            drawables.AddRange(legend);
        }

        public override void show()
        {
            if (!initialized)
            {
                for (int i = 0; i < 25; i++)
                    labels.Add(new LabelTL(i * 10));

                drawables.AddRange(labels);
                drawables.Add(new LabelTL(new Vector3(-0.28f, -1.18f, 0), "(t)"));
                initialized = true;
            }

            base.show();
        }

        public override float Scale
        {
            set
            {
                float x = value * 25;
                scale = value;

                horizontals[0].shift(new Vector3(0, 1, 0), new Vector3(x, 1, 0));
                horizontals[1].shift(new Vector3(0, 0, 0), new Vector3(x, 0, 0));
                horizontals[2].shift(new Vector3(0, -1, 0), new Vector3(x, -1, 0));

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
