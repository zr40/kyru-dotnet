﻿using System.IO;

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
			var iv = Crypto.GenerateIV();
			Assert.AreElementsEqual(data, Crypto.DecryptAes(Crypto.EncryptAes(data, key, iv), key, iv));
		}

		[Test]
		internal void TestAESStream()
		{
			var key = Crypto.GenerateAesKey();
			byte[] data, ddata;
			data = new byte[10001];
			new Random().NextBytes(data);
			var iv = Crypto.GenerateIV();

			using (MemoryStream ms = new MemoryStream(data), ems = new MemoryStream(), dms = new MemoryStream())
			{
				Crypto.EncryptAesStream(ms, ems, key, iv);

				ems.Seek(0,SeekOrigin.Begin);
				Crypto.DecryptAesStream(ems, dms, key, iv);
				ddata = dms.ToArray();
			}

			Assert.AreElementsEqual(data, ddata);
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

			r.NextBytes(data);

			data[data.Length-1] = 0;

			Assert.AreEqual(data.Length, Crypto.DecryptRsa(Crypto.EncryptRsa(data, rsaKeyPair.Public), rsaKeyPair).Length);
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