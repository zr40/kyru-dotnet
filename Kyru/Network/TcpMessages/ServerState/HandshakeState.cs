using System.Net;
using System.Net.Sockets;

using Kyru.Core;

using ProtoBuf;

namespace Kyru.Network.TcpMessages.ServerState
{
	internal class HandshakeState : IServerState
	{
		private readonly TcpClient client;
		private readonly NetworkStream stream;
		private readonly App app;

		internal HandshakeState(TcpClient client, NetworkStream stream, App app)
		{
			this.client = client;
			this.stream = stream;
			this.app = app;
		}

		public IServerState Process()
		{
			var serverHandshake = new ServerHandshake();
			serverHandshake.ProtocolVersion = Node.ProtocolVersion;
			serverHandshake.NodeId = app.Node.Id;
			Serializer.SerializeWithLengthPrefix(stream, serverHandshake, PrefixStyle.Base128);

			var handshake = Serializer.DeserializeWithLengthPrefix<ClientHandshake>(stream, PrefixStyle.Base128);
			if (handshake.GetObjectRequest != null && handshake.StoreObjectRequest != null)
			{
				this.Log("Ignoring TCP request from {0} containing multiple requests", client.Client.RemoteEndPoint);
				return null;
			}
			if (handshake.GetObjectRequest == null && handshake.StoreObjectRequest == null)
			{
				this.Log("Ignoring TCP request from {0} containing no requests", client.Client.RemoteEndPoint);
				return null;
			}

			var clientAddress = ((IPEndPoint) client.Client.RemoteEndPoint).Address;
			app.Node.Kademlia.AddVerifiedNode(new IPEndPoint(clientAddress, handshake.Port), handshake.NodeId);

			if (handshake.GetObjectRequest != null)
			{
				return new GetObjectState(stream, app, handshake.GetObjectRequest);
			}
			return new StoreObjectState(stream, app, handshake.StoreObjectRequest);
		}
	}
}