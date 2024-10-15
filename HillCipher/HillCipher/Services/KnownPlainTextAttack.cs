using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HillCipher.Models;

namespace HillCipher.Services
{
    public class KnownPlainTextAttack
    {
        public static void Attack(Matrix plainText, Matrix cipherText, int m)
        {
            if (!isValidTextMatrix(plainText, cipherText))
            {
                throw new Exception("Invalid text matrix");
            }


            // Create system of equations for K = C*[P]^-1


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
            if (plainText.Cols != 1)
            {
                return false;
            }
            if (cipherText.Cols != 1)
            {
                return false;
            }
            if (plainText.Rows < plainText.Cols)
            {
                return false;
            }
            if (cipherText.Rows < cipherText.Cols)
            {
                return false;
            }
            return true;
        }


    }
}