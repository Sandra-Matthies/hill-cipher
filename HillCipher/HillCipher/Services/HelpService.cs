using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
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

        public static Matrix mergeCipherText(Matrix[] cipherText, int n)
        {
            var result = new Matrix(0, 1);
            // create a new vector with the all the rows of the cipherText and the data of the cipherText
            foreach (var cipher in cipherText)
            {
                // create a new matrix with the size of the result matrix + 1
                var newResult = new Matrix(result.Rows + cipher.Rows, 1);
                // copy the data of the result matrix to the new result matrix
                for (int i = 0; i < result.Rows; i++)
                {
                    newResult.Data[i, 0] = result.Data[i, 0];
                }
                // copy the data of the cipher matrix to the new result matrix
                for (int i = 0; i < cipher.Rows; i++)
                {
                    newResult.Data[result.Rows + i, 0] = cipher.Data[i, 0];
                }
                // set the result matrix to the new result matrix
                result = newResult;
            }
            return result;
        }

        public static string readFromRessource(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "Ressources", fileName);
            try
            {
                string fileContent = File.ReadAllText(filePath);
                Console.WriteLine("File content: " + fileName);
                Console.WriteLine(fileContent);
                return fileContent;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            return "";
        }

        // Create a n*n matrix from a string
        public static Matrix createKeyMatrix(int[] text)
        {
            int n = (int)Math.Sqrt(text.Length);
            Matrix key = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    key.Data[i, j] = text[i * n + j];
                }
            }
            return key;
        }

        public static Matrix[] createTextMatrices(int[] text, int n)
        {
            int m = text.Length;
            int rows = m / n;
            Matrix[] matrices = new Matrix[rows];
            for (int i = 0; i < rows; i++)
            {
                matrices[i] = new Matrix(n, 1);
                for (int j = 0; j < n; j++)
                {
                    matrices[i].Data[j, 0] = text[i * n + j];
                }
            }
            return matrices;
        }

        public static Matrix createMatrixFromNumbers(int[] numbers)
        {
            // the key matrix is a square matrix  
            int m = (int)Math.Sqrt(numbers.Length);
            int n = numbers.Length / m;
            Matrix result = new Matrix(m, n);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result.Data[i, j] = numbers[i * n + j];
                }
            }


            return result;
        }

        public static int[] createarrayFromMatrix(Matrix matrix)
        {
            int[] result = new int[matrix.Rows * matrix.Cols];
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    result[i * matrix.Cols + j] = matrix.Data[i, j];
                }
            }
            return result;
        }
    }
}