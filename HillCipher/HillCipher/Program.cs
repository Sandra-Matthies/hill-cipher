
using HillCipher.Models;
using HillCipher.Services;

class Program
{
    static void Main(string[] args)
    {
        string res_key = HelpService.readFromRessource("Key.txt");
        string res_plain = HelpService.readFromRessource("PlainTextExample.txt");
        //string res_cipher = HelpService.readFromRessource("CipherTextExample.txt");
        string alphabet = HelpService.readFromRessource("Alphabet.txt");
        int m = alphabet.Length;
        var alphabet_numbers = Mapper.mapAlphabetToNumbers(alphabet);
        var key_numbers = Mapper.mapLettersByAlphabetToNumbers(res_key, alphabet_numbers);
        var key_matrix = HelpService.createKeyMatrix(key_numbers);

        key_matrix.Print();

        if (!HelpService.IsValidKey(key_matrix, m))
        {
            throw new Exception("Invalid key");
        }

        var plain_numbers = Mapper.mapLettersByAlphabetToNumbers(res_plain, alphabet_numbers);
        Matrix[] plain_matrices = HelpService.createTextMatrices(plain_numbers, key_matrix.Rows);

        Matrix[] cipher_matrices = new Matrix[plain_matrices.Length];
        for (int i = 0; i < plain_matrices.Length; i++)
        {
            cipher_matrices[i] = HillService.Encrypt(key_matrix, plain_matrices[i], m);
        }

        var cipher_matrix = HelpService.mergeCipherText(cipher_matrices, m);
        var cipher_numbers = new int[cipher_matrix.Rows];

        for (int i = 0; i < cipher_matrix.Rows; i++)
        {
            cipher_numbers[i] = cipher_matrix.Data[i, 0];
        }

        var cipher_text = Mapper.mapNumbersByAlphabetToLetters(cipher_numbers, alphabet_numbers);
        Console.WriteLine("Cipher text:");
        Console.WriteLine(cipher_text);

        Matrix[] res = new Matrix[cipher_matrices.Length];
        for(int i=0; i < res.Length; i++)
        {
            res[i] = HillService.Decrypt(key_matrix, cipher_matrices[i], m);
        }

        var res_matrix = HelpService.mergeCipherText(res, m);
        var res_numbers = new int[res_matrix.Rows];

        for (int i = 0; i < res_matrix.Rows; i++)
        {
            res_numbers[i] = res_matrix.Data[i, 0];
        }

        var res_text = Mapper.mapNumbersByAlphabetToLetters(res_numbers, alphabet_numbers);
        Console.WriteLine("Decrypted text:");
        Console.WriteLine(res_text);


    }

}