using System;
using System.Net;
using System.Threading;

using Kyru.Network;

using MbUnit.Framework;

namespace Tests
{
	internal sealed class KademliaContactPingTest
	{
		private Node node;
		private Kademlia kademlia;
		private Node node2;
		private Kademlia kademlia2;
		private KademliaId targetId;

		[SetUp]
		internal void PrepareKademlia()
		{
			node = new Node(null);
			kademlia = node.Kademlia;

			node2 = new Node(12345, null);
			kademlia2 = node2.Kademlia;

			targetId = node2.Id;

			var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), targetId);
			TestHelper.RegisterFakeContact(kademlia, ni);
			
			// set LastSeen to a time beyond the ping interval
			var contact = Mirror.ForObject(kademlia)["FirstContact"].Invoke();
			Mirror.ForObject(contact)["LastSeen"].Value = DateTime.Now - TimeSpan.FromHours(1.1);
		}

		[TearDown]
		internal void TearDown()
		{
			node.Dispose();
			node2.Dispose();
		}

		[Test]
		internal void KademliaRemovesContactAfterNonResponseOnPing()
		{
			kademlia.TimerElapsed();
			for (int i = 0; i < Node.TimeoutTicks * 2; i++)
			{
				Assert.AreEqual(1, kademlia.CurrentContactCount);
				node.TimerElapsed();
			}
			Assert.AreEqual(0, kademlia.CurrentContactCount);
		}

		[Test]
		internal void KademliaKeepsContactAfterResponseOnPing()
		{
			node.Start();
			node2.Start();

			kademlia.TimerElapsed();
			Thread.Sleep(TestParameters.LocalhostCommunicationTimeout);

			for (int i = 0; i < Node.TimeoutTicks * 2; i++)
			{
				Assert.AreEqual(1, kademlia.CurrentContactCount);
				node.TimerElapsed();
			}
			Assert.AreEqual(1, kademlia.CurrentContactCount);
			Assert.AreEqual(1, kademlia2.CurrentContactCount);
		}
	}
}