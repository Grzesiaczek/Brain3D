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
            this.radioButtonTree = new System.Windows.Forms.RadioButton();
            this.checkBoxAuto = new System.Windows.Forms.CheckBox();
            this.panelSpace = new System.Windows.Forms.Panel();
            this.radioButtonSquare = new System.Windows.Forms.RadioButton();
            this.radioButtonBox = new System.Windows.Forms.RadioButton();
            this.radioButtonSphere = new System.Windows.Forms.RadioButton();
            this.checkBoxState = new System.Windows.Forms.CheckBox();
            this.buttonBalance = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.trackBarDensity = new System.Windows.Forms.TrackBar();
            this.radioButtonCreation = new System.Windows.Forms.RadioButton();
            this.trackBarLength = new System.Windows.Forms.TrackBar();
            this.trackBarPace = new System.Windows.Forms.TrackBar();
            this.buttonLengthUp = new System.Windows.Forms.Button();
            this.radioButtonChart = new System.Windows.Forms.RadioButton();
            this.buttonLengthDown = new System.Windows.Forms.Button();
            this.radioButtonSimulation = new System.Windows.Forms.RadioButton();
            this.labelLength = new System.Windows.Forms.Label();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.display = new Brain3D.Display();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForth = new System.Windows.Forms.Button();
            this.trackBarFrame = new System.Windows.Forms.TrackBar();
            this.rightPanel.SuspendLayout();
            this.panelSpace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPace)).BeginInit();
            this.display.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(23, 287);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(100, 25);
            this.buttonPlay.TabIndex = 10;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonPaceDown
            // 
            this.buttonPaceDown.Location = new System.Drawing.Point(23, 197);
            this.buttonPaceDown.Name = "buttonPaceDown";
            this.buttonPaceDown.Size = new System.Drawing.Size(25, 25);
            this.buttonPaceDown.TabIndex = 2;
            this.buttonPaceDown.Text = "-";
            this.buttonPaceDown.UseVisualStyleBackColor = true;
            this.buttonPaceDown.Click += new System.EventHandler(this.buttonPaceDown_Click);
            // 
            // buttonPaceUp
            // 
            this.buttonPaceUp.Location = new System.Drawing.Point(107, 198);
            this.buttonPaceUp.Name = "buttonPaceUp";
            this.buttonPaceUp.Size = new System.Drawing.Size(25, 25);
            this.buttonPaceUp.TabIndex = 3;
            this.buttonPaceUp.Text = "+";
            this.buttonPaceUp.UseVisualStyleBackColor = true;
            this.buttonPaceUp.Click += new System.EventHandler(this.buttonPaceUp_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Location = new System.Drawing.Point(25, 446);
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
            this.labelPace.Location = new System.Drawing.Point(53, 198);
            this.labelPace.Name = "labelPace";
            this.labelPace.Size = new System.Drawing.Size(50, 25);
            this.labelPace.TabIndex = 6;
            this.labelPace.Text = "800";
            this.labelPace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.radioButtonTree);
            this.rightPanel.Controls.Add(this.checkBoxAuto);
            this.rightPanel.Controls.Add(this.panelSpace);
            this.rightPanel.Controls.Add(this.checkBoxState);
            this.rightPanel.Controls.Add(this.buttonBalance);
            this.rightPanel.Controls.Add(this.buttonLoad);
            this.rightPanel.Controls.Add(this.trackBarDensity);
            this.rightPanel.Controls.Add(this.radioButtonCreation);
            this.rightPanel.Controls.Add(this.trackBarLength);
            this.rightPanel.Controls.Add(this.trackBarPace);
            this.rightPanel.Controls.Add(this.buttonLengthUp);
            this.rightPanel.Controls.Add(this.radioButtonChart);
            this.rightPanel.Controls.Add(this.buttonLengthDown);
            this.rightPanel.Controls.Add(this.radioButtonSimulation);
            this.rightPanel.Controls.Add(this.labelLength);
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
            // radioButtonTree
            // 
            this.radioButtonTree.AutoSize = true;
            this.radioButtonTree.Location = new System.Drawing.Point(40, 110);
            this.radioButtonTree.Name = "radioButtonTree";
            this.radioButtonTree.Size = new System.Drawing.Size(47, 17);
            this.radioButtonTree.TabIndex = 41;
            this.radioButtonTree.Text = "Tree";
            this.radioButtonTree.UseVisualStyleBackColor = true;
            this.radioButtonTree.CheckedChanged += new System.EventHandler(this.radioButtonTree_CheckedChanged);
            // 
            // checkBoxAuto
            // 
            this.checkBoxAuto.AutoSize = true;
            this.checkBoxAuto.Checked = true;
            this.checkBoxAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAuto.Location = new System.Drawing.Point(27, 732);
            this.checkBoxAuto.Name = "checkBoxAuto";
            this.checkBoxAuto.Size = new System.Drawing.Size(87, 17);
            this.checkBoxAuto.TabIndex = 40;
            this.checkBoxAuto.Text = "AutoBalance";
            this.checkBoxAuto.UseVisualStyleBackColor = true;
            // 
            // panelSpace
            // 
            this.panelSpace.Controls.Add(this.radioButtonSquare);
            this.panelSpace.Controls.Add(this.radioButtonBox);
            this.panelSpace.Controls.Add(this.radioButtonSphere);
            this.panelSpace.Location = new System.Drawing.Point(37, 581);
            this.panelSpace.Name = "panelSpace";
            this.panelSpace.Size = new System.Drawing.Size(77, 87);
            this.panelSpace.TabIndex = 39;
            // 
            // radioButtonSquare
            // 
            this.radioButtonSquare.AutoSize = true;
            this.radioButtonSquare.Location = new System.Drawing.Point(12, 60);
            this.radioButtonSquare.Name = "radioButtonSquare";
            this.radioButtonSquare.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSquare.TabIndex = 39;
            this.radioButtonSquare.Text = "Square";
            this.radioButtonSquare.UseVisualStyleBackColor = true;
            this.radioButtonSquare.CheckedChanged += new System.EventHandler(this.radioButtonSquare_CheckedChanged);
            // 
            // radioButtonBox
            // 
            this.radioButtonBox.AutoSize = true;
            this.radioButtonBox.Checked = true;
            this.radioButtonBox.Location = new System.Drawing.Point(12, 12);
            this.radioButtonBox.Name = "radioButtonBox";
            this.radioButtonBox.Size = new System.Drawing.Size(43, 17);
            this.radioButtonBox.TabIndex = 37;
            this.radioButtonBox.TabStop = true;
            this.radioButtonBox.Text = "Box";
            this.radioButtonBox.UseVisualStyleBackColor = true;
            this.radioButtonBox.CheckedChanged += new System.EventHandler(this.radioButtonBox_CheckedChanged);
            // 
            // radioButtonSphere
            // 
            this.radioButtonSphere.AutoSize = true;
            this.radioButtonSphere.Location = new System.Drawing.Point(12, 36);
            this.radioButtonSphere.Name = "radioButtonSphere";
            this.radioButtonSphere.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSphere.TabIndex = 38;
            this.radioButtonSphere.Text = "Sphere";
            this.radioButtonSphere.UseVisualStyleBackColor = true;
            this.radioButtonSphere.CheckedChanged += new System.EventHandler(this.radioButtonSphere_CheckedChanged);
            // 
            // checkBoxState
            // 
            this.checkBoxState.AutoSize = true;
            this.checkBoxState.Checked = true;
            this.checkBoxState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxState.Location = new System.Drawing.Point(49, 542);
            this.checkBoxState.Name = "checkBoxState";
            this.checkBoxState.Size = new System.Drawing.Size(51, 17);
            this.checkBoxState.TabIndex = 35;
            this.checkBoxState.Text = "State";
            this.checkBoxState.UseVisualStyleBackColor = true;
            this.checkBoxState.CheckedChanged += new System.EventHandler(this.checkBoxState_CheckedChanged);
            // 
            // buttonBalance
            // 
            this.buttonBalance.Location = new System.Drawing.Point(25, 691);
            this.buttonBalance.Name = "buttonBalance";
            this.buttonBalance.Size = new System.Drawing.Size(100, 25);
            this.buttonBalance.TabIndex = 36;
            this.buttonBalance.Text = "Balance";
            this.buttonBalance.UseVisualStyleBackColor = true;
            this.buttonBalance.Click += new System.EventHandler(this.buttonBalance_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(25, 487);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(100, 25);
            this.buttonLoad.TabIndex = 33;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // trackBarDensity
            // 
            this.trackBarDensity.AutoSize = false;
            this.trackBarDensity.LargeChange = 2;
            this.trackBarDensity.Location = new System.Drawing.Point(10, 150);
            this.trackBarDensity.Maximum = 9;
            this.trackBarDensity.Minimum = 1;
            this.trackBarDensity.Name = "trackBarDensity";
            this.trackBarDensity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBarDensity.Size = new System.Drawing.Size(119, 33);
            this.trackBarDensity.TabIndex = 32;
            this.trackBarDensity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarDensity.Value = 4;
            this.trackBarDensity.Scroll += new System.EventHandler(this.trackBarDensity_Scroll);
            // 
            // radioButtonCreation
            // 
            this.radioButtonCreation.AutoSize = true;
            this.radioButtonCreation.Location = new System.Drawing.Point(40, 20);
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
            this.trackBarLength.Location = new System.Drawing.Point(13, 393);
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
            // trackBarPace
            // 
            this.trackBarPace.AutoSize = false;
            this.trackBarPace.Location = new System.Drawing.Point(13, 238);
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
            this.buttonLengthUp.Location = new System.Drawing.Point(104, 353);
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
            this.radioButtonChart.Location = new System.Drawing.Point(40, 80);
            this.radioButtonChart.Name = "radioButtonChart";
            this.radioButtonChart.Size = new System.Drawing.Size(50, 17);
            this.radioButtonChart.TabIndex = 19;
            this.radioButtonChart.Text = "Chart";
            this.radioButtonChart.UseVisualStyleBackColor = true;
            this.radioButtonChart.CheckedChanged += new System.EventHandler(this.radioButtonChart_CheckedChanged);
            // 
            // buttonLengthDown
            // 
            this.buttonLengthDown.Location = new System.Drawing.Point(20, 353);
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
            this.radioButtonSimulation.Location = new System.Drawing.Point(40, 50);
            this.radioButtonSimulation.Name = "radioButtonSimulation";
            this.radioButtonSimulation.Size = new System.Drawing.Size(73, 17);
            this.radioButtonSimulation.TabIndex = 20;
            this.radioButtonSimulation.TabStop = true;
            this.radioButtonSimulation.Text = "Simulation";
            this.radioButtonSimulation.UseVisualStyleBackColor = true;
            this.radioButtonSimulation.CheckedChanged += new System.EventHandler(this.radioButtonSimulation_CheckedChanged);
            // 
            // labelLength
            // 
            this.labelLength.BackColor = System.Drawing.SystemColors.Control;
            this.labelLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLength.Location = new System.Drawing.Point(50, 353);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(50, 25);
            this.labelLength.TabIndex = 22;
            this.labelLength.Text = "250";
            this.labelLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panelSpace.ResumeLayout(false);
            this.panelSpace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDensity)).EndInit();
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
        private System.Windows.Forms.Button buttonLengthUp;
        private System.Windows.Forms.Button buttonLengthDown;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.TrackBar trackBarPace;
        private System.Windows.Forms.TrackBar trackBarLength;
        private System.Windows.Forms.RadioButton radioButtonCreation;
        private System.Windows.Forms.TrackBar trackBarFrame;
        private System.Windows.Forms.TrackBar trackBarDensity;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private Display display;
        private System.Windows.Forms.Button buttonBalance;
        private System.Windows.Forms.CheckBox checkBoxState;
        private System.Windows.Forms.RadioButton radioButtonSphere;
        private System.Windows.Forms.RadioButton radioButtonBox;
        private System.Windows.Forms.Panel panelSpace;
        private System.Windows.Forms.CheckBox checkBoxAuto;
        private System.Windows.Forms.RadioButton radioButtonSquare;
        private System.Windows.Forms.RadioButton radioButtonTree;
    }
}

