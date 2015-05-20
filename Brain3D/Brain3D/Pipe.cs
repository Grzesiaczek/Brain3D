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
        Circle framework;

        Vector3 source;
        Vector3 target;

        Vector3 direction;
        Color[] palette;

        static List<Color[]> palettes;
        static bool initialized;

        int points;
        float r1, r2;

        public Pipe(Vector3 source, Vector3 target, float r1, float r2, int mode)
        {
            if (!initialized)
                throw new Exception("Pipe class not initialized!");

            this.source = source;
            this.target = target;

            this.r1 = r1;
            this.r2 = r2;

            framework = new Circle(Vector3.Zero, 4);
            points = framework.Points;

            palette = palettes[mode];
            vertices = new VertexPositionColor[2 * points];
            indices = new int[6 * points];
        }

        public static void initializePalettes()
        {
            palettes = new List<Color[]>();

            palettes.Add(addPalette(Color.Indigo, Color.IndianRed));
            palettes.Add(addPalette(Color.Wheat, Color.ForestGreen));

            initialized = true;
        }

        public static Color[] addPalette(Color start, Color end)
        {
            int interval = 4;
            Color[] palette = new Color[32];

            palette[0] = start;
            palette[interval] = end;

            int r = palette[interval].R - palette[0].R;
            int g = palette[interval].G - palette[0].G;
            int b = palette[interval].B - palette[0].B;

            for (int i = 1; i < interval; i++)
            {
                int red = palette[0].R + i * r / interval;
                int green = palette[0].G + i * g / interval;
                int blue = palette[0].B + i * b / interval;

                palette[i] = new Color(red, green, blue);
            }

            for (int i = 4; i < 8; i++)
                palette[i] = palette[8 - i];

            for (int i = 0; i < 8; i++)
            {
                palette[i + 8] = palette[i];
                palette[i + 16] = palette[i];
                palette[i + 24] = palette[i];
            }

            return palette;
        }

        public override void initialize()
        {
            for (int i = 0, j = 0; i < points; i++)
            {
                vertices[j++] = new VertexPositionColor(framework.Data[i] * r1 + source, palette[i]);
                vertices[j++] = new VertexPositionColor(framework.Data[i] * r2 + target, palette[i]);
            }

            int index = 6;
            int vertex = 2 * points - 2;

            indices[0] = 0;
            indices[1] = vertex;
            indices[2] = vertex + 1;
            indices[3] = 0;
            indices[4] = vertex + 1;
            indices[5] = 1;

            for (int i = 1; i < points; i++)
            {
                vertex = 2 * i;

                indices[index++] = vertex;
                indices[index++] = vertex - 2;
                indices[index++] = vertex - 1;
                indices[index++] = vertex;
                indices[index++] = vertex - 1;
                indices[index++] = vertex + 1;
            }

            offset = buffer.add(vertices, indices);
        }

        public override void move()
        {
            rotate();

            for (int i = 0, j = offset; i < points; i++)
            {
                buffer.Vertices[j++].Position = framework.Data[i] * r1 + source;
                buffer.Vertices[j++].Position = framework.Data[i] * r2 + target;
            }
        }

        public override void rotate()
        {
            framework.Direction = target - source;
            framework.rotate();
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
            }
        }
    }
}
