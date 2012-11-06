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

		internal static byte[] EncryptRsa(byte[] data, byte[] publicKey)
		{
			throw new NotImplementedException();
		}
		
		internal static byte[] DecryptRsa(byte[] data, byte[] privateKey)
		{
			throw new NotImplementedException();
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
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportCspBlob(privateKey);
			return rsa.Decrypt(Hash(data), true);
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
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportCspBlob(publicKey);
			var dataHash = Hash(data);
			var signHash = rsa.Encrypt(signature, true);
			if (dataHash.Length != signHash.Length) return false;

			return !dataHash.Where((t, i) => signHash[i] != t).Any();
		}
	}
}