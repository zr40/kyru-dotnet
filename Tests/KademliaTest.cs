using System.Net;
using System.Threading;

using Kyru.Network;
using Kyru.Network.UdpMessages;

using MbUnit.Framework;

namespace Tests
{
	internal sealed class KademliaTest
	{
		private Kademlia kademlia;
		private Node node;

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
		internal void KademliaStartsWith0Contacts()
		{
			Assert.AreEqual(0, kademlia.CurrentContacts);
		}

		[Test]
		internal void KademliaContactsAfterNewContact()
		{
			for (int i = 0; i < 10; i++)
			{
				var id = KademliaId.RandomId;
				var ni = new NodeInformation(new IPEndPoint(i + 1, i + 1), id);
				var message = new UdpMessage();
				kademlia.HandleIncomingRequest(ni, message);

				Assert.AreEqual(i, kademlia.CurrentContacts);
				Assert.IsNotNull(message.PingRequest);

				var response = new UdpMessage();
				response.ResponseId = message.RequestId;
				response.SenderNodeId = id;
				message.ResponseCallback(response);

				Assert.AreEqual(i + 1, kademlia.CurrentContacts);
			}
		}

		[Test]
		internal void AddContact()
		{
			using (var node2 = new Node(65432, null))
			{
				var kademlia2 = node2.Kademlia;

				node.Start();
				node2.Start();

				kademlia.AddNode(new IPEndPoint(IPAddress.Loopback, 65432));
				Thread.Sleep(TestParameters.LocalhostCommunicationTimeout);

				Assert.AreEqual(1, kademlia.CurrentContacts);
				Assert.AreEqual(1, kademlia2.CurrentContacts);
			}
		}
	}
}