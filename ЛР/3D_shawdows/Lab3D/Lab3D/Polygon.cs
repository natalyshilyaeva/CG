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
        public Vertex v1, v2, v3, v4;
        public DVector3 Center;
        //public double Intensive;
        public DVector3 Intensive;
        public DVector3 L;
        public DVector3 Eye;
        public DVector3 S;
        public DVector3 R;

        public Polygon() { }
        public Polygon(Vertex v1, Vertex v2, Vertex v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = null;
            this.Center = new DVector3((v1.X + v2.X + v3.X) / 3, (v1.Y + v2.Y + v3.Y) / 3, (v1.Z + v2.Z + v3.Z) / 3);
        }
        public Polygon(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.Center = new DVector3((v1.X + v2.X + v3.X + v4.X) / 4, (v1.Y + v2.Y + v3.Y + v4.Y) / 4, (v1.Z + v2.Z + v3.Z + v4.Z) / 4);
        }

        private void CreateNormal(DVector4[] v)
        {
            DVector3 l1 = new DVector3(v[1].X - v[0].X, v[1].Y - v[0].Y, v[1].Z - v[0].Z);
            DVector3 l2 = new DVector3(v[2].X - v[0].X, v[2].Y - v[0].Y, v[2].Z - v[0].Z);
            Normal = DVector3.CrossProduct(l2, l1).Normalized();
        }

        public double CalcNormal(DVector4[] v)
        {
            CreateNormal(v);
            return DVector3.DotProduct(new DVector3(0, 0, -1), Normal);
        }
    }
}
