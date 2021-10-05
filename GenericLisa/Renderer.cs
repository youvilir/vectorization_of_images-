using System.Collections.Generic;
using System.Drawing;

namespace GenericLisa
{
    public static class Renderer
    {
       public static void Render(DNAWorkarea area, Graphics g) // создание рисунка
       {
            g.Clear(Color.Black);

            foreach (DNAPolygon polygon in area.Polygons)
                    Render(polygon, g);          
       }
        
        private static void Render(DNAPolygon polygon,Graphics g) // создание полигона
        {
            using (Brush brush = GetBrush(polygon.Brush))
            {
                Point[] points = GetGdiPoints(polygon.Points);
                g.FillPolygon(brush, points);
            }
        }

        // преобразование списка DNAPoint в список точек System.Drawing.Point
        private static Point[] GetGdiPoints(List<DNAPoint> points)
        {
            Point[] pts = new Point[points.Count];
            int i = 0;
            foreach (DNAPoint pt in points)
            {
                pts[i++] = new Point(pt.X, pt.Y);
            }
            return pts;
        }

        //преобразование DNABrash в System.Drawing.Brush
        private static Brush GetBrush(DNABrush b)
        {
            return new SolidBrush(Color.FromArgb(b.A, b.R, b.G, b.B));
        }
    }
}
