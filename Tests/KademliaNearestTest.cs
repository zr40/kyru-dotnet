using System;

using Kyru;
using Kyru.Network;

using MbUnit.Framework;

namespace Tests
{
	internal sealed class KademliaNearestTest
	{
		private Node node;
		private Kademlia kademlia;

		[SetUp]
		internal void PrepareKademlia()
		{
			node = new Node(null);
			kademlia = node.Kademlia;
		}

		[TearDown]
		internal void TearDown()
		{
			node.Dispose();
		}

		[Test]
		internal void TestNearest([Column(0, 1, 20, 100)] int contacts)
		{
			TestHelper.PrepareFakeContacts(kademlia, contacts);

			var result = kademlia.NearestContactsTo(KademliaId.RandomId, null);
			result.ForEach(n => this.Log(n.NodeId.ToString()));
			Assert.AreEqual(Math.Min(contacts, 20), result.Count);
		}
	}
}