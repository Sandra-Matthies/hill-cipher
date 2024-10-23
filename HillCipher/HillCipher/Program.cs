
using HillCipher.Models;
using HillCipher.Services;
using System;
using System.IO;
using System.Text.Json;

class Program
{
    public static int KEY_DIMENSION { get; private set; }

    public static void initKeyDimension()
    {
        var settings = HelpService.readFromRessource("settings.json");
        var jsonDocument = JsonDocument.Parse(settings);
        KEY_DIMENSION = jsonDocument.RootElement.GetProperty("keyDimension").GetInt32();

    }

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
        initKeyDimension();
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
        Matrix[] plain_matrices = HelpService.createTextMatrices(plain_numbers, key_matrix.Rows, alphabet_numbers);

        var cipher = Encrypt(key_matrix, plain_matrices, alphabet_numbers, m);
        Console.WriteLine("Cipher text:");
        Console.WriteLine(cipher);

        var cipher_numbers = Mapper.mapLettersByAlphabetToNumbers(cipher, alphabet_numbers);
        Matrix[] cipher_matrices = HelpService.createTextMatrices(cipher_numbers, key_matrix.Rows, alphabet_numbers);

        var plain = Decrypt(key_matrix, cipher_matrices, alphabet_numbers, m);

        Console.WriteLine("Plain text:");
        Console.WriteLine(plain);

        var res_plain_numbers = Mapper.mapLettersByAlphabetToNumbers(res_plain, alphabet_numbers);
        var res_cipher_numbers = Mapper.mapLettersByAlphabetToNumbers(res_cipher, alphabet_numbers);
        Matrix[] res_cipher_matrices = HelpService.createTextMatrices(res_cipher_numbers, KEY_DIMENSION, alphabet_numbers);
        Matrix[] res_plain_matrices = HelpService.createTextMatrices(res_plain_numbers, KEY_DIMENSION, alphabet_numbers);

        var plain_text_matrix = HelpService.getSquareMatrix(res_plain_matrices, KEY_DIMENSION);
        var cipher_text_matrix = HelpService.getSquareMatrix(res_cipher_matrices, KEY_DIMENSION);

        var key = KnownPlainTextAttack.Attack(plain_text_matrix, cipher_text_matrix, m);
        Console.WriteLine("Key:");
        var res_key_numbers = HelpService.createarrayFromMatrix(key);
        var key_text = Mapper.mapNumbersByAlphabetToLetters(res_key_numbers, alphabet_numbers);
        Console.WriteLine(key_text);

    }

}