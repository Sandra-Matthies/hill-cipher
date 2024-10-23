using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            // Create system of equations for K = C * P^-1 mod m

            // Create the inverse of the plain text matrix
            Matrix inversePlainText = MatrixCalculation.inverseMatrix(plainText, m);


            var key = MatrixCalculation.multiplyMatrix(cipherText, inversePlainText);

            key = MatrixCalculation.modMatrix(key, m);
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