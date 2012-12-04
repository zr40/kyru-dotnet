using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Emil.GMP;

namespace Kyru.Utilities
{
	internal static class Crypto
	{
		internal const int RsaPublicKeySize = 2048 / 8;
		private const int Iterations = 10000;
		private static readonly SHA1CryptoServiceProvider hash = new SHA1CryptoServiceProvider();

		private const int AesHeaderSize = sizeof(int);

		internal struct RsaKeyPair
		{
			internal byte[] Public;
			internal byte[] Private;

			internal RsaKeyPair(byte[] pubKey, byte[] privKey) : this()
			{
				Public = pubKey;
				Private = privKey;
			}
		}

		/// <summary>
		/// Derives an RSA from a username and password
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Password</param>
		/// <returns>The derived private and public key</returns>
		internal static RsaKeyPair DeriveRsaKey(byte[] username, byte[] password)
		{
			Console.WriteLine("DeriveRsaKey: deriving key...");

			var derived = PBKDF2(password, username, RsaPublicKeySize);
			var one = new byte[RsaPublicKeySize / 2];
			var two = new byte[RsaPublicKeySize / 2];
			Array.Copy(derived, 0, one, 0, RsaPublicKeySize / 2);
			Array.Copy(derived, RsaPublicKeySize / 2, two, 0, RsaPublicKeySize / 2);

			Console.WriteLine("DeriveRsaKey: finding prime 1...");

			var p = new BigInt(one).NextPrimeGMP();
			Console.WriteLine("DeriveRsaKey: finding prime 2...");
			var q = new BigInt(two).NextPrimeGMP();

			Console.WriteLine("DeriveRsaKey: creating key pair...");

			var n = p * q;
			var φn = (p - 1) * (q - 1);
			var e = (BigInt) 0x10001;
			var d = e.InvertMod(φn);

			Console.WriteLine("DeriveRsaKey: done");

			return new RsaKeyPair(n.ToByteArray(), d.ToByteArray());
		}

		private static byte[] PBKDF2(byte[] P, byte[] S, int dkLen)
		{
			const int iterations = 10000;

			var hashAlg = new HMACSHA512();
			var hLen = hashAlg.HashSize / 8;

			if (dkLen > (uint.MaxValue) * hLen)
			{
				throw new ArgumentException("Output size too long");
			}
			if (dkLen % hLen != 0)
			{
				// shouldn't happen in kyru
				throw new NotImplementedException();
			}

			var l = (dkLen - 1) / hLen + 1;
			//var r = dkLen - (l - 1) * hLen;

			var DK = new byte[dkLen];

			for (uint i = 1; i <= l; i++)
			{
				// F
				var block = new byte[S.Length + 4];
				S.CopyTo(block, 0);
				BitConverter.GetBytes(i).CopyTo(block, S.Length);

				// iteration 1
				hashAlg = new HMACSHA512(block);
				block = hashAlg.ComputeHash(P);

				// following iterations
				for (int x = 2; x <= iterations; x++)
				{
					hashAlg = new HMACSHA512(block);
					block = hashAlg.ComputeHash(P);
				}

				block.CopyTo(DK, (i - 1) * hLen);
			}

			return DK;
		}

		/// <summary>
		/// Generates an AES key
		/// </summary>
		/// <returns>256 bit AES key</returns>
		internal static byte[] GenerateAesKey()
		{
			return Random.Bytes(32);
		}

		/// <summary>
		/// Generates an IV
		/// </summary>
		/// <returns>an IV</returns>
		internal static byte[] GenerateIV()
		{
			return Random.Bytes(16);
		}

		/// <summary>
		/// Extracts the public key from RSA private key data
		/// </summary>
		/// <param name="privateKey">The private key</param>
		/// <returns>The public key</returns>
		internal static RSAParameters ExtractPublicKey(RSAParameters privateKey)
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(privateKey);
				return rsa.ExportParameters(false);
			}
		}

		/// <summary>
		/// Encrypts data with an RSA public key
		/// </summary>
		/// <param name="data">Data to encrypt</param>
		/// <param name="publicKey">Public key to use for decryption</param>
		/// <returns>The encrypted data</returns>
		internal static byte[] EncryptRsa(byte[] data, RSAParameters publicKey)
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(publicKey);
				return rsa.Encrypt(data, true);
			}
		}

		/// <summary>
		/// Decrypts data with an RSA private key
		/// </summary>
		/// <param name="data">Data to decrypt</param>
		/// <param name="privateKey">Private key to use for decryption</param>
		/// <returns>The decrypted data</returns>
		internal static byte[] DecryptRsa(byte[] data, RSAParameters privateKey)
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(privateKey);
				return rsa.Decrypt(data, true);
			}
		}

		/// <summary>
		/// Encrypts data through AES with a provided EncryptionKey, because we use a null IV each key should only be used ONCE!
		/// </summary>
		/// <param name="data">Data to encrypt</param>
		/// <param name="encryptionKey">Key to use for encryption</param>
		/// <param name="IV">see http://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.iv(v=vs.95).aspx</param>
		/// <returns>The encrypted data</returns>
		internal static byte[] EncryptAes(byte[] data, byte[] encryptionKey, byte[] IV)
		{
			using (var aes = new AesCryptoServiceProvider {Padding = PaddingMode.ISO10126})
			using (var encryptor = aes.CreateEncryptor(encryptionKey, IV))
			using (var ms = new MemoryStream())
			{
				ms.Write(BitConverter.GetBytes(data.Length), 0, AesHeaderSize);
				using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
				{
					cs.Write(data, 0, data.Length);
					cs.FlushFinalBlock();
					return ms.ToArray();
				}
			}
		}

		/// <summary>
		/// Decrypts AES encrypted data
		/// </summary>
		/// <param name="data">Data to decrypt</param>
		/// <param name="encryptionKey">Key to decrypt the data with</param>
		/// <returns>The decrypted data</returns>
		internal static byte[] DecryptAes(byte[] data, byte[] encryptionKey, byte[] IV)
		{
			using (var aes = new AesCryptoServiceProvider {Padding = PaddingMode.ISO10126})
			using (var decryptor = aes.CreateDecryptor(encryptionKey, IV))
			using (var ms = new MemoryStream(data))
			{
				var lengthBytes = new byte[AesHeaderSize];
				ms.Read(lengthBytes, 0, AesHeaderSize);
				var length = BitConverter.ToInt32(lengthBytes, 0);

				using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
				{
					var decryptedData = new byte[length];
					cs.Read(decryptedData, 0, length);
					return decryptedData;
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
		internal static byte[] Sign(byte[] data, RSAParameters privateKey)
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
		internal static bool VerifySignature(byte[] data, RSAParameters publicKey, byte[] signature)
		{
			byte[] dataHash = Hash(data);
			byte[] signHash = EncryptRsa(signature, publicKey);

			return (dataHash.Length == signHash.Length) && signHash.SequenceEqual(dataHash);
		}
	}
}