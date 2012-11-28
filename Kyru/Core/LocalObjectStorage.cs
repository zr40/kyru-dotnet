using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Kyru.Network;
using Kyru.Network.Objects;
using Kyru.Utilities;

using ProtoBuf;

namespace Kyru.Core
{
	internal sealed class LocalObjectStorage : ITimerListener
	{
		private readonly Config config;
		private readonly Node node;

		/// <remarks>Value: access timestamp</remarks>
		private readonly Dictionary<KademliaId, DateTime> currentObjects = new Dictionary<KademliaId, DateTime>();

		internal LocalObjectStorage(Config config, Network.Node node)
		{
			this.config = config;
			this.node = node;

			Directory.CreateDirectory(config.StoreDirectory);

			foreach (var file in Directory.GetFiles(config.StoreDirectory))
			{
				// TODO: verify file names and contents?
				currentObjects[KademliaId.FromHex(Path.GetFileName(file))] = DateTime.Now;
			}

			KyruTimer.Register(this, 60);
		}

		internal IList<KademliaId> CurrentObjects
		{
			get
			{
				return currentObjects.Keys.ToList();
			}
		}

		internal const uint MaxObjectSize = 1024 * 1024 + Crypto.AesHeaderSize; // 1 MiB

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

		/// <summary>
		/// Stores an object on this or another node
		/// </summary>
		/// <param name="o">the object</param>
		internal void StoreObject(KyruObject o)
		{
			if (!o.VerifyData())
			{
				this.Warn("Object failed verification; it will not be stored. ID: {0}", o.ObjectId);
				return;
			}

			using (var stream = new MemoryStream())
			{
				Serializer.Serialize(stream, o);
				node.StoreObject(o.ObjectId, stream.ToArray());
			}
		}

		/// <summary>
		/// Stores the data on this node
		/// </summary>
		/// <param name="id">the id</param>
		/// <param name="bytes">data</param>
		internal void StoreBytes(KademliaId id, byte[] bytes)
		{
			using (var st = new MemoryStream(bytes))
			{
				if (!Serializer.Deserialize<KyruObject>(st).VerifyData())
				{
					this.Warn("Object failed verification; it will not be stored. ID: {0}", id);
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
				this.Warn("Object larger than 1 MiB; it will not be stored. ID: {0}", id);

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
			return Path.Combine(config.StoreDirectory, id.ToString());
		}

		public void TimerElapsed()
		{
			// TODO: check availability of objects; store the object with the least availability on a random node
		}
	}
}