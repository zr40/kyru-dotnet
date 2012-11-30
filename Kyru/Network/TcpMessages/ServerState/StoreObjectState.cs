using System.Net.Sockets;

using Kyru.Core;

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

			if (app.LocalObjectStorage.KeepObject(storeObjectRequest.ObjectId))
			{
				// TODO: allow user object merges if the hash is different
				response.Error = Error.ObjectAlreadyStored;
			}
			else if (storeObjectRequest.Length > LocalObjectStorage.MaxObjectSize)
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