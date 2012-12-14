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

		internal StoreObjectState(NetworkStream stream, KyruApplication app, StoreObjectRequest storeObjectRequest)
		{
			this.stream = stream;
			this.app = app;
			this.storeObjectRequest = storeObjectRequest;
		}

		public IServerState Process()
		{
			var response = new StoreObjectResponse();

			if (app.LocalObjectStorage.KeepObject(storeObjectRequest.ObjectId)) // Object already exists
			{
				var oldObject = app.LocalObjectStorage.GetObject(storeObjectRequest.ObjectId);
				using (var mstream = new MemoryStream())
				{
					if (oldObject != null)
						Serializer.Serialize(mstream, oldObject);
					if (oldObject is User && !Crypto.Hash(mstream.ToArray()).SequenceEqual(storeObjectRequest.Hash))
						// New object is a different version of an existing User object
						response.Error = Error.Success;
					else
						response.Error = Error.ObjectAlreadyStored;
				}
			}
			else if (storeObjectRequest.Length > LocalObjectStorage.MaxObjectSize + 8)
				// TODO: remove hack. (8 bytes overhead for 1 MiB chunk)
			{
				response.Error = Error.StoreRejected;
			}
			else
			{
				response.Error = Error.Success;
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
			return null;
		}
	}
}