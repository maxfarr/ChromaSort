using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spectrum;
using static Spectrum.Color;
using System.Threading;

namespace ColorSort
{

    struct Vector2
    {
        public int X;
        public int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public partial class Form1 : Form
    {
        Bitmap canvas;
        Bitmap colorImage;
        Graphics gfx;

        Random rand;

        Size initialSize;

        ColorLine[] lines;

        Action[] sorts;

        public Form1()
        {
            InitializeComponent();

            sorts = new Action[]
            {
                bubbleSort
            };

            rand = new Random();
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);
            colorImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            initialSize = pictureBox1.Size;
            lines = new ColorLine[initialSize.Width];
            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

            for (int i = 0; i < initialSize.Width; i++)
            {
                lines[i] = new ColorLine(initialSize.Height, rand);
                for (int j = 0; j < initialSize.Height; j++)
                {
                    RGB currentPixel = lines[i].Colors[j].ToRGB();
                    colorImage.SetPixel(i, j, System.Drawing.Color.FromArgb(255, currentPixel.R, currentPixel.G, currentPixel.B));
                }
            }

            gfx.DrawImage(colorImage, new Rectangle(Point.Empty, pictureBox1.Size));
            pictureBox1.Image = canvas;
        }

        private void sortTick_Tick(object sender, EventArgs e)
        {
            //gfx.Clear(BackColor);
            //image.SetPixel();
            colorImage = generateImage();
            gfx.DrawImage(colorImage, new Rectangle(Point.Empty, pictureBox1.Size));
            pictureBox1.Image = canvas;
        }

        void initialize()
        {
            for (int i = 0; i < initialSize.Width; i++)
            {
                lines[i] = new ColorLine(initialSize.Height, rand);
                for (int j = 0; j < initialSize.Height; j++)
                {
                    RGB currentPixel = lines[i].Colors[j].ToRGB();
                    canvas.SetPixel(i, j, System.Drawing.Color.FromArgb(255, currentPixel.R, currentPixel.G, currentPixel.B));
                }
            }

            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
            gfx.DrawImage(canvas, new Rectangle(Point.Empty, pictureBox1.Size));
            pictureBox1.Image = canvas;
        }

        void updateSwap(int[] swap)
        {
            RGB pixelA = lines[swap[0]].Colors[swap[1]].ToRGB();
            RGB pixelB = lines[swap[0]].Colors[swap[2]].ToRGB();

            colorImage.SetPixel(swap[0], swap[1], System.Drawing.Color.FromArgb(255, pixelB.R, pixelB.G, pixelB.B));
            colorImage.SetPixel(swap[0], swap[2], System.Drawing.Color.FromArgb(255, pixelA.R, pixelA.G, pixelA.B));

            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
            gfx.DrawImage(colorImage, new Rectangle(Point.Empty, pictureBox1.Size));
            pictureBox1.Image = canvas;
        }

        Bitmap generateImage()
        {
            Bitmap bitmap = new Bitmap(initialSize.Width, initialSize.Height);

            for (int i = 0; i < initialSize.Width; i++)
            {
                for (int j = 0; j < initialSize.Height; j++)
                {
                    RGB currentPixel = lines[i].Colors[j].ToRGB();
                    bitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(255, currentPixel.R, currentPixel.G, currentPixel.B));
                }
            }

            return bitmap;
        }

        void bubbleSort()
        {
            bool sorted = false;

            while (!sorted)
            {
                int sortedCount = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].Sorted)
                    {
                        lines[i].Sorted = true;

                        for (int j = 1; j < lines[i].Colors.Length; j++)
                        {
                            if (lines[i].Colors[j].H < lines[i].Colors[j - 1].H)
                            {
                                lines[i].Sorted = false;

                                HSV temp = lines[i].Colors[j];
                                lines[i].Colors[j] = lines[i].Colors[j - 1];
                                lines[i].Colors[j - 1] = temp;

                                int[] swap = new int[3] { i, j, j - 1 };
                            }
                        }
                    }
                    else
                    {
                        sortedCount++;
                    }
                }

                if (sortedCount == initialSize.Width)
                {
                    sorted = true;
                }
            }
        }

        private void sortsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bubbleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bubbleSort();
        }

        private void bubbleWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                string error = "Error: " + e.Error.Message;
            }
            generateImage();
        }

        private void bubbleWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            updateSwap((int[])e.UserState);
            //updateImage();
        }

        private void bubbleWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            bubbleSort();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);
            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

            colorImage = generateImage();
            gfx.DrawImage(colorImage, new Rectangle(Point.Empty, pictureBox1.Size));
            pictureBox1.Image = canvas;
        }

        private void randomHueUnlimitedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initialize();
        }
    }
}