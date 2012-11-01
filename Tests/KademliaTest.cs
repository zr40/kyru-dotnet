﻿using System.Net;
using System.Threading;

using Kyru.Network;
using Kyru.Network.Messages;

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
			node = new Node();
			kademlia = (Kademlia) Mirror.ForObject(node)["kademlia"].Value;
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
				var ni = new NodeInformation(new IPEndPoint(i + 1, i + 1), KademliaId.RandomId);
				var message = new UdpMessage();
				kademlia.HandleIncomingRequest(ni, message);
				Assert.AreEqual(i, kademlia.CurrentContacts);
				Assert.IsNotNull(message.PingRequest);

				message.ResponseCallback(null); // response message isn't needed for the kademlia object
				Assert.AreEqual(i + 1, kademlia.CurrentContacts);
			}
		}

		[Test]
		internal void AddContact()
		{
			var node2 = new Node(65432);
			var kademlia2 = (Kademlia) Mirror.ForObject(node2)["kademlia"].Value;

			node.Start();
			node2.Start();

			kademlia.AddNode(new IPEndPoint(IPAddress.Loopback, 65432));
			Thread.Sleep(100); // long enough to allow the two test nodes to communicate through localhost

			Assert.AreEqual(1, kademlia.CurrentContacts);
			Assert.AreEqual(1, kademlia2.CurrentContacts);
		}
	}
}