﻿using System.Collections.Generic;
using System.IO;

using Kyru.Core;
using Kyru.Network;
using Kyru.Network.Objects;

using MbUnit.Framework;

namespace Tests
{
	internal sealed class LocalObjectStorageTest
	{
		private Config config;
		private string tempPath;

		[SetUp]
		internal void CreateConfig()
		{
			tempPath = Path.Combine(Path.GetTempPath(), "KyruTest" + Path.GetRandomFileName());

			config = new Config();
			config.storeDirectory = tempPath;
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
			var storage = new LocalObjectStorage(config);
			Assert.AreEqual(0, storage.CurrentObjects.Count);
		}

		[Test]
		internal void StoreObject()
		{
			var storage = new LocalObjectStorage(config);

			var user = new User("foo");
			user.ObjectId = KademliaId.RandomId;

			storage.Store(user);
			Assert.AreEqual(1, storage.CurrentObjects.Count);

			storage.Store(user);
			Assert.AreEqual(1, storage.CurrentObjects.Count);

			user.ObjectId = KademliaId.RandomId;
			storage.Store(user);
			Assert.AreEqual(2, storage.CurrentObjects.Count);
		}

		[Test]
		internal void GetObject()
		{
			var storage = new LocalObjectStorage(config);

			var randomId = KademliaId.RandomId;
			ulong fileId = 123456789;
			var id = KademliaId.RandomId;

			var user = new User("foo");
			user.ObjectId = id;
			user.Add(new UserFile(new List<KademliaId> { randomId }) { FileId = fileId });

			storage.Store(user);

			var readUser = storage.Get(id).User;
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
			var storage = new LocalObjectStorage(config);

			var user = new User("foo");
			user.ObjectId = KademliaId.RandomId;
			user.Add(new UserFile(new List<KademliaId> { KademliaId.RandomId}));

			storage.Store(user);

			Assert.IsNull(storage.Get(KademliaId.RandomId));
		}

		[Test]
		internal void Initialization()
		{
			var storage = new LocalObjectStorage(config);

			var user = new User("foo");
			user.ObjectId = KademliaId.RandomId;
			user.Add(new UserFile(new List<KademliaId> { KademliaId.RandomId}));
			storage.Store(user);

			storage = new LocalObjectStorage(config);

			Assert.AreEqual(1, storage.CurrentObjects.Count);
			Assert.AreEqual(user.ObjectId, storage.CurrentObjects[0]);
		}
	}
}