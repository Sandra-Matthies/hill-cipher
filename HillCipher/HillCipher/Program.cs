
using HillCipher.Models;
using HillCipher.Services;

class Program
{

    static string Encrypt(Matrix key_matrix, Matrix[] plain_matrices, Dictionary<string, int> alphabet_numbers, int m)
    {
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
        return cipher_text;
    }

    static string Decrypt(Matrix key_matrix, Matrix[] cipher_matrices, Dictionary<string, int> alphabet_numbers, int m)
    {
        Matrix[] res = new Matrix[cipher_matrices.Length];
        for (int i = 0; i < res.Length; i++)
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
        return res_text;
    }

    static void Main(string[] args)
    {
        string res_key = HelpService.readFromRessource("Key.txt");
        string res_plain = HelpService.readFromRessource("PlainTextExample.txt");
        string res_cipher = HelpService.readFromRessource("CipherTextExample.txt");
        string alphabet = HelpService.readFromRessource("Alphabet.txt");
        int m = alphabet.Length;
        var alphabet_numbers = Mapper.mapAlphabetToNumbers(alphabet);
        var key_numbers = Mapper.mapLettersByAlphabetToNumbers(res_key, alphabet_numbers);
        var key_matrix = HelpService.createKeyMatrix(key_numbers);
        if (!HelpService.IsValidKey(key_matrix, m))
        {
            throw new Exception("Invalid key");
        }

        var plain_numbers = Mapper.mapLettersByAlphabetToNumbers(res_plain, alphabet_numbers);
        Matrix[] plain_matrices = HelpService.createTextMatrices(plain_numbers, key_matrix.Rows);

        var cipher = Encrypt(key_matrix, plain_matrices, alphabet_numbers, m);
        Console.WriteLine("Cipher text:");
        Console.WriteLine(cipher);

        var cipher_numbers = Mapper.mapLettersByAlphabetToNumbers(cipher, alphabet_numbers);
        Matrix[] cipher_matrices = HelpService.createTextMatrices(cipher_numbers, key_matrix.Rows);

        var plain = Decrypt(key_matrix, cipher_matrices, alphabet_numbers, m);

        Console.WriteLine("Plain text:");
        Console.WriteLine(plain);
        /*var plain_text_matrix = HelpService.createMatrixFromNumbers(plain_numbers);
        var cipher_text_matrix = HelpService.createMatrixFromNumbers(plain_numbers);

        var key = KnownPlainTextAttack.Attack(plain_text_matrix, cipher_text_matrix, m);
        Console.WriteLine("Key:");
        var res_key_numbers = HelpService.createarrayFromMatrix(key);
        var key_text = Mapper.mapNumbersByAlphabetToLetters(res_key_numbers, alphabet_numbers);
        key.Print();
        Console.WriteLine(key_text);*/

    }

}