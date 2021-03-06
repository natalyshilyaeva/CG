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
    public class Vertex
    {
        public DVector4 _Point; // точка в локальной системе координат
        public DVector4 _Normal; // точка в локальной системе координат
        public DVector4 Normal; // точка в локальной системе координат
        public DVector4 Point; // точка в мировой\видовой сиситеме координат
        public bool IsVisble;
        public DVector4 I;
        public DVector4 L;

        public Polygon[] Polygon;

        public Vertex(DVector3 point)
        {
            _Point = new DVector4(point, 1.0);
        }
    }
}
