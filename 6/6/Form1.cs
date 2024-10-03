using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace _6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitCube();
            InitMatrix();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private Point3D[] cubeVertices;
        private double[,] transformationMatrix;
        private const double scaleFactor = 100;

        private void InitCube()//вершины куба
        {
            cubeVertices = new Point3D[]
            {
                new Point3D(-0.5, -0.5, -0.5),
                new Point3D( 0.5, -0.5, -0.5), 
                new Point3D( 0.5,  0.5, -0.5), 
                new Point3D(-0.5,  0.5, -0.5), 
                new Point3D(-0.5, -0.5,  0.5),
                new Point3D( 0.5, -0.5,  0.5), 
                new Point3D( 0.5,  0.5,  0.5),
                new Point3D(-0.5,  0.5,  0.5)  
            };
        }
        private void InitMatrix()
        {
            transformationMatrix = new double[4, 4];
            for (int i = 0; i < 4; i++)
            {
                transformationMatrix[i, i] = 1;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.X:
                    RotateX(0.1);
                    break;
                case Keys.Y:
                    RotateY(0.1); 
                    break;
                case Keys.Z:
                    RotateZ(0.1);
                    break;
                case Keys.Q:
                    Moves(0.1, 0, 0); 
                    break;
                case Keys.W:
                    Moves(0, 0.1, 0);
                    break;
                case Keys.E:
                    Moves(0, 0, 0.1);
                    break;
                case Keys.F:
                    Moves(-0.1, 0, 0); 
                    break;
                case Keys.C:
                    Moves(0, -0.1, 0);
                    break;
                case Keys.A:
                    Scale(1.1, 1, 1);
                    break;
                case Keys.S:
                    Scale(1, 1.1, 1);
                    break;
                case Keys.D:
                    Scale(1, 1, 1.1); 
                    break;
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)//рисовка рёбер куба
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            PointF[] projectedPoints = new PointF[cubeVertices.Length];

            for (int i = 0; i < cubeVertices.Length; i++)
            {
                Point3D transformedVertex = ApplyTransformation(cubeVertices[i]);
                projectedPoints[i] = new PointF(
                    (float)(transformedVertex.X * scaleFactor + Width / 2),
                    (float)(-transformedVertex.Y * scaleFactor + Height / 2)
                );
            }

            DrawLine(g, projectedPoints[0], projectedPoints[1]);
            DrawLine(g, projectedPoints[1], projectedPoints[2]);
            DrawLine(g, projectedPoints[2], projectedPoints[3]);
            DrawLine(g, projectedPoints[3], projectedPoints[0]);

            DrawLine(g, projectedPoints[4], projectedPoints[5]);
            DrawLine(g, projectedPoints[5], projectedPoints[6]);
            DrawLine(g, projectedPoints[6], projectedPoints[7]);
            DrawLine(g, projectedPoints[7], projectedPoints[4]);

            DrawLine(g, projectedPoints[0], projectedPoints[4]);
            DrawLine(g, projectedPoints[1], projectedPoints[5]);
            DrawLine(g, projectedPoints[2], projectedPoints[6]);
            DrawLine(g, projectedPoints[3], projectedPoints[7]);
        }

        private void DrawLine(Graphics g, PointF p1, PointF p2)
        {
            g.DrawLine(Pens.Black, p1, p2);
        }

        private Point3D ApplyTransformation(Point3D point)
        {
            double[] result = new double[4];
            double[] input = { point.X, point.Y, point.Z, 1 };

            for (int i = 0; i < 4; i++)
            {
                result[i] = 0;
                for (int j = 0; j < 4; j++)
                {
                    result[i] += transformationMatrix[i, j] * input[j];
                }
            }

            return new Point3D(result[0], result[1], result[2]);
        }

        private void RotateX(double angle)
        {
            double[,] rotationMatrix = new double[4, 4];
            rotationMatrix[0, 0] = 1;
            rotationMatrix[1, 1] = Math.Cos(angle);
            rotationMatrix[1, 2] = -Math.Sin(angle);
            rotationMatrix[2, 1] = Math.Sin(angle);
            rotationMatrix[2, 2] = Math.Cos(angle);
            rotationMatrix[3, 3] = 1;

            MultiplyMatrix(rotationMatrix);
        }

        private void RotateY(double angle)
        {
            double[,] rotationMatrix = new double[4, 4];
            rotationMatrix[1, 1] = 1;
            rotationMatrix[0, 0] = Math.Cos(angle);
            rotationMatrix[0, 2] = Math.Sin(angle);
            rotationMatrix[2, 0] = -Math.Sin(angle);
            rotationMatrix[2, 2] = Math.Cos(angle);
            rotationMatrix[3, 3] = 1;

            MultiplyMatrix(rotationMatrix);
        }

        private void RotateZ(double angle)
        {
            double[,] rotationMatrix = new double[4, 4];
            rotationMatrix[2, 2] = 1;
            rotationMatrix[0, 0] = Math.Cos(angle);
            rotationMatrix[0, 1] = -Math.Sin(angle);
            rotationMatrix[1, 0] = Math.Sin(angle);
            rotationMatrix[1, 1] = Math.Cos(angle);
            rotationMatrix[3, 3] = 1;

            MultiplyMatrix(rotationMatrix);
        }

        private void Moves(double dx, double dy, double dz)
        {
            double[,] translationMatrix = new double[4, 4];
            for (int i = 0; i < 4; i++) translationMatrix[i, i] = 1;

            translationMatrix[0, 3] = dx;
            translationMatrix[1, 3] = dy;
            translationMatrix[2, 3] = dz;

            MultiplyMatrix(translationMatrix);
        }

        private void Scale(double sx, double sy, double sz)
        {
            double[,] scaleMatrix = new double[4, 4];
            scaleMatrix[0, 0] = sx;
            scaleMatrix[1, 1] = sy;
            scaleMatrix[2, 2] = sz;
            scaleMatrix[3, 3] = 1;

            MultiplyMatrix(scaleMatrix);
        }

        private void MultiplyMatrix(double[,] matrix)
        {
            double[,] result = new double[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        result[i, j] += transformationMatrix[i, k] * matrix[k, j];
                    }
                }
            }
            transformationMatrix = result;
        }
    
    }
}
