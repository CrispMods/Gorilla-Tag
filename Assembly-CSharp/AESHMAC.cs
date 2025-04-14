using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// Token: 0x0200069F RID: 1695
public static class AESHMAC
{
	// Token: 0x06002A13 RID: 10771 RVA: 0x000D10A0 File Offset: 0x000CF2A0
	public static byte[] NewKey()
	{
		byte[] array = new byte[32];
		AESHMAC.gRNG.GetBytes(array);
		return array;
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x000D10C1 File Offset: 0x000CF2C1
	public static string SimpleEncrypt(string plaintext, byte[] key, byte[] auth, byte[] salt = null)
	{
		if (string.IsNullOrEmpty(plaintext))
		{
			throw new ArgumentNullException("plaintext");
		}
		return Convert.ToBase64String(AESHMAC.SimpleEncrypt(Encoding.UTF8.GetBytes(plaintext), key, auth, salt));
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x000D10F0 File Offset: 0x000CF2F0
	public static string SimpleDecrypt(string ciphertext, byte[] key, byte[] auth, int saltLength = 0)
	{
		if (string.IsNullOrWhiteSpace(ciphertext))
		{
			throw new ArgumentNullException("ciphertext");
		}
		byte[] array = AESHMAC.SimpleDecrypt(Convert.FromBase64String(ciphertext), key, auth, saltLength);
		if (array != null)
		{
			return Encoding.UTF8.GetString(array);
		}
		return null;
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x000D112F File Offset: 0x000CF32F
	public static string SimpleEncryptWithKey(string plaintext, string key, byte[] salt = null)
	{
		if (string.IsNullOrEmpty(plaintext))
		{
			throw new ArgumentNullException("plaintext");
		}
		return Convert.ToBase64String(AESHMAC.SimpleEncryptWithKey(Encoding.UTF8.GetBytes(plaintext), key, salt));
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x000D115C File Offset: 0x000CF35C
	public static string SimpleDecryptWithKey(string ciphertext, string key, int saltLength = 0)
	{
		if (string.IsNullOrWhiteSpace(ciphertext))
		{
			throw new ArgumentNullException("ciphertext");
		}
		byte[] array = AESHMAC.SimpleDecryptWithKey(Convert.FromBase64String(ciphertext), key, saltLength);
		if (array != null)
		{
			return Encoding.UTF8.GetString(array);
		}
		return null;
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x000D119C File Offset: 0x000CF39C
	public static byte[] SimpleEncrypt(byte[] plaintext, byte[] key, byte[] auth, byte[] salt = null)
	{
		if (key == null || key.Length != 32)
		{
			throw new ArgumentException(string.Format("Key must be {0} bits", 256), "key");
		}
		if (auth == null || auth.Length != 32)
		{
			throw new ArgumentException(string.Format("Auth must be {0} bits", 256), "auth");
		}
		if (plaintext == null || plaintext.Length < 1)
		{
			throw new ArgumentNullException("plaintext");
		}
		if (salt == null)
		{
			salt = new byte[0];
		}
		byte[] iv;
		byte[] buffer;
		using (AesManaged aesManaged = new AesManaged
		{
			KeySize = 256,
			BlockSize = 128,
			Mode = CipherMode.CBC,
			Padding = PaddingMode.PKCS7
		})
		{
			aesManaged.GenerateIV();
			iv = aesManaged.IV;
			using (ICryptoTransform cryptoTransform = aesManaged.CreateEncryptor(key, iv))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(cryptoStream))
						{
							binaryWriter.Write(plaintext);
						}
					}
					buffer = memoryStream.ToArray();
				}
			}
		}
		byte[] result;
		using (HMACSHA256 hmacsha = new HMACSHA256(auth))
		{
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				using (BinaryWriter binaryWriter2 = new BinaryWriter(memoryStream2))
				{
					binaryWriter2.Write(salt);
					binaryWriter2.Write(iv);
					binaryWriter2.Write(buffer);
					binaryWriter2.Flush();
					byte[] buffer2 = hmacsha.ComputeHash(memoryStream2.ToArray());
					binaryWriter2.Write(buffer2);
				}
				result = memoryStream2.ToArray();
			}
		}
		return result;
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x000D13A8 File Offset: 0x000CF5A8
	public static byte[] SimpleDecrypt(byte[] ciphertext, byte[] key, byte[] auth, int saltLength = 0)
	{
		if (key == null || key.Length != 32)
		{
			throw new ArgumentException(string.Format("Key must be {0} bits", 256), "key");
		}
		if (auth == null || auth.Length != 32)
		{
			throw new ArgumentException(string.Format("Auth must be {0} bits", 256), "auth");
		}
		if (ciphertext == null || ciphertext.Length == 0)
		{
			throw new ArgumentNullException("ciphertext");
		}
		byte[] result;
		using (HMACSHA256 hmacsha = new HMACSHA256(auth))
		{
			byte[] array = new byte[hmacsha.HashSize / 8];
			byte[] array2 = hmacsha.ComputeHash(ciphertext, 0, ciphertext.Length - array.Length);
			int num = 16;
			if (ciphertext.Length < array.Length + saltLength + num)
			{
				result = null;
			}
			else
			{
				Array.Copy(ciphertext, ciphertext.Length - array.Length, array, 0, array.Length);
				int num2 = 0;
				for (int i = 0; i < array.Length; i++)
				{
					num2 |= (int)(array[i] ^ array2[i]);
				}
				if (num2 != 0)
				{
					result = null;
				}
				else
				{
					using (AesManaged aesManaged = new AesManaged
					{
						KeySize = 256,
						BlockSize = 128,
						Mode = CipherMode.CBC,
						Padding = PaddingMode.PKCS7
					})
					{
						byte[] array3 = new byte[num];
						Array.Copy(ciphertext, saltLength, array3, 0, array3.Length);
						using (ICryptoTransform cryptoTransform = aesManaged.CreateDecryptor(key, array3))
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
								{
									using (BinaryWriter binaryWriter = new BinaryWriter(cryptoStream))
									{
										binaryWriter.Write(ciphertext, saltLength + array3.Length, ciphertext.Length - saltLength - array3.Length - array.Length);
									}
								}
								result = memoryStream.ToArray();
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x000D1604 File Offset: 0x000CF804
	public static byte[] SimpleEncryptWithKey(byte[] plaintext, string key, byte[] salt = null)
	{
		if (salt == null)
		{
			salt = new byte[0];
		}
		if (string.IsNullOrWhiteSpace(key) || key.Length < 12)
		{
			throw new ArgumentException(string.Format("Key must be at least {0} characters", 12), "key");
		}
		if (plaintext == null || plaintext.Length == 0)
		{
			throw new ArgumentNullException("plaintext");
		}
		byte[] array = new byte[16 + salt.Length];
		Array.Copy(salt, array, salt.Length);
		int num = salt.Length;
		byte[] bytes;
		using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, 8, 10000))
		{
			byte[] salt2 = rfc2898DeriveBytes.Salt;
			bytes = rfc2898DeriveBytes.GetBytes(32);
			Array.Copy(salt2, 0, array, num, salt2.Length);
			num += salt2.Length;
		}
		byte[] bytes2;
		using (Rfc2898DeriveBytes rfc2898DeriveBytes2 = new Rfc2898DeriveBytes(key, 8, 10000))
		{
			byte[] salt3 = rfc2898DeriveBytes2.Salt;
			bytes2 = rfc2898DeriveBytes2.GetBytes(32);
			Array.Copy(salt3, 0, array, num, salt3.Length);
		}
		return AESHMAC.SimpleEncrypt(plaintext, bytes, bytes2, array);
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x000D171C File Offset: 0x000CF91C
	public static byte[] SimpleDecryptWithKey(byte[] ciphertext, string key, int saltLength = 0)
	{
		if (string.IsNullOrWhiteSpace(key) || key.Length < 12)
		{
			throw new ArgumentException(string.Format("Key must be at least {0} characters", 12), "key");
		}
		if (ciphertext == null || ciphertext.Length == 0)
		{
			throw new ArgumentNullException("ciphertext");
		}
		byte[] array = new byte[8];
		byte[] array2 = new byte[8];
		Array.Copy(ciphertext, saltLength, array, 0, array.Length);
		Array.Copy(ciphertext, saltLength + array.Length, array2, 0, array2.Length);
		byte[] key2 = AESHMAC.Rfc2898DeriveBytes(key, array, 10000, 32);
		byte[] auth = AESHMAC.Rfc2898DeriveBytes(key, array2, 10000, 32);
		return AESHMAC.SimpleDecrypt(ciphertext, key2, auth, array.Length + array2.Length + saltLength);
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x000D17C4 File Offset: 0x000CF9C4
	private static byte[] Rfc2898DeriveBytes(string password, byte[] salt, int iterations, int numBytes)
	{
		byte[] bytes;
		using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
		{
			bytes = rfc2898DeriveBytes.GetBytes(numBytes);
		}
		return bytes;
	}

	// Token: 0x04002F9C RID: 12188
	private static readonly RandomNumberGenerator gRNG = RandomNumberGenerator.Create();

	// Token: 0x04002F9D RID: 12189
	public const int BlockBitSize = 128;

	// Token: 0x04002F9E RID: 12190
	public const int KeyBitSize = 256;

	// Token: 0x04002F9F RID: 12191
	public const int SaltBitSize = 64;

	// Token: 0x04002FA0 RID: 12192
	public const int Iterations = 10000;

	// Token: 0x04002FA1 RID: 12193
	public const int MinPasswordLength = 12;
}
