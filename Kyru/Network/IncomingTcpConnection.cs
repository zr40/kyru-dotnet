using System.Net.Sockets;
using System.Threading;

using Kyru.Core;
using Kyru.Network.TcpMessages.ServerState;

namespace Kyru.Network
{
	internal class IncomingTcpConnection
	{
		internal readonly App app;
		internal readonly TcpClient client;
		private IServerState serverState;

		internal IncomingTcpConnection(App app, TcpClient client)
		{
			this.app = app;
			this.client = client;
		}

		internal void Accept()
		{
			new Thread(LoopUntilDone).Start();
		}

		private void LoopUntilDone()
		{
			this.Log("Accepted TCP connection from {0}", client.Client.RemoteEndPoint);
			using (client)
			using (var stream = client.GetStream())
			{
				serverState = new HandshakeState(client, stream, app);
				while (serverState != null)
				{
					serverState = serverState.Process();
				}
			}
			this.Log("TCP connection from {0} closed", client.Client.RemoteEndPoint);
		}
	}
}