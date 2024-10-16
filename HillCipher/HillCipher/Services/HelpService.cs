using System.Drawing;
using System.IO;
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
                result =
            }
            return result;
        }

        public static string readFromRessource(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            // TODO: Change dynamic path to the ressources folder
            string filePath = Path.Combine("", fileName);
            try
            {
                string fileContent = File.ReadAllText(filePath);
                Console.WriteLine("File content:");
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
    }
}