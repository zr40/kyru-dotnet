using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Kyru.Network.UdpMessages;
using Kyru.Utilities;

namespace Kyru.Network
{
	internal sealed class MetadataStorage : ITimerListener
	{
		private readonly Node node;
		private readonly Dictionary<KademliaId, List<KyruObjectMetadata>> storage = new Dictionary<KademliaId, List<KyruObjectMetadata>>();

		internal MetadataStorage(Node node)
		{
			this.node = node;

			KyruTimer.Register(this, 60);
		}

		/// <summary>
		/// Returns the metadata if it is stored, otherwise null.
		/// </summary>
		internal KyruObjectMetadata[] Get(KademliaId id)
		{
			List<KyruObjectMetadata> metadata;
			if (storage.TryGetValue(id, out metadata) && metadata.Count != 0)
				return metadata.ToArray();
			return null;
		}

		internal void Store(KademliaId id, KyruObjectMetadata[] newMetadata)
		{
			if (!storage.ContainsKey(id))
			{
				storage[id] = new List<KyruObjectMetadata>();
			}
			var metadata = storage[id];

			foreach (var newItem in newMetadata)
			{
				// make sure the timestamp isn't in the future
				newItem.Timestamp = Math.Min(DateTime.Now.UnixTimestamp(), newItem.Timestamp);

				var item = metadata.FirstOrDefault(m => m.IpAddress == newItem.IpAddress && m.NodeId == newItem.NodeId);

				if (item == null)
				{
					VerifyOwnership(id, newItem);
					metadata.Add(newItem);
				}
				else
				{
					item.Timestamp = Math.Max(item.Timestamp, newItem.Timestamp);
				}
			}
		}

		private void VerifyOwnership(KademliaId objectId, KyruObjectMetadata newItem)
		{
			var message = new UdpMessage();
			message.KeepObjectRequest = new KeepObjectRequest();
			message.KeepObjectRequest.ObjectId = objectId;
			message.ResponseCallback = delegate(UdpMessage udpMessage)
			                           {
				                           if (!udpMessage.KeepObjectResponse.HasObject) return;

				                           if (!storage.ContainsKey(objectId))
					                           storage[objectId] = new List<KyruObjectMetadata>();

				                           newItem.Timestamp = DateTime.Now.UnixTimestamp();
				                           storage[objectId].Add(newItem);
			                           };
			node.SendUdpMessage(message, new IPEndPoint(newItem.IpAddress, newItem.Port), newItem.NodeId);
		}

		public void TimerElapsed()
		{
			// TODO: republish metadata, expire old entries

			//throw new NotImplementedException();
		}
	}
}