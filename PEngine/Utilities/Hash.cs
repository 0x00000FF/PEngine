using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace PEngine.Utilities;

public static class Hash
{
    public static byte[] Digest(this string plaintext)
    {
        return Digest(plaintext.AsBytes());
    }
    
    public static byte[] Digest(this byte[] plaintext)
    {
        return plaintext.Digest(Array.Empty<byte>());
    }

    public static byte[] Digest(this byte[] plaintext, string appendingText)
    {
        return plaintext.Digest(appendingText.AsBytes());
    }
    public static byte[] Digest(this byte[] plaintext, byte[] appendingText)
    {
        var sha = SHA512.Create();
        var buff = plaintext;

        if (buff.Length > plaintext.Length)
        {
            buff = new byte[plaintext.Length + appendingText.Length];
            
            Array.Copy(plaintext, 0, buff, 0, plaintext.Length);
            Array.Copy(appendingText, 0, buff, plaintext.Length, appendingText.Length);
        }
        
        return sha.ComputeHash(buff);
    }
    
    // TODO: Hmac
    public static byte[] Hmac(this byte[] data, string key)
    {
        return Hmac(data, key.AsBytes());
    }

    public static byte[] Hmac(this byte[] data, byte[] key)
    {
        return HMACSHA512.HashData(key, data);
    }

    // TODO: Password
    public static string MakePassword(string password, string salt)
    {
        var passwordBytes = password.AsBytes();
        var saltBytes = salt.AsBase64Bytes();
        var iterations = 310000;
        
        return Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, iterations, 
                                    HashAlgorithmName.SHA512, 64)
                                  .AsBase64();
    }
}