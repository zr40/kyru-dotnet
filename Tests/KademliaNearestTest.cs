using System;
using System.Net;

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
			node = new Node();
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
			Prepare(contacts);
			var result = kademlia.NearestContactsTo(KademliaId.RandomId);
			Assert.AreEqual(Math.Min(contacts, 20), result.Count);
		}

		private void Prepare(int contacts)
		{
			for (int i = 0; i < contacts; i++)
			{
				var id = KademliaId.RandomId;
				var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), id);
				var message = new UdpMessage();
				kademlia.HandleIncomingRequest(ni, message);

				if (message.ResponseCallback == null)
					continue;

				var response = new UdpMessage();
				response.ResponseId = message.RequestId;
				response.SenderNodeId = id;
				message.ResponseCallback(response);
			}
		}
	}
}