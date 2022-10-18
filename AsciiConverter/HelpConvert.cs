using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AsciiConverter
{
    internal class HelpConvert
    {
        public static Bitmap FileToBitmap(string full_path)
        {
            Bitmap bmp = new Bitmap(full_path);
            return bmp;
        }

        public static string BitmapToAscii(Bitmap src, uint height, uint width)
        {
            if (src == null) throw new Exception("The image source is null");
            if (height == 0 || width == 0) throw new Exception("The height or width is 0");

            //Create a buffer big enough for the image (and line breaks)
            uint size = height * (width + 1) * 2;
            int index = 0;
            Char[] res = new Char[size];

            //Calculate the pixel per character for both dimensions
            double px_per_ch_v = src.Height / (double)height;
            double px_per_ch_h = src.Width / (double)width;

            //Calculate the actual current char position
            double actual_ch_v = 0.0;
            double actual_ch_h = 0.0;

            //Initialize the list of colors (may be helpful later)
            int colors_index = 0;
            List<Color> colors = new List<Color>(new Color[size]);

            for (uint i = 0; i < height; ++i)
            {
                actual_ch_h = 0.0;
                for (uint j = 0; j < width; ++j)
                {
                    int R = 0, G = 0, B = 0, A = 0, count = 0;
                    for (int bmp_i = (int)Math.Round(actual_ch_v); bmp_i < (int)Math.Round(actual_ch_v + px_per_ch_v); ++bmp_i) {
                        for (int bmp_j = (int)Math.Round(actual_ch_h); bmp_j < (int)Math.Round(actual_ch_h + px_per_ch_h); ++bmp_j)
                        {
                            var curr_px = src.GetPixel(bmp_j, bmp_i);
                            R += curr_px.R * curr_px.R;
                            G += curr_px.G * curr_px.G;
                            B += curr_px.B * curr_px.B;
                            A += curr_px.A * curr_px.A;
                            ++count;
                        }
                    }
                    Color color = Color.FromArgb((int)Math.Sqrt(A/count), (int)Math.Sqrt(R/count), (int)Math.Sqrt(G/count), (int)Math.Sqrt(B/count));
                    colors[colors_index++] = color;
                    res[index++] = ColorToChar(color);
                    res[index++] = ColorToChar(color);
                    actual_ch_h += px_per_ch_h;
                }
                res[index++] = '\n';
                actual_ch_v += px_per_ch_v;

                Console.WriteLine("Row " + i.ToString() + "/" + height.ToString() + " completed");
            }
            return new string(res);
        }

        public static Char ColorToChar(Color src)
        {
            int step = 256 / 5;
            var value = (src.R + src.G + src.B) == 0 ? 0 : (src.R + src.G + src.B) / 3;
            if (value < step) return '█';
            if (value < 2*step) return '▓';
            if (value < 3*step) return '▒';
            if (value < 4*step) return '░';
            return ' ';
        }
    }
}
