using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static Matrix multiplyMatrixWithNumber(Matrix a, int b)
        {
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    result.Data[i, j] = a.Data[i, j] * b;
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
                    var value = a.Data[i, j] % m;
                    result.Data[i, j] = value < 0? value + m : value;
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

            // create augmented Matrix
            int[,] augmented = new int[n, n * 2];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = a.Data[i, j];
                    augmented[i, j + n] = (i == j) ? 1 : 0;
                }
            }
            var aug = new Matrix(augmented);

            // Gaussian Elimination in mod m
            var g_matrix = GaussianEliminationMod(aug, m);

            // Extract the inverse matrix from the augmented matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result.Data[i, j] = g_matrix.Data[i, j + n];
                }
            }

            return result;
        }

        public static Matrix GaussianEliminationMod(Matrix input, int m)
        {
            Matrix output = new Matrix(input.Rows, input.Cols);
            int n = input.Rows;
            int[,] a = input.Data;
            Matrix[] rows = new Matrix[n];

            // Create a matrix for each row
            for (int i = 0; i < n; i++)
            {
                rows[i] = new Matrix(1, input.Cols);
                for (int j = 0; j < input.Cols; j++)
                {
                    rows[i].Data[0, j] = a[i, j];
                }
            }

            // Perform Gaussian Elimination
            int x = 0;
            foreach (var row in rows)
            {
                var pivot = row.Data[0, x];
                var pivot_inverse = modInverse(pivot, m);
                rows[x] = modMatrix(multiplyMatrixWithNumber(rows[x], pivot_inverse), m);
                for (int j = 0; j < n; j++)
                {
                    if (j != x)
                    {
                        var factor = rows[j].Data[0, x];
                        rows[j] = modMatrix(subMatrix(rows[j], modMatrix(multiplyMatrixWithNumber(rows[x], factor), m)), m);
                    }
                }
                x++;
            }

            // create output matrix
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Cols; j++)
                {
                    output.Data[i, j] = rows[i].Data[0, j];
                }

            }
            return output;
        }


        static Matrix subMatrix(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
            {
                throw new Exception("Matrices must have the same dimensions");
            }
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    result.Data[i, j] = a.Data[i, j] - b.Data[i, j];
                }
            }
            return result;
        }

        public static bool checkForIdentitiyMatrix(Matrix a, Matrix b, int m)
        {
            if(a.Rows > b.Cols)
            {
                return false;
            }
            Matrix i = new Matrix(a.Rows, a.Cols);
            for (int j = 0; j < a.Rows; j++)
            {
                for( int y =0; y <a.Cols; y++)
                {
                    if(j == y)
                        i.Data[j, y] = 1;
                }
            }
            var res = multiplyMatrix(a, b);
            res = modMatrix(res, m);
            return res.Equals(i);
        }

        // Calculate the left inverse of a matrix
        // Left Inverse exists if the matrix has more rows than columns

        public static Matrix leftInverse(Matrix matrix, int m)
        {
            // Calculate the transpose of the matrix
            Matrix transpose_matrix = transpose(matrix);

            // Calculate (A^T * A)^-1
            Matrix product = multiplyMatrix(transpose_matrix, matrix);
            Matrix inverseProduct = inverseMatrix(product, m);

            // Calculate the left inverse: (A^T * A)^-1 * A^T
            Matrix leftInverse = multiplyMatrix(inverseProduct, transpose_matrix);

            return leftInverse;
        }

        // Calculate the right inverse of a matrix
        // Right Inverse exists if the matrix has more columns than rows
        public static Matrix rightInverse(Matrix matrix, int m)
        {
            // Calculate the transpose of the matrix
            Matrix transpose_matrix = transpose(matrix);

            // Calculate (A * A^T)^-1
            Matrix product = multiplyMatrix(matrix, transpose_matrix);
            Matrix product_m = modMatrix(product, m);
            Matrix inverseProduct = inverseMatrix(product_m, m);

            // Calculate the right inverse: A^T * (A * A^T)^-1
            Matrix rightInverseMatrix = multiplyMatrix(transpose_matrix, inverseProduct);

            return modMatrix(rightInverseMatrix, m);
        }


        public static Matrix transpose(Matrix matrix)
        {
            Matrix result = new Matrix(matrix.Cols, matrix.Rows);
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    result.Data[j, i] = matrix.Data[i, j];
                }
            }
            return result;
        }

        private static int modInverse(int a, int m)
        {
            // Get the Faktor x of a * x = 1 mod m
            int m0 = m;
            int y = 0, x = 1;

            if (m == 1)
                return 0; 

            while (a > 1)
            {
                int q = a / m;
                int t = m;

                m = a % m;
                a = t;

                t = y;
                y = x - q * y;
                x = t;
            }

            // Make sure x is positive
            if (x < 0)
                x += m0;

            return x;
        

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