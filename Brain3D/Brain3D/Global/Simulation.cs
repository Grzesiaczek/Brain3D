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

        Brain brain;

        Animation animation;
        Creation creation;
        Charting charting;

        Presentation presentation;
        StateBar stateBar;

        int frames;
        int length;
        int pace;

        Mode mode;
        FormWindowState state;
        Drawable visible;

        #endregion

        public Simulation()
        {
            InitializeComponent();
            Constant.load();
            initialize();
            prepareAnimation();

            //create();
            animate();
        }

        #region funkcje inicjalizacji dla poszczególnych trybów

        void animate()
        {
            presentation = animation;
            mode = Mode.Query;
        }

        void create()
        {
            presentation = creation;
            mode = Mode.Creation;
        }

        void chart()
        {
            presentation = creation;
            visible = charting;
            charting.show();
            mode = Mode.Chart;
        }

        #endregion

        #region inicjalizacja

        void initialize()
        {
            brain = new Brain();

            charting = new Charting();
            animation = new Animation(display);
            creation = new Creation(display);
            stateBar = new StateBar();

            Controls.Add(display);
            Controls.Add(charting);

            animation.setBar(stateBar);

            BrainElement.initialize(250);
            Neuron.initialize();

            Padding margin = new Padding(0, 0, rightPanel.Width + 20, 100);
            charting.setMargin(margin);

            display.setMargin(margin.Right);
            display.resize();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);

            trackBarDensity.KeyDown += keySuppress;
            trackBarFrame.KeyDown += keySuppress;
            trackBarLength.KeyDown += keySuppress;
            trackBarPace.KeyDown += keySuppress;
            trackBarScale.KeyDown += keySuppress;

            rightPanel.Controls.Add(stateBar);
            stateBar.show();

            length = trackBarLength.Value * 10;
            pace = trackBarPace.Value * 100;

            animation.changePace(pace);
            animation.animationStop += new EventHandler(animationStop);
            animation.balanceFinished += new EventHandler(balanceFinished);
            animation.queryAccepted += new EventHandler(queryAccepted);
            animation.frameChanged += new EventHandler(frameChanged);
            animation.framesChanged += new EventHandler(framesChanged);

            creation.animationStop += new EventHandler(animationStop);
            creation.creationFinished += new EventHandler(creationFinished);
            creation.frameChanged += new EventHandler(frameChanged);
            creation.framesChanged += new EventHandler(framesChanged);
            creation.brainCreated += new EventHandler(brainCreated);

            Presentation.factorChanged += new EventHandler(factorChanged);

            resize();
        }

        void simulate()
        {
            brain.simulate(length);
            animation.load(length);
        }

        void prepareAnimation()
        {
            buttonPlay.Enabled = true;
            buttonBack.Enabled = true;
            buttonForth.Enabled = true;
        }

        #endregion

        #region obsługa przycisków

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (presentation.started())
            {
                prepareAnimation();
                presentation.stop();
                return;
            }

            buttonSimulate.Enabled = true;

            buttonBack.Enabled = false;
            buttonForth.Enabled = false;
            buttonPlay.Text = "Stop";

            presentation.changePace(pace);
            presentation.start();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            simulate();
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            if (!radioButtonQuery.Checked)
                return;

            presentation.stop();
            animation.newQuery();
        }

        #endregion

        #region obsługa przycisków sterujących

        private void buttonPaceUp_Click(object sender, EventArgs e)
        {
            if (pace < 2000)
            {
                pace += 100;
                presentation.changePace(pace);
            }

            labelPace.Text = pace.ToString();
            trackBarPace.Value = pace / 100;
        }

        private void buttonPaceDown_Click(object sender, EventArgs e)
        {
            if (pace > 200)
            {
                pace -= 100;
                presentation.changePace(pace);
            }

            labelPace.Text = pace.ToString();
            trackBarPace.Value = pace / 100;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                brain.undo();

            presentation.back();
        }

        private void buttonForth_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                brain.tick();

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
                case Keys.F1:
                    visible.save();
                    break;
                case Keys.F4:

                    break;
                case Keys.F11:
                    animation.stopBalance();
                    break;
                case Keys.Left:
                    presentation.back();
                    break;
                case Keys.Right:
                    presentation.forth();
                    break;
                case Keys.Add:
                    //if (e.Modifiers == Keys.Control)

                    break;
                case Keys.Subtract:
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
                    display.down();
                    break;
                case Keys.NumPad4:
                    display.left();
                    break;
                case Keys.NumPad6:
                    display.right();
                    break;
                case Keys.NumPad8:
                    display.up();
                    break;
                case Keys.NumPad1:
                    display.tighten();
                    break;
                case Keys.NumPad3:
                    display.broaden();
                    break;
                case Keys.NumPad7:
                    display.closer();
                    break;
                case Keys.NumPad9:
                    display.farther();
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
            rightPanel.Height = py + 20;

            trackBarFrame.Width = px - 120;
            trackBarFrame.Top = py;

            buttonBack.Top = py;
            buttonForth.Top = py;
            buttonForth.Left = px - 50;
        }

        private void resizeEnd(object sender, EventArgs e)
        {
            charting.resize();
            display.resize();
        }
        #endregion

        #region obsługa zmiany trybu pracy

        private void radioButtonCreation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonCreation.Checked)
                return;

            mode = Mode.Creation;
            presentation = creation;

            display.clear();
            animation.create(creation);

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = false;
        }

        private void radioButtonChart_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonChart.Checked)
                return;

            mode = Mode.Chart;
            //change(charting);

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = false;
        }

        private void radioButtonManual_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonSimulation.Checked)
                return;

            presentation = animation;

            mode = Mode.Manual;
            animation.setMode(Mode.Manual);

            buttonPlay.Enabled = false;
            buttonQuery.Enabled = false;
            /*
            if (MessageBox.Show("Reset all data?", "Data Reset", MessageBoxButtons.YesNo) == DialogResult.No)
                return;*/

            animation.unload();
            brain.erase(true);
        }

        private void radioButtonQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonQuery.Checked)
                return;

            presentation = animation;

            mode = Mode.Query;
            animation.setMode(Mode.Query);

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = true;
        }

        private void change(Drawable area)
        {
            if (visible.Equals(area))
                return;

            presentation.stop();
            visible.hide();
            area.show();
            visible = area;
        }
        #endregion

        #region obsługa zdarzeń

        private void Simulation_Load(object sender, EventArgs e)
        {
            List<CreationSequence> sequences = new List<CreationSequence>();
            /*
            sequences.Add(brain.addSentence("type new sequence"));
            sequences.Add(brain.addSentence("or load from file"));*/

            sequences.Add(brain.addSentence("i have a monkey"));
            sequences.Add(brain.addSentence("my monkey is very small"));
            sequences.Add(brain.addSentence("it is very lovely"));
            sequences.Add(brain.addSentence("it likes to sit on my head"));
            sequences.Add(brain.addSentence("it can jump very quickly"));
            sequences.Add(brain.addSentence("it is also very clever"));
            sequences.Add(brain.addSentence("it learns quickly"));
            sequences.Add(brain.addSentence("my monkey is lovely"));
            sequences.Add(brain.addSentence("i have also a small dog"));

            creation.load(sequences);
            animation.loadBrain(brain);
            charting.loadBrain(brain);

            //animation.create(creation);
            animation.show();
            simulate();
            //openData("Files\\data.xml");
        }

        private void animationStop(object sender, EventArgs e)
        {
            buttonPlay.Text = "Play";
            buttonSimulate.Enabled = true;
        }

        private void balanceFinished(object sender, EventArgs e)
        {
            if (mode != Mode.Manual)
                buttonPlay.Enabled = true;
        }

        private void factorChanged(object sender, EventArgs e)
        {
            trackBarScale.Minimum = (int)sender;
            trackBarScale.Value = (int)sender;

            //AnimatedElement.Factor = (float)(int)sender / 100;
        }

        private void frameChanged(object sender, EventArgs e)
        {
            trackBarFrame.Value = (int)sender;
        }

        private void framesChanged(object sender, EventArgs e)
        {
            frames = (int)sender;
            trackBarFrame.Maximum = frames;
        }

        private void queryAccepted(object sender, EventArgs e)
        {
            simulate();
            charting.addQuery((QuerySequence)sender);
        }

        private void creationFinished(object sender, EventArgs e)
        {
            buttonForth.Enabled = false;
        }

        private void brainCreated(object sender, EventArgs e)
        {
            brain = (Brain)sender;
            animation.clear();
            animation.loadBrain(brain);
        }

        #endregion

        #region obsługa suwaków

        private void trackBarPace_Scroll(object sender, EventArgs e)
        {
            pace = trackBarPace.Value * 100;
            presentation.changePace(pace);
            labelPace.Text = pace.ToString();
        }

        private void trackBarLength_Scroll(object sender, EventArgs e)
        {
            length = trackBarLength.Value * 10;
            labelLength.Text = length.ToString();
        }

        private void trackBarFrame_Scroll(object sender, EventArgs e)
        {
            presentation.changeFrame(trackBarFrame.Value);
            //labelFrame.Text = trackBarFrame.Value.ToString() + "/" + frames.ToString();
        }

        private void trackBarScale_Scroll(object sender, EventArgs e)
        {
            float factor = (float)trackBarScale.Value / 100;
            //AnimatedElement.Factor = factor;
        }

        private void trackBarDensity_Scroll(object sender, EventArgs e)
        {
            
        }

        #endregion

        void openData(String path)
        {
            brain.clear();
            animation.clear();
            creation.clear();

            DateTime date = DateTime.Now;
            String name = date.ToString("yyyyMMdd-HHmmss");

            StreamReader reader = new StreamReader(File.Open(path, FileMode.Open));
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode node = xml.ChildNodes.Item(1).FirstChild;
            //Dictionary<Neuron, SequenceNeuron> map = new Dictionary<Neuron, SequenceNeuron>();
            List<CreationSequence> sequences = new List<CreationSequence>();

            foreach (XmlNode xn in node.ChildNodes)
                sequences.Add(brain.addSentence(xn.InnerText.ToLower()));

            animation.loadBrain(brain);
            creation.load(sequences);
            animation.create(creation);
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
            openFile.ShowDialog();
        }

        private void buttonBalance_Click(object sender, EventArgs e)
        {
            animation.balance();
        }

        private void checkBoxAutoBalance_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBoxScreenBalance_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxScreenBalance.Checked)
                GraphBalancing.Instance.ScreenBalance = true;
            else
                GraphBalancing.Instance.ScreenBalance = false;
        }
    }
}