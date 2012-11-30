using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

using Kyru;
using Kyru.Core;
using Kyru.Network;
using Kyru.Network.Objects;
using Kyru.Network.TcpMessages;
using Kyru.Utilities;

using MbUnit.Framework;

using ProtoBuf;

namespace Tests.SystemTests
{
	[Ignore]
	internal sealed class NetworkTest
	{
		private List<Node> nodes;

		private KademliaId objectId;
		private byte[] bytes;

		private const int NodeCount = 60;

		[SetUp]
		private void CreateNodes()
		{
			nodes = new List<Node>();

			for (int i = 0; i < NodeCount; i++)
			{
				var port = (ushort) (12000 + i);

				var node = new KyruApplication(port).Node;
				node.Start();
				nodes.Add(node);

				if (i != 0)
				{
					node.Kademlia.AddNode(new IPEndPoint(IPAddress.Loopback, port - 1));
				}

				Thread.Sleep(TestParameters.LocalhostCommunicationTimeout * 5);
			}

			KyruTimer.Start();

			this.Warn("Waiting for the node initialization dust to settle...\n\n\n\n");

			Thread.Sleep(TestParameters.LocalhostCommunicationTimeout * 160);

			this.Warn("Continuing...\n\n\n\n");
		}

		private void StoreTestObject()
		{
			// create the test object

			objectId = KademliaId.RandomId;
			var chunkId = KademliaId.RandomId;

			var user = new User();
			user.ObjectId = objectId;
			var userFile = new UserFile();
			userFile.ChunkList.Add(chunkId);
			user.Add(userFile);

			var ms = new MemoryStream();
			Serializer.Serialize(ms, user);
			bytes = ms.ToArray();

			// store it

			var ct = new CallbackTimeout<Error>();
			var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, nodes[1].Port), nodes[1].Id);
			nodes[0].StoreObject(ni, objectId, bytes, ct.Done);
			if (!ct.Block(TestParameters.LocalhostCommunicationTimeout))
			{
				Assert.Fail("No response within timeout");
			}
			Assert.AreEqual(Error.Success, ct.Result);
		}

		[TearDown]
		private void StopNodes()
		{
			KyruTimer.Reset();

			foreach (var node in nodes)
			{
				node.Dispose();
			}
		}

		[Test]
		private void FindNode()
		{
			var ct = new CallbackTimeout<List<NodeInformation>>();

			var node = nodes[NodeCount - 1];
			node.Kademlia.NodeLookup(nodes[0].Id, ct.Done);
			if (!ct.Block(TestParameters.LocalhostCommunicationTimeout * 120))
			{
				Assert.Fail("Not completed within timeout");
			}

			Assert.AreNotEqual(0, nodes.Count);
			Assert.AreEqual(nodes[0].Port, ct.Result[0].EndPoint.Port);
			Assert.AreEqual(nodes[0].Id, ct.Result[0].NodeId);
		}

		[Test, Ignore]
		private void RetrieveObject()
		{
			StoreTestObject();

			var node = nodes[NodeCount - 1];

			var ct = new CallbackTimeout<Error, byte[]>();
			node.GetObjectFromNetwork(objectId, ct.Done);
			if (!ct.Block(TestParameters.LocalhostCommunicationTimeout * 100))
			{
				Assert.Fail("Not completed within timeout");
			}
			Assert.AreEqual(Error.Success, ct.Result1);
			Assert.AreElementsEqual(bytes, ct.Result2);
		}
	}
}