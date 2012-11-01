using System.Net;

using Kyru.Network;
using Kyru.Network.Messages;

using MbUnit.Framework;

namespace Tests
{
	internal sealed class NodeTest
	{
		[Test]
		internal void RemoveFromKademliaWhenRequestTimeout()
		{
			var node = new Node();
			var kademlia = (Kademlia) Mirror.ForObject(node)["kademlia"].Value;

			var id = KademliaId.RandomId;
			var ni = new NodeInformation(new IPEndPoint(IPAddress.None, 65432), id);
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