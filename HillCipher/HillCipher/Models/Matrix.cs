using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HillCipher.Models
{
    public class Matrix
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int[,] Data { get; set; }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Data = new int[rows, cols];
        }

        public Matrix()
        {
            Cols = 0;
            Rows = 0;
            Data = new int[0, 0];
        }

        public Matrix(int[,] data)
        {
            Rows = data.GetLength(0);
            Cols = data.GetLength(1);
            Data = data;
        }

        public void Print()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.Write(Data[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}