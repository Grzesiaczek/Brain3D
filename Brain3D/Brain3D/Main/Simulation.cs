using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Brain3D
{
    public partial class Simulation : Form
    {
        #region deklaracje

        Animation animation;
        Creation creation;
        Charting charting;
        Response response;

        Presentation presentation;
        BrainContainer brainContainer;
        Player player;
        Help help;

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
            help = new Help();

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

            trackBarDensity.KeyDown += KeySuppress;
            trackBarFrame.KeyDown += KeySuppress;
            trackBarLength.KeyDown += KeySuppress;
            trackBarPace.KeyDown += KeySuppress;

            length = trackBarLength.Value * 10;
            Presentation.Length = length;

            animation.ChangePace(trackBarPace.Value);

            Graph.AnimationStop += new EventHandler(AnimationStop);
            Graph.BalanceFinished += new EventHandler(BalanceFinished);

            Presentation.IntervalChanged += new EventHandler(IntervalChanged);
            Presentation.QueryAccepted += new EventHandler(QueryAccepted);

            Brain.SimulationFinished += new EventHandler(SimulationFinished);

            resize();
        }

        void Simulate()
        {
            brainContainer.Simulate(length);
            buttonSimulate.Enabled = false;

            charting.Load();
            response.Load();
        }

        #endregion

        #region obsługa przycisków

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (presentation.Started)
            {
                presentation.Stop();
            }
            else
            {
                buttonSimulate.Enabled = false;
                buttonBack.Enabled = false;
                buttonForth.Enabled = false;

                buttonPlay.Text = "Stop";

                presentation.ChangePace(trackBarPace.Value);
                presentation.Start();
            }
        }

        private void ButtonSimulate_Click(object sender, EventArgs e)
        {
            Simulate();
        }

        #endregion

        #region obsługa przycisków sterujących

        private void ButtonPaceUp_Click(object sender, EventArgs e)
        {
            if (trackBarPace.Value != trackBarPace.Maximum)
            {
                trackBarPace.Value++;
            }
        }

        private void ButtonPaceDown_Click(object sender, EventArgs e)
        {
            if (trackBarPace.Value != trackBarPace.Minimum)
            {
                trackBarPace.Value--;
            }
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            presentation.Back();
        }

        private void ButtonForth_Click(object sender, EventArgs e)
        {
            presentation.Forth();
        }

        private void ButtonLengthDown_Click(object sender, EventArgs e)
        {
            if (length > 50)
            {
                length -= 10;
            }

            labelLength.Text = length.ToString();
            trackBarLength.Value = length / 10;
        }

        private void ButtonLengthUp_Click(object sender, EventArgs e)
        {
            if (length < 500)
            {
                length += 10;
            }

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
                    help.Switch();
                    break;

                case Keys.F4:
                    display.Print(presentation);
                    break;

                case Keys.F7:
                    player.Stop();
                    break;

                case Keys.F8:
                    if (checkBoxMusic.Checked)
                    {
                        response.Play(player);
                    }

                    break;
                    
                case Keys.F9:
                    presentation.BalanceSynapses();
                    break;

                case Keys.F10:
                    Balancing.Instance.PhaseFour();
                    break;

                case Keys.F12:
                    presentation.CurrentQuery.Switch();
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
                    presentation.NextQuery();
                    break;

                case Keys.PageDown:
                    presentation.PreviousQuery();
                    break;

                case (Keys.Control | Keys.PageUp):
                    presentation.NextBrain();
                    break;

                case (Keys.Control | Keys.PageDown):
                    presentation.PreviousBrain();
                    break;                
                
                default:

                    int code = msg.WParam.ToInt32();

                    if (code > 64 && code < 91)
                    {
                        if ((keyData & Keys.Shift) != Keys.Shift)
                        {
                            code += 32;
                        }

                        presentation.Add((char)code);
                    }

                    break;
            }

            if (keyData != Keys.F1)
            {
                display.Focus();
            }

            return false;
        }

        void KeySuppress(object sender, KeyEventArgs e)
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

        private void RadioButtonCreation_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCreation.Checked)
            {
                Show(creation);
            }
        }

        private void RadioButtonChart_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonChart.Checked)
            {
                Show(charting);
            }
        }

        private void RadioButtonSimulation_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSimulation.Checked)
            {
                Show(animation);
            }
        }

        private void RadioButtonTree_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTree.Checked)
            {
                Show(response);
            }
        }

        void Show(Presentation presentation)
        {
            this.presentation.Hide();
            this.presentation = presentation;

            presentation.Show();
            presentation.ShowQuery();
            display.Focus();
        }

        #endregion

        #region obsługa zdarzeń

        private void Simulation_Load(object sender, EventArgs e)
        {
            /*
            brain.addSentence("type new sequence"));
            brain.addSentence("or load from file"));*/

            brainContainer = new BrainContainer();
            Presentation.BrainContainer = brainContainer;
            Presentation.Controller.Add(trackBarFrame);

            brainContainer.Load();
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

        private void TrackBarPace_Scroll(object sender, EventArgs e)
        {
            presentation.ChangePace(trackBarPace.Value);
            labelPace.Text = trackBarPace.Value.ToString();
        }

        private void TrackBarLength_Scroll(object sender, EventArgs e)
        {
            length = trackBarLength.Value * 10;
            labelLength.Text = length.ToString();
        }

        private void TrackBarFrame_Scroll(object sender, EventArgs e)
        {
            presentation.ChangeFrame(trackBarFrame.Value);
        }

        private void TrackBarDensity_Scroll(object sender, EventArgs e)
        {
            
        }

        #endregion

        void OpenData(string path)
        {
            Brain brain = new Brain();
            DateTime date = DateTime.Now;
            string name = date.ToString("yyyyMMdd-HHmmss");

            StreamReader reader = new StreamReader(File.Open(path, FileMode.Open));
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode node = xml.ChildNodes.Item(1).FirstChild;

            foreach (XmlNode xn in node.ChildNodes)
            {
                brain.AddSentence(xn.InnerText.ToLower());
            }

            Simulate();
            Show(presentation);

            if (presentation is Graph)
            {
                animation.Balance();
            }
        }

        private void OpenFile_FileOk(object sender, CancelEventArgs e)
        {
            OpenData(openFile.FileName);
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            openFile.DefaultExt = ".xml";
            openFile.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            DialogResult test = openFile.ShowDialog();
        }

        private void ButtonBalance_Click(object sender, EventArgs e)
        {
            animation.Balance();
            buttonPlay.Enabled = false;
        }

        private void CheckBoxState_CheckedChanged(object sender, EventArgs e)
        {
            Number3D.Visible = checkBoxState.Checked;
        }

        private void radioButtonBox_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonBox.Checked)
            {
                Constant.Space = SpaceMode.Box;
            }
        }

        private void RadioButtonSphere_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSphere.Checked)
            {
                Constant.Space = SpaceMode.Sphere;
            }
        }

        private void RadioButtonSquare_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxWhite_CheckedChanged(object sender, EventArgs e)
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