using System;
using System.Collections;
using System.Linq;
using SharpGL;
using CGLabPlatform;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

public abstract class CGLabDemoOGL : OGLApplicationTemplate<CGLabDemoOGL>
{
    [STAThread] static void Main() { RunApplication(); }

    public double[,,] W =
    {
        { {-25, -25, 5 }, {-25, -15, 10}, {-28, -5, 15}, {-28, 5, 15}, {-25, 15, 10}, {-25, 25, 5} },
        { {-15, -25, 5 }, {-15, -15, 10}, {-18, -5, 15}, {-18, 5, 15}, {-15, 15, 10}, {-15, 25, 5} },
        { {-5, -28, 10 }, {-5, -18, 15}, {-7, -7, 25}, {-7, 7, 25}, {-5, 18, 15}, {-5, 28, 10} },
        { {5, -28, 10 }, {5, -18, 15}, {7, -7, 25}, {7, 7, 25}, {5, 18, 15}, {5, 28, 10} },
        { {15, -25, 5 }, {15, -15, 10}, {18, -5, 15}, {18, 5, 15}, {15, 15, 10}, {15, 25, 5} },
        { {25, -25, 5 }, {25, -15, 10}, {28, -5, 15}, {28, 5, 15}, {25, 15, 10}, {25, 25, 5} }
    };

    public int mx = 6, my = 6;

    public double dt = 0.2;

    public double angleX = 0;
    public double angleY = 0;
    public double angleZ = 0;

    public double shiftX = 0;
    public double shiftY = 0;
    public double shiftZ = 0;

    public double toRad = Math.PI / 180;

    public double xyz(int i, int j, double u, double v, int k)
    {
        double u0 = u - i;
        double u02 = u0 * u0;
        double u03 = u02 * u0;
        double u1 = 1 - u0;
        double u12 = u1 * u1;
        double u13 = u12 * u1;
        double v0 = v - j;
        double v02 = v0 * v0;
        double v03 = v02 * v0;
        double v1 = 1 - v0;
        double v12 = v1 * v1;
        double v13 = v12 * v1;
        double[] SV = { 0, 0, 0, 0 };
        for (int i2 = i; i2 < i + 4; i2++)
            SV[i2 - i] = v13 * W[i2, j, k] + (3 * v03 - 6 * v02 + 4) * W[i2, j + 1, k] + (-3 * v03 + 3 * v02 + 3 * v0 + 1) * W[i2, j + 2, k] + v03 * W[i2, j + 3, k];
        double res = (u13 * SV[0] + (3 * u03 - 6 * u02 + 4) * SV[1] + (-3 * u03 + 3 * u02 + 3 * u0 + 1) * SV[2] + u03 * SV[3]) / 36;
        return res;
    }

    #region Свойства
    [DisplayNumericProperty(new[] { 0d, 0d }, 1, 0, "WP_idx", 0, 5)]
    public virtual DVector2 WP_idx
    {
        set { if (Set<DVector2>(value)) {
                int x = (int)value.X;
                int y = (int)value.Y;
                Set<DVector3>(new DVector3(Enumerable.Range(0, 3).Select(z => W[x, y, z]).ToArray()), "WP_val");
            }
        }
        get { return Get<DVector2>(); }
    }

    [DisplayNumericProperty(new[] { -25d, -25d, 5d }, 1, "WP_val")]
    public virtual DVector3 WP_val
    {
        set
        {
            {
                Set<DVector3>(value);
                W[(int)WP_idx.X, (int)WP_idx.Y, 0] = value.X;
                W[(int)WP_idx.X, (int)WP_idx.Y, 1] = value.Y;
                W[(int)WP_idx.X, (int)WP_idx.Y, 2] = value.Z;
            }
        }
        get { return Get<DVector3>(); }
    }

    [DisplayNumericProperty(new[] { 0d, 0d, 0d }, 1, 0, "Положение камеры (X/Y/Z)")]
    public virtual DVector3 cameraAngle
    {
        get { return Get<DVector3>(); }
        set { if (Set(value)) UpdateModelViewMatrix(); }
    }

    [DisplayNumericProperty(1.0d, 0.1, 2, "Удаленность камеры")]
    public virtual double cameraDistance
    {
        get { return Get<double>(); }
        set { if (Set(value)) UpdateModelViewMatrix(); }
    }

    [DisplayNumericProperty(5, 1, "Апроксимация", 2, 10)]
    public virtual double apr { set; get; }  
    #endregion
    protected override void OnMainWindowLoad(object sender, EventArgs args)
    {
        base.VSPanelWidth = 300;
        base.ValueStorage.RightColWidth = 60;
        base.RenderDevice.VSync = 1;

        #region Обработчики событий мыши и клавиатуры -------------------------------------------------------
        RenderDevice.MouseMoveWithRightBtnDown += (s, e) => cameraAngle += new DVector3(e.MovDeltaX, e.MovDeltaY, 0);
        RenderDevice.MouseWheel += (s, e) => cameraDistance += e.Delta / 1000.0;
        RenderDevice.MouseMoveWithRightBtnDown += (s, e) =>
        {
            shiftX += e.MovDeltaX * 0.2;
            shiftY += e.MovDeltaY * 0.2;
        };
        RenderDevice.MouseMoveWithLeftBtnDown += (s, e) =>
        {
            angleX += e.MovDeltaX * 10;
            angleY += e.MovDeltaY * 10;
            angleZ += e.MovDeltaX * 10;
        };
        #endregion
        #region  Инициализация OGL и параметров рендера -----------------------------------------------------
        RenderDevice.AddScheduleTask((gl, s) =>
        {
            gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.ClearColor(0, 0, 0, 0);
        });
        #endregion
    }

    private void UpdateModelViewMatrix()
    {
        #region Обновление объектно-видовой матрицы ---------------------------------------------------------
        RenderDevice.AddScheduleTask((gl, s) =>
        {
            // TODO: 
        });
        #endregion
    }

    protected unsafe override void OnDeviceUpdate(object s, OGLDeviceUpdateArgs e)
    {
        double u = 0, v = 0;
        int i, j;
        dt = 1 / apr;
        var gl = e.gl;

        gl.ClearColor(1, 1, 1, 1);
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
        gl.MatrixMode(OpenGL.GL_PROJECTION);
        gl.LoadIdentity();
        gl.Ortho(-40, 40, -40, 40, -40, 40);
        gl.Scale(cameraDistance, cameraDistance, cameraDistance);
        gl.Translate(shiftX, shiftY, shiftZ);
        gl.Rotate((float)(toRad * angleX), (float)(toRad * angleY), (float)(toRad * angleZ));
        gl.Rotate((float)(toRad * angleX), (float)(toRad * angleY), (float)(toRad * angleZ));
        gl.Enable(OpenGL.GL_POINT_SMOOTH);

        gl.PointSize(6);
        gl.Color(1f, 0, 0);
        gl.Begin(OpenGL.GL_POINTS);
        for (i = 0; i < mx; i++) 
            for (j = 0; j < my; j++)
                gl.Vertex(W[i, j, 0], W[i, j, 1], W[i, j, 2]);
        gl.End();

        gl.PointSize(16);
        gl.Color(0, 0, 0);
        gl.Begin(OpenGL.GL_POINTS);
        gl.Vertex(WP_val.X, WP_val.Y, WP_val.Z);
        gl.End();

        gl.Color(0, 1f, 1f);
        gl.LineWidth(1);
        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
        gl.Begin(OpenGL.GL_QUADS);
        for (i = 0; i < mx - 1; i++)
        {
            for (j = 0; j < my - 1; j++)
            {
                gl.Vertex(W[i, j, 0], W[i, j, 1], W[i, j, 2]);
                gl.Vertex(W[i + 1, j, 0], W[i + 1, j, 1], W[i + 1, j, 2]);
                gl.Vertex(W[i + 1, j + 1, 0], W[i + 1, j + 1, 1], W[i + 1, j + 1, 2]);
                gl.Vertex(W[i, j + 1, 0], W[i, j + 1, 1], W[i, j + 1, 2]);
            }
        }
        gl.End();

        gl.LineWidth(2);
        gl.Color(0, 0, 0);
        gl.Begin(OpenGL.GL_QUADS);
        for (i = 0; i < mx - 3; i++)
        {
            for (j = 0; j < my - 3; j++)
            {
                u = i;
                while (u <= i + 1)
                {
                    v = j;
                    while (v <= j + 1)
                    {
                        {
                            gl.Vertex(xyz(i, j, u, v, 0), xyz(i, j, u, v, 1), xyz(i, j, u, v, 2));
                            gl.Vertex(xyz(i, j, u + dt, v, 0), xyz(i, j, u + dt, v, 1), xyz(i, j, u + dt, v, 2));
                            gl.Vertex(xyz(i, j, u + dt, v + dt, 0), xyz(i, j, u + dt, v + dt, 1), xyz(i, j, u + dt, v + dt, 2));
                            gl.Vertex(xyz(i, j, u, v + dt, 0), xyz(i, j, u, v + dt, 1), xyz(i, j, u, v + dt, 2));
                        }
                        
                        v += dt;
                    }
                    u += dt;
                }
            }
        }
        gl.End();

        gl.Flush();
    }

}