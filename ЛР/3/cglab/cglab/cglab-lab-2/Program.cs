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

    [DisplayNumericProperty(100, 0.1, "Высота пирамиды", 1)]
    public abstract double height { get; set; }

    [DisplayNumericProperty(75, 1, "Ребро основания", 1)]
    public abstract double rebro { get; set; }

    [DisplayNumericProperty(480, 1, "Сдвиг по X")]
    public abstract double ShiftX { get; set; }

    [DisplayNumericProperty(480, 0.1, "Сдвиг по Y")]
    public virtual double ShiftY { get; set; }

    [DisplayNumericProperty(480, 0.1, "Сдвиг по Z")]
    public virtual double ShiftZ { get; set; }

    /*[DisplayNumericProperty(0, 0.1, "Сдвиг по Z")]
    public virtual double ShiftZ { get; set; }*/

    [DisplayNumericProperty(0, 0.001, "Вокруг OX", -100)]
    public virtual double Rotx { get; set; }

    [DisplayNumericProperty(0, 0.001, "Вокруг OY", -100)]
    public virtual double Roty { get; set; }

    [DisplayNumericProperty(0, 0.001, "Вокруг OZ", -100)]
    public virtual double Rotz { get; set; }

    [DisplayNumericProperty(1, 0.5, "Масштаб X", 10)]
    public virtual double zoomx { get; set; }

    [DisplayNumericProperty(1, 0.5, "Масштаб Y", 10)]
    public virtual double zoomy { get; set; }

    [DisplayNumericProperty(1, 0.5, "Масштаб Z", 10)]
    public virtual double zoomz { get; set; }

    [DisplayNumericProperty(0, 1, "Сброс", 0, 1)]
    public virtual int reset { get; set; }

    [DisplayNumericProperty(1, 1, "Проекции", 1, 3)]
    public virtual double view { get; set; }

    [DisplayNumericProperty(0, 1, "Виды", 0, 3)]
    public virtual double view_2 { get; set; }

    [DisplayNumericProperty(0, 1, "Изометрия", 0, 1)]
    public virtual double iso { get; set; }

    [DisplayNumericProperty(new[] { 0d, 0d }, 1, "Смещение")]
    public abstract DVector2 Offset { get; set; }

    [DisplayTextBoxProperty("Лабораторная Работа 2. Скуридин А.А. М8О-304Б-17.", "Заголовок")]
    public virtual string LabelTxt
    {
        get { return _LabelTxt; }
        set
        {
            _LabelTxt = value;
        }
    }
    private string _LabelTxt;

    public List<DVector4> list1 = new List<DVector4>();//лист ключевых точек фигуры

    public List<Vertex> vlist = new List<Vertex>();

    public List<Polygon> fplist = new List<Polygon>();//лист проецированных полигонов (на плоскости)
    public List<Polygon> plist = new List<Polygon>();//лист полигонов (в пространстве)

    public List<DVector4> axis = new List<DVector4>();//лист для осей

    public void Solv(double h, double r)//Функция построения фигуры
    {
        double ro = r / (2 * Math.Cos(2 * Math.PI / 13));//сначала хитрым образом делаем основание для пирамиды
        double fi = 0;//очень хитрым
        list1.Clear();

        list1.Add(new DVector4(0, 0, h, 1));//добавляем высоту (номер 1)(индекс 0)
        DVector3 hg = new DVector3(0, 0, h);
        vlist.Add(new Vertex(hg));//заполняем лист вертексов (часть 1)

        for (int i = 1; i <= 13; i++)
        {
            list1.Add(new DVector4(ro * Math.Cos(fi), ro * Math.Sin(fi), 0, 1));
            fi = fi + (2 * Math.PI / 13);
            DVector3 dt = new DVector3(ro * Math.Cos(fi), ro * Math.Sin(fi), 0);
            vlist.Add(new Vertex(dt));//заполняем лист вертексов (часть 2)
        }
        //основание построено (первые 13 точек)(индексы 1 - 13)

        list1.Add(new DVector4(0, 0, 0, 1));//добавляем хитрую точку (проекция вершины на основание)
        DVector3 joj = new DVector3(0, 0, 0);//индекс 14
        vlist.Add(new Vertex(joj));//заполняем лист вертексов (14 точка) (часть 3)

        //Суть хитрости:
        //Одна дополнительная точка позволяет спокойно создать основание пирамиды!

        //а что, звучит хайпово)))))))))))
    }

    public void Proec_2()
    {
        fplist.Clear();

        if (view == 1)
        {
            for (int i = 0; i <= 25; i++)
            {
                DVector4 A = new DVector4(plist[i].Pointa.X, plist[i].Pointa.Y, 0, 1);
                DVector4 B = new DVector4(plist[i].Pointb.X, plist[i].Pointb.Y, 0, 1);
                DVector4 C = new DVector4(plist[i].Pointc.X, plist[i].Pointc.Y, 0, 1);
                fplist.Add(new Polygon(A, B, C));
                //хоба
            }
        }
        //все 14 полигонов спроецированы на плоскость XY

        if (view == 2)
        {
            for (int i = 0; i <= 25; i++)
            {
                DVector4 A = new DVector4(plist[i].Pointa.X, 0, plist[i].Pointa.Z, 1);//единичку приписал...
                DVector4 B = new DVector4(plist[i].Pointb.X, 0, plist[i].Pointb.Z, 1);
                DVector4 C = new DVector4(plist[i].Pointc.X, 0, plist[i].Pointc.Z, 1);
                fplist.Add(new Polygon(A, B, C));
                //хоба
            }
        }
        //все 14 полигонов спроецированы на плоскость XZ

        if (view == 3)
        {
            for (int i = 0; i <= 25; i++)
            {
                DVector4 A = new DVector4(0, plist[i].Pointa.Y, plist[i].Pointa.Z, 1);//единичку приписал...
                DVector4 B = new DVector4(0, plist[i].Pointb.Y, plist[i].Pointb.Z, 1);
                DVector4 C = new DVector4(0, plist[i].Pointc.Y, plist[i].Pointc.Z, 1);
                fplist.Add(new Polygon(A, B, C));
                //хоба
            }
        }
        //все 14 полигонов спроецированы на плоскость YZ
    }//проекции на три основные плоскости

    public void Invisible_Sides()
    {
        List<Polygon> plistt = new List<Polygon>();
        //plistt.Clear();
        DVector4 zero = new DVector4(0, 0, 0, 1);
        double cos = 0;

        for (int i = 0; i <= plist.Count - 1; i++)
        {
            //DVector4 normal = new DVector4(plist[i].Pointa.Y*plist[i].Pointc.Z - plist[i].Pointb.Y * plist[i].Pointc.Z - plist[i].Pointa.Y * plist[i].Pointb.Z - plist[i].Pointc.Y * plist[i].Pointa.Z + plist[i].Pointc.Y * plist[i].Pointb.Z + plist[i].Pointb.Y * plist[i].Pointa.Z, plist[i].Pointa.Z* plist[i].Pointc.X - plist[i].Pointb.Z * plist[i].Pointc.X - plist[i].Pointa.Z * plist[i].Pointb.X - plist[i].Pointc.Z * plist[i].Pointa.X + plist[i].Pointc.Z * plist[i].Pointb.X + plist[i].Pointb.Z * plist[i].Pointa.X, plist[i].Pointa.X * plist[i].Pointc.Y - plist[i].Pointb.X * plist[i].Pointc.Y - plist[i].Pointa.X * plist[i].Pointb.Y - plist[i].Pointc.X * plist[i].Pointa.Y + plist[i].Pointc.X * plist[i].Pointb.Y + plist[i].Pointb.X * plist[i].Pointa.Y,1);
            DVector4 v1 = new DVector4(plist[i].Pointa.X - plist[i].Pointb.X, plist[i].Pointa.Y - plist[i].Pointb.Y, plist[i].Pointa.Z - plist[i].Pointb.Z, 1);
            DVector4 v2 = new DVector4(plist[i].Pointa.X - plist[i].Pointc.X, plist[i].Pointa.Y - plist[i].Pointc.Y, plist[i].Pointa.Z - plist[i].Pointc.Z, 1);
            DVector4 normal = new DVector4();
            normal = v2 * v1;

            if (view == 1)
            {
                DVector4 look = new DVector4(0, 0, -1, 1);
                cos = (look.X * normal.X + look.Y * normal.Y + look.Z * normal.Z) / (Math.Sqrt((look.X) * (look.X) + (look.Y) * (look.Y) + (look.Z) * (look.Z)) + Math.Sqrt((normal.X) * (normal.X) + (normal.Y) * (normal.Y) + (normal.Z) * (normal.Z)));
            }
            if (view == 2)
            {
                DVector4 look = new DVector4(0, -1, 0, 1);
                cos = (look.X * normal.X + look.Y * normal.Y + look.Z * normal.Z) / (Math.Sqrt((look.X) * (look.X) + (look.Y) * (look.Y) + (look.Z) * (look.Z)) + Math.Sqrt((normal.X) * (normal.X) + (normal.Y) * (normal.Y) + (normal.Z) * (normal.Z)));
            }
            if (view == 3)
            {
                DVector4 look = new DVector4(-1, 0, 0, 1);
                cos = (look.X * normal.X + look.Y * normal.Y + look.Z * normal.Z) / (Math.Sqrt((look.X) * (look.X) + (look.Y) * (look.Y) + (look.Z) * (look.Z)) + Math.Sqrt((normal.X) * (normal.X) + (normal.Y) * (normal.Y) + (normal.Z) * (normal.Z)));
            }

            if (cos <= 0)
            {
                plistt.Add(new Polygon(plist[i].Pointa, plist[i].Pointb, plist[i].Pointc));
            }
            else
            {
                plistt.Add(new Polygon(zero, zero, zero));
            }
        }
        plist = plistt;
    }//хм

    public void Pcreate()
    {
        plist.Clear();//очищаем лист от старых точек
        for (int i = 0; i <= 11; i++)
        {
            plist.Add(new Polygon(list1[0], list1[i + 1], list1[i + 2]));//полигоны боковые кроме одного
        }
        plist.Add(new Polygon(list1[0], list1[13], list1[1]));//последний полигон боковой

        for (int i = 0; i <= 11; i++)
        {
            //plist.Add(new Polygon(list1[14], list1[i + 1], list1[i + 2]));//полигоны основания (не все)
            plist.Add(new Polygon(list1[i + 1], list1[14], list1[i + 2]));
        }
        //plist.Add(new Polygon(list1[14], list1[13], list1[1]));//
        plist.Add(new Polygon(list1[13], list1[14], list1[1]));
    }

    public void Rotatex()
    {
        List<DVector4> list1t = new List<DVector4>();
        for (int i = 0; i <= 14; i++)
        {
            double xt;
            double yt;
            double zt;

            xt = list1[i].X;
            yt = list1[i].Y;
            zt = list1[i].Z;

            list1t.Add(new DVector4(xt, yt * Math.Cos(Rotx) - zt * Math.Sin(Rotx), yt * Math.Sin(Rotx) + zt * Math.Cos(Rotx), 0));
        }
        list1 = list1t;
    }

    public void Rotatey()
    {
        List<DVector4> list1t = new List<DVector4>();
        for (int i = 0; i <= 14; i++)
        {
            double xt;
            double yt;
            double zt;

            xt = list1[i].X;
            yt = list1[i].Y;
            zt = list1[i].Z;

            list1t.Add(new DVector4(zt * Math.Sin(Roty) + xt * Math.Cos(Roty), yt, zt * Math.Cos(Roty) - xt * Math.Sin(Roty), 0));
        }
        list1 = list1t;
    }
    public void Rotatez()
    {
        List<DVector4> list1t = new List<DVector4>();
        for (int i = 0; i <= 14; i++)
        {
            double xt;
            double yt;
            double zt;

            xt = list1[i].X;
            yt = list1[i].Y;
            zt = list1[i].Z;

            list1t.Add(new DVector4(xt * Math.Cos(Rotz) - yt * Math.Sin(Rotz), xt * Math.Sin(Rotz) + yt * Math.Cos(Rotz), zt, 0));
        }
        list1 = list1t;
    }

    public void Rotate_True()
    {

        if (reset == 1)
        {
            height = 100;
            rebro = 75;
            ShiftX = 480;
            ShiftY = 320;
            Rotx = 0;
            Roty = 0;
            Rotz = 0;
            zoomx = 0;
            zoomy = 0;
            zoomz = 0;
            view = 1;
        }
        axis.Clear();
        DMatrix4 rtx = new DMatrix4(1, 0, 0, 0, 0, Math.Cos(Rotx), -Math.Sin(Rotx), 0, 0, Math.Sin(Rotx), Math.Cos(Rotx), 0, 0, 0, 0, 1);
        DMatrix4 rty = new DMatrix4(Math.Cos(Roty), 0, Math.Sin(Roty), 0, 0, 1, 0, 0, -Math.Sin(Roty), 0, Math.Cos(Roty), 0, 0, 0, 0, 1);
        DMatrix4 rtz = new DMatrix4(Math.Cos(Rotz), -Math.Sin(Rotz), 0, 0, Math.Sin(Rotz), Math.Cos(Rotz), 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        DMatrix4 dotx = new DMatrix4(25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1);
        DMatrix4 doty = new DMatrix4(0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1);
        DMatrix4 dotz = new DMatrix4(0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 1, 0, 0, 1);
        DMatrix4 res1 = new DMatrix4();
        DMatrix4 res2 = new DMatrix4();
        DMatrix4 res3 = new DMatrix4();
        res1 = rtx * dotx;
        res1 = rty * res1;
        res1 = rtz * res1;
        axis.Add(new DVector4(0, 0, 0, 1));//первая точка начала координат
        axis.Add(new DVector4(res1.M11, res1.M21, res1.M31, 1));

        res2 = rtx * doty;
        res2 = rty * res2;
        res2 = rtz * res2;
        axis.Add(new DVector4(res2.M11, res2.M21, res2.M31, 1));

        res3 = rtx * dotz;
        res3 = rty * res3;
        res3 = rtz * res3;
        axis.Add(new DVector4(res3.M11, res3.M21, res3.M31, 1));
    }

    public void Zoomzoom()
    {
        List<DVector4> list1t = new List<DVector4>();
        for (int i = 0; i <= 14; i++)
        {
            DMatrix4 zoom = new DMatrix4(zoomx, 0, 0, 0, 0, zoomy, 0, 0, 0, 0, zoomz, 0, 0, 0, 0, 1);
            DMatrix4 dot = new DMatrix4(list1[i].X, 0, 0, 0, list1[i].Y, 0, 0, 0, list1[i].Z, 0, 0, 0, 1, 0, 0, 0);
            DMatrix4 res = new DMatrix4();
            res = zoom * dot;
            list1t.Add(new DVector4(res.M11, res.M21, res.M31, res.M41));
        }
        list1 = list1t;
    }
    public void view_2_f()
    {
        List<DVector4> list1t = new List<DVector4>();
        if (view_2 == 1)
        {
            for (int i = 0; i <= 14; i++)
            {
                DMatrix4 zoom = new DMatrix4(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
                DMatrix4 dot = new DMatrix4(list1[i].X, 0, 0, 0, list1[i].Y, 0, 0, 0, list1[i].Z, 0, 0, 0, 1, 0, 0, 0);
                DMatrix4 res = new DMatrix4();
                res = zoom * dot;
                list1t.Add(new DVector4(res.M11, res.M21, res.M31, res.M41));
            }
        }

        if (view_2 == 2)
        {
            for (int i = 0; i <= 14; i++)
            {
                DMatrix4 zoom = new DMatrix4(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
                DMatrix4 dot = new DMatrix4(list1[i].X, 0, 0, 0, list1[i].Y, 0, 0, 0, list1[i].Z, 0, 0, 0, 1, 0, 0, 0);
                DMatrix4 res = new DMatrix4();
                res = zoom * dot;
                list1t.Add(new DVector4(res.M11, res.M21, res.M31, res.M41));
            }
        }

        if (view_2 == 3)
        {
            for (int i = 0; i <= 14; i++)
            {
                DMatrix4 zoom = new DMatrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
                DMatrix4 dot = new DMatrix4(list1[i].X, 0, 0, 0, list1[i].Y, 0, 0, 0, list1[i].Z, 0, 0, 0, 1, 0, 0, 0);
                DMatrix4 res = new DMatrix4();
                res = zoom * dot;
                list1t.Add(new DVector4(res.M11, res.M21, res.M31, res.M41));
            }
        }

        list1 = list1t;
    }
    public void isometry()
    {
        List<DVector4> list1t = new List<DVector4>();
        for (int i = 0; i <= 14; i++)
        {
            DMatrix4 rtx = new DMatrix4(1, 0, 0, 0, 0, Math.Cos(0.610865), -Math.Sin(0.610865), 0, 0, Math.Sin(0.610865), Math.Cos(0.610865), 0, 0, 0, 0, 1);
            DMatrix4 rty = new DMatrix4(Math.Cos(0.785398), 0, Math.Sin(0.785398), 0, 0, 1, 0, 0, -Math.Sin(0.785398), 0, Math.Cos(0.785398), 0, 0, 0, 0, 1);
            DMatrix4 dot = new DMatrix4(list1[i].X, 0, 0, 0, list1[i].Y, 0, 0, 0, list1[i].Z, 0, 0, 0, 1, 0, 0, 1);
            DMatrix4 res1 = new DMatrix4();
            res1 = rtx * dot;
            res1 = rty * res1;
            list1t.Add(new DVector4(res1.M11, res1.M21, res1.M31, 1));
        }
        list1 = list1t;
    }


    public ModelBuilder Buildr()//void
    {
        ModelBuilder MB = new ModelBuilder();
        DVector3 a = new DVector3();//координата 1
        DVector3 b = new DVector3();//координата 2
        DVector3 c = new DVector3();//координата 3

        for (int i = 0; i <= 11; i++)
        {
            a.X = list1[0].X;
            a.Y = list1[0].Y;
            a.Z = list1[0].Z;

            b.X = list1[i + 1].X;
            b.Y = list1[i + 1].Y;
            b.Z = list1[i + 1].Z;

            c.X = list1[i + 2].X;
            c.Y = list1[i + 2].Y;
            c.Z = list1[i + 2].Z;

            MB.AddPolygon(a, b, c);
        }

        a.X = list1[0].X;
        a.Y = list1[0].Y;
        a.Z = list1[0].Z;

        b.X = list1[13].X;
        b.Y = list1[13].Y;
        b.Z = list1[13].Z;

        c.X = list1[1].X;
        c.Y = list1[1].Y;
        c.Z = list1[1].Z;//боковые грани пирамиды готовы

        MB.AddPolygon(a, b, c);//боковые грани пирамиды готовы

        //теперь основание пирамиды

        for (int i = 0; i <= 11; i++)
        {
            a.X = list1[14].X;
            a.Y = list1[14].Y;
            a.Z = list1[14].Z;

            b.X = list1[i + 1].X;
            b.Y = list1[i + 1].Y;
            b.Z = list1[i + 1].Z;

            c.X = list1[i + 2].X;
            c.Y = list1[i + 2].Y;
            c.Z = list1[i + 2].Z;

            MB.AddPolygon(a, b, c);
        }

        a.X = list1[14].X;
        a.Y = list1[14].Y;
        a.Z = list1[14].Z;

        b.X = list1[13].X;
        b.Y = list1[13].Y;
        b.Z = list1[13].Z;

        c.X = list1[1].X;
        c.Y = list1[1].Y;
        c.Z = list1[1].Z;

        MB.AddPolygon(a, b, c);//основание пирамиды готово!

        return MB;
    }
    /*public ModelBuilder Buildr_2()//void
    {
        ModelBuilder MB = new ModelBuilder();
        DVector3 a = new DVector3();//точка 1
        DVector3 b = new DVector3();//точка 2
        DVector3 c = new DVector3();//точка 3

        for (int i = 0; i <= 11; i++)
        {
            a.X = flist[0].X;
            a.Y = flist[0].Y;
            a.Z = 0;

            b.X = flist[i + 1].X;
            b.Y = flist[i + 1].Y;
            b.Z = 0;

            c.X = flist[i + 2].X;
            c.Y = flist[i + 2].Y;
            c.Z = 0;

            MB.AddPolygon(a, b, c);
        }

        a.X = list1[0].X;
        a.Y = list1[0].Y;
        a.Z = list1[0].Z;

        b.X = list1[13].X;
        b.Y = list1[13].Y;
        b.Z = list1[13].Z;

        c.X = list1[1].X;
        c.Y = list1[1].Y;
        c.Z = list1[1].Z;//боковые грани пирамиды готовы

        MB.AddPolygon(a, b, c);//боковые грани пирамиды готовы

        //теперь основание пирамиды

        for (int i = 0; i <= 11; i++)
        {
            a.X = list1[14].X;
            a.Y = list1[14].Y;
            a.Z = list1[14].Z;

            b.X = list1[i + 1].X;
            b.Y = list1[i + 1].Y;
            b.Z = list1[i + 1].Z;

            c.X = list1[i + 2].X;
            c.Y = list1[i + 2].Y;
            c.Z = list1[i + 2].Z;

            MB.AddPolygon(a, b, c);
        }

        a.X = list1[14].X;
        a.Y = list1[14].Y;
        a.Z = list1[14].Z;

        b.X = list1[13].X;
        b.Y = list1[13].Y;
        b.Z = list1[13].Z;

        c.X = list1[1].X;
        c.Y = list1[1].Y;
        c.Z = list1[1].Z;

        MB.AddPolygon(a, b, c);//основание пирамиды готово!

        return MB;
    }*/

    [STAThread]
    static void Main()
    {
        RunApplication();
    }

    public class Vertex
    {
        public DVector4 _Point; // точка в локальной системе координат
        public DVector4 Point; // точка в мировой\видовой сиситеме координат

        public Polygon[] Polygon;

        public Vertex(DVector3 point)
        {
            _Point = new DVector4(point, 1.0);
        }
    }

    public class Polygon
    {
        public DVector4 _Normal;
        public DVector4 Normal;

        public Vertex[] Vertex;
        public DVector4 Pointa;
        public DVector4 Pointb;
        public DVector4 Pointc;

        public int Color;

        public Polygon(DVector4 point1, DVector4 point2, DVector4 point3)
        {
            Pointa = point1;
            Pointb = point2;
            Pointc = point3;
        }

        public Polygon()
        {
        }
    }

    public Vertex[] Vertecis;
    public Polygon[] Polygons;

    public class ModelBuilder
    {
        public List<DVector3> vertex = new List<DVector3>();
        public List<List<int>> polyvert = new List<List<int>>();//список вершин по полигонам
        public List<List<int>> vertpoly = new List<List<int>>();//список полигонов по вершинам

        public void AddPolygon(DVector3 point1, DVector3 point2, DVector3 point3, params DVector3[] points)
        {
            AddPolygon(new[] { point1, point2, point3 }.Concat(points).ToArray());
        }

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
            for (int v = _Vertex.Length; 0 != v--;)
            {
                _Vertex[v].Polygon = vertpoly[v].Select(p => _Polygon[p]).ToArray();
            }
            Vertex = _Vertex;
            Polygon = _Polygon;
        }
    }

    protected override void OnMainWindowLoad(object sender, EventArgs args)
    {
        // Solv(0, 0);
        base.RenderDevice.BufferBackCol = 0xB2;
        base.ValueStorage.Font = new Font("Arial", 12f);
        base.ValueStorage.ForeColor = Color.FloralWhite;
        base.ValueStorage.RowHeight = 30;
        base.ValueStorage.BackColor = Color.BlueViolet;
        base.MainWindow.BackColor = Color.DarkMagenta;
        base.ValueStorage.RightColWidth = 50;
        base.VSPanelWidth = 300;
        base.VSPanelLeft = true;
        base.MainWindow.Size = new Size(960, 640);

        base.RenderDevice.MouseMoveWithRightBtnDown += (s, e) => {//смещение по осям
            Rotx += 0.01 * e.MovDeltaX;
            Roty += 0.01 * e.MovDeltaY;
        };
        base.RenderDevice.MouseMoveWithLeftBtnDown += (s, e) => {//вращение по осям OX и OY
            ShiftX += 2 * e.MovDeltaX;
            ShiftY += 2 * e.MovDeltaY;
        };

        base.RenderDevice.MouseMoveWithMiddleBtnDown += (s, e) => {//вращение по оси OZ
            Rotz += 0.01 * e.MovDeltaX;//двигаем по оси X (по столу), фигура вращается вокруг OZ
        };

        //Горячие клавиши из фреймворка
        RenderDevice.HotkeyRegister(Keys.Up, (s, e) => ++ShiftY);
        RenderDevice.HotkeyRegister(Keys.Down, (s, e) => --ShiftY);
        RenderDevice.HotkeyRegister(Keys.Left, (s, e) => --ShiftX);
        RenderDevice.HotkeyRegister(Keys.Right, (s, e) => ++ShiftX);
        RenderDevice.HotkeyRegister(KeyMod.Shift, Keys.Up, (s, e) => ShiftY += 10);
        RenderDevice.HotkeyRegister(KeyMod.Shift, Keys.Down, (s, e) => ShiftY -= 10);
        RenderDevice.HotkeyRegister(KeyMod.Shift, Keys.Left, (s, e) => ShiftX -= 10);
        RenderDevice.HotkeyRegister(KeyMod.Shift, Keys.Right, (s, e) => ShiftX += 10);
    }

    protected override void OnDeviceUpdate(object s, GDIDeviceUpdateArgs e)
    {
        //ShiftX = e.Width / 2;
        // ShiftY = e.Heigh / 2;

        Solv(height, rebro);//находим координаты всех точек фигуры

        if (view_2 == 0)
        {
            Zoomzoom();//масштаб
        }
        else
        {
            Zoomzoom();//масштаб
            view_2_f();
        }

        if (iso == 1)
            isometry();

        Rotatex();//поворот вокруг оси OX
        Rotatey();//поворот вокруг оси OY
        Rotatez();//поворот вокрун оси OZ

        Rotate_True();

        Pcreate();//заполняем лист полигонов (в трехмерном пространстве)
        Invisible_Sides();
        Proec_2();//проецируем полигоны на плоскость

        double a = e.Width / 960;
        double b = e.Heigh / 640;
        if (a < b)
        {
            b = a;
        }
        else
        {
            a = b;
        }

        if (view == 1)
        {
            for (int i = 0; i <= fplist.Count - 1; i++)
            {
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointa.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointa.Y * a + ShiftY - ShiftY * (1 - b), fplist[i].Pointb.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointb.Y * a + ShiftY - ShiftY * (1 - b));
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointb.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointb.Y * a + ShiftY - ShiftY * (1 - b), fplist[i].Pointc.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointc.Y * a + ShiftY - ShiftY * (1 - b));
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointc.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointc.Y * a + ShiftY - ShiftY * (1 - b), fplist[i].Pointa.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointa.Y * a + ShiftY - ShiftY * (1 - b));
                //пробегаемся по полигонам и соединяем их точки линиями
            }//axis, 0 - точка x, 1 - точка y, 2 - точка z

            e.Graphics.DrawLine(Pens.Green, axis[0].X + 100, axis[0].Y + 500, axis[1].X + 100, axis[1].Y + 500);
            e.Graphics.DrawLine(Pens.Blue, axis[0].X + 100, axis[0].Y + 500, axis[2].X + 100, axis[2].Y + 500);
            e.Graphics.DrawLine(Pens.Red, axis[0].X + 100, axis[0].Y + 500, axis[3].X + 100, axis[3].Y + 500);
            e.Graphics.DrawString("Плоскость XY", new Font("Arial", 13f, FontStyle.Bold), Brushes.DarkRed, new PointF(100, 20));
            e.Graphics.DrawString("Ось X", new Font("Arial", 10f, FontStyle.Bold), Brushes.Green, new PointF(100, 40));
            e.Graphics.DrawString("Ось Y", new Font("Arial", 10f, FontStyle.Bold), Brushes.Blue, new PointF(100, 60));
            e.Graphics.DrawString("Ось Z", new Font("Arial", 10f, FontStyle.Bold), Brushes.Red, new PointF(100, 80));
        }//XY

        if (view == 2)
        {
            for (int i = 0; i <= fplist.Count - 1; i++)
            {
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointa.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointa.Z * a + ShiftZ - ShiftZ * (1 - b), fplist[i].Pointb.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointb.Z * a + ShiftZ - ShiftZ * (1 - b));
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointb.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointb.Z * a + ShiftZ - ShiftZ * (1 - b), fplist[i].Pointc.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointc.Z * a + ShiftZ - ShiftZ * (1 - b));
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointc.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointc.Z * a + ShiftZ - ShiftZ * (1 - b), fplist[i].Pointa.X * a + ShiftX - ShiftX * (1 - a), fplist[i].Pointa.Z * a + ShiftZ - ShiftZ * (1 - b));
                //пробегаемся по полигонам и соединяем их точки линиями
            }

            e.Graphics.DrawLine(Pens.Green, axis[0].X + 100, axis[0].Z + 500, axis[1].X + 100, axis[1].Z + 500);
            e.Graphics.DrawLine(Pens.Blue, axis[0].X + 100, axis[0].Z + 500, axis[2].X + 100, axis[2].Z + 500);
            e.Graphics.DrawLine(Pens.Red, axis[0].X + 100, axis[0].Z + 500, axis[3].X + 100, axis[3].Z + 500);
            e.Graphics.DrawString("Плоскость XZ", new Font("Arial", 13f, FontStyle.Bold), Brushes.DarkRed, new PointF(100, 20));
            e.Graphics.DrawString("Ось X", new Font("Arial", 10f, FontStyle.Bold), Brushes.Green, new PointF(100, 40));
            e.Graphics.DrawString("Ось Y", new Font("Arial", 10f, FontStyle.Bold), Brushes.Blue, new PointF(100, 60));
            e.Graphics.DrawString("Ось Z", new Font("Arial", 10f, FontStyle.Bold), Brushes.Red, new PointF(100, 80));

        }//XZ

        if (view == 3)
        {
            for (int i = 0; i <= fplist.Count - 1; i++)
            {
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointa.Y * a + ShiftY - ShiftY * (1 - a), fplist[i].Pointa.Z * a + ShiftZ - ShiftZ * (1 - b), fplist[i].Pointb.Y * a + ShiftY - ShiftY * (1 - a), fplist[i].Pointb.Z * a + ShiftZ - ShiftZ * (1 - b));
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointb.Y * a + ShiftY - ShiftY * (1 - a), fplist[i].Pointb.Z * a + ShiftZ - ShiftZ * (1 - b), fplist[i].Pointc.Y * a + ShiftY - ShiftY * (1 - a), fplist[i].Pointc.Z * a + ShiftZ - ShiftZ * (1 - b));
                e.Graphics.DrawLine(Pens.White, fplist[i].Pointc.Y * a + ShiftY - ShiftY * (1 - a), fplist[i].Pointc.Z * a + ShiftZ - ShiftZ * (1 - b), fplist[i].Pointa.Y * a + ShiftY - ShiftY * (1 - a), fplist[i].Pointa.Z * a + ShiftZ - ShiftZ * (1 - b));
                //пробегаемся по полигонам и соединяем их точки линиями
            }

            e.Graphics.DrawLine(Pens.Green, axis[0].Y + 100, axis[0].Z + 500, axis[1].Y + 100, axis[1].Z + 500);
            e.Graphics.DrawLine(Pens.Blue, axis[0].Y + 100, axis[0].Z + 500, axis[2].Y + 100, axis[2].Z + 500);
            e.Graphics.DrawLine(Pens.Red, axis[0].Y + 100, axis[0].Z + 500, axis[3].Y + 100, axis[3].Z + 500);
            e.Graphics.DrawString("Плоскость YZ", new Font("Arial", 13f, FontStyle.Bold), Brushes.DarkRed, new PointF(100, 20));
            e.Graphics.DrawString("Ось X", new Font("Arial", 10f, FontStyle.Bold), Brushes.Green, new PointF(100, 40));
            e.Graphics.DrawString("Ось Y", new Font("Arial", 10f, FontStyle.Bold), Brushes.Blue, new PointF(100, 60));
            e.Graphics.DrawString("Ось Z", new Font("Arial", 10f, FontStyle.Bold), Brushes.Red, new PointF(100, 80));
        }//YZ

        if (view == 4)//прикол
        {
            e.Graphics.DrawLine(Pens.White, new Point(-50 + 300, -50 + 200), new Point(-50 + 300, 0 + 200));
            e.Graphics.DrawLine(Pens.White, new Point(-50 + 300, 0 + 200), new Point(0 + 300, 0 + 200));
            e.Graphics.DrawLine(Pens.White, new Point(0 + 300, 0 + 200), new Point(0 + 300, -50 + 200));
            e.Graphics.DrawLine(Pens.White, new Point(0 + 300, -50 + 200), new Point(-50 + 300, -50 + 200));
            e.Graphics.DrawLine(Pens.White, new Point(-40 + 300, -40 + 200), new Point(-40 + 300, -10 + 200));
        }
        //ModelBuilder A = Buildr();
        //Polygons.QuickSort(p => p.Vertex.Average(v => v.Point.Z));
    }

}

