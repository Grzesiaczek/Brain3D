using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Brain3D
{
    public partial class Simulation : Form
    {
        #region deklaracje

        //Brain brain;

        Animation animation;
        Creation creation;
        Charting charting;
        Tree tree;

        Presentation presentation;

        int length;

        FormWindowState state;

        #endregion

        public Simulation()
        {
            InitializeComponent();
            Constant.load();
            initialize();

            presentation = animation;
        }

        #region inicjalizacja

        void initialize()
        {
            Presentation.Display = display;
            Controls.Add(display);

            BrainElement.initialize(100, 20);
            Neuron.initializeArrays();

            animation = new Animation();
            creation = new Creation();
            charting = new Charting();
            tree = new Tree();

            display.setMargin(rightPanel.Width + 20);
            display.resize();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);

            trackBarDensity.KeyDown += keySuppress;
            trackBarFrame.KeyDown += keySuppress;
            trackBarLength.KeyDown += keySuppress;
            trackBarPace.KeyDown += keySuppress;

            length = trackBarLength.Value * 10;

            animation.changePace(trackBarPace.Value);

            animation.balanceFinished += new EventHandler(balanceFinished);
            animation.intervalChanged += new EventHandler(intervalChanged);
            animation.queryAccepted += new EventHandler(queryAccepted);

            animation.animationStop += new EventHandler(animationStop);
            creation.animationStop += new EventHandler(animationStop);

            resize();
        }

        void simulate()
        {
            tree.clear();
            animation.simulate(length);
            charting.load();
            buttonSimulate.Enabled = false;
        }

        #endregion

        #region obsługa przycisków

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (presentation.Started)
            {
                presentation.stop();
                return;
            }

            buttonSimulate.Enabled = false;
            buttonBack.Enabled = false;
            buttonForth.Enabled = false;

            buttonPlay.Text = "Stop";

            presentation.changePace(trackBarPace.Value);
            presentation.start();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            simulate();
        }

        #endregion

        #region obsługa przycisków sterujących

        private void buttonPaceUp_Click(object sender, EventArgs e)
        {
            if (trackBarPace.Value != trackBarPace.Maximum)
                trackBarPace.Value++;
        }

        private void buttonPaceDown_Click(object sender, EventArgs e)
        {
            if (trackBarPace.Value != trackBarPace.Minimum)
                trackBarPace.Value--;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            presentation.back();
        }

        private void buttonForth_Click(object sender, EventArgs e)
        {
            presentation.forth();
        }

        private void buttonLengthDown_Click(object sender, EventArgs e)
        {
            if (length > 50)
                length -= 10;

            labelLength.Text = length.ToString();
            trackBarLength.Value = length / 10;
        }

        private void buttonLengthUp_Click(object sender, EventArgs e)
        {
            if (length < 500)
                length += 10;

            labelLength.Text = length.ToString();
            trackBarLength.Value = length / 10;
        }

        #endregion

        #region obsługa klawiatury

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    Balancing.Instance.stop();
                    break;
                case Keys.F1:
                    display.print(presentation);
                    break;
                case Keys.F4:

                    break;
                case Keys.F11:
                    Balancing.Instance.phaseFour();
                    break;
                case Keys.F12:
                    presentation.balanceSynapses();
                    break;
                case Keys.Insert:
                    presentation.changeInsertion();
                    break;
                case Keys.Left:
                    presentation.back();
                    break;
                case Keys.Right:
                    presentation.forth();
                    break;
                case Keys.Up:
                    presentation.higher();
                    break;
                case Keys.Down:
                    presentation.lower();
                    break;
                case Keys.Space:
                    presentation.space();
                    break;
                case Keys.Back:
                    presentation.erase();
                    break;
                case Keys.Enter:
                    presentation.enter();
                    break;
                case Keys.NumPad2:
                    presentation.down();
                    break;
                case Keys.NumPad4:
                    presentation.left();
                    break;
                case Keys.NumPad6:
                    presentation.right();
                    break;
                case Keys.NumPad8:
                    presentation.up();
                    break;
                case Keys.NumPad1:
                    presentation.tighten();
                    break;
                case Keys.NumPad3:
                    presentation.broaden();
                    break;
                case Keys.NumPad7:
                    presentation.closer();
                    break;
                case Keys.NumPad9:
                    presentation.farther();
                    break;
                case Keys.NumPad5:
                    presentation.center();
                    break;
                default:

                    int code = msg.WParam.ToInt32();

                    if (code > 64 && code < 91)
                    {
                        if ((keyData & Keys.Shift) != Keys.Shift)
                            code += 32;

                        presentation.add((char)code);
                    }
                    break;
            }

            display.Focus();
            return false;
        }

        void keySuppress(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        #endregion

        #region obsługa zmian rozmiaru

        private void resize(object sender, EventArgs e)
        {
            resize();

            if (WindowState != state)
            {
                resizeEnd(this, null);
                state = WindowState;
            }
        }

        private void resize()
        {
            int px = Width - rightPanel.Width - 20;
            int py = Height - 80;

            rightPanel.Left = px;
            rightPanel.Height = py + 40;

            trackBarFrame.Left = 140;
            trackBarFrame.Width = px - 200;
            trackBarFrame.Top = py;

            buttonBack.Top = py;
            buttonBack.Left = 100;

            buttonForth.Top = py;
            buttonForth.Left = px - 50;
        }

        private void resizeEnd(object sender, EventArgs e)
        {
            display.resize();
        }

        #endregion

        #region obsługa zmiany trybu pracy

        private void radioButtonCreation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonCreation.Checked)
                return;

            show(creation);
        }

        private void radioButtonChart_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonChart.Checked)
                return;

             show(charting);
        }

        private void radioButtonSimulation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonSimulation.Checked)
                return;

            show(animation);
        }

        private void radioButtonTree_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonTree.Checked)
                return;

            show(tree);
        }

        void show(Presentation presentation)
        {
            this.presentation.hide();
            this.presentation = presentation;
            presentation.show();
            display.Focus();
        }

        #endregion

        #region obsługa zdarzeń

        private void Simulation_Load(object sender, EventArgs e)
        {
            /*
            brain.addSentence("type new sequence"));
            sbrain.addSentence("or load from file"));*/

            Brain brain = new Brain();
            
            brain.addSentence("i have a monkey");
            brain.addSentence("my monkey is very small");
            brain.addSentence("it is very lovely");
            brain.addSentence("it likes to sit on my head");
            brain.addSentence("it can jump very quickly");
            brain.addSentence("it is also very clever");
            brain.addSentence("it learns quickly");
            brain.addSentence("my monkey is lovely");
            brain.addSentence("i have also a small dog");

            //sequences.Add(brain.addSentence("this is"));

            Presentation.Brain = brain;
            Presentation.Controller.add(trackBarFrame);

            simulate();
            show(presentation);
            //radioButtonTree.Checked = true;

            WindowState = FormWindowState.Maximized;
            
            //openData("Files\\data.xml");
        }

        private void Simulation_Close(object sender, EventArgs e)
        {
            presentation.hide();
        }

        private void animationStop(object sender, EventArgs e)
        {
            buttonPlay.Text = "Play";
            buttonSimulate.Enabled = true;
        }

        private void balanceFinished(object sender, EventArgs e)
        {
            buttonPlay.Invoke(new ThreadStart(delegate()
            {
                buttonPlay.Enabled = true;
            }));
        }

        private void intervalChanged(object sender, EventArgs e)
        {
            simulate();
        }

        private void queryAccepted(object sender, EventArgs e)
        {
            simulate();
        }

        #endregion

        #region obsługa suwaków

        private void trackBarPace_Scroll(object sender, EventArgs e)
        {
            presentation.changePace(trackBarPace.Value);
            labelPace.Text = trackBarPace.Value.ToString();
        }

        private void trackBarLength_Scroll(object sender, EventArgs e)
        {
            length = trackBarLength.Value * 10;
            labelLength.Text = length.ToString();
        }

        private void trackBarFrame_Scroll(object sender, EventArgs e)
        {
            presentation.changeFrame(trackBarFrame.Value);
        }

        private void trackBarDensity_Scroll(object sender, EventArgs e)
        {
            
        }

        #endregion

        void openData(String path)
        {
            Brain brain = new Brain();
            DateTime date = DateTime.Now;
            String name = date.ToString("yyyyMMdd-HHmmss");

            StreamReader reader = new StreamReader(File.Open(path, FileMode.Open));
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode node = xml.ChildNodes.Item(1).FirstChild;

            foreach (XmlNode xn in node.ChildNodes)
                brain.addSentence(xn.InnerText.ToLower());

            Presentation.Brain = brain;
            simulate();
            show(presentation);
        }

        void saveData(String path)
        {

        }

        private void openFile_FileOk(object sender, CancelEventArgs e)
        {
            openData(openFile.FileName);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openFile.DefaultExt = ".xml";
            openFile.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            DialogResult test = openFile.ShowDialog();
        }

        private void buttonBalance_Click(object sender, EventArgs e)
        {
            animation.balance();
            buttonPlay.Enabled = false;
        }

        private void checkBoxState_CheckedChanged(object sender, EventArgs e)
        {
            Number3D.Visible = checkBoxState.Checked;
        }

        private void radioButtonBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonBox.Checked)
                return;

            Constant.Space = SpaceMode.Box;
        }

        private void radioButtonSphere_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonSphere.Checked)
                return;

            Constant.Space = SpaceMode.Sphere;
        }

        private void radioButtonSquare_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxWhite_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxWhite.Checked)
            {
                display.Background = Microsoft.Xna.Framework.Color.White;
                trackBarFrame.BackColor = Color.White;
            }
            else
            {
                display.Background = Microsoft.Xna.Framework.Color.Gainsboro;
                trackBarFrame.BackColor = Color.Gainsboro;
            }
        }
    }
}