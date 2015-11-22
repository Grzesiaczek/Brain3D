using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Brain3D
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            StreamReader reader = new StreamReader(File.Open("Files\\help.xml", FileMode.Open));
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode data = xml.LastChild;
            XmlNode shortcuts = data.FirstChild;

            int index = 0;

            foreach(XmlNode node in shortcuts.ChildNodes)
            {
                Print(node, ++index);
            }
        }

        void Print(XmlNode node, int index)
        {
            int top = 40 + 24 * index;

            Label keys = new Label
            {
                Left = 32,
                Height = 20,
                Text = node.FirstChild.InnerText,
                Top = top,
                Width = 96,
            };

            Label description = new Label
            {
                Left = 128,
                Height = 20,
                Text = node.LastChild.InnerText,
                Top = top,
                Width = 320,
            };

            Controls.Add(keys);
            Controls.Add(description);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1:
                    Close();
                    break;
            }

            return false;
        }

        private void MyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        public void Switch()
        {
            if (Visible)
            {
                Close();
            }
            else
            {
                Show();
            }
        }
    }
}
