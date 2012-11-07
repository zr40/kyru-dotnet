using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Kyru.Network;
using Kyru.Network.Objects;

using ProtoBuf;

namespace Kyru.Core
{
	internal sealed class LocalObjectStorage
	{
		private readonly Config config;

		/// <remarks>Value: access timestamp</remarks>
		private readonly Dictionary<KademliaId, DateTime> currentObjects = new Dictionary<KademliaId, DateTime>();

		internal LocalObjectStorage(Config config)
		{
			this.config = config;

			Directory.CreateDirectory(config.storeDirectory);

			foreach (var file in Directory.GetFiles(config.storeDirectory))
			{
				// TODO: verify file names and contents?
				currentObjects[KademliaId.FromHex(Path.GetFileName(file))] = DateTime.Now;
			}
		}

		internal IList<KademliaId> CurrentObjects
		{
			get
			{
				return currentObjects.Keys.ToList();
			}
		}

		public const uint MaxObjectSize = 1024 * 1024; // 1 MiB

		/// <summary>
		/// Determines whether an object is locally stored. If it is, the access timestamp is updated.
		/// </summary>
		/// <returns>Whether the object with this id is locally stored.</returns>
		internal bool KeepObject(KademliaId id)
		{
			if (currentObjects.ContainsKey(id))
			{
				currentObjects[id] = DateTime.Now;
				return true;
			}
			return false;
		}

		internal void StoreObject(KyruObject o)
		{
			if (!o.VerifyData())
			{
				this.Log("Object failed verification; it will not be stored. ID: {0}", o.ObjectId);
				return;
			}

			using (var stream = new MemoryStream())
			{
				Serializer.Serialize(stream, o);
				Store(o.ObjectId, stream.ToArray());
			}
		}

		internal void StoreBytes(KademliaId id, byte[] bytes)
		{
			using (var st = new MemoryStream(bytes))
			{
				if (!Serializer.Deserialize<KyruObject>(st).VerifyData())
				{
					this.Log("Object failed verification; it will not be stored. ID: {0}", id);
					return;
				}
			}

			Store(id, bytes);
		}

		private void Store(KademliaId id, byte[] bytes)
		{
			if (id.Bytes.All(b => b == 0))
				throw new InvalidOperationException("Possible bug: tried to store object with id zero");
			if (bytes.Length > MaxObjectSize)
				this.Log("Object larger than 1 MiB; it will not be stored. ID: {0}", id);

			File.WriteAllBytes(PathFor(id), bytes);

			currentObjects[id] = DateTime.Now;
		}

		internal KyruObject GetObject(KademliaId id)
		{
			var bytes = Get(id);
			if (bytes == null)
				return null;

			using (var stream = new MemoryStream(bytes))
			{
				var obj = Serializer.Deserialize<KyruObject>(stream);
				obj.ObjectId = id;
				return obj;
			}
		}

		internal byte[] GetBytes(KademliaId id)
		{
			return Get(id);
		}

		private byte[] Get(KademliaId id)
		{
			if (id.Bytes.All(b => b == 0))
				throw new InvalidOperationException("Possible bug: tried to get object with id zero");

			if (currentObjects.ContainsKey(id))
				currentObjects[id] = DateTime.Now;
			else
				return null;

			return File.ReadAllBytes(PathFor(id));
		}

		private string PathFor(KademliaId id)
		{
			return Path.Combine(config.storeDirectory, id.ToString());
		}
	}
}