using System.IO;
using System.Net;
using System.Net.Sockets;

using Kyru.Core;

using ProtoBuf;

namespace Kyru.Network.TcpMessages.Clients
{
	internal abstract class ClientBase
	{
		protected readonly App App;
		protected readonly NodeInformation TargetNode;

		protected ClientBase(App app, NodeInformation targetNode)
		{
			App = app;
			TargetNode = targetNode;
		}

		internal void ThreadStart()
		{
			this.Log("Opening TCP connection to {0}", TargetNode.EndPoint);

			using (var client = new TcpClient())
			{
				client.Connect(TargetNode.EndPoint);
				using (var stream = client.GetStream())
				{
					var serverHandshake = Serializer.DeserializeWithLengthPrefix<ServerHandshake>(stream, PrefixStyle.Base128);
					if (serverHandshake.ProtocolVersion != Node.ProtocolVersion)
					{
						this.Log("Node at {0} uses unknown protocol version {1}. Dropping connection.", TargetNode.EndPoint, serverHandshake.ProtocolVersion);
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