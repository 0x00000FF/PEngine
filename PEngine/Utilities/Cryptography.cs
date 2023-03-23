using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PEngine.Utilities;

public static class Cryptography
{
    public static byte[] Random(int size)
    {
        if (size < 0)
        {
            return Array.Empty<byte>();
        }
        
        return RandomNumberGenerator.GetBytes(size);
    }

    public static byte[] AsBytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static string AsString(this byte[] bytes)
    {
        return AsString(bytes, Encoding.UTF8);
    }
    
    public static string AsString(this byte[] bytes, Encoding encoding)
    {
        return encoding.GetString(bytes);
    }

    public static string AsBase64(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    public static byte[] AsBase64Bytes(this string str)
    {
        return Convert.FromBase64String(str);
    }

    public static byte[] EncryptSymmetric(byte[] plaintext, byte[] key, byte[] iv)
    {
        var defaultAes = Aes.Create("AES-256");

        if (defaultAes is null)
            return Array.Empty<byte>();
        
        return EncryptSymmetric(defaultAes, plaintext, key, iv);
    }

    public static byte[] EncryptSymmetric(SymmetricAlgorithm symmetricAlgorithm, byte[] plaintext, byte[] key, byte[] iv)
    {
        return symmetricAlgorithm.EncryptCbc(plaintext, iv);
    }

    public static byte[] DecryptSymmetric(byte[] ciphertext, byte[] key, byte[] iv)
    {
        var defaultAes = Aes.Create("AES-256");

        if (defaultAes is null)
            return Array.Empty<byte>();

        return DecryptSymmetric(defaultAes, ciphertext, key, iv);
    }

    public static byte[] DecryptSymmetric(SymmetricAlgorithm symmetricAlgorithm, byte[] ciphertext, byte[] key, byte[] iv)
    {
        return symmetricAlgorithm.DecryptCbc(ciphertext, iv);
    }

    public static byte[] EncryptAsymmetric()
    {
        throw new NotImplementedException();
    }

    public static byte[] DecryptAsymmetric()
    {
        throw new NotImplementedException();
    }

    public static byte[] Sign(this byte[] data, byte[] key)
    {
        var defaultRsa = RSA.Create();
        defaultRsa.ImportRSAPrivateKey(key, out _);

        return X509SignatureGenerator.CreateForRSA(defaultRsa, RSASignaturePadding.Pkcs1)
                                      .SignData(data, HashAlgorithmName.SHA512);
    }

    public static bool Verify(this byte[] data, byte[] signature, byte[] publicKey)
    {
        var defaultRsa = RSA.Create();
        defaultRsa.ImportRSAPublicKey(publicKey, out _);

        return defaultRsa.VerifyData(data, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
    }
}