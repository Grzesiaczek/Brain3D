namespace Brain3D
{
    partial class Help
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
            this.labelShortcuts = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.labelShortcuts.AutoSize = true;
            this.labelShortcuts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelShortcuts.Location = new System.Drawing.Point(32, 32);
            this.labelShortcuts.Name = "label1";
            this.labelShortcuts.Size = new System.Drawing.Size(134, 16);
            this.labelShortcuts.TabIndex = 0;
            this.labelShortcuts.Text = "Skróty klawiszowe";
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.labelShortcuts);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MyForm_FormClosing);
            this.Name = "Help";
            this.Text = "Pomoc";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelShortcuts;
    }
}