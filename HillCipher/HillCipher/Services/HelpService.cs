using System.Drawing;
using HillCipher.Models;

namespace HillCipher.Services
{
    internal class HelpService
    {
        public static bool IsValidKey(Matrix key, int m)
        {
            if (key.Rows != key.Cols)
            {
                return false;
            }
            // Determinant of the key matrix must be != 0
            var det = MatrixCalculation.getDeterminant(key);
            if (det == 0)
            {
                return false;
            }

            // The Determinant and m must be coprime
            if (GCD(det, m) != 1)
            {
                return false;
            }
            return true;
        }

        public static int GCD(int a, int b)
        {
            if (b == 0)
            {
                return a;
            }
            else
            {
                return GCD(b, a % b);
            }
        }

        public static Matrix[] splitPlainText(Matrix plainText, int n)
        {
            if (plainText.Cols > 1)
            {
                throw new Exception("The plain text must be a vector");
            }
            Matrix[] result = new Matrix[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = new Matrix(plainText.Rows / n, 1);
                for (int j = 0; j < plainText.Rows / n; j++)
                {
                    result[i].Data[j, 0] = plainText.Data[i * plainText.Rows / n + j, 0];
                }
            }
            return result;
        }

        public static Matrix mergeCipherText(Matrix[] cipherText, int n)
        {
            var result = new Matrix();
            result.Cols = 1;
            foreach (var c in cipherText)
            {
                result.Rows += c.Rows;
                foreach (var d in c.Data)
                {
                    result.Data.SetValue(d, result.Rows - c.Rows, 0);
                }

            }
            return result;
        }
    }
}