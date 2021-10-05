using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace GenericLisa
{
    public class DNAPolygon: ICloneable
    {
        public List<DNAPoint> Points { get; set; }
        public DNABrush Brush { get; set; }

        public void SetRandom()// генерация полигона
        {
            Points = new List<DNAPoint>();

            DNAPoint center = new DNAPoint(); // создание рандомной точки
            center.SetRandom();

            for (int i = 0; i < Settings.ActivePointsPerPolygonMin; i++) // создание полигона вокруг исходной точки
            {
                var point = new DNAPoint();
                point.X = Math.Min(Math.Max(0, center.X + Tools.GetRandomNumber(-3, 3)), Tools.MaxWidth);
                point.Y = Math.Min(Math.Max(0, center.Y + Tools.GetRandomNumber(-3, 3)), Tools.MaxHeight);

                Points.Add(point);
            }

            Brush = new DNABrush();
            Brush.SetRandom();
        }
        public void Mutate(DNAWorkarea drawing)
        {
            if (Tools.WillMutate(Settings.ActiveAddPointMutationRate))
                AddPoint(drawing);

            if (Tools.WillMutate(Settings.ActiveRemovePointMutationRate))
                RemovePoint(drawing);

            Brush.Mutate(drawing);
            Points.ForEach(p => p.Mutate(drawing));
        }
        private void AddPoint(DNAWorkarea drawing)
        {
            if (Points.Count < Settings.ActivePointsPerPolygonMax)
            {
                if (drawing.PointCount < Settings.ActivePointsMax)
                {
                    var newPoint = new DNAPoint();

                    int index = Tools.GetRandomNumber(1, Points.Count - 1);

                    DNAPoint prev = Points[index - 1];
                    DNAPoint next = Points[index];

                    newPoint.X = (prev.X + next.X) / 2;
                    newPoint.Y = (prev.Y + next.Y) / 2;


                    Points.Insert(index, newPoint);

                    drawing.IsChange = true;
                }
            }
        }
        private void RemovePoint(DNAWorkarea drawing)
        {
            if (Points.Count > Settings.ActivePointsPerPolygonMin)
            {
                if (drawing.PointCount > Settings.ActivePointsMin)
                {
                    int index = Tools.GetRandomNumber(0, Points.Count);
                    Points.RemoveAt(index);

                    drawing.IsChange = true;
                }
            }
        }

        public object Clone()
        {
            DNAPolygon newpolygon = new DNAPolygon();
            newpolygon.Brush = Brush.Clone() as DNABrush;
            newpolygon.Points = new List<DNAPoint>();
            Points.ForEach(p => newpolygon.Points.Add(p.Clone() as DNAPoint));
            return newpolygon;
        }
    }
}
