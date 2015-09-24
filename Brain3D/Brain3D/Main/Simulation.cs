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
        Response response;

        Presentation presentation;
        QueryContainer container;
        Player player;

        FormWindowState state;

        int length;

        #endregion

        public Simulation()
        {
            InitializeComponent();
            Constant.Load();
            Initialize();

            presentation = animation;
        }

        #region inicjalizacja

        void Initialize()
        {
            Presentation.Display = display;
            Controls.Add(display);
            player = new Player();

            SimulatedElement.initialize(100, 20);
            SimulatedNeuron.InitializeArrays();

            animation = new Animation();
            creation = new Creation();
            charting = new Charting();
            response = new Response();

            display.SetMargin(rightPanel.Width + 20);
            display.ResizeWindow();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);

            trackBarDensity.KeyDown += keySuppress;
            trackBarFrame.KeyDown += keySuppress;
            trackBarLength.KeyDown += keySuppress;
            trackBarPace.KeyDown += keySuppress;

            length = trackBarLength.Value * 10;
            Presentation.Length = length;

            animation.ChangePace(trackBarPace.Value);

            animation.BalanceFinished += new EventHandler(BalanceFinished);
            animation.IntervalChanged += new EventHandler(IntervalChanged);
            animation.queryAccepted += new EventHandler(QueryAccepted);

            animation.animationStop += new EventHandler(AnimationStop);
            creation.animationStop += new EventHandler(AnimationStop);

            Brain.simulationFinished += new EventHandler(SimulationFinished);

            resize();
        }

        void Simulate()
        {
            container.Simulate(length);
            buttonSimulate.Enabled = false;

            charting.Reload();
            response.Reload();
        }

        #endregion

        #region obsługa przycisków

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (presentation.Started)
            {
                presentation.Stop();
                return;
            }

            buttonSimulate.Enabled = false;
            buttonBack.Enabled = false;
            buttonForth.Enabled = false;

            buttonPlay.Text = "Stop";

            presentation.ChangePace(trackBarPace.Value);
            presentation.Start();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            Simulate();
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
            presentation.Back();
        }

        private void buttonForth_Click(object sender, EventArgs e)
        {
            presentation.Forth();
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
                    Balancing.Instance.Stop();
                    break;

                case Keys.F1:
                    display.Print(presentation);
                    break;

                case Keys.F4:

                    break;

                case Keys.F7:
                    player.Stop();
                    break;

                case Keys.F8:
                    if (checkBoxMusic.Checked)
                        response.Play(player);
                    break;

                case Keys.F11:
                    Balancing.Instance.phaseFour();
                    break;

                case Keys.F12:
                    presentation.BalanceSynapses();
                    break;

                case Keys.Insert:
                    presentation.ChangeInsertion();
                    break;

                case Keys.Left:
                    presentation.Back();
                    break;

                case Keys.Right:
                    presentation.Forth();
                    break;

                case Keys.Up:
                    presentation.Higher();
                    break;

                case Keys.Down:
                    presentation.Lower();
                    break;

                case Keys.Space:
                    presentation.Space();
                    break;

                case Keys.Back:
                    presentation.Erase();
                    break;

                case Keys.Enter:
                    presentation.Enter();
                    break;

                case Keys.NumPad2:
                    presentation.Down();
                    break;

                case Keys.NumPad4:
                    presentation.Left();
                    break;

                case Keys.NumPad6:
                    presentation.Right();
                    break;

                case Keys.NumPad8:
                    presentation.Up();
                    break;

                case Keys.NumPad1:
                    presentation.Tighten();
                    break;

                case Keys.NumPad3:
                    presentation.Broaden();
                    break;

                case Keys.NumPad7:
                    presentation.Closer();
                    break;

                case Keys.NumPad9:
                    presentation.Farther();
                    break;

                case Keys.NumPad5:
                    presentation.Center();
                    break;

                case Keys.PageUp:
                    container.Next();
                    break;

                case Keys.PageDown:
                    container.Prev();
                    break;
                
                default:

                    int code = msg.WParam.ToInt32();

                    if (code > 64 && code < 91)
                    {
                        if ((keyData & Keys.Shift) != Keys.Shift)
                            code += 32;

                        presentation.Add((char)code);
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
            display.ResizeWindow();
        }

        #endregion

        #region obsługa zmiany trybu pracy

        private void radioButtonCreation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonCreation.Checked)
                return;

            Show(creation);
        }

        private void radioButtonChart_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonChart.Checked)
                return;

             Show(charting);
        }

        private void radioButtonSimulation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonSimulation.Checked)
                return;

            Show(animation);
        }

        private void radioButtonTree_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonTree.Checked)
                return;

            Show(response);
        }

        void Show(Presentation presentation)
        {
            this.presentation.Hide();
            this.presentation = presentation;
            presentation.Show();
            display.Focus();

            if (presentation is Creation)
                container.Visible = false;
            else
                container.Visible = true;
        }

        #endregion

        #region obsługa zdarzeń

        private void Simulation_Load(object sender, EventArgs e)
        {
            /*
            brain.addSentence("type new sequence"));
            sbrain.addSentence("or load from file"));*/

            Brain brain = new Brain();
            container = new QueryContainer(brain, display);

            /*
            brain.addSentence("i have a monkey");
            brain.addSentence("my monkey is very small");
            brain.addSentence("it is very lovely");
            brain.addSentence("it likes to sit on my head");
            brain.addSentence("it can jump very quickly");
            brain.addSentence("it is also very clever");
            brain.addSentence("it learns quickly");
            brain.addSentence("my monkey is lovely");
            brain.addSentence("i have also a small dog");
            */
           

            brain.AddSentence("G4N4 E4N4 E4N4 F4N4 D4N4 D4N4 C4N8 E4N8 G4N2");
            brain.AddSentence("E5N8 Eb5N8 E5N8 Eb5N8 E5N8 H4N8 D5N8 C5N8 A4D4 C4N8 E4N8 A4N8 H4D4 E4N8 G#4N8 H4N8 C5D4");
            brain.AddSentence("D4N2 G4N4 H4N2 A4N4 G4N2 F4N4 G4N4 F4N4 E4N4 D4N2 D4N4 H3N2 C4N4 D4D2");
            brain.AddSentence("C4N4 D4N4 E4N4 F4N4 G4N4 A4N4 H4N4 C5N4");

            //sequences.Add(brain.addSentence("this is"));

            container.Add(new QuerySequence("C4N4 D4N4 E4N4", 10 * length + 1));
            container.Add(new QuerySequence("C4N4 D4N4", 10 * length + 1));
            container.Add(new QuerySequence("G4N4 E4N4 E4N4", 10 * length + 1));

            Presentation.Brain = brain;
            Presentation.Container = container;
            Presentation.Controller.Add(trackBarFrame);

            Simulate();
            Show(presentation);
            //radioButtonTree.Checked = true;

            WindowState = FormWindowState.Maximized; 
            
            //openData("Files\\data.xml");
        }

        private void Simulation_Close(object sender, EventArgs e)
        {
            presentation.Hide();
            player.Close();
        }

        private void SimulationFinished(object sender, EventArgs e)
        {
            QuerySequence query = (QuerySequence)sender;
            query.LoadTiles();
        }

        private void AnimationStop(object sender, EventArgs e)
        {
            buttonPlay.Text = "Play";
            buttonSimulate.Enabled = true;
        }

        private void BalanceFinished(object sender, EventArgs e)
        {
            buttonPlay.Invoke(new ThreadStart(delegate()
            {
                buttonPlay.Enabled = true;
            }));
        }

        private void IntervalChanged(object sender, EventArgs e)
        {
            Simulate();
        }

        private void QueryAccepted(object sender, EventArgs e)
        {
            Simulate();
        }

        #endregion

        #region obsługa suwaków

        private void trackBarPace_Scroll(object sender, EventArgs e)
        {
            presentation.ChangePace(trackBarPace.Value);
            labelPace.Text = trackBarPace.Value.ToString();
        }

        private void trackBarLength_Scroll(object sender, EventArgs e)
        {
            length = trackBarLength.Value * 10;
            labelLength.Text = length.ToString();
        }

        private void trackBarFrame_Scroll(object sender, EventArgs e)
        {
            presentation.ChangeFrame(trackBarFrame.Value);
        }

        private void trackBarDensity_Scroll(object sender, EventArgs e)
        {
            
        }

        #endregion

        void OpenData(String path)
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
                brain.AddSentence(xn.InnerText.ToLower());

            Presentation.Brain = brain;

            Simulate();
            Show(presentation);

            if (presentation is Graph)
                animation.Balance();
        }

        private void openFile_FileOk(object sender, CancelEventArgs e)
        {
            OpenData(openFile.FileName);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openFile.DefaultExt = ".xml";
            openFile.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            DialogResult test = openFile.ShowDialog();
        }

        private void buttonBalance_Click(object sender, EventArgs e)
        {
            animation.Balance();
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