﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Chart : DrawableComposite
    {
        static Vector2[] angles;

        List<Tuple<Disk, int>> disks;
        NeuronActivity[] activity;
        
        Vector3[] active;
        Vector3[] normal;

        int count;
        int vertex;

        float value;
        float previous;
        float next;

        public Chart(Neuron neuron, Color color)
        {
            activity = neuron.Activity;
            count = activity.Length;
            vertex = 2 * count;

            this.color = color;
            scale = 1;

            active = new Vector3[vertex];
            normal = new Vector3[vertex];

            vertices = new VertexPositionColor[vertex];
            indices = new int[6 * count - 6];
        }

        public static void initializeAngles()
        {
            double angle = Math.PI / 2 - 1.57;
            angles = new Vector2[314];

            for (int i = 0; i < 314; i++, angle += 0.01)
                angles[i] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void activate()
        {
            for (int i = 0, j = offset; i < vertex; i++)
                buffer.Vertices[j++].Position = active[i];
        }

        public void idle()
        {
            for (int i = 0, j = offset; i < vertex; i++)
                buffer.Vertices[j++].Position = normal[i];
        }

        public override void show()
        {
            prepareCircles();

            if (buffer == null)
                display.add(this);
            else
            {
                buffer.show(indices);
                base.show();
            }

            visible = true;
        }

        public override void hide()
        {
            base.hide();
            buffer.hide(indices);
            visible = false;
        }

        public override void initialize()
        {
            prepare();

            for (int i = 0; i < vertex; i++)
                vertices[i] = new VertexPositionColor(normal[i], color);

            for (int i = 1, j = 0; i < count; i++)
            {
                int index = 2 * i;

                indices[j++] = index - 2;
                indices[j++] = index + 1;
                indices[j++] = index + 0;
                indices[j++] = index - 2;
                indices[j++] = index - 1;
                indices[j++] = index + 1;
            }

            offset = buffer.add(vertices, indices);
            initialized = true;
        }

        void prepareCircles()
        {
            Circle pattern = new Circle(1);
            pattern.Direction = new Vector3(0, 0, 1);
            pattern.rotate();

            disks = new List<Tuple<Disk, int>>();
            drawables.Clear();

            for (int i = 0; i < activity.Length; i++)
            {
                Disk disk = null;

                if (activity[i].Phase == ActivityPhase.Start)
                    disk = new Disk(new Vector3(0.01f * (i + 1), 1, 0.001f), pattern, Color.LightSalmon, 0.04f);
                if (activity[i].Phase == ActivityPhase.Finish)
                    disk = new Disk(new Vector3(0.01f * (i + 1), -1, 0.001f), pattern, Color.LightSkyBlue, 0.04f);
                if (activity[i].Phase == ActivityPhase.Break)
                    disk = new Disk(new Vector3(0.01f * (i + 1), (float)activity[i].Value, 0.001f), pattern, color, 0.025f);

                if (disk == null)
                    continue;

                disk.Scale = 1;
                drawables.Add(disk);
                disks.Add(new Tuple<Disk, int>(disk, i + 1));
            }
        }

        void prepare()
        {
            float constant = 0;
            float ratio = scale * 0.01f;

            float thin = 0.008f;
            float thick = 0.016f;

            normal[0] = new Vector3(0, -thin, 0);
            normal[1] = new Vector3(0, thin, 0);
            normal[2] = new Vector3(0, -thin, 0);
            normal[3] = new Vector3(0, thin, 0);

            active[0] = new Vector3(0, -thick, 0);
            active[1] = new Vector3(0, thick, 0);
            active[2] = new Vector3(0, -thick, 0);
            active[3] = new Vector3(0, thick, 0);

            for (int i = 2; i < count; i++)
            {
                NeuronActivity data = activity[i];
                constant = ratio * i;

                switch(data.Phase)
                {
                    case ActivityPhase.Start:
                        next = 1;
                        break;
                    case ActivityPhase.Finish:
                        next = -1;
                        break;
                    case ActivityPhase.Active:
                        next = 1 - (float)data.Refraction / 15;
                        break;
                    case ActivityPhase.Normal:
                        next = (float)data.Value;
                        break;
                    case ActivityPhase.Break:
                        next = (float)data.Value;
                        break;
                }

                double a1 = Math.Atan((value - previous) * 100 / scale);
                double a2 = Math.Atan((next - value) * 100 / scale);
                double an = (a1 + a2) / 2;
                double ad = an - a1;

                if (ad < 0)
                    ad = -ad;

                int index = (int)(157 + an * 100);
                float factor = (float)(thin / Math.Cos(ad));

                if (factor > 0.02f)
                    factor = 0.02f;

                Vector2 angle = angles[index] * factor;

                normal[2 * i] = new Vector3(constant - angle.X, value - angle.Y, 0);
                normal[2 * i + 1] = new Vector3(constant + angle.X, value + angle.Y, 0);

                angle *= thick / thin;

                active[2 * i] = new Vector3(constant - angle.X, value - angle.Y, 0.0005f);
                active[2 * i + 1] = new Vector3(constant + angle.X, value + angle.Y, 0.0005f);

                previous = value;
                value = next;
            }
        }

        public override void rescale()
        {
            prepare();
            idle();

            foreach (Tuple<Disk, int> tuple in disks)
            {
                Vector3 position = tuple.Item1.Position;
                position = new Vector3(0.01f * scale * tuple.Item2, position.Y, position.Z);
                tuple.Item1.Position = position;
                tuple.Item1.move();
            }
        }

        public override GraphicsBuffer Buffer
        {
            set
            {
                base.Buffer = value;
                buffer = value;

                if (buffer != null)
                    buffer.add(this);
            }
        }

        public override float Scale
        {
            set
            {
                scale = value;
                rescale(); 
            }
        }
    }
}
