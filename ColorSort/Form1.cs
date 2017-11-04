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

    public enum InitType
    {
        UNLIMITED,
        RAINBOW
    }

    public partial class Form1 : Form
    {
        Bitmap canvas;
        Bitmap colorImage;
        Graphics gfx;

        Random rand;

        Size initialSize;

        ColorLine[] lines;

        Func<bool>[] sorts;
        bool sorted;
        bool makeSwap;

        Queue<int[]> swaps;

        enum UpdateType
        {
            SWAP,
            LINE,
            FULL
        }

        UpdateType updateMode = UpdateType.FULL;

        enum Sorts
        {
            BUBBLE,
            QUICK,
            NONE
        }

        int bubbleHeight;
        int bubbleWidth;

        Sorts currentSort;

        public Form1()
        {
            InitializeComponent();

            sorts = new Func<bool>[]
            {
                bubbleSort,
                quickSort,
                () => { return false; }
            };

            currentSort = Sorts.NONE;
            sorted = false;

            //sort variables
            bubbleHeight = 1;
            bubbleWidth = 0;
            makeSwap = false;
            swaps = new Queue<int[]>();

            rand = new Random();
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);
            initialSize = new Size(128, 64);
            colorImage = new Bitmap(initialSize.Width, initialSize.Height);
            lines = new ColorLine[initialSize.Width];
            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
            gfx.PixelOffsetMode = PixelOffsetMode.Half;

            for (int i = 0; i < initialSize.Width; i++)
            {
                lines[i] = new ColorLine(initialSize.Height, rand, InitType.UNLIMITED);
                for (int j = 0; j < initialSize.Height; j++)
                {
                    RGB currentPixel = ColorConversions.toRGB(lines[i].Colors[j]);
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
            if(!sorted)
            {
                if (sorts[(int)currentSort]())
                {
                    sorted = true;
                }
                
                switch(updateMode)
                {
                    case UpdateType.FULL:
                        colorImage = generateImage();
                        break;

                    case UpdateType.LINE:

                        break;

                    case UpdateType.SWAP:
                        updateSwap();
                        break;
                }
                
                gfx.DrawImage(colorImage, new Rectangle(Point.Empty, pictureBox1.Size));
                pictureBox1.Image = canvas;
            }
        }

        void updateLineSwap()
        {
            if (sorted)
                return;

            for(int i = 0; i < initialSize.Width; i++)
            {
                RGB pixelA = ColorConversions.toRGB(lines[i].Colors[bubbleHeight]);
                RGB pixelB = ColorConversions.toRGB(lines[i].Colors[bubbleHeight - 1]);

                colorImage.SetPixel(i, bubbleHeight, System.Drawing.Color.FromArgb(255, pixelA.R, pixelA.G, pixelA.B));
                colorImage.SetPixel(i, bubbleHeight - 1, System.Drawing.Color.FromArgb(255, pixelB.R, pixelB.G, pixelB.B));
            }
        }

        void initialize(InitType init)
        {
            for (int i = 0; i < initialSize.Width; i++)
            {
                lines[i] = new ColorLine(initialSize.Height, rand, init);
                for (int j = 0; j < initialSize.Height; j++)
                {
                    RGB currentPixel = ColorConversions.toRGB(lines[i].Colors[j]);
                    colorImage.SetPixel(i, j, System.Drawing.Color.FromArgb(255, currentPixel.R, currentPixel.G, currentPixel.B));
                }
            }

            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
            gfx.DrawImage(colorImage, new Rectangle(Point.Empty, pictureBox1.Size));
            pictureBox1.Image = canvas;
        }

        void updateSwap()
        {
            if (swaps.Count == 0)
            {
                return;
            }
            else
            {
                int[] swap = swaps.Dequeue();
                RGB pixelA = lines[swap[0]].Colors[swap[1]].ToRGB();
                RGB pixelB = lines[swap[0]].Colors[swap[2]].ToRGB();

                colorImage.SetPixel(swap[0], swap[2], System.Drawing.Color.FromArgb(255, pixelB.R, pixelB.G, pixelB.B));
                colorImage.SetPixel(swap[0], swap[1], System.Drawing.Color.FromArgb(255, pixelA.R, pixelA.G, pixelA.B));
            }
        }

        Bitmap generateImage()
        {
            Bitmap bitmap = new Bitmap(initialSize.Width, initialSize.Height);

            for (int i = 0; i < initialSize.Width; i++)
            {
                for (int j = 0; j < initialSize.Height; j++)
                {
                    RGB currentPixel = ColorConversions.toRGB(lines[i].Colors[j]);
                    bitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(255, currentPixel.R, currentPixel.G, currentPixel.B));
                }
            }

            return bitmap;
        }

        bool quickSort()
        {
            int sortedCount = 0;
            for(int i = 0; i < initialSize.Width; i++)
            {
                if (lines[i].quickLeft != null)
                {
                    if(lines[i].quickEnd - lines[i].quickStart <= 1)
                    {
                        lines[i].quickLeft = null;
                        lines[i].quickRight = null;
                        lines[i].quickStart = null;
                        lines[i].quickEnd = null;
                        continue;
                    }
                    else if (lines[i].quickEnd - lines[i].quickStart == 2)
                    {
                        if(lines[i].Colors[(int)lines[i].quickStart].H > lines[i].Colors[(int)lines[i].quickEnd - 1].H)
                        {
                            HSV temp = lines[i].Colors[(int)lines[i].quickStart];
                            lines[i].Colors[(int)lines[i].quickStart] = lines[i].Colors[(int)lines[i].quickEnd - 1];
                            lines[i].Colors[(int)lines[i].quickEnd - 1] = temp;

                            swaps.Enqueue(new int[] { i, (int)lines[i].quickStart, (int)lines[i].quickEnd - 1 });
                        }
                        lines[i].quickLeft = null;
                        lines[i].quickRight = null;
                        lines[i].quickStart = null;
                        lines[i].quickEnd = null;
                        continue;
                    }
                    else
                    {
                        if (lines[i].quickLeft > lines[i].quickRight)
                        {
                            HSV temp = lines[i].Colors[(int)lines[i].quickLeft];
                            lines[i].Colors[(int)lines[i].quickLeft] = lines[i].Colors[(int)lines[i].quickEnd - 1];
                            lines[i].Colors[(int)lines[i].quickEnd - 1] = temp;

                            swaps.Enqueue(new int[] { i, (int)lines[i].quickLeft, (int)lines[i].quickEnd - 1 });

                            lines[i].quickSubArrays.Enqueue(new int[] { (int)lines[i].quickLeft + 1, (int)lines[i].quickEnd - 1, (int)lines[i].quickLeft + 1, (int)lines[i].quickEnd });
                            lines[i].quickSubArrays.Enqueue(new int[] { (int)lines[i].quickStart, (int)lines[i].quickLeft - 1, (int)lines[i].quickStart, (int)lines[i].quickLeft });

                            lines[i].foundLeft = false;
                            lines[i].foundRight = false;

                            lines[i].quickLeft = null;
                            lines[i].quickRight = null;
                            lines[i].quickStart = null;
                            lines[i].quickEnd = null;
                        }
                        else
                        {
                            if (lines[i].foundLeft && lines[i].foundRight)
                            {
                                HSV temp = lines[i].Colors[(int)lines[i].quickLeft];
                                lines[i].Colors[(int)lines[i].quickLeft] = lines[i].Colors[(int)lines[i].quickRight];
                                lines[i].Colors[(int)lines[i].quickRight] = temp;

                                swaps.Enqueue(new int[] { i, (int)lines[i].quickLeft, (int)lines[i].quickRight });

                                lines[i].foundLeft = false;
                                lines[i].foundRight = false;

                                lines[i].quickLeft++;
                                lines[i].quickRight--;
                            }
                            else
                            {
                                if (!lines[i].foundLeft)
                                {
                                    if (lines[i].Colors[(int)lines[i].quickLeft].H >= lines[i].Colors[(int)lines[i].quickEnd - 1].H)
                                    {
                                        lines[i].foundLeft = true;
                                    }
                                    else
                                    {
                                        lines[i].quickLeft++;
                                    }
                                }

                                if (!lines[i].foundRight)
                                {
                                    if (lines[i].Colors[(int)lines[i].quickRight].H < lines[i].Colors[(int)lines[i].quickEnd - 1].H)
                                    {
                                        lines[i].foundRight = true;
                                    }
                                    else
                                    {
                                        lines[i].quickRight--;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if(lines[i].quickSubArrays.Count > 0)
                    {
                        int[] values = lines[i].quickSubArrays.Dequeue();
                        lines[i].quickLeft = values[0];
                        lines[i].quickRight = values[1];
                        lines[i].quickStart = values[2];
                        lines[i].quickEnd = values[3];
                    }
                    else
                    {
                        sortedCount++;
                    }
                }
            }

            return (sortedCount == initialSize.Width);
        }

        bool bubbleSort()
        {
            if(bubbleHeight >= lines[0].Colors.Length)
            {
                int sortedCount = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    if(lines[i].Sorted)
                    {
                        sortedCount++;
                    }
                }

                if (sortedCount == initialSize.Width)
                {
                    return true;
                }
                else
                {
                    bubbleHeight = 1;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        lines[i].Sorted = true;
                    }
                }
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Colors[bubbleHeight].H < lines[i].Colors[bubbleHeight - 1].H)
                {
                    lines[bubbleWidth].Sorted = false;
                    //makeSwap = true;
                    //swap = new int[] { i, bubbleHeight, bubbleHeight - 1 };

                    HSV temp = lines[i].Colors[bubbleHeight];
                    lines[i].Colors[bubbleHeight] = lines[i].Colors[bubbleHeight - 1];
                    lines[i].Colors[bubbleHeight - 1] = temp;
                }
            }

            bubbleHeight++;
            
            return false;
        }

        private void sortsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bubbleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentSort = Sorts.BUBBLE;
            bubbleHeight = 1;
            sorted = false;

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Sorted = true;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);
            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
            gfx.PixelOffsetMode = PixelOffsetMode.Half;

            colorImage = generateImage();
            gfx.DrawImage(colorImage, new Rectangle(Point.Empty, pictureBox1.Size));
            pictureBox1.Image = canvas;
        }

        private void randomHueUnlimitedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initialize(InitType.UNLIMITED);
        }

        private void rainbowHueROYGBIVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initialize(InitType.RAINBOW);
        }

        private void quickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentSort = Sorts.QUICK;
            sorted = false;
        }
    }
}