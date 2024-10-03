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

namespace _2
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
                Pen pen = new Pen(Color.Black);

                g.DrawRectangle(pen, e.X, e.Y, 1, 1);
                point.Add(new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Right)
            {
                Graphics g = this.CreateGraphics();
                g.Clear(this.BackColor);

                if (point.Count > 1)
                {
                    for (int i = 0; i < point.Count - 1; i++)
                    {
                        Point p1 = point[i];
                        Point p2 = point[i+1]; //% point.Count
                        DrawLineB( p1.X, p1.Y, p2.X, p2.Y, Color.Red);
                       //g.DrawLine(Pens.Red, p1, p2);
                    }
                    DrawLineB(point[point.Count - 1].X, point[point.Count - 1].Y, point[0].X, point[0].Y, Color.Red);              
                }
                point.Clear();
            }
        }

        private void DrawLineB(int x0, int y0, int x1, int y1, Color color)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            Graphics g = this.CreateGraphics();
            g.FillRectangle(new SolidBrush(color), x0, y0, 1, 1);

            while (true)
            {
                if (x0 == x1 && y0 == y1)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }

                g.FillRectangle(new SolidBrush(color), x0, y0, 1, 1);
            }
        }
        
    }
}
