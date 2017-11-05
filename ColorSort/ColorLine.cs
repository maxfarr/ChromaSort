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

namespace ChromaSort
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
            quickRight = length - 1;
            quickStart = 0;
            quickEnd = length;

            for (int i = 0; i < length; i++)
            {
                HSV currentColor = new HSV(0, 0, 0);
                 
                switch(init)
                {
                    case InitType.UNLIMITED:
                        currentColor = new HSV(rand.Next(0, 360), 1, 1);
                        break;

                    case InitType.RAINBOW:
                        switch (rand.Next(0, 7))
                        {
                            case 0:
                                //currentColor = (new RGB(162, 56, 54)).ToHSV();
                                currentColor = ColorConversions.toHSV(new RGB(255, 0, 0));
                                break;

                            case 1:
                                currentColor = ColorConversions.toHSV(new RGB(221, 111, 0));
                                break;

                            case 2:
                                currentColor = ColorConversions.toHSV(new RGB(208, 217, 10));
                                break;

                            case 3:
                                currentColor = ColorConversions.toHSV(new RGB(0, 255, 20));
                                break;

                            case 4:
                                currentColor = ColorConversions.toHSV(new RGB(0, 167, 244));
                                break;

                            case 5:
                                currentColor = ColorConversions.toHSV(new RGB(119, 47, 182));
                                break;

                            case 6:
                                currentColor = ColorConversions.toHSV(new RGB(161, 32, 123));
                                break;
                        }
                        break;
                }

                Colors[i] = new HSV(currentColor.H, currentColor.S, currentColor.V);
            }
        }
    }
}
