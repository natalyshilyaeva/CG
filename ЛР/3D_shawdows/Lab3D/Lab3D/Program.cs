using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using CGLabPlatform;

namespace Lab3D
{
    public abstract class CGLab3D : GFXApplicationTemplate<CGLab3D>
    {
        public List<Vertex> Vertexies = new List<Vertex>();
        public List<Polygon> Polygons = new List<Polygon>();

        [DisplayTextBoxProperty("Лабораторная №3. Шиляева Наталья", "Заголовок")]
        public virtual string LabelTxt
        {
            get { return _LabelTxt; }
            set { _LabelTxt = value; }
        }
        private string _LabelTxt;

        [STAThread] static void Main() { RunApplication(); }

        [DisplayNumericProperty(1, 0.1, "Изменить размер", 1)]
        public virtual double Size { get; set; }

        [DisplayNumericProperty(2, 0.1, "Высота", 1)]
        public virtual double Height { get; set; }

        [DisplayNumericProperty(1, 0.1, "Параметр a", 1)]
        public virtual double a { get; set; }

        [DisplayNumericProperty(1, 0.1, "Параметр b", 1)]
        public virtual double b { get; set; }

        [DisplayNumericProperty(20, 1, "Аппроксимация", 3)]
        public virtual int Apr { get; set; }

        [DisplayNumericProperty(0, 0.1, "Сдвиг по X")]
        public abstract int ShiftX { get; set; }

        [DisplayNumericProperty(0, 0.1, "Сдвиг по Y")]
        public virtual double ShiftY { get; set; }

        [DisplayNumericProperty(0, 0.1, "Сдвиг по Z")]
        public virtual double ShiftZ { get; set; }

        [DisplayNumericProperty(new double[] { 0.0, 0.0, 1.0 }, 0.01, "Параметр Ka", 0, 1)]
        public virtual DVector3 Ka { get; set; }

        [DisplayNumericProperty(new double[] { 0, 1.0, 0.0 }, 0.01, "Параметр Kd", 0, 1)]
        public virtual DVector3 Kd { get; set; }

        [DisplayNumericProperty(new double[] { 1.0, 0.0, 0.0 }, 0.01, "Параметр Ks", 0, 1)]
        public virtual DVector3 Ks { get; set; }

        [DisplayNumericProperty(new double[] { 0, 0, 0 }, 1, "Координаты света")]
        public virtual DVector3 Lighting { get; set; }

        [DisplayCheckerProperty(false, "Включить вращение")]
        public bool EnableRot
        {
            get { return _EnableRot; }                           
            set
            {
                _EnableRot = value;
                base.OnPropertyChanged();  
            }                               
        }                                  
        private bool _EnableRot;

        [DisplayEnumListProperty(View.NV, "Вид")]
        public View view
        {
            get { return Get<View>(); }
            set { if (!Set<View>(value)) return; }
        }

        public enum View
        {
            [Description("Не задан")] NV,
            [Description("Вид сбоку")] SV,
            [Description("Вид спереди")] FV,
            [Description("Вид сверху")] UV,
            [Description("Изометрия")] IZ
        }

        [DisplayEnumListProperty(Draw.Flat, "Закраска")]
        public Draw draw
        {
            get { return Get<Draw>(); }
            set { if (!Set<Draw>(value)) return; }
        }

        public enum Draw
        {
            [Description("Не задан")] ND,
            [Description("Плоская")] Flat,
            [Description("Гуро")] Gouro,
        }

        public double angleX = 0;
        public double angleY = 0;
        public double angleZ = 0;

        public DVector3 I1, I2, I3, I4;

        protected override void OnMainWindowLoad(object sender, EventArgs args)
        {
            base.RenderDevice.BufferBackCol = 0xB0;
            base.ValueStorage.Font = new Font("Arial", 12f);
            base.ValueStorage.ForeColor = Color.Firebrick;
            base.ValueStorage.RowHeight = 30;
            base.ValueStorage.BackColor = Color.BlanchedAlmond;
            base.MainWindow.BackColor = Color.DarkGoldenrod;
            base.ValueStorage.RightColWidth = 50;
            base.VSPanelWidth = 300;
            base.VSPanelLeft = true;
            base.MainWindow.Size = new Size(960, 640);

            base.RenderDevice.MouseMoveWithRightBtnDown += (s, e) =>
            {
                ShiftX += e.MovDeltaX;
                ShiftY += e.MovDeltaY;
                ShiftZ += e.MovDeltaX;
            };
            base.RenderDevice.MouseMoveWithLeftBtnDown += (s, e) =>
            {
                angleX += e.MovDeltaX;
                angleY += e.MovDeltaY;
                angleZ += e.MovDeltaX; 
            };
        }

        public void CreateFigure(double h, double a, double b)
        {
            Vertex v0up = new Vertex(0, 0, h / 2);
            Vertex v0down = new Vertex(0, 0, -h / 2);

            double fi = 0;
            double x, y;
            double x1, y1;
            double stepfi = (2 * Math.PI) / Apr;
            double step = h / Apr;

            Vertexies.Clear();
            Polygons.Clear();

            Vertexies.Add(v0up);    // 0 index
            Vertexies.Add(v0down);  // 1 index

            int l = 0;
            int i = 1;
            while (fi < 2 * Math.PI)
            {
                l = 0;

                x = a * Math.Cos(fi);
                y = b * Math.Sin(fi);
                i++;
                Vertexies.Add(new Vertex(x, y, (h / 2) - l * step));

                x1 = a * Math.Cos(fi + stepfi);
                y1 = b * Math.Sin(fi + stepfi);
                Vertexies.Add(new Vertex(x1, y1, (h / 2) - l * step));
                i++;

                Polygons.Add(new Polygon(Vertexies[0], Vertexies[i], Vertexies[i - 1]));

                for (int j = 0; j < Apr; j++)
                {
                    l++;
                    Vertexies.Add(new Vertex(x, y, (h / 2) - l * step));
                    Vertexies.Add(new Vertex(x1, y1, (h / 2) - l * step));
                    i += 2;
                    Polygons.Add(new Polygon(Vertexies[i - 3], Vertexies[i - 2], Vertexies[i], Vertexies[i - 1]));                    
                }

                x = a * Math.Cos(fi);
                y = b * Math.Sin(fi);
                Vertexies.Add(new Vertex(x, y, (h / 2) - l * step));
                i++;

                x1 = a * Math.Cos(fi + stepfi);
                y1 = b * Math.Sin(fi + stepfi);
                Vertexies.Add(new Vertex(x1, y1, (h / 2) - l * step));
                i++;

                Polygons.Add(new Polygon(Vertexies[i], Vertexies[1], Vertexies[i - 1]));
                fi += stepfi;    
            }
        }

        public void Rotation()
        {
            if (EnableRot)
            {
                angleX += 0.1;
                angleY += 0.1;
                angleZ += 0.1;
            }
        }

        public DVector3 In(GDIDeviceUpdateArgs e, Polygon p, DVector3 ka, DVector3 kd, DVector3 ks, DVector3 light, DMatrix4 m, int v)
        {
            DVector3 Ia, Id, Is, I = new DVector3();
            double LN, RS;
            double d = 1;

            var v1 = p.v1;
            var v2 = p.v2;
            var v3 = p.v3;
            DVector3 l = new DVector3(light - p.Center);
            l.Normalize();
            DVector3 Normal = p.Normal;
            p.Eye = new DVector3(e.Width / 2, e.Heigh / 2, -100000000000000).Normalized();
            p.R = -l.Reflect(Normal);
            p.R.Normalize();
            Ia = ka;
            Id = kd * l.DotProduct(Normal);
            Is = ks * p.Eye.DotProduct(p.R);

            if (Ia.X < 0)
                Ia.X = 0;
            if (Ia.Y < 0)
                Ia.Y = 0;
            if (Ia.Z < 0)
                Ia.Z = 0;
            if (Ia.X > 1)
                Ia.X = 1;
            if (Ia.Y > 1)
                Ia.Y = 1;
            if (Ia.Z > 1)
                Ia.Z = 1;

            if (Id.X < 0)
                Id.X = 0;
            if (Id.Y < 0)
                Id.Y = 0;
            if (Id.Z < 0)
                Id.Z = 0;
            if (Id.X > 1)
                Id.X = 1;
            if (Id.Y > 1)
                Id.Y = 1;
            if (Id.Z > 1)
                Id.Z = 1;

            if (Is.X < 0)
                Is.X = 0;
            if (Is.Y < 0)
                Is.Y = 0;
            if (Is.Z < 0)
                Is.Z = 0;
            if (Is.X > 1)
                Is.X = 1;
            if (Is.Y > 1)
                Is.Y = 1;
            if (Is.Z > 1)
                Is.Z = 1;

            I = Ia + Id + Is;

            if (I.X < 0)
                I.X = 0;
            if (I.Y < 0)
                I.Y = 0;
            if (I.Z < 0)
                I.Z = 0;
            if (I.X > 1)
                I.X = 1;
            if (I.Y > 1)
                I.Y = 1;
            if (I.Z > 1)
                I.Z = 1;

            switch (v)
            {
                case 0:
                    {
                        d = (p.Center - light).GetLength() / 300;    
                        break;
                    }
                case 1:
                    {
                        d = (new DVector3(p.v1.X, p.v1.Y, p.v1.Z) - light).GetLength() / 300;
                        break;
                    }
                case 2:
                    {
                        d = (new DVector3(p.v2.X, p.v2.Y, p.v2.Z) - light).GetLength() / 300;
                        break;
                    }
                case 3:
                    {
                        d = (new DVector3(p.v3.X, p.v3.Y, p.v3.Z) - light).GetLength() / 300;
                        break;
                    }
                case 4:
                    {
                        var v4 = p.v4;
                        d = (new DVector3(p.v4.X, p.v4.Y, p.v4.Z) - light).GetLength() / 300;
                        break;
                    }
                default: { break; }
            }
            
            return I;
        }

        public void Intensive(GDIDeviceUpdateArgs e, Polygon p, DVector3 ka, DVector3 kd, DVector3 ks, DVector3 light, DMatrix4 m)
        {
            //проверку координаты z для R > 0
            //перевести light в мировую систему координат
            if (draw == Draw.Flat) { p.Intensive = In(e, p, ka, kd, ks, light, m, 0); }
            else
            {
                I1 = In(e, p, ka, kd, ks, light, m, 1);
                I2 = In(e, p, ka, kd, ks, light, m, 2);
                I3 = In(e, p, ka, kd, ks, light, m, 3);
                if (p.v4 != null)
                {
                    I4 = In(e, p, ka, kd, ks, light, m, 4);
                    p.Intensive = (I1 + I2 + I3 + I4) / 4;
                }
                else p.Intensive = (I1 + I2 + I3) / 3;
            }
        }

        public void DrawTriangle(DVector4[] v, GDIDeviceUpdateArgs e, Polygon p, DMatrix4 m)
        {
            //Color color = Color.FromArgb(128, 124, 252, 0);
            PointF p1 = new PointF((float)v[0].X, (float)v[0].Y);
            PointF p2 = new PointF((float)v[1].X, (float)v[1].Y);
            PointF p3 = new PointF((float)v[2].X, (float)v[2].Y);
            PointF[] ps = new PointF[] { p1, p2, p3 };
            if (draw == Draw.ND)
            {
                Brush brush = new SolidBrush(Color.FromArgb(128, 224, 255, 255));
                e.Graphics.DrawLine(Pens.GhostWhite, v[0].X, v[0].Y, v[1].X, v[1].Y);
                e.Graphics.DrawLine(Pens.GhostWhite, v[1].X, v[1].Y, v[2].X, v[2].Y);
                e.Graphics.DrawLine(Pens.GhostWhite, v[2].X, v[2].Y, v[0].X, v[0].Y);
                e.Graphics.FillPolygon(brush, ps);
            }
            if (draw == Draw.Flat)
            {
                DVector3 c = new DVector3(224, 255, 255);
                Intensive(e, p, Ka, Kd, Ks, Lighting, m);
                Brush brush = new SolidBrush(Color.FromArgb((int)(p.Intensive.X * 255), (int)(p.Intensive.Y * 255), (int)(p.Intensive.Z * 255)));
                e.Graphics.FillPolygon(brush, ps);
            }
            if (draw == Draw.Gouro)
            {
                DVector3 c = new DVector3(224, 255, 255);
                Intensive(e, p, Ka, Kd, Ks, Lighting, m);
                Brush brush = new SolidBrush(Color.FromArgb((int)(p.Intensive.X * 255), (int)(p.Intensive.Y * 255), (int)(p.Intensive.Z * 255)));
                e.Graphics.FillPolygon(brush, ps);
            }
        }

        public void DrawPolygon(DVector4[] v, GDIDeviceUpdateArgs e, Polygon p, DMatrix4 m)
        {
            PointF p1 = new PointF((float)v[0].X, (float)v[0].Y);
            PointF p2 = new PointF((float)v[1].X, (float)v[1].Y);
            PointF p3 = new PointF((float)v[2].X, (float)v[2].Y);
            PointF p4 = new PointF((float)v[3].X, (float)v[3].Y);
            PointF[] ps = new PointF[] { p1, p2, p3, p4 };
            if (draw == Draw.ND)
            {
                Brush brush = new SolidBrush(Color.FromArgb(128, 224, 255, 255));
                e.Graphics.DrawLine(Pens.GhostWhite, v[0].X, v[0].Y, v[1].X, v[1].Y);
                e.Graphics.DrawLine(Pens.GhostWhite, v[1].X, v[1].Y, v[2].X, v[2].Y);
                e.Graphics.DrawLine(Pens.GhostWhite, v[2].X, v[2].Y, v[3].X, v[3].Y);
                e.Graphics.DrawLine(Pens.GhostWhite, v[0].X, v[0].Y, v[3].X, v[3].Y);
                e.Graphics.FillPolygon(brush, ps);
            }
            if (draw == Draw.Flat)
            {
                DVector3 c = new DVector3(224, 255, 255);
                Intensive(e, p, Ka, Kd, Ks, Lighting, m);
                Brush brush = new SolidBrush(Color.FromArgb((int)(p.Intensive.X * 255), (int)(p.Intensive.Y * 255), (int)(p.Intensive.Z * 255)));
                e.Graphics.FillPolygon(brush, ps);
            }
            if (draw == Draw.Gouro)
            {
                DVector3 c = new DVector3(224, 255, 255);
                Intensive(e, p, Ka, Kd, Ks, Lighting, m);
                Brush brush = new SolidBrush(Color.FromArgb((int)(p.Intensive.X * 255), (int)(p.Intensive.Y * 255), (int)(p.Intensive.Z * 255)));
                e.Graphics.FillPolygon(brush, ps);
            }
        }

        public void UploadCenter(Polygon P, DMatrix4 m)
        {
            P.Center = (DVector3)(m * new DVector4(P.Center, 1));
        }

        protected override void OnDeviceUpdate(object s, GDIDeviceUpdateArgs e)
        {
            double W = e.Width / 2;
            double H = e.Heigh / 2;
            CreateFigure(Height, a, b);
            // обычный вид
            if (view == View.NV)
            {
                for (int i = 0; i < Polygons.Count; i++)
                {
                    Polygon P = Polygons[i];

                    DMatrix4 m = Vertex.Shift(W + ShiftX, H + ShiftY, ShiftZ) * Vertex.Scale((float)Size * (float)W / 2) * Vertex.Rotate(angleX, angleY, angleZ);
                    DMatrix4 TRANSFORM = /*Vertex.Shift(W, H, 0) **/ Vertex.Scale(300) ;
                    if (P.v4 == null)
                    {
                        var v = new[] { P.v1, P.v2, P.v3 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawTriangle(v, e, P, TRANSFORM); }
                    }
                    else
                    {
                        var v = new[] { P.v1, P.v2, P.v3, P.v4 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawPolygon(v, e, P, TRANSFORM); }
                    }                    
                }
                Rotation();
            }
            // вид спереди
            if (view == View.FV)
            {
                for (int i = 0; i < Polygons.Count; i++)
                {
                    Polygon P = Polygons[i];

                    DMatrix4 m = Vertex.Shift(W + ShiftX, H + ShiftY, ShiftZ) * Vertex.Scale((float)Size * (float)W / 2) * Vertex.Rotate(angleX, angleY, angleZ) * Vertex.ViewYZ();
                    if (P.v4 == null)
                    {
                        var v = new[] { P.v1, P.v2, P.v3 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawTriangle(v, e, P, m); }
                    }
                    else
                    {
                        var v = new[] { P.v1, P.v2, P.v3, P.v4 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawPolygon(v, e, P, m); }
                    }
                }
                Rotation();
            }
            // вид сбоку
            if (view == View.SV)
            {
                for (int i = 0; i < Polygons.Count; i++)
                {
                    Polygon P = Polygons[i];

                    DMatrix4 m = Vertex.Shift(W + ShiftX, H + ShiftY, ShiftZ) * Vertex.Scale((float)Size * (float)W / 2) * Vertex.Rotate(angleX, angleY, angleZ) * Vertex.ViewXZ();
                    if (P.v4 == null)
                    {
                        var v = new[] { P.v1, P.v2, P.v3 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawTriangle(v, e, P, m); }
                    }
                    else
                    {
                        var v = new[] { P.v1, P.v2, P.v3, P.v4 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawPolygon(v, e, P, m); }
                    }
                }
                Rotation();
            }
            // вид сверху
            if (view == View.UV)
            {
                for (int i = 0; i < Polygons.Count; i++)
                {
                    Polygon P = Polygons[i];

                    DMatrix4 m = Vertex.Shift(W + ShiftX, H + ShiftY, ShiftZ) * Vertex.Scale((float)Size * (float)W / 2) * Vertex.Rotate(angleX, angleY, angleZ) * Vertex.ViewXY();
                    if (P.v4 == null)
                    {
                        var v = new[] { P.v1, P.v2, P.v3 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawTriangle(v, e, P, m); }
                    }
                    else
                    {
                        var v = new[] { P.v1, P.v2, P.v3, P.v4 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawPolygon(v, e, P, m); }
                    }
                }
                Rotation();
            }
            // изометрия
            if (view == View.IZ)
            {
                for (int i = 0; i < Polygons.Count; i++)
                {
                    Polygon P = Polygons[i];

                    DMatrix4 m = Vertex.Shift(W + ShiftX, H + ShiftY, ShiftZ) * Vertex.Scale((float)Size * (float)W / 2) * Vertex.Rotate(45 + angleX, 35 + angleY, angleZ);
                    if (P.v4 == null)
                    {
                        var v = new[] { P.v1, P.v2, P.v3 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawTriangle(v, e, P, m); }
                    }
                    else
                    {
                        var v = new[] { P.v1, P.v2, P.v3, P.v4 }.Select(p => m * p.Vector).ToArray();
                        UploadCenter(P, m);
                        if (P.CalcNormal(v) > 0) { DrawPolygon(v, e, P, m); }
                    }
                }
                Rotation();
            }
            var mat = Vertex.Shift(W / 4, H / 4, 0) * Vertex.Scale(30) * Vertex.Rotate(angleX, angleY, angleY);
            var ax = mat * (new DVector4(1, 0, 0, 1));
            var ay = mat * (new DVector4(0, 1, 0, 1));
            var az = mat * (new DVector4(0, 0, 1, 1));
            e.Surface.DrawLine(Color.Red.ToArgb(), W / 4, H / 4, ax.X, ax.Y);
            e.Surface.DrawLine(Color.Blue.ToArgb(), W / 4, H / 4, ay.X, ay.Y);
            e.Surface.DrawLine(Color.Green.ToArgb(), W / 4, H / 4, az.X, az.Y);
        }   

        public void drawVec(Color col, GDIDeviceUpdateArgs e, Polygon p, DVector3 v)
        {
            var p1 = (DVector2)p.Center;
            var p2 = p1 + (DVector2)v * 40;
            var p0 = (p1 - 1).ToFloatArray();
            e.Surface.DrawLine(col.ToArgb(), p1, p2);
            e.Graphics.DrawRectangle(new Pen(col), p0[0], p0[1], 3, 3);
        }
    }
}
