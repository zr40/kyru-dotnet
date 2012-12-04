using Kyru.Utilities;

using MbUnit.Framework;

using Random = System.Random;

namespace Tests
{
	internal sealed class CryptoTest
	{
		[Test]
		internal void TestAES()
		{
			var key = Crypto.GenerateAesKey();
			var data = new byte[10001];
			new Random().NextBytes(data);
			var IV = Crypto.GenerateIV();
			Assert.AreElementsEqual(data, Crypto.DecryptAes(Crypto.EncryptAes(data, key, IV), key, IV));
		}

		[Test]
		internal void TestRSAEncryption()
		{
			var user = new byte[64];
			var pass = new byte[64];
			var r = new Random();
			r.NextBytes(user);
			r.NextBytes(pass);

			var keyPair = Crypto.DeriveRsaKey(user, pass);

			var data = new byte[64];
			r.NextBytes(data);

			var encrypted = Crypto.EncryptRsa(data, keyPair.Public);
			Assert.AreElementsNotEqual(data, encrypted);

			var decrypted = Crypto.DecryptRsa(encrypted, keyPair.Public, keyPair.Private);
			Assert.AreElementsEqual(data, decrypted);
		}

		[Test]
		internal void TestRSASigning()
		{
			var user = new byte[64];
			var pass = new byte[64];
			var r = new Random();
			r.NextBytes(user);
			r.NextBytes(pass);

			var keyPair = Crypto.DeriveRsaKey(user, pass);

			var data = new byte[64];
			r.NextBytes(data);

			var signature = Crypto.Sign(data, keyPair.Public, keyPair.Private);
			Assert.IsTrue(Crypto.VerifySignature(data, keyPair.Public, signature));

			signature[0] = (byte) ~signature[0];
			Assert.IsFalse(Crypto.VerifySignature(data, keyPair.Public, signature));

			signature[0] = (byte) ~signature[0];
			data[0] = (byte) ~data[0];
			Assert.IsFalse(Crypto.VerifySignature(data, keyPair.Public, signature));
		}
	}
}