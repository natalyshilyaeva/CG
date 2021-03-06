using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using CGLabPlatform;

public abstract class CGLabEmpty : GFXApplicationTemplate<CGLabEmpty>
{
    [STAThread] static void Main() { RunApplication(); }

    public class Vertex
    {
        public DVector4 _Point; // точка в локальной системе координат
        public DVector4  Point; // точка в мировой\видовой сиситеме координат

        public Polygon[] Polygon;

        public Vertex(DVector3 point)
        {
            _Point = new DVector4(point, 1.0);
        }
    }

    public class Polygon
    {
        public DVector4 _Normal; 
        public DVector4  Normal;

        public Vertex[] Vertex;

        public int Color;
    }

    public Vertex[]  Vertecis;
    public Polygon[] Polygons;

    public class ModelBuilder
    {
        private List<DVector3> vertex = new List<DVector3>();
        private List<List<int>> polyvert = new List<List<int>>();
        private List<List<int>> vertpoly = new List<List<int>>();

        public void AddPolygon(DVector3[] points)
        {
            polyvert.Add(new List<int>());
            for (int i = 0; i < points.Length; ++i)
            {
                int _v = vertex.FindLastIndex(v =>
                    DVector3.ApproxEqual(v, points[i]));
                if (_v < 0)
                {
                    _v = vertex.Count;
                    vertex.Add(points[i]);
                    vertpoly.Add(new List<int>());
                }
                polyvert.Last().Add(_v);
                vertpoly[_v].Add(polyvert.Count - 1);
            }
        }

        public void Compile(out Vertex[] Vertex, out Polygon[] Polygon)
        {
            var _Vertex = vertex.Select(v => new Vertex(v)).ToArray();
            var _Polygon = polyvert.Select(p => new Polygon()
            {
                Vertex = p.Select(v => _Vertex[v]).ToArray()
            }).ToArray();
            for (int v = _Vertex.Length; 0 != v--; )
            {
                _Vertex[v].Polygon = vertpoly[v].Select(p => _Polygon[p]).ToArray();
            }
            Vertex = _Vertex;
            Polygon = _Polygon;

        }
    }


    protected override void OnMainWindowLoad(object sender, EventArgs args)
    {
    }

    protected override void OnDeviceUpdate(object s, GDIDeviceUpdateArgs e)
    {
        Polygons.QuickSort(p => p.Vertex.Average(v => v.Point.Z));

    }

}
