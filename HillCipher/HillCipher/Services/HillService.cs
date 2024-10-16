using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HillCipher.Models;

namespace HillCipher.Services
{
    internal class HillService
    {
        // Encrypt the given text using the given key
        // The key k is a matrix
        // The plaintext p is a vector or a matrix with one column
        public static Matrix Encrypt(Matrix k, Matrix p, int m)
        {
            if (!HelpService.IsValidKey(k, m))
            {
                throw new Exception("Invalid key");
            }
            Matrix result = MatrixCalculation.multiplyMatrix(k, p);
            Matrix resultMod = MatrixCalculation.modMatrix(result, m);
            return resultMod;
        }

        // Decrypt the given cipher using the given key
        // The key k is a matrix
        // The cipher c is a vector or a matrix with one column
        public static Matrix Decrypt(Matrix k, Matrix c, int m)
        {
            if (!HelpService.IsValidKey(k, m))
            {
                throw new Exception("Invalid key");
            }
            Matrix kInverse = MatrixCalculation.inverseMatrix(k, m);
            Matrix result = MatrixCalculation.multiplyMatrix(kInverse, c);
            Matrix resultMod = MatrixCalculation.modMatrix(result, m);
            return resultMod;
        }
    }
}
