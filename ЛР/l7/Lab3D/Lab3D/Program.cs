using System;
using SharpGL;
using CGLabPlatform;


// Создание и работа с классом приложения аналогична предыдущим примерам, только в 
// в данном случае наследуемся от шаблона OGLApplicationTemplate<T>, в силу чего
// для вывода графики будет использоваться элемент управления OGLDevice работающий
// через OpenGL (далее OGL). Код OGLDevice размещается в Controls\OGLDevice.cs
public abstract class CGLabDemoOGL : OGLApplicationTemplate<CGLabDemoOGL>
{
    [STAThread] static void Main() { RunApplication(); }

    public double angleX = 0;
    public double angleY = 0;
    public double angleZ = 0;

    public double k = 0;

    public double shiftX = 0;
    public double shiftY = 0;
    public double shiftZ = 0;

    public double angle = 0;

    public double toRad = Math.PI / 180;

    public double [] xy(double t)
    {
        double [] res = new double[2];
        double resx, resy;
        resx = P1.X + P_1.X * t + (3 * (P2.X - P1.X) - 2 * P_1.X - P_2.X) * t * t + (2 * (P1.X - P2.X) + P_1.X + P_2.X) * t * t * t;
        resy = P1.Y + P_1.Y * t + (3 * (P2.Y - P1.Y) - 2 * P_1.Y - P_2.Y) * t * t + (2 * (P1.Y - P2.Y) + P_1.Y + P_2.Y) * t * t * t;
        res[0] = resx;
        res[1] = resy;
        return res;
    }

    #region Свойства

    [DisplayNumericProperty(new[] { 0d, 0d, 0d }, 1, 0, "Положение камеры (X/Y/Z)")]
    public virtual DVector3 cameraAngle
    {
        get { return Get<DVector3>(); }
        set { if (Set(value)) UpdateModelViewMatrix(); }
    }

    [DisplayNumericProperty(new double[] { 20, 80 }, 1, "Точка 1")]
    public virtual DVector2 P1 { set; get; }

    [DisplayNumericProperty(new double[] { 50, 0 }, 1, "Точка 2")]
    public virtual DVector2 P2 { set; get; }

    [DisplayNumericProperty(new double[] { -70, -180 }, 1, "Касательный вектор в точке 1")]
    public virtual DVector2 P_1 { set; get; }

    [DisplayNumericProperty(new double[] { -90, -20 }, 1, "Касательный вектор в точке 2")]
    public virtual DVector2 P_2 { set; get; }


    [DisplayNumericProperty(1.0d, 0.01, 2, "Удаленность камеры", 0.01)]
    public virtual double cameraDistance { set; get; }

    [DisplayNumericProperty(16, 1, "Аппроксимация", 2)]
    public virtual double apr { set; get; }

    [DisplayTextBoxProperty( "Шиляева Н. С._Сегмент кубического сплайна по конечным точкам и касательным", "Задание")]
    public virtual string s { set; get; }

    #endregion

    // Само создание объекта типа OpenGL осуществляется при создании устройства вывода (класс OGLDevice)
    // и доступ к нему можно получить при помощи свойства gl данного объекта (RenderDevice) или объекта
    // типа OGLDeviceUpdateArgs передаваемого в качестве параметра методу OnDeviceUpdate(). Данный метод,
    // как и сама работа с устройством OpenGL реализуются в параллельном потоке. Обращение к устройству
    // OpenGL из другого потока не допускается (создание многопоточного рендера возможно, но это достаточно
    // специфическая архитектура, например рендинг частей экрана в текустуры а потом их объединение).
    // Для большинства функций библиотеки OpenGL при отладке DEBUG конфигурации осуществляется проверка
    // ошибок выполнения и их вывод в окно вывода Microsoft Visual Studio. Поэтому при отладке и написании 
    // кода связанного с OpenGL необходимо также контролировать ошибки библиотеки OpenGL в окне вывода. 

    protected override void OnMainWindowLoad(object sender, EventArgs args)
    {
        base.VSPanelWidth = 300;
        base.ValueStorage.RightColWidth = 60;
        base.RenderDevice.VSync = 1;

        #region Обработчики событий мыши и клавиатуры -------------------------------------------------------
        RenderDevice.MouseMoveWithLeftBtnDown += (s, e) => cameraAngle += new DVector3(e.MovDeltaX, e.MovDeltaY, 0);
        RenderDevice.MouseWheel += (s, e) => cameraDistance += e.Delta / 1000.0;
        RenderDevice.MouseMoveWithRightBtnDown += (s, e) =>
        {
            shiftX += e.MovDeltaX * 0.2;
            shiftY -= e.MovDeltaY * 0.2;
        };
        RenderDevice.MouseMoveWithLeftBtnDown += (s, e) =>
        {
            angleX += e.MovDeltaX;
            angleY += e.MovDeltaY;
            angle += e.MovDelta;
        };
        #endregion


        // Как было отмеченно выше вся работа связанная с OGL должна выполнятся в одном потоке. Тут работа с OGL
        // осуществляется в отдельном потоке, а метод OnMainWindowLoad() является событием возбуждаемым потоком
        // пользовательского интерфейса (UI). Поэтой причине весь код ниже добавляется в диспетчер устройства
        // вывода (метод AddScheduleTask() объекта RenderDevice) и выполняется ассинхронно в контексте потока
        // OGL. Сам диспетчер является очередью типа FIFO (First In First Out - т.е. задания обрабатываются 
        // строго в порядке их поступления) и гарантирует, что все задания добавленные в OnMainWindowLoad()
        // будут выполнены до первого вызова метода OnDeviceUpdate() (aka OnPaint)

        #region  Инициализация OGL и параметров рендера -----------------------------------------------------
        RenderDevice.AddScheduleTask((gl, s) =>
        {
            gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.ClearColor(0, 0, 0, 0);
        });
        #endregion

        #region Инициализация буфера вершин -----------------------------------------------------------------
        RenderDevice.AddScheduleTask((gl, s) =>
        {
            // TODO: 
        }, this);
        #endregion

        #region Уничтожение буфера вершин по завершению работы OGL ------------------------------------------
        RenderDevice.Closed += (s, e) => // Событие выполняется в контексте потока OGL при завершении работы
        {
            var gl = e.gl;
            // TODO: 
        };
        #endregion

        #region Обновление матрицы проекции при изменении размеров окна и запуске приложения ----------------
        RenderDevice.Resized += (s, e) =>
        {
            var gl = e.gl;
            // TODO: 
        };
        #endregion
    }

    private void UpdateModelViewMatrix() // метод вызывается при измении свойств cameraAngle и cameraDistance
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
        double t = 0;
        
        double dt = 1 / apr;
        var gl = e.gl;
        gl.ClearColor(1, 1, 1, 1);
        
        // Очищаем буфер экрана и буфер глубины (иначе рисоваться все будет поверх старого)
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
        gl.MatrixMode(OpenGL.GL_PROJECTION);
        gl.LoadIdentity();
        gl.Ortho(-10, 90, -10, 90, -1, 1);
        gl.Scale(cameraDistance, cameraDistance, cameraDistance); 
        gl.Translate(shiftX, shiftY, 0);
        gl.Enable(OpenGL.GL_POINT_SMOOTH);
        
        //точки
        gl.PointSize(6);
        gl.Color(1f, 0f, 0f);
        gl.Begin(OpenGL.GL_POINTS);
        gl.Vertex(P1.X, P1.Y);
        gl.Vertex(P2.X, P2.Y);
        gl.End();
        
        //касательные
        gl.Color(0, 1f, 1f);
        gl.LineWidth(2);
        DVector2 n1 = (P1 + P_1).Normalized();
        gl.Begin(OpenGL.GL_LINES);
        gl.Vertex(P1.X, P1.Y);
        gl.Vertex(P_1.X, P_1.Y);
        gl.End();
        gl.Color(0, 1f, 1f);
        gl.LineWidth(2);
        DVector2 n2 = (P2 + P_2).Normalized();
        gl.Begin(OpenGL.GL_LINES);
        gl.Vertex(P2.X, P2.Y);
        gl.Vertex(P_2.X, P_2.Y);
        gl.End();
        
        //кривая
        gl.Color(0f, 0f, 0f);
        gl.LineWidth(2);
        gl.Begin(OpenGL.GL_LINES);
        while (t < 1)
        {
            double[] p1 = xy(t);
            double[] p2 = xy(t + dt);
            if (t + dt > 1)
            {
                gl.Vertex(p1);
                gl.Vertex(P2.X, P2.Y);
                break;
            }
            else
            {               
                gl.Vertex(p1);
                gl.Vertex(p2);
                t += dt;
            }    
        }
        gl.End();
        gl.Flush();
    }

}