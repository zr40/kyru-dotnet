using System.Net;

using Kyru.Network;
using Kyru.Network.UdpMessages;

using MbUnit.Framework;

namespace Tests
{
	internal sealed class NodeTest
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
		internal void RemoveFromKademliaWhenRequestTimeout()
		{
			var id = KademliaId.RandomId;
			var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 65432), id);
			var message = new UdpMessage();
			kademlia.HandleIncomingRequest(ni, message);
			var response = new UdpMessage();
			response.ResponseId = message.RequestId;
			response.SenderNodeId = id;

			message.ResponseCallback(response);
			Assert.AreEqual(1, kademlia.CurrentContacts);

			// actual test

			bool noResponse = false;
			message = new UdpMessage();
			message.PingRequest = new PingRequest();
			message.ResponseCallback = udpMessage => Assert.Fail("The response callback should not be called in this test");
			message.NoResponseCallback = () => noResponse = true;

			node.SendUdpMessage(message, ni.EndPoint, id);

			for (int i = 0; i < Node.TimeoutTicks; i++)
				node.TimerElapsed();
			Assert.AreEqual(1, kademlia.CurrentContacts);

			for (int i = 0; i < Node.TimeoutTicks; i++)
				node.TimerElapsed();
			Assert.IsTrue(noResponse, "The no-response callback should be called in this test");
			Assert.AreEqual(0, kademlia.CurrentContacts);
		}
	}
}