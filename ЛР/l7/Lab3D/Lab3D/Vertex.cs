using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using CGLabPlatform;

namespace Lab3D
{
    public class Vertex
    {
        public DVector4 Vector;

        double x, y, z;
        public double X { set { x = value; } get { return x; } }
        public double Y { set { y = value; } get { return y; } }
        public double Z { set { z = value; } get { return z; } }

        public Vertex() { }
        public Vertex(double x, double y, double z)
        {
            this.Vector = new DVector4(x, y, z, 1);
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static DMatrix4 RotateX(double angle)
        {
            angle = angle * Math.PI / 180;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return new DMatrix4(1, 0,   0,    0,
                                0, cos, -sin, 0,
                                0, sin, cos,  0,
                                0, 0,   0,    1);
        }

        public static DMatrix4 RotateY(double angle)
        {
            angle = angle * Math.PI / 180;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return new DMatrix4(cos,  0, sin, 0,
                                0,    1, 0,   0,
                                -sin, 0, cos, 0,
                                0,    0, 0,   1);
        }

        public static DMatrix4 RotateZ(double angle)
        {
            angle = angle * Math.PI / 180;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return new DMatrix4(cos, -sin, 0, 0,
                                sin, cos,  0, 0,
                                0,   0,    1, 0,
                                0,   0,    0, 1);
        }

        public static DMatrix4 Rotate(double angleX, double angleY, double angleZ)
        {
            return RotateX(angleX) * RotateY(angleY) * RotateZ(angleZ);
        }

        public static DMatrix4 Scale(double val)
        {
            return new DMatrix4(val, 0, 0, 0,
                                0, val, 0, 0,
                                0, 0, val, 0,
                                0, 0, 0, 1);
        }

        public static DMatrix4 Shift(double x, double y, double z)
        {
            return new DMatrix4(1, 0, 0, x,
                                0, 1, 0, y,
                                0, 0, 1, z,
                                0, 0, 0, 1);
        }

        public static DMatrix4 ViewXY()
        {
            return new DMatrix4(1, 0, 0, 0,
                                0, 1, 0, 0,
                                0, 0, 0, 0,
                                0, 0, 0, 1);
        }

        public static DMatrix4 ViewXZ()
        {
            return new DMatrix4(1, 0, 0, 0,
                                0, 0, 0, 0,
                                0, 0, 1, 0,
                                0, 0, 0, 1);
        }

        public static DMatrix4 ViewYZ()
        {
            return new DMatrix4(0, 0, 0, 0,
                                0, 1, 0, 0,
                                0, 0, 1, 0,
                                0, 0, 0, 1);
        }
    }
}
