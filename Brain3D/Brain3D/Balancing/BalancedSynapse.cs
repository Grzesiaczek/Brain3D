using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class BalancedSynapse
    {
        static List<AnimatedVector> vectors = new List<AnimatedVector>();
        
        AnimatedSynapse synapse;
        AnimatedVector vector;

        bool duplex;

        float origin;
        float target;
        float length;

        public BalancedSynapse(AnimatedSynapse synapse, bool duplex = false)
        {
            this.synapse = synapse;
            this.duplex = duplex;

            if (!duplex)
            {
                vectors.Add(synapse.Vector);
            }

            origin = synapse.Factor;
            vector = synapse.Vector;
        }

        public void Calculate()
        {
            List<float> crosses = new List<float>();

            length = vector.Length;
            crosses.Add(0);

            foreach (AnimatedVector vec in vectors)
            {
                if (vector == vec)
                {
                    continue;
                }

                if (vector.Source == vec.Source || vector.Target == vec.Target)
                {
                    continue;
                }

                if (vector.Source == vec.Target || vector.Target == vec.Source)
                {
                    continue;
                }

                float? value = Cross(vector, vec);

                if (value.HasValue)
                {
                    crosses.Add(value.Value * length);
                }
            }

            crosses.Sort();
            crosses.Reverse();

            if (Place(crosses[0], length, true))
            {
                return;
            }

            int index = 0;

            while (++index < crosses.Count)
            {
                if (Place(crosses[index], crosses[index - 1]))
                {
                    return;
                }
            }
        }

        bool Place(float start, float end, bool first = false)
        {
            float treshold = 1.6f;

            if (first)
            {
                treshold = 3;
            }

            float segment = end - start;

            if (segment < treshold)
            {
                return false;
            }

            float min = 0;
            float max = 0;
            float position;
            float half = segment * 0.6f;

            if(first)
            {
                min = start + 1;
                max = end - 1.5f;

                if (segment < 8)
                {
                    position = start + half;
                }
                else
                {
                    min = start + half;
                    position = end - 2.4f - 0.1f * length;
                }
            }
            else
            {
                min = start + 0.5f;
                max = end - 0.5f;

                if (segment < 4)
                    position = start + half;
                else
                {
                    min = start + half;
                    position = end - 1.2f - 0.1f * length;
                }
            }

            if (position > max)
            {
                position = max;
            }

            if (position < min)
            {
                position = min;
            }

            target = position / length;

            return true;
        }

        float? Cross(AnimatedVector v1, AnimatedVector v2)
        {
            if (!Cross(v1.Source, v1.Target, v2.Source, v2.Target))
            {
                return null;
            }

            Tuple<float, float> V1 = Line(v1.Source, v1.Target);
            Tuple<float, float> V2 = Line(v2.Source, v2.Target);

            float x = (V2.Item2 - V1.Item2) / (V2.Item1 - V1.Item1);
            float y = V1.Item2 - V1.Item1 * x;

            return (y - v1.Source.Y) / (v1.Target.Y - v1.Source.Y);
        }

        Tuple<float, float> Line(Vector3 A, Vector3 B)
        {
            if (A.X == B.X)
            {
                return new Tuple<float, float>(0, A.X);
            }

            float a = (B.Y - A.Y) / (B.X - A.X);
            float b = A.Y - a * A.X;

            return new Tuple<float, float>(a, b);
        }

        bool Cross(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
        {
            if (Side(A, B, C) * Side(A, B, D) > 0)
            {
                return false;
            }

            if (Side(C, D, A) * Side(C, D, B) > 0)
            {
                return false;
            }

            return true;
        }

        float Side(Vector3 A, Vector3 B, Vector3 P)
        {
            return A.X * (B.Y - P.Y) + B.X * (P.Y - A.Y) + P.X * (A.Y - B.Y);
        }

        public void Update(float scale)
        {
            synapse.Factor = origin + (target - origin) * scale;
        }

        public void Update()
        {
            synapse.Factor = target;
        }
    }
}
