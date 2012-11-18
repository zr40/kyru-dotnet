using System;
using System.IO;

using Kyru.Core;

using ProtoBuf;

namespace Kyru.Network.TcpMessages.Clients
{
	internal sealed class GetObjectClient : ClientBase
	{
		private readonly KademliaId objectId;
		private readonly Action<Error, byte[]> done;

		internal GetObjectClient(KyruApplication app, NodeInformation targetNode, KademliaId objectId, Action<Error, byte[]> done) : base(app, targetNode)
		{
			this.objectId = objectId;
			this.done = done;
		}

		protected override void Execute(Stream stream)
		{
			this.Log("Sending GetObject request to {0} for object ID {1}", TargetNode.EndPoint, objectId);

			var clientHandshake = new ClientHandshake();
			clientHandshake.NodeId = App.Node.Id;
			clientHandshake.Port = App.Node.Port;
			clientHandshake.GetObjectRequest = new GetObjectRequest();
			clientHandshake.GetObjectRequest.ObjectId = objectId;
			Serializer.SerializeWithLengthPrefix(stream, clientHandshake, PrefixStyle.Base128);

			var getObjectResponse = Serializer.DeserializeWithLengthPrefix<GetObjectResponse>(stream, PrefixStyle.Base128);
			if (getObjectResponse.Error != Error.Success)
			{
				this.Log("Got GetObject response {0} from {1} for object ID {2}", getObjectResponse.Error, TargetNode.EndPoint, objectId);
				done(getObjectResponse.Error, null);
				return;
			}

			var buffer = new byte[getObjectResponse.Length];
			var offset = 0;
			var remaining = (int) getObjectResponse.Length;
			while (remaining != 0)
			{
				int read = stream.Read(buffer, offset, remaining);
				offset += read;
				remaining -= read;
			}

			this.Log("Got object {0} from {1}.", objectId, TargetNode.EndPoint);

			done(Error.Success, buffer);
		}
	}
}