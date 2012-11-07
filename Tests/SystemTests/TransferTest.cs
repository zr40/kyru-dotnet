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
	internal sealed class TransferTest
	{
		private Node nodeA;
		private Node nodeB;
		private Node nodeC;

		private NodeInformation nodeAInfo;
		private NodeInformation nodeBInfo;
		private NodeInformation nodeCInfo;

		[SetUp]
		internal void CreateNodes()
		{
			KyruTimer.Reset();

			nodeA = new App(12345).Node;
			nodeB = new App(12346).Node;
			nodeC = new App(12347).Node;

			nodeAInfo = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), nodeA.Id);
			nodeBInfo = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12346), nodeB.Id);
			nodeCInfo = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12347), nodeC.Id);

			nodeA.Start();
			nodeB.Start();
			nodeC.Start();

			nodeA.Kademlia.AddNode(nodeBInfo.EndPoint);
			nodeC.Kademlia.AddNode(nodeBInfo.EndPoint);

			KyruTimer.Start();

			Thread.Sleep(TestParameters.LocalhostCommunicationTimeout);
		}

		[TearDown]
		internal void DisposeNodes()
		{
			KyruTimer.Reset();
			nodeA.Dispose();
			nodeB.Dispose();
			nodeC.Dispose();
		}

		[Test]
		internal void Test()
		{
			var objectId = KademliaId.RandomId;
			var chunkId = KademliaId.RandomId;

			var user = new User();
			user.ObjectId = objectId;
			var userFile = new UserFile();
			userFile.ChunkList.Add(chunkId);
			user.Add(userFile);

			var ms = new MemoryStream();
			Serializer.Serialize(ms, user);
			var bytes = ms.ToArray();

			// store the object

			var ct1 = new CallbackTimeout<Error>();
			nodeA.StoreObject(nodeBInfo, objectId, bytes, ct1.Done);
			if (!ct1.Block(TestParameters.LocalhostCommunicationTimeout))
			{
				Assert.Fail("No response within timeout");
			}
			Assert.AreEqual(Error.Success, ct1.Result);

			// now retrieve it

			var ct2 = new CallbackTimeout<Error, byte[]>();
			nodeC.GetObject(nodeBInfo, objectId, ct2.Done);
			if (!ct2.Block(TestParameters.LocalhostCommunicationTimeout))
			{
				Assert.Fail("No response within timeout");
			}
			Assert.AreEqual(Error.Success, ct2.Result1);
			Assert.AreElementsEqual(bytes, ct2.Result2);
		}
	}
}