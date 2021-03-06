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
    public class Polygon
    {
        public DVector3 Normal;
        public Vertex v1, v2, v3;

        public Polygon() { }
        public Polygon(Vertex v1, Vertex v2, Vertex v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public double CalcNormal(DVector4[] v)
        {
            DVector3 l1 = new DVector3(v[1].X - v[0].X, v[1].Y - v[0].Y, v[1].Z - v[0].Z);
            DVector3 l2 = new DVector3(v[2].X - v[0].X, v[2].Y - v[0].Y, v[2].Z - v[0].Z);
            Normal = DVector3.CrossProduct(l1, l2);
            return DVector3.DotProduct(new DVector3(0, 0, -1), Normal);
        }
    }
}
