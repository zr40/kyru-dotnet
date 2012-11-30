using System.Collections.Generic;
using System.IO;

using Kyru.Core;
using Kyru.Network;
using Kyru.Network.Objects;

using MbUnit.Framework;

namespace Tests
{
	[Ignore("Fake node required")]
	internal sealed class LocalObjectStorageTest
	{
		private Config config;
		private string tempPath;

		private Node node;

		[SetUp]
		internal void CreateConfig()
		{
			tempPath = Path.Combine(Path.GetTempPath(), "KyruTest" + Path.GetRandomFileName());

			config = new Config();
			config.StoreDirectory = tempPath;
		}

		[TearDown]
		internal void DeleteTemp()
		{
			if (Directory.Exists(tempPath))
				Directory.Delete(tempPath, true);
		}

		[Test]
		internal void EmptyStorage()
		{
			var storage = new LocalObjectStorage(config, node);
			Assert.AreEqual(0, storage.CurrentObjects.Count);
		}

		[Test]
		internal void StoreObject()
		{
			var storage = new LocalObjectStorage(config, node);

			var user = new User();
			user.ObjectId = KademliaId.RandomId;

			storage.StoreObject(user, false);
			Assert.AreEqual(1, storage.CurrentObjects.Count);

			storage.StoreObject(user, false);
			Assert.AreEqual(1, storage.CurrentObjects.Count);

			user.ObjectId = KademliaId.RandomId;
			storage.StoreObject(user, false);
			Assert.AreEqual(2, storage.CurrentObjects.Count);
		}

		[Test]
		internal void GetObject()
		{
			var storage = new LocalObjectStorage(config, node);

			var randomId = KademliaId.RandomId;
			ulong fileId = 123456789;
			var id = KademliaId.RandomId;

			var user = new User();
			user.ObjectId = id;
			user.Add(new UserFile { FileId = fileId, ChunkList = new List<KademliaId> { randomId } });

			storage.StoreObject(user, false);

			var readUser = storage.GetObject(id) as User;
			Assert.AreEqual(1, readUser.Files.Count);
			Assert.AreEqual(0, readUser.DeletedFiles.Count);

			var userFile = readUser.Files[0];
			Assert.AreEqual(1, userFile.ChunkList.Count);
			Assert.AreEqual(randomId, userFile.ChunkList[0]);
			Assert.AreEqual(fileId, userFile.FileId);
		}

		[Test]
		internal void GetObjectNotPresent()
		{
			var storage = new LocalObjectStorage(config, node);

			var user = new User();
			user.ObjectId = KademliaId.RandomId;
			user.Add(new UserFile { ChunkList = new List<KademliaId> { KademliaId.RandomId } });

			storage.StoreObject(user, false);

			Assert.IsNull(storage.GetObject(KademliaId.RandomId));
		}

		[Test]
		internal void Initialization()
		{
			var storage = new LocalObjectStorage(config, node);

			var user = new User();
			user.ObjectId = KademliaId.RandomId;
			user.Add(new UserFile {ChunkList = new List<KademliaId> {KademliaId.RandomId}});
			storage.StoreObject(user, false);

			storage = new LocalObjectStorage(config, node);

			Assert.AreEqual(1, storage.CurrentObjects.Count);
			Assert.AreEqual(user.ObjectId, storage.CurrentObjects[0]);
		}
	}
}