﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Ring : DrawableElement
    {
        Circle pattern;

        int points;
        int points2;

        float r1;
        float r2;

        public Ring(Vector3 position, Circle pattern, Color color)
        {
            this.position = position;
            this.pattern = pattern;

            this.color = color;
            points = pattern.Points;
            points2 = 2 * points;

            framework = new Vector3[points2];
            vertices = new VertexPositionColor[points2];
            indices = new int[6 * points];
        }

        public override void Initialize()
        {
            for (int i = 0, j = 0; i < points; i++)
            {
                framework[j++] = pattern.Data[i] * r1 * scale;
                framework[j++] = pattern.Data[i] * r2 * scale;
            }

            for (int i = 0; i < points2; i++)
            {
                vertices[i] = new VertexPositionColor(framework[i] + position, color);
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

            offset = buffer.Add(vertices, indices);
            initialized = true;
        }

        public override void Move()
        {
            if (initialized)
            {
                for (int i = 0, j = offset; i < points2; i++)
                {
                    buffer.Vertices[j++].Position = framework[i] + position;
                }
            }
        }

        public override void Repaint()
        {
            for (int i = 0, j = offset; i < points2; i++)
            {
                buffer.Vertices[j++].Color = color;
            }
        }

        public override void Rescale()
        {
            for (int i = 0, j = 0; i < points; i++)
            {
                framework[j++] = pattern.Data[i] * r1 * scale;
                framework[j++] = pattern.Data[i] * r2 * scale;
            }

            Move();
        }

        public float R1
        {
            set
            {
                r1 = value;
            }
        }

        public float R2
        {
            set
            {
                r2 = value;
            }
        }
    }
}
