using Kyru.Network;

using MbUnit.Framework;

namespace Tests
{
	internal sealed class KademliaIdTest
	{
		[Test, ExpectedInvalidOperationException]
		public void AllZeroIdMustNotHaveKademliaBucket()
		{
			var id = new KademliaId(new byte[20]);
			id.KademliaBucket();
		}

		[Test]
		public void TestKademliaBucket()
		{
			var bytes = new byte[20];

			bytes[19] = 0xff;
			var id = new KademliaId(bytes);
			Assert.AreEqual(8, id.KademliaBucket());

			bytes[19] = 0x89;
			id = new KademliaId(bytes);
			Assert.AreEqual(8, id.KademliaBucket());

			bytes[19] = 0x01;
			id = new KademliaId(bytes);
			Assert.AreEqual(1, id.KademliaBucket());

			bytes[19] = 0x74;
			id = new KademliaId(bytes);
			Assert.AreEqual(7, id.KademliaBucket());

			bytes[10] = 0xff;
			id = new KademliaId(bytes);
			Assert.AreEqual(79, id.KademliaBucket());

			bytes[0] = 0xff;
			id = new KademliaId(bytes);
			Assert.AreEqual(159, id.KademliaBucket());
		}
	}
}