using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Xml.Schema;

namespace _7
{
    public partial class Form1 : Form
    {   
        private int iter = 1; 
        private PictureBox pictureBox;
        private double xMin = -1, yMin = -1, xMax = 1, yMax = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int width = 1600;
            int height = 800;
            DrawFractal(width, height);
            pictureBox.MouseClick += Form1_MouseClick;
        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                iter += 20;
                DrawFractal(1600, 800);
                pictureBox.Invalidate();
            }
            else
            {
                xMin += (xMax - xMin) / 3;
                yMin += (yMax - yMin) / 3;
                xMax -= (xMax - xMin) / 3;
                yMax -= (yMax - yMin) / 3;
                Bitmap juliaSet = PlotJuliaSet(new Complex(-0.2, 0.75), 1600, 800, iter, xMin, yMin, xMax, yMax);

                if (pictureBox == null)
                {
                    pictureBox = new PictureBox
                    {
                        Dock = DockStyle.Fill
                    };
                    this.Controls.Add(pictureBox);
                }

                pictureBox.Image = juliaSet;
                pictureBox.Invalidate();
            }
        }

        private void DrawFractal(int width, int height)
        {
            Complex c = new Complex(-0.2, 0.75); 

            Bitmap juliaSet = PlotJuliaSet(c, width, height, iter);

            if (pictureBox == null)
            {
                pictureBox = new PictureBox
                {
                    Dock = DockStyle.Fill
                };
                this.Controls.Add(pictureBox);
            }

            pictureBox.Image = juliaSet;
        }

        static Bitmap PlotJuliaSet(Complex c, int w, int h, int maxIter,
          double xMin = double.NaN, double yMin = double.NaN, double xMax = double.NaN, double yMax = double.NaN)
        {
            double r = CalculateR(c);
            if (double.IsNaN(xMin) || double.IsNaN(xMax) || double.IsNaN(yMin) || double.IsNaN(yMax))
            {
                xMin = -r;
                yMin = -r;
                xMax = r;
                yMax = r;
            }

            double xStep = Math.Abs(xMax - xMin) / w;
            double yStep = Math.Abs(yMax - yMin) / h;
            Bitmap bmp = new Bitmap(w, h);

            IDictionary<int, IDictionary<int, int>> xyIdx = new Dictionary<int, IDictionary<int, int>>();
            int maxIdx = 0;
            for (int i = 0; i < w; i++)
            {
                xyIdx.Add(i, new Dictionary<int, int>());
                for (int j = 0; j < h; j++)
                {
                    double x = xMin + i * xStep;
                    double y = yMin + j * yStep;
                    Complex z = new Complex(x, y);
                    IList<Complex> zIter = SqPolyIteration(z, c, maxIter, r);
                    int idx = zIter.Count - 1;
                    if (maxIdx < idx)
                    {
                        maxIdx = idx;
                    }
                    xyIdx[i].Add(j, idx);
                }
            }

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int idx = xyIdx[i][j];
                    double x = xMin + i * xStep;
                    double y = yMin + j * yStep;
                    Complex z = new Complex(x, y);
                    bmp.SetPixel(w - i - 1, j, ComplexHeatMap(idx, 0, maxIdx, z, r));
                }
            }

            return bmp;
        }

        private static IList<Complex> SqPolyIteration(Complex z0, Complex c, int n, double r = 0)
        {
            IList<Complex> res = new List<Complex>();
            res.Add(z0);
            for (int i = 0; i < n; i++)
            {
                if (r > 0 && res.Last().Magnitude > r)
                {
                    break;
                }
                res.Add(res.Last() * res.Last() + c); 
            }
            return res;
        }

        private static double CalculateR(Complex c)
        {
            return (1 + Math.Sqrt(1 + 4 * c.Magnitude)) / 2;
        }

        public static Color ComplexHeatMap(int value, int min, int max, Complex z, double r)
        {
            double val = (double)(value - min) / (max - min);
            return Color.FromArgb(
                255,
                Convert.ToByte(255 * val),
                Convert.ToByte(255 * (1 - val)),
                Convert.ToByte(255 * (z.Magnitude / r > 1 ? 1 : z.Magnitude / r))
            );
        }

    }
}
