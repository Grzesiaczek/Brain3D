using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            if(!duplex)
                vectors.Add(synapse.Vector);

            origin = synapse.Factor;
            vector = synapse.Vector;
        }

        public void calculate()
        {
            List<float> crosses = new List<float>();

            length = vector.Length;
            crosses.Add(0);

            foreach(AnimatedVector vec in vectors)
            {
                if (vector == vec)
                    continue;

                if (vector.Source == vec.Source || vector.Target == vec.Target)
                    continue;

                if (vector.Source == vec.Target || vector.Target == vec.Source)
                    continue;

                Nullable<float> value = cross(vector, vec);

                if (value.HasValue)
                    crosses.Add(value.Value * length);
            }

            crosses.Sort();
            crosses.Reverse();

            if (place(crosses[0], length, true))
                return;

            int index = 0;

            while(++index < crosses.Count)
                if(place(crosses[index], crosses[index - 1]))
                    return;
        }

        bool place(float start, float end, bool first = false)
        {
            float treshold = 1.6f;

            if (first)
                treshold = 3;

            float segment = end - start;

            if (segment < treshold)
                return false;

            float min = 0;
            float max = 0;
            float position;
            float half = segment * 0.6f;

            if(first)
            {
                min = start + 1;
                max = end - 1.5f;

                if (segment < 8)
                    position = start + half;
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

            if(position > max)
                position = max;

            if (position < min)
                position = min;

            target = position / length;

            return true;
        }

        Nullable<float> cross(AnimatedVector v1, AnimatedVector v2)
        {
            if (!cross(v1.Source, v1.Target, v2.Source, v2.Target))
                return null;

            Tuple<float, float> V1 = line(v1.Source, v1.Target);
            Tuple<float, float> V2 = line(v2.Source, v2.Target);

            float x = (V2.Item2 - V1.Item2) / (V2.Item1 - V1.Item1);
            float y = V1.Item2 - V1.Item1 * x;

            return (y - v1.Source.Y) / (v1.Target.Y - v1.Source.Y);
        }

        Tuple<float, float> line(Vector3 A, Vector3 B)
        {
            if (A.X == B.X)
                return new Tuple<float, float>(0, A.X);

            float a = (B.Y - A.Y) / (B.X - A.X);
            float b = A.Y - a * A.X;

            return new Tuple<float, float>(a, b);
        }

        bool cross(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
        {
            if (side(A, B, C) * side(A, B, D) > 0)
                return false;

            if (side(C, D, A) * side(C, D, B) > 0)
                return false;

            return true;
        }

        float side(Vector3 A, Vector3 B, Vector3 P)
        {
            return A.X * (B.Y - P.Y) + B.X * (P.Y - A.Y) + P.X * (A.Y - B.Y);
        }

        public void update(float scale)
        {
            synapse.Factor = origin + (target - origin) * scale;
        }

        public void update()
        {
            synapse.Factor = target;
        }
    }
}
