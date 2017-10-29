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

        Random rand;

        ColorLine[] lines;

        public Form1()
        {
            InitializeComponent();

            rand = new Random();
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(image);
            lines = new ColorLine[pictureBox1.Width];

            for (int i = 0; i < pictureBox1.Width; i++)
            {
                lines[i] = new ColorLine(pictureBox1.Height, rand);
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

            for (int i = 0; i < pictureBox1.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Height; j++)
                {
                    RGB currentPixel = lines[i].Colors[j].ToRGB();
                    image.SetPixel(i, j, System.Drawing.Color.FromArgb(255, currentPixel.R, currentPixel.G, currentPixel.B));
                }
            }

            pictureBox1.Image = image;
        }

        void updateImage()
        {
            
        }

        void bubbleSort()
        {
            bool sorted = false;

            while (!sorted)
            {
                int sortedCount = 0;

                for (int i = 0; i < pictureBox1.Width; i++)
                {
                    if (!lines[i].Sorted)
                    {
                        lines[i].Sorted = true;

                        for (int j = 1; j < pictureBox1.Height; j++)
                        {
                            if (lines[i].Colors[j].H < lines[i].Colors[j - 1].H)
                            {
                                lines[i].Sorted = false;
                                HSV temp = lines[i].Colors[j];
                                lines[i].Colors[j] = lines[i].Colors[j - 1];
                                lines[i].Colors[j - 1] = temp;
                            }
                        }

                        updateImage();
                    }
                    else
                    {
                        sortedCount++;
                    }
                }

                if (sortedCount == pictureBox1.Width)
                {
                    sorted = true;
                }
            }
        }

        private void sortsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void bubbleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                bubbleSort();
            });
            //bubbleSort();
        }
    }
}
