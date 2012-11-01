using System;
using System.Net;
using System.Threading;

using Kyru.Network;
using Kyru.Network.Messages;

using MbUnit.Framework;

namespace Tests
{
	internal sealed class NodeCommunicationTest
	{
		private Node node;
		private Kademlia kademlia;
		private Node node2;
		private Kademlia kademlia2;
		private IPEndPoint targetEndPoint;
		private KademliaId targetId;

		[SetUp]
		internal void PrepareKademlia()
		{
			node = new Node();
			kademlia = (Kademlia) Mirror.ForObject(node)["kademlia"].Value;

			node2 = new Node(12345);
			kademlia2 = (Kademlia) Mirror.ForObject(node)["kademlia"].Value;

			targetEndPoint = new IPEndPoint(IPAddress.Loopback, 12345);
			targetId = node2.Id;

			kademlia.AddNode(new IPEndPoint(IPAddress.Loopback, 12345));
			Thread.Sleep(TestParameters.LocalhostCommunicationTimeout);
		}

		[TearDown]
		internal void TearDown()
		{
			node.Dispose();
			node2.Dispose();
		}

		[Test]
		internal void TestFindNode([Column(0, 1, 2, 19, 20, 21, 100)] int contacts, [EnumData(typeof(TestHelper.PrepareType))] TestHelper.PrepareType prepareType)
		{
			int expectedContacts = 0;
			switch (prepareType)
			{
				case TestHelper.PrepareType.Local:
					TestHelper.PrepareFakeContacts(kademlia, contacts);
					expectedContacts = contacts + 1;
					break;

				case TestHelper.PrepareType.Remote:
					TestHelper.PrepareFakeContacts(kademlia2, contacts);
					expectedContacts = contacts + 1;
					break;

				case TestHelper.PrepareType.Both:
					TestHelper.PrepareFakeContacts(kademlia, kademlia2, contacts);
					expectedContacts = contacts * 2 + 1;
					break;

				case TestHelper.PrepareType.Overlapped:
					TestHelper.PrepareOverlappedFakeContacts(kademlia, kademlia2, contacts);
					expectedContacts = contacts * 3 + 1;
					break;
			}

			var to = new Timeout();
			var message = new UdpMessage();
			message.FindNodeRequest = new FindNodeRequest {NodeId = KademliaId.RandomId};
			message.ResponseCallback = to.Done;

			node.SendUdpMessage(message, targetEndPoint, targetId);
			if (!to.Block(TestParameters.LocalhostCommunicationTimeout))
			{
				Assert.Fail("No response within timeout");
			}

			Assert.AreEqual(expectedContacts, kademlia.CurrentContacts);
		}
	}
}