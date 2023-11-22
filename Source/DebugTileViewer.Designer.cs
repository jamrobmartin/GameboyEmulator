namespace GameboyEmulator
{
    partial class DebugTileViewer
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
            menuStrip1 = new MenuStrip();
            scaleToolStripMenuItem = new ToolStripMenuItem();
            increaseToolStripMenuItem = new ToolStripMenuItem();
            decreaseToolStripMenuItem = new ToolStripMenuItem();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { scaleToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(578, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // scaleToolStripMenuItem
            // 
            scaleToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { increaseToolStripMenuItem, decreaseToolStripMenuItem });
            scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            scaleToolStripMenuItem.Size = new Size(46, 20);
            scaleToolStripMenuItem.Text = "Scale";
            // 
            // increaseToolStripMenuItem
            // 
            increaseToolStripMenuItem.Name = "increaseToolStripMenuItem";
            increaseToolStripMenuItem.Size = new Size(121, 22);
            increaseToolStripMenuItem.Text = "Increase";
            increaseToolStripMenuItem.Click += increaseToolStripMenuItem_Click;
            // 
            // decreaseToolStripMenuItem
            // 
            decreaseToolStripMenuItem.Name = "decreaseToolStripMenuItem";
            decreaseToolStripMenuItem.Size = new Size(121, 22);
            decreaseToolStripMenuItem.Text = "Decrease";
            decreaseToolStripMenuItem.Click += decreaseToolStripMenuItem_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 27);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 50);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            // 
            // panel1
            // 
            panel1.Location = new Point(12, 771);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 100);
            panel1.TabIndex = 3;
            // 
            // DebugTileViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 987);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MaximumSize = new Size(594, 1026);
            MinimumSize = new Size(594, 1026);
            Name = "DebugTileViewer";
            Text = "DebugTileViewer";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem scaleToolStripMenuItem;
        private ToolStripMenuItem increaseToolStripMenuItem;
        private ToolStripMenuItem decreaseToolStripMenuItem;
        private PictureBox pictureBox1;
        private Panel panel1;
    }
}