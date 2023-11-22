using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameboyEmulator
{
    public partial class DebugTileViewer : Form
    {
        private int ZoomScale = 3;

        Brush[] brushes = { Brushes.Transparent, Brushes.LightGray, Brushes.DarkGray, Brushes.Black };

        public DebugTileViewer()
        {
            InitializeComponent();

            DoubleBuffered = true;

            int bitmapHeight = 24 * 8 * ZoomScale + 24 * ZoomScale;
            int bitmapWidth = 16 * 8 * ZoomScale + 16 * ZoomScale;

            pictureBox1.Size = new Size(bitmapWidth, bitmapHeight);
            pictureBox1.BackColor = Color.White;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 250;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void pictureBox1_Paint(object? sender, PaintEventArgs e)
        {
            int bitmapHeight = 24 * 8 * ZoomScale + 24 * ZoomScale;
            int bitmapWidth = 16 * 8 * ZoomScale + 16 * ZoomScale;

            pictureBox1.Size = new Size(bitmapWidth, bitmapHeight);
            UpdateWindow(e.Graphics);
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void DisplayTile(Graphics g, Word startLocation, Word tileNumber, int x, int y)
        {
            Rectangle rc = new Rectangle();

            for (int tileY = 0; tileY < 16; tileY += 2)
            {
                Byte b1 = Bus.Read(startLocation + (tileNumber * 16) + tileY);
                Byte b2 = Bus.Read(startLocation + (tileNumber * 16) + tileY + 1);

                for (int bit = 7; bit >= 0; bit--)
                {
                    Byte hi = (b1 & (1 << bit)) >> (bit - 1);
                    Byte lo = (b2 & (1 << bit)) >> (bit);

                    Byte color = hi | lo;

                    rc.X = x + ((7 - bit) * ZoomScale);
                    rc.Y = y + (tileY / 2 * ZoomScale);
                    rc.Width = ZoomScale;
                    rc.Height = ZoomScale;

                    g.FillRectangle(brushes[color], rc);

                }
            }
        }


        private void UpdateWindow(Graphics g)
        {
            int xDraw = 0;
            int yDraw = 0;
            int tileNum = 0;

            Rectangle rc = new Rectangle(0,0, panel1.Width, panel1.Height);
            g.FillRectangle(Brushes.White, rc);


            Word address = 0x8000;

            for (int y = 0; y < 24; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    DisplayTile(g, address, tileNum, xDraw + (x * ZoomScale), yDraw + (y * ZoomScale));
                    xDraw += (8 * ZoomScale);
                    tileNum++;
                }

                yDraw += (8 * ZoomScale);
                xDraw = 0;
            }
        }

        private void increaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomScale++;
        }

        private void decreaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ZoomScale >= 2)
            {
                ZoomScale--;
            }
        }

    }
}
