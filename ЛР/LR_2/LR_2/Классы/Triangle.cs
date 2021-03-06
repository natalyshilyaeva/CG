using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR_2
{
    class Triangle
    {
        public Triangle(MyPoint a, MyPoint b, MyPoint c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public MyPoint n()
        {
            return (b - a) ^ (c - a);
        }

        static public Triangle operator *(Matrix m, Triangle t)
        {
            return new Triangle(m * t.a, m * t.b, m * t.c);
        }

        public void draw(Pen pen, Graphics g, bool isWithInvLin)
        {

            if (!isWithInvLin)
            {
                if (n().z > 0)
                {
                    g.DrawLine(pen, (int)a.x, (int)a.y, (int)b.x, (int)b.y);
                    g.DrawLine(pen, (int)b.x, (int)b.y, (int)c.x, (int)c.y);
                    g.DrawLine(pen, (int)c.x, (int)c.y, (int)a.x, (int)a.y);
                }
            }
            else
            {
                g.DrawLine(pen, (int)a.x, (int)a.y, (int)b.x, (int)b.y);
                g.DrawLine(pen, (int)b.x, (int)b.y, (int)c.x, (int)c.y);
                g.DrawLine(pen, (int)c.x, (int)c.y, (int)a.x, (int)a.y);
            }
        }

        public MyPoint a, b, c;
    }
}



