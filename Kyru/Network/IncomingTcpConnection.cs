using System.Net.Sockets;

namespace Kyru.Network
{
	internal class IncomingTcpConnection
	{
		private readonly TcpClient client;

		internal IncomingTcpConnection(TcpClient client)
		{
			this.client = client;
		}

		internal void Accept()
		{
			// TODO
		}
	}
}