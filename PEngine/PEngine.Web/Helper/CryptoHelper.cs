using System.Security.Cryptography;
using System.Text;

namespace PEngine.Web.Helper;

public static class CryptoHelper
{
    public static string ToBase64(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    public static byte[] FromBase64(this string str)
    {
        return Convert.FromBase64String(str);
    }

    public static byte[] ToBytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static byte[] Password(this string password, string salt)
    {
        var saltBytes = salt.FromBase64();
        return Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, 100000, HashAlgorithmName.SHA256, 32);
    }

    public static byte[] Random(int size = 4)
    {
        return RandomNumberGenerator.GetBytes(size);
    }

    public static Aes CreateAes(byte[] key)
    {
        var aes = Aes.Create();

        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Key = key;

        return aes;
    }
    
    public static byte[] Encrypt(this byte[] plaintext, byte[] key, byte[] iv)
    {
        return CreateAes(key).EncryptCbc(plaintext, iv);
    }

    public static byte[] Decrypt(this byte[] ciphertext, byte[] key, byte[] iv)
    {
        return CreateAes(key).DecryptCbc(ciphertext, iv);
    }
}