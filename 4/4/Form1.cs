using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Point> LinePoints = new List<Point>();
        private Rectangle? rect = null;
        bool DrawRect = false;
        List<Tuple<Point, Point>> clippedSegments = new List<Tuple<Point, Point>>();
        
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = this.CreateGraphics();
                Pen pen = new Pen(Color.Red);
                

                LinePoints.Add(e.Location);

                if (LinePoints.Count > 1)
                {
                    g.DrawLine(pen, LinePoints[LinePoints.Count - 2], LinePoints[LinePoints.Count - 1]);
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                //clippedSegments.Clear();
                //this.Invalidate(); 
                DrawRect = true;
                rect = new Rectangle(e.X, e.Y, 0, 0);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (DrawRect && rect.HasValue)
            {
                Graphics g = this.CreateGraphics();
                Pen pen = new Pen(Color.Black);
                

                rect = new Rectangle(rect.Value.X, rect.Value.Y, e.X - rect.Value.X, e.Y - rect.Value.Y);
               
                g.DrawRectangle(pen, rect.Value);
                this.Invalidate();
            }
        } 
        
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && DrawRect)
            {
                DrawRect = false;
                if(rect.HasValue)
                {
                    Graphics g = this.CreateGraphics();
                    Pen pen1 = new Pen(Color.Black);
                    Pen pen2 = new Pen(Color.Blue);

                    g.DrawRectangle(pen1, rect.Value);

                    //clippedSegments.Clear();

                    if (LinePoints.Count > 1)
                    {
                        for (int i = 0; i < LinePoints.Count-1; i++)
                        {
                            g.DrawLine(Pens.Red, LinePoints[i], LinePoints[i+1]);
                        }
                    }
                    for (int i = 0; i < LinePoints.Count - 1; i++)
                    {
                        var clippedSegment = Line(LinePoints[i], LinePoints[i + 1], rect.Value);

                        if (clippedSegment != null)
                        {
                            clippedSegments.Add(clippedSegment);
                            
                            g.DrawLine(pen2, clippedSegment.Item1, clippedSegment.Item2);
                        }
                    }
                }
            }
        }

        private Tuple<Point, Point> Line(Point _p1, Point _p2, Rectangle rect)
        {
            int code1 = ComputeOutCode(_p1, rect);
            int code2 = ComputeOutCode(_p2, rect);
            bool accept = false;
            PointF p1 = _p1;
            PointF p2 = _p2;
            while (true)
            {
               /* if ((code1| code2) == 0)
                {
                    accept = true;
                    break;
                }*/
                else if ((code1 & code2) != 0)
                {
                    break;
                }
                else
                {
                    int outcodeOut = code1;
                    if(code1 == 0)
                    {
                        outcodeOut = code2;
                    }
                    // code1 != 0 ? code1 : code2;
                    

                    PointF intersection = new PointF();

                    if ((outcodeOut & 1) == 1) 
                    {
                        intersection.Y = p1.Y + (p2.Y - p1.Y) * (rect.Left - p1.X) / (p2.X - p1.X);
                        intersection.X = rect.Left;
                    }
                    if ((outcodeOut & 2) == 2)
                    {
                        intersection.Y = p1.Y + (p2.Y - p1.Y) * (rect.Right - p1.X) / (p2.X - p1.X);
                        intersection.X = rect.Right; 
                    }
                    if ((outcodeOut & 4) == 4)
                    {
                        intersection.X = p1.X + (p2.X - p1.X) * (rect.Bottom - p1.Y) / (p2.Y - p1.Y);
                        intersection.Y = rect.Bottom;
                    }
                    if ((outcodeOut & 8) == 8)
                    {
                        intersection.X = p1.X + (p2.X - p1.X) * (rect.Top - p1.Y) / (p2.Y - p1.Y);
                        intersection.Y = rect.Top;
                    }
                            
                    
                    if (outcodeOut == code1)
                    {
                        p1 = intersection;
                        code1 = ComputeOutCode(p1, rect);
                    }
                    else
                    {
                        p2 = intersection;
                        code2 = ComputeOutCode(p2, rect);
                    }
                }
            }

            if (accept)
            {
                return Tuple.Create(new Point((int)p1.X, (int)p1.Y), new Point((int)p2.X,(int)p2.Y)); 
            }

            return null; 
        }

        private int ComputeOutCode(PointF p, Rectangle rect)
        {
            int code = 0;

            if (p.X < rect.Left) code |= 1; 
            else if (p.X > rect.Right) code |= 2; 
            if (p.Y < rect.Top) code |= 8; 
            else if (p.Y > rect.Bottom) code |= 4; 
            return code;
        }

    }
}
