using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericLisa
{
    public class DNAWorkarea: ICloneable
    {
        public List<DNAPolygon> Polygons { get; set; }
        public bool IsChange { get; set; }

        public int PointCount // подсчет всех точек
        {
            get
            {
                int pointCount = 0;
                foreach (DNAPolygon polygon in Polygons)
                    pointCount += polygon.Points.Count;

                return pointCount;
            }
        }
        public void SetRandom()// генерация полигонов
        {
            Polygons = new List<DNAPolygon>();

            for (int i = 0; i < Settings.ActivePolygonsMin; i++)
                AddPolygon();

            IsChange = true;
        }

        public void Mutate() // мутация
        {
            if (Tools.WillMutate(Settings.ActiveAddPolygonMutationRate))
                AddPolygon();

            if (Tools.WillMutate(Settings.ActiveRemovePolygonMutationRate))
                RemovePolygon();

            foreach (DNAPolygon polygon in Polygons)
                polygon.Mutate(this);
        }

        public void AddPolygon()// добавление нового полигона
        {
            if (Polygons.Count < Settings.ActivePolygonsMax)
            {
                var newPolygon = new DNAPolygon();
                newPolygon.SetRandom();

                int index = Tools.GetRandomNumber(0, Polygons.Count);

                Polygons.Insert(index, newPolygon);
                IsChange = true;
            }
        }

        public void RemovePolygon()//удаление полигона
        {
            if (Polygons.Count > Settings.ActivePolygonsMin)
            {
                int index = Tools.GetRandomNumber(0, Polygons.Count);
                Polygons.RemoveAt(index);
                IsChange = true;

            }
        }

        //public double Fitness(Color[,] colors)
        //{
        //    BitmapData data;
        //    double fitness = 0;
        //    Bitmap img = Draw();
        //    //FastBitmap fastimg = new FastBitmap(img);
        //    for (int i = 0; i < Helper.Width; i++)
        //    {
        //        for (int j = 0; j < Helper.Height; j++)
        //        {
        //            Color c1 = img.GetPixel(i, j);
        //            Color c2 = colors[i, j];
        //            int r = c1.R - c2.R;
        //            int g = c1.G - c2.G;
        //            int b = c1.B - c2.B;
        //            fitness += r * r + g * g + b * b;
        //        }
        //    }
        //    //fastimg.Release();
        //    img.Dispose();
        //    return fitness;
        //}

        //public Bitmap Draw()
        //{
        //    Bitmap img = new Bitmap(Helper.Width, Helper.Height);
        //    using (Graphics g = Graphics.FromImage(img))
        //    {
        //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //        g.Clear(Color.Black);

        //        Polygons.ForEach(p => p.Draw(g));
        //    }
        //    return img;
        //}

        public object Clone()
        {
            DNAWorkarea newarea = new DNAWorkarea();
            newarea.Polygons = new List<DNAPolygon>();
            foreach (DNAPolygon polygon in Polygons)
                newarea.Polygons.Add(polygon.Clone() as DNAPolygon);

            return newarea;
        }
    }
}
