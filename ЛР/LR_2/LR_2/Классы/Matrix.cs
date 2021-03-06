using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_2
{
class Matrix
{
    public Matrix()
    {
       _m = new Double[4,4];
        for(int i = 0; i < 4; ++i)
            for(int j = 0; j < 4; ++j)
                _m[i,j] = 0;
    }
        
    // операции над векторами и матрицами:
    
    // умножение матриц
    static public Matrix operator *(Matrix left, Matrix right) 
    {
        Matrix res = new Matrix();
        for(int i = 0; i < 4; ++i)
            for(int j = 0; j < 4; ++j)
                for(int k = 0; k < 4; ++k)
                    res._m[i,j] += left._m[i,k] * right._m[k,j];
        return res;
    }
    // умножение матриц на число
    static public Matrix operator *(Matrix left, double chislo)
    {
        Matrix res = new Matrix();
        for (int i = 0; i < 4; ++i)
            for (int j = 0; j < 4; ++j)
                for (int k = 0; k < 4; ++k)
                    res._m[i, j] = left._m[i, j] * chislo;
        return res;
    }
    // умножение вектора на матрицу
    static public MyPoint operator *(Matrix left, MyPoint p)
    {
        double[] t = {0, 0, 0, 0};
        double[] tp = {p.x, p.y, p.z, p.w};
        
        for(int i = 0; i < 4; ++i)
            for(int j = 0; j < 4; ++j)
                t[i] += left._m[i,j] * tp[j];
        
        return new MyPoint(t[0], t[1], t[2], t[3]);
    }
    // присваивание
    static public Matrix operator ==(Matrix left, Matrix right)
    {
        for(int i = 0; i < 4; ++i)
            for(int j = 0; j < 4; ++j)
                left._m[i,j] = right._m[i,j];
        
        return left;
    }

    static public Matrix operator !=(Matrix left, Matrix right)
    {
        return left;
    }

    public double[,] _m;
};

//матрица масштабирования
class ScalingMatrix: Matrix
{
    public ScalingMatrix(double a, double b, double c)
    {
        _m[0,0] = a;
        _m[1,1] = b; 
        _m[2,2] = c; 
        _m[3,3] = 1;
    }
}

//матрица поворота
class RotationMatrix: Matrix
{
        public RotationMatrix(char axis, double a)
        {
            switch(axis)
            {
                case 'X':
                    _m[1,1] = _m[2,2] = Math.Cos(a);
                    _m[1,2] = -1 *(_m[2,1] = Math.Sin(a));
                    _m[0,0] = 1;
                    break;
                case 'Y':
                    _m[0,0] = _m[2,2] = Math.Cos(a);
                    _m[2,0] = -1 *(_m[0,2] = Math.Sin(a));
                    _m[1,1] = 1;
                    break;
                case 'Z':
                    _m[0,0] = _m[1,1] = Math.Cos(a);
                    _m[1,0] = -1 *(_m[0,1] = Math.Sin(a));
                    _m[2,2] = 1;
                    break;
            }
            _m[3,3] = 1;
        }
}

//матрица сдвига
class ShiftMatrix: Matrix
{
    public ShiftMatrix(double a, double b, double c)
    {
        _m[0,3] = a;
        _m[1,3] = b;
        _m[2,3] = c;
        for(int i = 0; i < 4; ++i)
            _m[i,i] = 1;
    }
}

}
