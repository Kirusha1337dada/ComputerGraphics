using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace _3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int x0, x1, y0, y1, a, b;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x0 = e.X;
            y0 = e.Y;
        }

        Bitmap bitmap = new Bitmap(600, 400);
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bitmap.Dispose();
            bitmap = new Bitmap(600, 400);
     

            x1 = e.X;
            y1 = e.Y;

            a = Math.Abs(x0 - x1) / 2;
            b = Math.Abs(y0 - y1) / 2;

            drawRectangle(x0,  y0, x1, y1);
            drawEllipse((x0 + x1) / 2, (y0 + y1) / 2, a - 1, b);

            pictureBox1.Image = bitmap;
        }

        private void drawRectangle(int xa, int ya, int xb, int yb)
        {
            x1 = Math.Max(xa, xb);
            x0 = Math.Min(xa, xb);
            y1 = Math.Max(ya, yb);
            y0 = Math.Min(ya, yb);
            
            for (int x = x0; x <= x1; x++)
            {
                bitmap.SetPixel(x, y0, Color.Black);
                bitmap.SetPixel(x, y1, Color.Black);
            }
            for (int y = y0; y <= y1; y++)
            {
                bitmap.SetPixel(x0, y, Color.Black);
                bitmap.SetPixel(x1, y, Color.Black);
            }
            pictureBox1.Image = bitmap;
        }
        private void drawEllipse(int x, int y, int a, int b)
        {            
            int _x = 0;
            int _y = b; 
            int a_sqr = a * a; //большая полуось
            int b_sqr = b * b; //малая полуось
            int delta = 4 * b_sqr * ((_x + 1) * (_x + 1)) + a_sqr * ((2 * _y - 1) * (2 * _y - 1)) - 4 * a_sqr * b_sqr; 
            while (a_sqr * (2 * _y - 1) > 2 * b_sqr * (_x + 1))
            {
                Set4Pixel(x, y, _x, _y, Color.Red);
                if (delta < 0) 
                {
                    _x++;
                    delta += 4 * b_sqr * (2 * _x + 3);
                }
                else 
                {
                    _x++;
                    delta = delta - 8 * a_sqr * (_y - 1) + 4 * b_sqr * (2 * _x + 3);
                    _y--;   
                }
            }
            delta = b_sqr * ((2 * _x + 1) * (2 * _x + 1)) + 4 * a_sqr * ((_y + 1) * (_y + 1)) - 4 * a_sqr * b_sqr; 
            while (_y + 1 != 0) 
            {
                Set4Pixel(x, y, _x, _y,Color.Red);
                if (delta < 0) 
                {
                    _y--;
                    delta += 4 * a_sqr * (2 * _y + 3);
                }
                else 
                {
                    _y--;
                    delta = delta - 8 * b_sqr * (_x + 1) + 4 * a_sqr * (2 * _y + 3);
                    _x++;
                }
            } 
        }

        private void SetPixel(int x,int y,Color color)
        {
            bitmap.SetPixel(x, y, color);
        }

        private void Set4Pixel(int x, int y, int dx, int dy, Color color)
        {
            SetPixel(x0 + dx + a, y0 + dy + b, color);
            SetPixel(x0 + dx + a, y0 - dy + b, color);
            SetPixel(x0 - dx + a, y0 + dy + b, color);
            SetPixel(x0 - dx + a, y0 - dy + b, color);
            pictureBox1.Image = bitmap;
        }

    }
}
