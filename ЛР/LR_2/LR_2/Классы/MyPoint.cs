using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_2
{
    class MyPoint
{
    public MyPoint(double x = 0, double y = 0, double z = 0, double w = 1) 
    {
        this.x = x; 
        this.y = y; 
        this.z = z;
        this.w = w;
    }
    public MyPoint(MyPoint rhs)
    {
        this.x = rhs.x; 
        this.y = rhs.y; 
        this.z = rhs.z;
        this.w = rhs.w;
    }

    // операции над векторами:

    // вычитание
    static public MyPoint operator -(MyPoint left, MyPoint right)
    {
        return new MyPoint(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
    }
    // присваивание
    static public MyPoint operator ==(MyPoint thisPoint, MyPoint rhs)
    {
        thisPoint.x = rhs.x;
        thisPoint.y = rhs.y;
        thisPoint.z = rhs.z;
        thisPoint.w = rhs.w;
        return thisPoint;
    }
    
    static public MyPoint operator !=(MyPoint thisPoint, MyPoint rhs)
    {
        return thisPoint;
    }
	// скалярное произведение
    static public double operator *(MyPoint left, MyPoint right)
    {
	    return left.x * right.x + left.y * right.y + left.z * right.z;
    }
    // векторное произведение
    static public MyPoint operator ^(MyPoint left, MyPoint right)
    {
	    MyPoint res = new MyPoint();
        res.x = left.y * right.z - left.z * right.y;
        res.y = left.z * right.x - left.x * right.z;
        res.z = left.x * right.y - left.y * right.x;
        res.w = 0;

        return res;
    }

    public double x, y, z, w;
};
}