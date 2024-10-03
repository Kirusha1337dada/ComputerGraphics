using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        List<Point> point = new List<Point>();
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = this.CreateGraphics();
                g.Clear(BackColor);
                Pen pen = new Pen(Color.Black,2);

                g.DrawRectangle(pen, e.X, e.Y, 1, 1);
                point.Add(new Point(e.X, e.Y));

                if (point.Count > 1)
                {
                    for (int i = 0; i < point.Count - 1; i++)
                    {
                        Point p1 = point[i];
                        Point p2 = point[i + 1];
                        g.DrawLine(pen, p1, p2);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                Graphics g = this.CreateGraphics();
                Pen pen = new Pen(Color.Black, 2);
                SolidBrush brush = new SolidBrush(Color.Blue);

                g.DrawLine(pen, point[point.Count - 1].X, point[point.Count - 1].Y, point[0].X, point[0].Y);
               
                ScanLineFill(g, point.ToArray(), Color.Blue);

                for (int i = 0; i < point.Count - 1; i++)
                {
                    Point p1 = point[i];
                    Point p2 = point[i + 1];
                    g.DrawLine(pen, p1, p2);
                }

                g.DrawLine(pen, point[point.Count - 1], point[0]);
            }
        }
       private void ScanLineFill(Graphics g, Point[] polygon, Color color)
        {
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (var point in polygon)
            {
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            SolidBrush brush = new SolidBrush(color);
            Pen pen = new Pen(color);

            for (int y = minY; y <= maxY; y++)
            {
                List<int> intersections = FindIntersections(polygon, y);

                intersections.Sort();

                for (int i = 0; i < intersections.Count - 1; i += 2)
                {
                    int x1 = intersections[i];
                    int x2 = intersections[i + 1];

                    g.DrawLine(pen, x1, y, x2, y);
                }
            }
        }

       
        private List<int> FindIntersections(Point[] polygon, int y)
        {
            List<int> intersections = new List<int>();

            for (int i = 0; i < polygon.Length; i++)
            {
                Point p1 = polygon[i];
                Point p2 = polygon[(i + 1) % polygon.Length];

                if ((p1.Y <= y && p2.Y >    y) || (p2.Y <= y && p1.Y > y))
                {
                    int x = p1.X + (y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y);
                    intersections.Add(x);
                }
            }

            return intersections;
        }
    
    }
}
