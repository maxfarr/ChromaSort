using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectrum;
using static Spectrum.Color;

namespace ChromaSort
{
    static class ColorConversions
    {
        public static HSV toHSV (RGB rgb)
        {
            float h = 0;
            float s;
            float v;
            float rPrime = (float)rgb.R / 255;
            float gPrime = (float)rgb.G / 255;
            float bPrime = (float)rgb.B / 255;
            float cMax = Math.Max(Math.Max(rPrime, gPrime), bPrime);
            float cMin = Math.Min(Math.Min(rPrime, gPrime), bPrime);
            float delta = cMax - cMin;

            if(delta == 0)
            {
                h = 0;
            }
            else if(cMax == rPrime)
            {
                h = 60 * (((gPrime - bPrime)/delta)%6);
            }
            else if(cMax == gPrime)
            {
                h = 60 * (((bPrime - rPrime)/delta)+2);
            }
            else if(cMax == bPrime)
            {
                h = 60 * (((rPrime - gPrime)/delta)+4);
            }

            s = cMax == 0 ? 0 : (delta/cMax);

            v = cMax;

            return new HSV(h >= 0 ? h : 360 + h, s, v);
        }

        public static RGB toRGB (HSV hsv)
        {
            float r;
            float g;
            float b;

            double c = hsv.V * hsv.S;
            double x = c * (1 - Math.Abs((hsv.H/60)%2 - 1));
            double m = hsv.V - c;

            float rPrime = 0;
            float gPrime = 0;
            float bPrime = 0;
            if(hsv.H >= 0 && hsv.H < 60)
            {
                rPrime = (float)c;
                gPrime = (float)x;
            }
            else if(hsv.H >= 60 && hsv.H < 120)
            {
                rPrime = (float)x;
                gPrime = (float)c;
            }
            else if (hsv.H >= 120 && hsv.H < 180)
            {
                gPrime = (float)c;
                bPrime = (float)x;
            }
            else if (hsv.H >= 180 && hsv.H < 240)
            {
                gPrime = (float)x;
                bPrime = (float)c;
            }
            else if (hsv.H >= 240 && hsv.H < 300)
            {
                rPrime = (float)x;
                bPrime = (float)c;
            }
            else if (hsv.H >= 300 && hsv.H < 360)
            {
                rPrime = (float)c;
                bPrime = (float)x;
            }

            r = (float)(rPrime + m) * 255;
            g = (float)(gPrime + m) * 255;
            b = (float)(bPrime + m) * 255;

            return new RGB((byte)r, (byte)g, (byte)b);
        }
    }
}
