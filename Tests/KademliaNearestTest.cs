using System;
using System.Net;

using Kyru;
using Kyru.Network;
using Kyru.Network.Messages;

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
			kademlia = (Kademlia) Mirror.ForObject(node)["kademlia"].Value;
		}

		[TearDown]
		internal void TearDown()
		{
			node.Dispose();
		}

		[Test]
		internal void TestNearest([Column(0, 1, 2, 19, 20, 21, 100)] int contacts)
		{
			TestHelper.PrepareFakeContacts(kademlia, contacts);

			var result = kademlia.NearestContactsTo(KademliaId.RandomId, null);
			result.ForEach(n => this.Log(n.NodeId.ToString()));
			Assert.AreEqual(Math.Min(contacts, 20), result.Count);
		}

	}
}