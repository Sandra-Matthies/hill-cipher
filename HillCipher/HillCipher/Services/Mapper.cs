using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillCipher.Services
{
    internal class Mapper
    {

        // Map the given aplpbet to an array of numbers
        public static int[] mapAlphabetToNumber(string aplphabet)
        {
            int[] numbers = new int[aplphabet.Length];
            for (int i = 0; i < aplphabet.Length; i++)
            {
                numbers[i] = aplphabet[i] - 65;
            }
            return numbers;
        }

        public static string mapNumberToAlphabet(int[] numbers)
        {
            string alphabet = "";
            for (int i = 0; i < numbers.Length; i++)
            {
                alphabet += (char)(numbers[i] + 65);
            }
            return alphabet;
        }
    }
}
