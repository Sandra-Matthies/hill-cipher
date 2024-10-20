using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HillCipher.Models;

namespace HillCipher.Services
{
    public class MatrixCalculation
    {
        public static Matrix multiplyMatrix(Matrix a, Matrix b)
        {
            if (a.Cols != b.Rows)
            {
                throw new Exception("The number of columns of the first matrix must be equal to the number of rows of the second matrix");
            }
            Matrix result = new Matrix(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Cols; j++)
                {
                    for (int k = 0; k < a.Cols; k++)
                    {
                        result.Data[i, j] += a.Data[i, k] * b.Data[k, j];
                    }
                }
            }
            return result;
        }

        public static Matrix modMatrix(Matrix a, int m)
        {
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    result.Data[i, j] = a.Data[i, j] % m;
                }
            }
            return result;
        }

        public static Matrix inverseMatrix(Matrix a, int m)
        {
            if (a.Rows != a.Cols)
            {
                throw new Exception("Matrix must be square");
            }

            int n = a.Rows;
            Matrix result = new Matrix(n, n);
            double[,] augmented = new double[n, 2 * n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = a.Data[i, j];
                }
                augmented[i, n + i] = 1;
            }

            for (int i = 0; i < n; i++)
            {
                int diagElement = (int)augmented[i, i];
                int invDiagElement = ModInverse(diagElement, m);
                if (invDiagElement == 0)
                {
                    throw new Exception("Matrix is singular and cannot be inverted");
                }
                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] = (augmented[i, j] * invDiagElement) % m;
                }

                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        int factor = (int)augmented[k, i];
                        for (int j = 0; j < 2 * n; j++)
                        {
                            augmented[k, j] = (augmented[k, j] - factor * augmented[i, j] + m * m) % m;
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result.Data[i, j] = (int)augmented[i, n + j];
                }
            }

            return result;
        }

        private static int ModInverse(int a, int m)
        {
            int i = m, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= m;
            if (v < 0) v = (v + m) % m;
            return v;
        }

        public static int getDeterminant(Matrix a)
        {
            if (a.Rows != a.Cols)
            {
                throw new Exception("Matrix must be square");
            }
            if (a.Rows == 1)
            {
                return a.Data[0, 0];
            }
            if (a.Rows == 2)
            {
                return a.Data[0, 0] * a.Data[1, 1] - a.Data[0, 1] * a.Data[1, 0];
            }
            int determinant = 0;
            for (int i = 0; i < a.Rows; i++)
            {
                Matrix minor = new Matrix(a.Rows - 1, a.Cols - 1);
                for (int j = 1; j < a.Rows; j++)
                {
                    for (int k = 0; k < a.Cols; k++)
                    {
                        if (k < i)
                        {
                            minor.Data[j - 1, k] = a.Data[j, k];
                        }
                        else if (k > i)
                        {
                            minor.Data[j - 1, k - 1] = a.Data[j, k];
                        }
                    }
                }
                int sign = (i % 2 == 0) ? 1 : -1;
                determinant += sign * a.Data[0, i] * getDeterminant(minor);
            }
            return determinant;
        }
    }
}