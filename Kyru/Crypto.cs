using System;
using System.Security.Cryptography;

namespace Kyru
{
	internal static class Crypto
	{
		internal static readonly SHA1CryptoServiceProvider Hash = new SHA1CryptoServiceProvider();

		internal const int RsaPublicKeySize = 2048;

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

		internal static byte[] Sign(byte[] data, byte[] privateKey)
		{
			throw new NotImplementedException();
		}

		internal static bool VerifySignature(byte[] data, byte[] publicKey, byte[] signature)
		{
			throw new NotImplementedException();
		}
	}
}