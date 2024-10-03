using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _1
{
    public partial class Form1 : Form
    {
        Bitmap bmp;

        public Form1()
        {
            InitializeComponent();
            //bmp=new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        List<Point> point = new List<Point>();
        List<Color> color = new List<Color>();
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = this.CreateGraphics();
                Pen pen = new Pen(Color.Black);

                g.DrawRectangle(pen, e.X, e.Y, 1, 1);
                point.Add(new Point(e.X, e.Y));
                color.Add(Color.Black);

                /*bmp.SetPixel(e.X, e.Y,Color.Black);
                pictureBox1.Image = bmp;*/
            }
            else if (e.Button == MouseButtons.Right)
            {
                Random random = new Random();
                Color colorRandom = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));

                for (int i = 0; i < point.Count; i++)
                {
                    Graphics g = this.CreateGraphics();
                    Pen pen = new Pen(colorRandom);
                    
                    color[i] = colorRandom;
                    g.DrawRectangle(pen, point[i].X, point[i].Y, 1, 1);
                    
                    //bmp.SetPixel(e.X, e.Y, colorRandom);
                }
                //pictureBox1.Image = bmp;
            }
        }


    }
}
