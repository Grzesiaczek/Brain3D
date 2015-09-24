using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace Brain3D
{
    class Number3D : Text3D
    {
        static Tuple<Vector3[], int[]>[] patterns;
        static bool change;
        static bool visible;

        int number;

        public Number3D(Vector3 position)
        {
            this.position = position;
            color = Color.DarkSlateBlue;
            number = 0;
        }

        public static void initializePatterns()
        {
            patterns = new Tuple<Vector3[], int[]>[101];

            for (int i = 0; i < 100; i++)
                patterns[i] = getPattern(i.ToString());

            patterns[100] = getPattern("R");
        }

        public override void Initialize()
        {
            pattern = patterns[number].Item1;
            int[] numbers = patterns[number].Item2;

            int vertex = pattern.Length;
            int index = numbers.Length;
            
            framework = new Vector3[pattern.Length];
            vertices = new VertexPositionColor[pattern.Length];
            indices = new int[index];

            for (int i = 0; i < pattern.Length; i++)
            {
                framework[i] = Vector3.Transform(pattern[i], camera.Rotation) * scale;
                vertices[i] = new VertexPositionColor(framework[i] + position, Color.Black);
            }

            for (int i = 0; i < index; i++)
                indices[i] = numbers[i];

            offset = buffer.Add(vertices, indices);
            initialized = true;
        }

        public int Value
        {
            set
            {
                if (value != number)
                {
                    number = value;
                    change = true;
                }
            }
        }

        public static bool Change
        {
            get
            {
                return change;
            }
        }

        public static bool Visible
        {
            set
            {
                visible = value;
            }
        }
    }
}
