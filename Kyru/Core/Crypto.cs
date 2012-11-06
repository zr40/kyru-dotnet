using System;
using System.Linq;
using System.Security.Cryptography;

namespace Kyru.Core
{
	internal static class Crypto
	{
		private static readonly SHA1CryptoServiceProvider hash = new SHA1CryptoServiceProvider();

		internal const int RsaPublicKeySize = 2048;

		/// <summary>
		/// Generates a private key from a username and password
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Password</param>
		/// <returns>The private key</returns>
		internal static byte[] GeneratePrivateKey(string username, string password)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Extracts the public key from private key data
		/// </summary>
		/// <param name="privateKey">The private key</param>
		/// <returns>The public key</returns>
		internal static byte[] ExtractPublicKey(byte[] privateKey)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Encrypts data with an RSA public key
		/// </summary>
		/// <param name="data">Data to encrypt</param>
		/// <param name="publicKey">Public key to use for decryption</param>
		/// <returns>The encrypted data</returns>
		internal static byte[] EncryptRsa(byte[] data, byte[] publicKey)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportCspBlob(publicKey);
			return rsa.Encrypt(data, true);
		}
		
		/// <summary>
		/// Decrypts data with an RSA private key
		/// </summary>
		/// <param name="data">Data to decrypt</param>
		/// <param name="privateKey">Private key to use for decryption</param>
		/// <returns>The decrypted data</returns>
		internal static byte[] DecryptRsa(byte[] data, byte[] privateKey)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportCspBlob(privateKey);
			return rsa.Decrypt(data, true);
		}

		internal static byte[] EncryptAes(byte[] data, byte[] encryptionKey)
		{
			throw new NotImplementedException();
		}

		internal static byte[] DecryptAes(byte[] data, byte[] encryptionKey)
		{
			throw new NotImplementedException();
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
			return DecryptRsa(Hash(data),privateKey);
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
			var dataHash = Hash(data);
			var signHash = EncryptRsa(signature, publicKey);

			return  (dataHash.Length == signHash.Length) && signHash.SequenceEqual(dataHash);
		}
	}
}