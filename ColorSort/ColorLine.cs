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
    class ColorLine
    {
        public HSV[] Colors;
        public bool Sorted;

        public Queue<int[]> quickSubArrays;
        public int? quickLeft;
        public int? quickRight;
        public int? quickEnd;
        public int? quickStart;
        public bool foundLeft;
        public bool foundRight;

        

        public ColorLine(int length, Random rand, InitType init)
        {
            Sorted = false;
            Colors = new HSV[length];
            quickSubArrays = new Queue<int[]>();

            quickLeft = 0;
            quickRight = length;
            quickStart = 0;
            quickEnd = length;

            for (int i = 0; i < length; i++)
            {
                HSV currentColor = new HSV(0, 0, 0);
                 
                switch(init)
                {
                    case InitType.UNLIMITED:
                        currentColor = (new RGB((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256))).ToHSV();
                        break;

                    case InitType.RAINBOW:
                        switch (rand.Next(0, 7))
                        {
                            case 0:
                                //currentColor = (new RGB(162, 56, 54)).ToHSV();
                                currentColor = (new RGB(255, 0, 0)).ToHSV();
                                break;

                            case 1:
                                currentColor = (new RGB(221, 111, 0)).ToHSV();
                                break;

                            case 2:
                                currentColor = (new RGB(208, 217, 10)).ToHSV();
                                break;

                            case 3:
                                currentColor = (new RGB(0, 255, 20)).ToHSV();
                                break;

                            case 4:
                                currentColor = (new RGB(113, 202, 208)).ToHSV();
                                break;

                            case 5:
                                currentColor = (new RGB(88, 86, 199)).ToHSV();
                                break;

                            case 6:
                                currentColor = (new RGB(123, 75, 195)).ToHSV();
                                break;
                        }
                        break;
                }

                Colors[i] = new HSV(currentColor.H, currentColor.S, currentColor.V);
            }
        }
    }
}
