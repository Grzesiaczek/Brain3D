﻿namespace Brain3D
{
    partial class Simulation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonPaceDown = new System.Windows.Forms.Button();
            this.buttonPaceUp = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.labelPace = new System.Windows.Forms.Label();
            this.changeFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.rightPanel = new System.Windows.Forms.GroupBox();
            this.buttonBalance = new System.Windows.Forms.Button();
            this.checkBoxScreenBalance = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoBalance = new System.Windows.Forms.CheckBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.trackBarDensity = new System.Windows.Forms.TrackBar();
            this.trackBarScale = new System.Windows.Forms.TrackBar();
            this.radioButtonCreation = new System.Windows.Forms.RadioButton();
            this.trackBarLength = new System.Windows.Forms.TrackBar();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.trackBarPace = new System.Windows.Forms.TrackBar();
            this.buttonLengthUp = new System.Windows.Forms.Button();
            this.radioButtonChart = new System.Windows.Forms.RadioButton();
            this.buttonLengthDown = new System.Windows.Forms.Button();
            this.radioButtonSimulation = new System.Windows.Forms.RadioButton();
            this.labelLength = new System.Windows.Forms.Label();
            this.radioButtonQuery = new System.Windows.Forms.RadioButton();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.display = new Brain3D.Display();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForth = new System.Windows.Forms.Button();
            this.trackBarFrame = new System.Windows.Forms.TrackBar();
            this.rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPace)).BeginInit();
            this.display.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(28, 408);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(100, 25);
            this.buttonPlay.TabIndex = 10;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonPaceDown
            // 
            this.buttonPaceDown.Location = new System.Drawing.Point(23, 323);
            this.buttonPaceDown.Name = "buttonPaceDown";
            this.buttonPaceDown.Size = new System.Drawing.Size(25, 25);
            this.buttonPaceDown.TabIndex = 2;
            this.buttonPaceDown.Text = "-";
            this.buttonPaceDown.UseVisualStyleBackColor = true;
            this.buttonPaceDown.Click += new System.EventHandler(this.buttonPaceDown_Click);
            // 
            // buttonPaceUp
            // 
            this.buttonPaceUp.Location = new System.Drawing.Point(110, 323);
            this.buttonPaceUp.Name = "buttonPaceUp";
            this.buttonPaceUp.Size = new System.Drawing.Size(25, 25);
            this.buttonPaceUp.TabIndex = 3;
            this.buttonPaceUp.Text = "+";
            this.buttonPaceUp.UseVisualStyleBackColor = true;
            this.buttonPaceUp.Click += new System.EventHandler(this.buttonPaceUp_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Location = new System.Drawing.Point(28, 549);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(100, 25);
            this.buttonSimulate.TabIndex = 5;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // labelPace
            // 
            this.labelPace.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPace.Location = new System.Drawing.Point(54, 323);
            this.labelPace.Name = "labelPace";
            this.labelPace.Size = new System.Drawing.Size(50, 25);
            this.labelPace.TabIndex = 6;
            this.labelPace.Text = "800";
            this.labelPace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.buttonBalance);
            this.rightPanel.Controls.Add(this.checkBoxScreenBalance);
            this.rightPanel.Controls.Add(this.checkBoxAutoBalance);
            this.rightPanel.Controls.Add(this.buttonLoad);
            this.rightPanel.Controls.Add(this.trackBarDensity);
            this.rightPanel.Controls.Add(this.trackBarScale);
            this.rightPanel.Controls.Add(this.radioButtonCreation);
            this.rightPanel.Controls.Add(this.trackBarLength);
            this.rightPanel.Controls.Add(this.buttonQuery);
            this.rightPanel.Controls.Add(this.trackBarPace);
            this.rightPanel.Controls.Add(this.buttonLengthUp);
            this.rightPanel.Controls.Add(this.radioButtonChart);
            this.rightPanel.Controls.Add(this.buttonLengthDown);
            this.rightPanel.Controls.Add(this.radioButtonSimulation);
            this.rightPanel.Controls.Add(this.labelLength);
            this.rightPanel.Controls.Add(this.radioButtonQuery);
            this.rightPanel.Controls.Add(this.buttonSimulate);
            this.rightPanel.Controls.Add(this.buttonPlay);
            this.rightPanel.Controls.Add(this.buttonPaceDown);
            this.rightPanel.Controls.Add(this.buttonPaceUp);
            this.rightPanel.Controls.Add(this.labelPace);
            this.rightPanel.Location = new System.Drawing.Point(840, 0);
            this.rightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(140, 760);
            this.rightPanel.TabIndex = 19;
            this.rightPanel.TabStop = false;
            // 
            // buttonBalance
            // 
            this.buttonBalance.Location = new System.Drawing.Point(23, 719);
            this.buttonBalance.Name = "buttonBalance";
            this.buttonBalance.Size = new System.Drawing.Size(100, 25);
            this.buttonBalance.TabIndex = 36;
            this.buttonBalance.Text = "Balance";
            this.buttonBalance.UseVisualStyleBackColor = true;
            this.buttonBalance.Click += new System.EventHandler(this.buttonBalance_Click);
            // 
            // checkBoxScreenBalance
            // 
            this.checkBoxScreenBalance.AutoSize = true;
            this.checkBoxScreenBalance.Checked = true;
            this.checkBoxScreenBalance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxScreenBalance.Location = new System.Drawing.Point(32, 680);
            this.checkBoxScreenBalance.Name = "checkBoxScreenBalance";
            this.checkBoxScreenBalance.Size = new System.Drawing.Size(99, 17);
            this.checkBoxScreenBalance.TabIndex = 35;
            this.checkBoxScreenBalance.Text = "ScreenBalance";
            this.checkBoxScreenBalance.UseVisualStyleBackColor = true;
            this.checkBoxScreenBalance.CheckedChanged += new System.EventHandler(this.checkBoxScreenBalance_CheckedChanged);
            // 
            // checkBoxAutoBalance
            // 
            this.checkBoxAutoBalance.AutoSize = true;
            this.checkBoxAutoBalance.Checked = true;
            this.checkBoxAutoBalance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoBalance.Location = new System.Drawing.Point(32, 648);
            this.checkBoxAutoBalance.Name = "checkBoxAutoBalance";
            this.checkBoxAutoBalance.Size = new System.Drawing.Size(87, 17);
            this.checkBoxAutoBalance.TabIndex = 34;
            this.checkBoxAutoBalance.Text = "AutoBalance";
            this.checkBoxAutoBalance.UseVisualStyleBackColor = true;
            this.checkBoxAutoBalance.CheckedChanged += new System.EventHandler(this.checkBoxAutoBalance_CheckedChanged);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(28, 600);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(100, 25);
            this.buttonLoad.TabIndex = 33;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // trackBarDensity
            // 
            this.trackBarDensity.LargeChange = 2;
            this.trackBarDensity.Location = new System.Drawing.Point(86, 173);
            this.trackBarDensity.Maximum = 9;
            this.trackBarDensity.Minimum = 1;
            this.trackBarDensity.Name = "trackBarDensity";
            this.trackBarDensity.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarDensity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBarDensity.Size = new System.Drawing.Size(45, 133);
            this.trackBarDensity.TabIndex = 32;
            this.trackBarDensity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarDensity.Value = 4;
            this.trackBarDensity.Scroll += new System.EventHandler(this.trackBarDensity_Scroll);
            // 
            // trackBarScale
            // 
            this.trackBarScale.Location = new System.Drawing.Point(30, 173);
            this.trackBarScale.Maximum = 100;
            this.trackBarScale.Minimum = 25;
            this.trackBarScale.Name = "trackBarScale";
            this.trackBarScale.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarScale.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBarScale.Size = new System.Drawing.Size(45, 133);
            this.trackBarScale.TabIndex = 30;
            this.trackBarScale.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarScale.Value = 25;
            this.trackBarScale.Scroll += new System.EventHandler(this.trackBarScale_Scroll);
            // 
            // radioButtonCreation
            // 
            this.radioButtonCreation.AutoSize = true;
            this.radioButtonCreation.Location = new System.Drawing.Point(42, 19);
            this.radioButtonCreation.Name = "radioButtonCreation";
            this.radioButtonCreation.Size = new System.Drawing.Size(64, 17);
            this.radioButtonCreation.TabIndex = 26;
            this.radioButtonCreation.Text = "Creation";
            this.radioButtonCreation.UseVisualStyleBackColor = true;
            this.radioButtonCreation.CheckedChanged += new System.EventHandler(this.radioButtonCreation_CheckedChanged);
            // 
            // trackBarLength
            // 
            this.trackBarLength.AutoSize = false;
            this.trackBarLength.Location = new System.Drawing.Point(16, 508);
            this.trackBarLength.Maximum = 50;
            this.trackBarLength.Minimum = 5;
            this.trackBarLength.Name = "trackBarLength";
            this.trackBarLength.Size = new System.Drawing.Size(125, 24);
            this.trackBarLength.SmallChange = 2;
            this.trackBarLength.TabIndex = 0;
            this.trackBarLength.TickFrequency = 10;
            this.trackBarLength.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLength.Value = 25;
            this.trackBarLength.Scroll += new System.EventHandler(this.trackBarLength_Scroll);
            // 
            // buttonQuery
            // 
            this.buttonQuery.Enabled = false;
            this.buttonQuery.Location = new System.Drawing.Point(25, 142);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(100, 25);
            this.buttonQuery.TabIndex = 25;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // trackBarPace
            // 
            this.trackBarPace.AutoSize = false;
            this.trackBarPace.Location = new System.Drawing.Point(16, 364);
            this.trackBarPace.Maximum = 20;
            this.trackBarPace.Minimum = 2;
            this.trackBarPace.Name = "trackBarPace";
            this.trackBarPace.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBarPace.Size = new System.Drawing.Size(125, 24);
            this.trackBarPace.SmallChange = 2;
            this.trackBarPace.TabIndex = 0;
            this.trackBarPace.TickFrequency = 100;
            this.trackBarPace.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarPace.Value = 8;
            this.trackBarPace.Scroll += new System.EventHandler(this.trackBarPace_Scroll);
            // 
            // buttonLengthUp
            // 
            this.buttonLengthUp.Location = new System.Drawing.Point(110, 468);
            this.buttonLengthUp.Name = "buttonLengthUp";
            this.buttonLengthUp.Size = new System.Drawing.Size(25, 25);
            this.buttonLengthUp.TabIndex = 24;
            this.buttonLengthUp.Text = "+";
            this.buttonLengthUp.UseVisualStyleBackColor = true;
            this.buttonLengthUp.Click += new System.EventHandler(this.buttonLengthUp_Click);
            // 
            // radioButtonChart
            // 
            this.radioButtonChart.AutoSize = true;
            this.radioButtonChart.Location = new System.Drawing.Point(42, 87);
            this.radioButtonChart.Name = "radioButtonChart";
            this.radioButtonChart.Size = new System.Drawing.Size(50, 17);
            this.radioButtonChart.TabIndex = 19;
            this.radioButtonChart.Text = "Chart";
            this.radioButtonChart.UseVisualStyleBackColor = true;
            this.radioButtonChart.CheckedChanged += new System.EventHandler(this.radioButtonChart_CheckedChanged);
            // 
            // buttonLengthDown
            // 
            this.buttonLengthDown.Location = new System.Drawing.Point(23, 468);
            this.buttonLengthDown.Name = "buttonLengthDown";
            this.buttonLengthDown.Size = new System.Drawing.Size(25, 25);
            this.buttonLengthDown.TabIndex = 23;
            this.buttonLengthDown.Text = "-";
            this.buttonLengthDown.UseVisualStyleBackColor = true;
            this.buttonLengthDown.Click += new System.EventHandler(this.buttonLengthDown_Click);
            // 
            // radioButtonSimulation
            // 
            this.radioButtonSimulation.AutoSize = true;
            this.radioButtonSimulation.Checked = true;
            this.radioButtonSimulation.Location = new System.Drawing.Point(41, 42);
            this.radioButtonSimulation.Name = "radioButtonSimulation";
            this.radioButtonSimulation.Size = new System.Drawing.Size(73, 17);
            this.radioButtonSimulation.TabIndex = 20;
            this.radioButtonSimulation.TabStop = true;
            this.radioButtonSimulation.Text = "Simulation";
            this.radioButtonSimulation.UseVisualStyleBackColor = true;
            this.radioButtonSimulation.CheckedChanged += new System.EventHandler(this.radioButtonManual_CheckedChanged);
            // 
            // labelLength
            // 
            this.labelLength.BackColor = System.Drawing.SystemColors.Control;
            this.labelLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLength.Location = new System.Drawing.Point(54, 468);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(50, 25);
            this.labelLength.TabIndex = 22;
            this.labelLength.Text = "250";
            this.labelLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonQuery
            // 
            this.radioButtonQuery.AutoSize = true;
            this.radioButtonQuery.Location = new System.Drawing.Point(42, 110);
            this.radioButtonQuery.Name = "radioButtonQuery";
            this.radioButtonQuery.Size = new System.Drawing.Size(53, 17);
            this.radioButtonQuery.TabIndex = 21;
            this.radioButtonQuery.Text = "Query";
            this.radioButtonQuery.UseVisualStyleBackColor = true;
            this.radioButtonQuery.CheckedChanged += new System.EventHandler(this.radioButtonQuery_CheckedChanged);
            // 
            // openFile
            // 
            this.openFile.InitialDirectory = "Files";
            this.openFile.FileOk += new System.ComponentModel.CancelEventHandler(this.openFile_FileOk);
            // 
            // display
            // 
            this.display.Controls.Add(this.buttonBack);
            this.display.Controls.Add(this.buttonForth);
            this.display.Controls.Add(this.trackBarFrame);
            this.display.Location = new System.Drawing.Point(0, 0);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(840, 760);
            this.display.TabIndex = 29;
            this.display.Text = "display";
            // 
            // buttonBack
            // 
            this.buttonBack.Enabled = false;
            this.buttonBack.Location = new System.Drawing.Point(20, 720);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(32, 24);
            this.buttonBack.TabIndex = 9;
            this.buttonBack.Text = "<<";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonForth
            // 
            this.buttonForth.Enabled = false;
            this.buttonForth.Location = new System.Drawing.Point(760, 720);
            this.buttonForth.Name = "buttonForth";
            this.buttonForth.Size = new System.Drawing.Size(32, 24);
            this.buttonForth.TabIndex = 10;
            this.buttonForth.Text = ">>";
            this.buttonForth.UseVisualStyleBackColor = true;
            this.buttonForth.Click += new System.EventHandler(this.buttonForth_Click);
            // 
            // trackBarFrame
            // 
            this.trackBarFrame.AutoSize = false;
            this.trackBarFrame.BackColor = System.Drawing.Color.CornflowerBlue;
            this.trackBarFrame.Location = new System.Drawing.Point(60, 720);
            this.trackBarFrame.Maximum = 250;
            this.trackBarFrame.Name = "trackBarFrame";
            this.trackBarFrame.Size = new System.Drawing.Size(690, 24);
            this.trackBarFrame.SmallChange = 2;
            this.trackBarFrame.TabIndex = 27;
            this.trackBarFrame.TickFrequency = 10;
            this.trackBarFrame.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarFrame.Scroll += new System.EventHandler(this.trackBarFrame_Scroll);
            // 
            // Simulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.display);
            this.Controls.Add(this.rightPanel);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 800);
            this.Name = "Simulation";
            this.Text = "AAS Simulation Tool";
            this.Shown += new System.EventHandler(this.Simulation_Load);
            this.ResizeEnd += new System.EventHandler(this.resizeEnd);
            this.Resize += new System.EventHandler(this.resize);
            this.rightPanel.ResumeLayout(false);
            this.rightPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPace)).EndInit();
            this.display.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonPaceDown;
        private System.Windows.Forms.Button buttonPaceUp;
        private System.Windows.Forms.Button buttonSimulate;
        private System.Windows.Forms.Label labelPace;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForth;
        private System.Windows.Forms.FolderBrowserDialog changeFolderDialog;
        private System.Windows.Forms.GroupBox rightPanel;
        private System.Windows.Forms.RadioButton radioButtonSimulation;
        private System.Windows.Forms.RadioButton radioButtonChart;
        private System.Windows.Forms.RadioButton radioButtonQuery;
        private System.Windows.Forms.Button buttonLengthUp;
        private System.Windows.Forms.Button buttonLengthDown;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.TrackBar trackBarPace;
        private System.Windows.Forms.TrackBar trackBarLength;
        private System.Windows.Forms.RadioButton radioButtonCreation;
        private System.Windows.Forms.TrackBar trackBarFrame;
        private System.Windows.Forms.TrackBar trackBarScale;
        private System.Windows.Forms.TrackBar trackBarDensity;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private Display display;
        private System.Windows.Forms.Button buttonBalance;
        private System.Windows.Forms.CheckBox checkBoxScreenBalance;
        private System.Windows.Forms.CheckBox checkBoxAutoBalance;
    }
}

