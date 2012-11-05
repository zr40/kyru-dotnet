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

		internal void Store(KyruObject o)
		{
			if (o.ObjectId.Bytes.All(b => b == 0))
			{
				throw new InvalidOperationException("Possible bug: tried to store object with id zero");
			}

			using (var stream = File.OpenWrite(PathFor(o.ObjectId)))
			{
				Serializer.Serialize(stream, o);
			}
			currentObjects[o.ObjectId] = DateTime.Now;
		}

		internal KyruObject Get(KademliaId id)
		{
			if (id.Bytes.All(b => b == 0))
			{
				throw new InvalidOperationException("Possible bug: tried to get object with id zero");
			}

			if (currentObjects.ContainsKey(id))
			{
				using (var stream = File.OpenRead(PathFor(id)))
				{
					return Serializer.Deserialize<KyruObject>(stream);
				}
			}
			return null;
		}

		private string PathFor(KademliaId id)
		{
			return Path.Combine(config.storeDirectory, id.ToString());
		}
	}
}