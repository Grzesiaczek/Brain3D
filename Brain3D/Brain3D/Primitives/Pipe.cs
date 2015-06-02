using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Pipe : DrawableElement
    {
        static List<Tuple<Color, Color>> palettes;
        Tuple<Color, Color> palette;

        Vector3 source;
        Vector3 target;

        Vector3 vector;
        Vector3 direction;

        Vector3 v1, v2, v3;
        float r1, r2;

        public Pipe(Vector3 source, Vector3 target, float r1, float r2, int mode)
        {
            if (palettes == null)
                throw new Exception("Pipe class not initialized!");

            framework = new Vector3[6];
            vertices = new VertexPositionColor[6];
            indices = new int[12];

            this.source = source;
            this.target = target;

            this.r1 = r1;
            this.r2 = r2;

            palette = palettes[mode];
            vector = target - source;
            rotate();
        }

        public static void initializePalettes()
        {
            palettes = new List<Tuple<Color, Color>>();

            palettes.Add(new Tuple<Color, Color>(Color.Indigo, Color.Brown));
            palettes.Add(new Tuple<Color, Color>(Color.Thistle, Color.IndianRed));
        }

        public override void initialize()
        {
            rotate();

            for (int i = 0; i < 6; i++)
                vertices[i] = new VertexPositionColor(framework[i], palette.Item1);

            vertices[2].Color = palette.Item2;
            vertices[3].Color = palette.Item2;

            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 3;
            indices[3] = 0;
            indices[4] = 3;
            indices[5] = 1;
            indices[6] = 2;
            indices[7] = 4;
            indices[8] = 5;
            indices[9] = 2;
            indices[10] = 5;
            indices[11] = 3;

            offset = buffer.add(vertices, indices);
            initialized = true;
        }

        public override void move()
        {
            if (!initialized)
                return;

            rotate();

            for (int i = 0, j = offset; i < 6; i++)
                buffer.Vertices[j++].Position = framework[i];
        }

        public override void rotate()
        {
            direction = vector;
            direction.Z = 0;

            direction.Normalize();
            direction = new Vector3(-direction.Y, direction.X, 0);

            v1 = direction * r1;
            v2 = direction * (r1 + (r2 - r1) * scale);
            v3 = target * scale;

            framework[0] = source - v1;
            framework[2] = source;
            framework[4] = source + v1;

            framework[1] = v3 - v2;
            framework[3] = v3;
            framework[5] = v3 + v2;
        }

        public override void rescale()
        {
            rotate();
        }

        public void add()
        {
            display.add(this);
            buffer.block(indices);
        }

        public Vector3 Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                vector = target - source;
            }
        }

        public Vector3 Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                vector = target - source;
            }
        }
    }
}
