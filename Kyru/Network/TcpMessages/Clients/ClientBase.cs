using System.IO;
using System.Net;
using System.Net.Sockets;

using Kyru.Core;

using ProtoBuf;

namespace Kyru.Network.TcpMessages.Clients
{
	internal abstract class ClientBase
	{
		protected readonly KyruApplication App;
		protected readonly NodeInformation TargetNode;

		protected ClientBase(KyruApplication app, NodeInformation targetNode)
		{
			App = app;
			TargetNode = targetNode;
		}

		internal void ThreadStart()
		{
			using (var client = new TcpClient())
			{
				client.Connect(TargetNode.EndPoint);
				using (var stream = client.GetStream())
				{
					var serverHandshake = Serializer.DeserializeWithLengthPrefix<ServerHandshake>(stream, PrefixStyle.Base128);
					if (serverHandshake.ProtocolVersion != Node.ProtocolVersion)
					{
						this.Warn("Node at {0} uses unknown protocol version {1}. Dropping connection.", TargetNode.EndPoint, serverHandshake.ProtocolVersion);
						return;
					}
					App.Node.Kademlia.AddVerifiedNode((IPEndPoint) client.Client.RemoteEndPoint, serverHandshake.NodeId);

					Execute(stream);
				}
			}
		}

		protected abstract void Execute(Stream stream);
	}
}