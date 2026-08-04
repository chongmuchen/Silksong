using System;
using System.Security.Cryptography;
using System.Text;

namespace TeamCherry.SharedUtils;

public static class Encryption
{
	private static readonly byte[] _keyArray = Encoding.UTF8.GetBytes("UKu52ePUBwetZ9wNX88o54dnfKRu0T1l");

	public static byte[] Encrypt(byte[] decryptedBytes)
	{
		using RijndaelManaged rijndaelManaged = new RijndaelManaged
		{
			Key = _keyArray,
			Mode = CipherMode.ECB,
			Padding = PaddingMode.PKCS7
		};
		return rijndaelManaged.CreateEncryptor().TransformFinalBlock(decryptedBytes, 0, decryptedBytes.Length);
	}

	public static byte[] Decrypt(byte[] encryptedBytes)
	{
		using RijndaelManaged rijndaelManaged = new RijndaelManaged
		{
			Key = _keyArray,
			Mode = CipherMode.ECB,
			Padding = PaddingMode.PKCS7
		};
		return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
	}

	public static string Encrypt(string unencryptedString)
	{
		byte[] array = Encrypt(Encoding.UTF8.GetBytes(unencryptedString));
		return Convert.ToBase64String(array, 0, array.Length);
	}

	public static string Decrypt(string encryptedString)
	{
		byte[] bytes = Decrypt(Convert.FromBase64String(encryptedString));
		return Encoding.UTF8.GetString(bytes);
	}
}
