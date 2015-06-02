using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class BalancedState
    {
        static List<AnimatedVector> vectors = new List<AnimatedVector>();
        
        AnimatedState state;
        AnimatedVector vector;

        bool duplex;

        float origin;
        float target;
        float length;

        public BalancedState(AnimatedState state, bool duplex = false)
        {
            this.state = state;
            this.duplex = duplex;

            if(!duplex)
                vectors.Add(state.Vector);

            origin = state.Factor;
            vector = state.Vector;
            length = vector.Vector.Length();
        }

        public void calculate()
        {
            List<float> crosses = new List<float>();

            crosses.Add(length);

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

            if (duplex)
                crosses.Reverse();

            float half = length / 2;
            
            if(crosses[0] > 4 || crosses.Count == 1)
            {
                place(0, crosses[0], true);
                return;
            }

            for(int i = 1; i < crosses.Count && crosses[i] < half; i++)
                if(crosses[i] - crosses[i - 1] > 1)
                {
                    place(crosses[i - 1], crosses[i]);
                    return;
                }
        }

        void place(float start, float end, bool first = false)
        {
            float position = (end - start) / 2;
            target = 1 - position / length;
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
            state.Factor = origin + (target - origin) * scale;
        }

        public void update()
        {
            state.Factor = target;
        }
    }
}
