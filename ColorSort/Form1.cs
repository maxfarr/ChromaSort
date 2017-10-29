using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spectrum;
using static Spectrum.Color;

namespace ColorSort
{
    public partial class Form1 : Form
    {
        Bitmap image;
        Graphics gfx;

        ColorLine[] lines;

        public Form1()
        {
            InitializeComponent();

            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(image);
            lines = new ColorLine[pictureBox1.Width];

            for (int i = 0; i < pictureBox1.Width; i++)
            {
                lines[i] = new ColorLine(pictureBox1.Height);
            }

            for (int i = 0; i < pictureBox1.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Height; j++)
                {
                    RGB currentPixel = lines[i].Colors[j].ToRGB();
                    image.SetPixel(i, j, System.Drawing.Color.FromArgb(255, currentPixel.R, currentPixel.G, currentPixel.B));
                }
            }
        }

        private void sortTick_Tick(object sender, EventArgs e)
        {
            //gfx.Clear(BackColor);
            //image.SetPixel();

            

            pictureBox1.Image = image;
        }


    }
}
