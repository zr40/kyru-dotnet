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
		internal void TestRSA()
		{
			var data = new byte[32];
			var r = new Random();

			var user = new byte[64];
			var pass = new byte[64];
			r.NextBytes(user);
			r.NextBytes(pass);

			var rsaKeyPair = Crypto.DeriveRsaKey(user, pass);

			for (int i = 0; i < 10000; i++)
			{
				r.NextBytes(data);
				Assert.AreEqual(data.Length, Crypto.DecryptRsa(Crypto.EncryptRsa(data, rsaKeyPair.Public), rsaKeyPair).Length);
			}
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

			var decrypted = Crypto.DecryptRsa(encrypted, keyPair);
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

			var signature = Crypto.Sign(data, keyPair);
			Assert.IsTrue(Crypto.VerifySignature(data, keyPair.Public, signature));

			signature[0] = (byte) ~signature[0];
			Assert.IsFalse(Crypto.VerifySignature(data, keyPair.Public, signature));

			signature[0] = (byte) ~signature[0];
			data[0] = (byte) ~data[0];
			Assert.IsFalse(Crypto.VerifySignature(data, keyPair.Public, signature));
		}
	}
}