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
        static String path;
        static StringFormat format;

        static float pi2;
        static float pi4;

        static Vector3 box;
        static float radius;

        static SpaceMode space;
        public static event EventHandler spaceChanged;

        public static void load()
        {
            StreamReader reader;

            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

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
            pi4 = (float)Math.PI / 4;

            box = new Vector3(48, 30, 4);
            radius = 25;

            space = SpaceMode.Box;
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

        public static void changePath(String path)
        {
            Path = path;
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

        public static String Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        public static StringFormat Format
        {
            get
            {
                return format;
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

        public static float PI4
        {
            get
            {
                return pi4;
            }
        }
    }
}
