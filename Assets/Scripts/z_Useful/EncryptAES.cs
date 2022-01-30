using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

public static class EncryptAES
{
    public static string GenKey()
	{
        RijndaelManaged myRijndaelManaged = new RijndaelManaged(); 
        myRijndaelManaged.Mode = CipherMode.CBC; 
        myRijndaelManaged.Padding = PaddingMode.PKCS7; 
        myRijndaelManaged.GenerateIV(); 
        myRijndaelManaged.GenerateKey(); 
        
        string newKey = ByteArrayToHexString(myRijndaelManaged.Key); 
        string newinitVector = ByteArrayToHexString(myRijndaelManaged.IV);
        return newKey;
    }

    public static string ByteArrayToHexString(byte[] ba) 
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2); 
        foreach (byte b in ba) hex.AppendFormat("{0:x2}", b); 
        return hex.ToString(); 
    }

    public static string Decrypt256(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 256;

        rijndaelCipher.BlockSize = 256;

        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[32];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

        return Encoding.UTF8.GetString(plainText);

    }

    public static string Encrypt256(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 256;

        rijndaelCipher.BlockSize = 256;

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[32];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));

    }
}