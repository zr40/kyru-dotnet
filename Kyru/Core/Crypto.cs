using System;
using System.Linq;
using System.Security.Cryptography;

namespace Kyru.Core
{
	internal static class Crypto
	{
		private static readonly SHA1CryptoServiceProvider hash = new SHA1CryptoServiceProvider();

		internal const int RsaPublicKeySize = 2048;
		internal static void GenerateKeyPair(string username, string password, out byte[] privateKey, out byte[] publicKey)
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
		internal static byte[] Hash(byte[] data)
		{
			return hash.ComputeHash(data);
		}

		internal static byte[] Sign(byte[] data, byte[] privateKey)
		{
			throw new NotImplementedException();
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