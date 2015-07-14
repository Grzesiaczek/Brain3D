using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class Constant
    {
        #region deklaracje

        static Random random;
        static String path;

        static Vector3 box;
        static Vector3 balance;
        static Vector3 normal;
        static Vector3 shift;

        static Vector2[] circle;

        static float pi2;
        static float radius;

        static SpaceMode space;
        public static event EventHandler spaceChanged;

        #endregion

        public static void load()
        {
            StreamReader reader;
            random = new Random();

            try
            {
                reader = new StreamReader(File.Open("Files\\config.xml", FileMode.Open));
                XmlDocument xml = new XmlDocument();
                xml.Load(reader);
                reader.Close();
                XmlNode node;

                node = xml.FirstChild.NextSibling.FirstChild;
                path = node.InnerText;

                node = node.NextSibling;
                int text = Int32.Parse(node.InnerText);
            }
            catch(Exception)
            {
                loadDefault();
            }

            pi2 = (float)Math.PI / 2;
            radius = 25;

            balance = new Vector3(48, 30, 16);
            normal = new Vector3(48, 30, 4);

            shift = normal - balance;
            box = normal;

            space = SpaceMode.Box;

            initializeCircle();
        }

        static void initializeCircle()
        {
            int total = 128;
            float interval = (float)Math.PI * 2 / total;

            circle = new Vector2[total];

            for (int i = 0; i < total; i++)
                circle[i] = new Vector2((float)Math.Cos(interval * i), (float)Math.Sin(interval * i));
        }

        public static void save()
        {
            StreamReader reader = new StreamReader(File.Open("Files\\config.xml", FileMode.Open));
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            StreamWriter writer = new StreamWriter(File.Open("Files\\config.xml", FileMode.Open));
            xml.FirstChild.NextSibling.FirstChild.InnerText = path;
            xml.Save(writer);
            writer.Close();
        }

        static void changePath(String path)
        {
            Constant.path = path;
            Constant.save();

            if (File.Exists(System.IO.Path.Combine(path, "data.xml")))
                return;

            Directory.CreateDirectory(path);
            File.Create(System.IO.Path.Combine(path, "data.xml"));
            Directory.CreateDirectory(System.IO.Path.Combine(path, "Data"));
            Directory.CreateDirectory(System.IO.Path.Combine(path, "Images"));
            Directory.CreateDirectory(System.IO.Path.Combine(path, "Save"));
            Directory.CreateDirectory(System.IO.Path.Combine(path, "Simulation"));
        }

        static void loadDefault()
        {
            changePath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Files"));
        }

        public static Vector3 randomPoint()
        {
            Vector3 area;

            if (Constant.Space == SpaceMode.Box)
                area = Constant.Box;
            else
            {
                float radius = Constant.Radius * 0.7f;
                area = new Vector3(radius, radius, radius);
            }

            float x = random.Next((int)-area.X + 1, (int)area.X - 1);
            float y = random.Next((int)-area.Y + 1, (int)area.Y - 1);
            float z = random.Next((int)-area.Z + 1, (int)area.Z - 1);

            return new Vector3(x, y, z);
        }

        public static void setBox(Balancing.Phase phase)
        {
            if (phase == Balancing.Phase.Three)
                box = normal;
            else
                box = balance;
        }

        public static void setBox(float factor)
        {
            box = balance + factor * shift;
        }

        public static Tuple<Vector2, float> getDistance(Vector3 source, Vector3 target, Vector3 point)
        {
            Vector3 vector = target - source;
            Vector3 position = point - source;
            vector.Z = 0;

            float a = vector.Y;
            float b = vector.X;
            float c = b * position.X + a * position.Y;
            float x, y;
            float eq; // punkt równowagi

            if (a == 0)
            {
                x = c / b;
                y = 0;
                eq = x / b;
            }
            else if (b == 0)
            {
                x = 0;
                y = c / a;
                eq = y / a;
            }
            else
            {
                x = c * b / (vector.LengthSquared());
                eq = x / b;
                y = a * eq;
            }

            return new Tuple<Vector2, float>(new Vector2(position.X - x, position.Y - y), eq);
        }

        #region własciwości

        public static Vector2[] Circle
        {
            get
            {
                return circle;
            }
        }

        public static String Path
        {
            get
            {
                return path;
            }
        }

        public static SpaceMode Space
        {
            get
            {
                return space;
            }
            set
            {
                space = value;
                spaceChanged(space, null);
            }
        }

        public static Vector3 Box
        {
            get
            {
                return box;
            }
        }

        public static float Radius
        {
            get
            {
                return radius;
            }
        }

        public static float PI2
        {
            get
            {
                return pi2;
            }
        }

        #endregion
    }
}
