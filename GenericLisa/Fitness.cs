using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericLisa
{
    class Fitness
    {
        //https://habr.com/ru/post/196578/
        
        public static double GetDrawingFitness(DNAWorkarea newDrawing, Color[,] sourceColors)
        {
            double error = 0;

            using (var b = new Bitmap(Tools.MaxWidth, Tools.MaxHeight, PixelFormat.Format24bppRgb))
            using (Graphics g = Graphics.FromImage(b))
            {
                Renderer.Render(newDrawing, g);

                BitmapData bmd1 = b.LockBits(new Rectangle(0, 0, Tools.MaxWidth, Tools.MaxHeight), ImageLockMode.ReadOnly,//Блокирует объект Bitmap в системной памяти.
                                             PixelFormat.Format24bppRgb);//Format24bppRgb - это только значения красного, зеленого и синего каждого пикселя. В 8 бит на цвет вы получаете 24 бит на пиксель.


                for (int y = 0; y < Tools.MaxHeight; y++)
                {
                    for (int x = 0; x < Tools.MaxWidth; x++)
                    {
                        Color c1 = GetPixel(bmd1, x, y);
                        Color c2 = sourceColors[x, y];

                        double pixelError = GetColorFitness(c1, c2);
                        error += pixelError;
                    }
                }

                b.UnlockBits(bmd1);
            }

            return error;
        }

        //https://ru.stackoverflow.com/questions/1206105/Поясните-как-работать-с-lockbits-классом

        //https://progi.pro/neobhodimo-znat-raznicu-mezhdu-formatom-formata-bitmap-format32bppargb-i-format24bpprgb-9487881#:~:text=Format24bppRgb%20-%20это%20только%20значения,получаете%2024%20бит%20на%20пиксель
        private static unsafe Color GetPixel(BitmapData bmd, int x, int y)
        {
            byte* p = (byte*)bmd.Scan0 + y * bmd.Stride + 3 * x; // т.к. изображение 24 - битное, то есть 3 байта на пиксель, поэтому шаг = 3
            return Color.FromArgb(p[2], p[1], p[0]);
        }

        private static double GetColorFitness(Color c1, Color c2)
        {
            double r = c1.R - c2.R;
            double g = c1.G - c2.G;
            double b = c1.B - c2.B;

            return r * r + g * g + b * b;
        }
    }
}
