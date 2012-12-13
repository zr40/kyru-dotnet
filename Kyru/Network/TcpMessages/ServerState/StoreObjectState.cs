using System.IO;
using System.Linq;
using System.Net.Sockets;

using Kyru.Core;
using Kyru.Network.Objects;
using Kyru.Utilities;
using ProtoBuf;

namespace Kyru.Network.TcpMessages.ServerState
{
	internal sealed class StoreObjectState : IServerState
	{
		private readonly NetworkStream stream;
		private readonly KyruApplication app;
		private readonly StoreObjectRequest storeObjectRequest;
		private const int ObjectHeaderSize = 8;

		internal StoreObjectState(NetworkStream stream, KyruApplication app, StoreObjectRequest storeObjectRequest)
		{
			this.stream = stream;
			this.app = app;
			this.storeObjectRequest = storeObjectRequest;
		}

		public IServerState Process()
		{
			var response = new StoreObjectResponse();
			using (var mstream = new MemoryStream()) // TODO: Clean up user object detection
			{
				var oldObject = app.LocalObjectStorage.GetObject(storeObjectRequest.ObjectId);
				if (oldObject != null)
					Serializer.Serialize(mstream, oldObject);

				if (app.LocalObjectStorage.KeepObject(storeObjectRequest.ObjectId) && !(oldObject is User && !Crypto.Hash(mstream.ToArray()).SequenceEqual(storeObjectRequest.Hash)))
				{
					response.Error = Error.ObjectAlreadyStored;
				}
				else if (storeObjectRequest.Length > LocalObjectStorage.MaxObjectSize + ObjectHeaderSize)
				{
					response.Error = Error.StoreRejected;
				}
				else
				{
					response.Error = Error.Success;
				}
			}
			Serializer.SerializeWithLengthPrefix(stream, response, PrefixStyle.Base128);

			if (response.Error == Error.Success)
			{
				var buffer = new byte[storeObjectRequest.Length];
				var offset = 0;
				var remaining = (int) storeObjectRequest.Length;
				while (remaining != 0)
				{
					int read = stream.Read(buffer, offset, remaining);
					offset += read;
					remaining -= read;
				}

				app.LocalObjectStorage.StoreBytes(storeObjectRequest.ObjectId, buffer, false);
			}
			return null; // No next server state
		}
	}
}