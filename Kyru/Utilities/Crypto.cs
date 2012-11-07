using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Kyru.Utilities
{
	internal static class Crypto
	{
		internal const int RsaPublicKeySize = 2048;
		private const int Iterations = 10000;
		private static readonly SHA1CryptoServiceProvider hash = new SHA1CryptoServiceProvider();

		/// <summary>
		/// Generates an RSA from a username and password
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Password</param>
		/// <returns>The private key</returns>
		internal static byte[] GenerateRsaKey(byte[] username, byte[] password)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Generates a valid AES key
		/// </summary>
		/// <returns>valid 256bit AES key</returns>
		internal static byte[] GenerateAesKey()
		{
			return Random.Bytes(32);
		}

		/// <summary>
		/// Extracts the public key from RSA private key data
		/// </summary>
		/// <param name="privateKey">The private key</param>
		/// <returns>The public key</returns>
		internal static byte[] ExtractPublicKey(byte[] privateKey)
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportCspBlob(privateKey);
				return rsa.ExportCspBlob(false);
			}
		}

		/// <summary>
		/// Encrypts data with an RSA public key
		/// </summary>
		/// <param name="data">Data to encrypt</param>
		/// <param name="publicKey">Public key to use for decryption</param>
		/// <returns>The encrypted data</returns>
		internal static byte[] EncryptRsa(byte[] data, byte[] publicKey)
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportCspBlob(publicKey);
				return rsa.Encrypt(data, true);
			}
		}

		/// <summary>
		/// Decrypts data with an RSA private key
		/// </summary>
		/// <param name="data">Data to decrypt</param>
		/// <param name="privateKey">Private key to use for decryption</param>
		/// <returns>The decrypted data</returns>
		internal static byte[] DecryptRsa(byte[] data, byte[] privateKey)
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportCspBlob(privateKey);
				return rsa.Decrypt(data, true);
			}
		}

		/// <summary>
		/// Encrypts data through AES with a provided EncryptionKey, because we use a null IV each key should only be used ONCE!
		/// </summary>
		/// <param name="data">Data to encrypt</param>
		/// <param name="encryptionKey">Key to use for encryption</param>
		/// <returns>The encrypted data</returns>
		internal static byte[] EncryptAes(byte[] data, byte[] encryptionKey)
		{
			using (var aes = new AesCryptoServiceProvider {Padding = PaddingMode.None})
			{
				using (var encryptor = aes.CreateEncryptor(encryptionKey, new byte[16]))
				{
					using (var ms = new MemoryStream())
					{
						using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
						{
							cs.Write(data, 0, data.Length);
							cs.FlushFinalBlock();
							return ms.ToArray();
						}
					}
				}
			}
		}

		/// <summary>
		/// Decrypts AES encrypted data
		/// </summary>
		/// <param name="data">Data to decrypt</param>
		/// <param name="encryptionKey">Key to decrypt the data with</param>
		/// <returns>The decrypted data</returns>
		internal static byte[] DecryptAes(byte[] data, byte[] encryptionKey)
		{
			using (var aes = new AesCryptoServiceProvider {Padding = PaddingMode.None})
			{
				using (var decryptor = aes.CreateDecryptor(encryptionKey, new byte[16]))
				{
					using (var ms = new MemoryStream(data))
					{
						using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
						{
							var decryptedData = new byte[data.Length];
							cs.Read(decryptedData, 0, data.Length);
							return decryptedData;
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns a Kyru spec hash of the data provided
		/// </summary>
		/// <param name="data">Data to hash</param>
		/// <returns>The Hash of the data</returns>
		internal static byte[] Hash(byte[] data)
		{
			return hash.ComputeHash(data);
		}

		/// <summary>
		/// Generates a signature for the data
		/// </summary>
		/// <param name="data">Data to sign</param>
		/// <param name="privateKey">Key to sign the data with</param>
		/// <returns>The signature of the hashed data</returns>
		internal static byte[] Sign(byte[] data, byte[] privateKey)
		{
			return DecryptRsa(Hash(data), privateKey);
		}

		/// <summary>
		/// Verifies a signature
		/// </summary>
		/// <param name="data">The data that is signed</param>
		/// <param name="publicKey">The public key to check the signature with</param>
		/// <param name="signature">The signature to verify</param>
		/// <returns></returns>
		internal static bool VerifySignature(byte[] data, byte[] publicKey, byte[] signature)
		{
			byte[] dataHash = Hash(data);
			byte[] signHash = EncryptRsa(signature, publicKey);

			return (dataHash.Length == signHash.Length) && signHash.SequenceEqual(dataHash);
		}
	}
}