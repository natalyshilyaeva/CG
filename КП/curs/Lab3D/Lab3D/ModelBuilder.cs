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
    public class ModelBuilder
    {
        //private List<DVector3> vertex = new List<DVector3>();
        //private List<List<int>> polyvert = new List<List<int>>();
        //private List<List<int>> vertpoly = new List<List<int>>();

        //public void AddPolygon(DVector3[] points)
        //{
        //    polyvert.Add(new List<int>());
        //    for (int i = 0; i < points.Length; ++i)
        //    {
        //        int _v = vertex.FindLastIndex(v =>
        //            DVector3.ApproxEqual(v, points[i]));
        //        if (_v < 0)
        //        {
        //            _v = vertex.Count;
        //            vertex.Add(points[i]);
        //            vertpoly.Add(new List<int>());
        //        }
        //        polyvert.Last().Add(_v);
        //        vertpoly[_v].Add(polyvert.Count - 1);
        //    }
        //}

        //public void Compile(out Vertex[] Vertex, out Polygon[] Polygon)
        //{
        //    var _Vertex = vertex.Select(v => new Vertex(v)).ToArray();
        //    var _Polygon = polyvert.Select(p => new Polygon()
        //    {
        //        Vertex = p.Select(v => _Vertex[v]).ToArray()
        //    }).ToArray();
        //    for (int v = _Vertex.Length; 0 != v--; )
        //    {
        //        _Vertex[v].Polygon = vertpoly[v].Select(p => _Polygon[p]).ToArray();
        //    }
        //    Vertex = _Vertex;
        //    Polygon = _Polygon;

        //}
    }
}
