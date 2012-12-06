using System;
using System.IO;

using Kyru.Core;
using Kyru.Utilities;
using ProtoBuf;

namespace Kyru.Network.TcpMessages.Clients
{
	internal sealed class StoreObjectClient : ClientBase
	{
		private readonly KademliaId objectId;
		private readonly byte[] bytes;
		private readonly Action<Error> done;

		internal StoreObjectClient(KyruApplication app, NodeInformation targetNode, KademliaId objectId, byte[] bytes, Action<Error> done) : base(app, targetNode)
		{
			this.objectId = objectId;
			this.bytes = bytes;
			this.done = done;
		}

		protected override void Execute(Stream stream)
		{
			this.Log("Sending StoreObject request to {0} for object ID {1}", TargetNode.EndPoint, objectId);

			var clientHandshake = new ClientHandshake();
			clientHandshake.NodeId = App.Node.Id;
			clientHandshake.Port = App.Node.Port;
			clientHandshake.StoreObjectRequest = new StoreObjectRequest();
			clientHandshake.StoreObjectRequest.Hash = Crypto.Hash(bytes);
			clientHandshake.StoreObjectRequest.Length = (uint) bytes.Length;
			clientHandshake.StoreObjectRequest.ObjectId = objectId;
			Serializer.SerializeWithLengthPrefix(stream, clientHandshake, PrefixStyle.Base128);

			var storeObjectResponse = Serializer.DeserializeWithLengthPrefix<StoreObjectResponse>(stream, PrefixStyle.Base128);
			if (storeObjectResponse.Error != Error.Success)
			{
				this.Log("Got StoreObject response {0} from {1} for object ID {2}", storeObjectResponse.Error, TargetNode.EndPoint, objectId);
				done(storeObjectResponse.Error);
				return;
			}

			// TODO: update local metadata

			stream.Write(bytes, 0, bytes.Length);
			done(Error.Success);
		}
	}
}