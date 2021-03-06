using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using CGLabPlatform;

namespace ConsoleApplication1
{
    public class Polygon
    {
        public DVector4 _Normal;
        public DVector4 Normal;
        public bool Invisible;
        public DVector4 I;
        public DVector4 L;
        public DVector3 R;
        public DVector3 E;

        public Vertex[] Vertex;

        public DVector4 intensity;


        public Polygon(DVector4 point1, DVector4 point2, DVector4 point3, DVector4 point4, DVector4 i1 = default(DVector4))
        {
            Vertex = new[] { point1, point2, point3, point4 }
                    .Select(p => new Vertex((DVector3)p)).ToArray();
            intensity = new DVector4(i1);

            DVector4 v1 = new DVector4();
            DVector4 v2 = new DVector4();
            v1 = point2 - point1;
            v2 = point3 - point2;

            _Normal = v1 * v2;
            Normal = v1 * v2;
        }

        public Polygon(DVector4 point1, DVector4 point2, DVector4 point3)
        {
            Vertex = new[] { point1, point2, point3 }
                    .Select(p => new Vertex((DVector3)p)).ToArray();

            DVector4 v1 = new DVector4();
            DVector4 v2 = new DVector4();
            v1 = point2 - point1;
            v2 = point3 - point2;

            _Normal = v1 * v2;
            Normal = v1 * v2;
        }

        public Polygon()
        {
            _Normal.X = 3;
        }
    }
}
