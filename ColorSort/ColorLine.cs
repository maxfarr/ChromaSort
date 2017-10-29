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

        public ColorLine(int length, Random rand)
        {
            Sorted = false;
            Colors = new HSV[length];
            for (int i = 0; i < length; i++)
            {
                HSV currentColor = (new RGB((byte)rand.Next(0, 265), (byte)rand.Next(0, 265), (byte)rand.Next(0, 265))).ToHSV();
                Colors[i] = new HSV(currentColor.H, currentColor.S, currentColor.V);
            }
        }
    }
}
