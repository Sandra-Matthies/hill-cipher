using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HillCipher.Models;

namespace HillCipher.Services
{
    public class KnownPlainTextAttack
    {
        public static Matrix Attack(Matrix plainText, Matrix cipherText, int m)
        {
            if (!isValidTextMatrix(plainText, cipherText))
            {
                throw new Exception("Invalid text matrix");
            }

            // Create system of equations for K = C*[P]^-1

            var n = plainText.Rows;
            var key = new Matrix(n, n);
            Matrix inversePlainTextMatrix;
            if (plainText.Cols < plainText.Rows)
            {
                inversePlainTextMatrix = MatrixCalculation.leftInverse(plainText, m);
            }
             else if (plainText.Cols > plainText.Rows)
            {
                inversePlainTextMatrix = MatrixCalculation.rightInverse(plainText, m);
            }
            else { 
                inversePlainTextMatrix = MatrixCalculation.inverseMatrix(plainText, m);
            } 

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    key.Data[i, j] = 0;
                    for (int k = 0; k < n; k++)
                    {
                        key.Data[i, j] += cipherText.Data[i, k] * inversePlainTextMatrix.Data[k, j];
                    }
                    key.Data[i, j] = key.Data[i, j] % m;
                }
            }
            return key;
        }

        public static bool isValidTextMatrix(Matrix plainText, Matrix cipherText)
        {
            if (plainText.Rows != cipherText.Rows)
            {
                return false;
            }
            if (plainText.Cols != cipherText.Cols)
            {
                return false;
            }
            return true;
        }


    }
}