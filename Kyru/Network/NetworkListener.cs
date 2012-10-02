using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Kyru.Network
{
	internal sealed class NetworkListener
	{
		private readonly UdpClient udp;
		private readonly TcpListener tcp;

		internal NetworkListener() : this(12045)
		{
		}

		internal NetworkListener(int port)
		{
			udp = new UdpClient(port);
			tcp = new TcpListener(IPAddress.Any, port);

			new Thread(UdpListen).Start();
			new Thread(TcpListen).Start();
		}

		private void UdpListen()
		{
			while (true)
			{
				var endPoint = new IPEndPoint(IPAddress.Any, 0);
				var data = udp.Receive(ref endPoint);

				// TODO
			}
		}

		private void TcpListen()
		{
			while (true)
			{
				var client = tcp.AcceptTcpClient();
				new Thread(new IncomingTcpConnection(client).Accept).Start();
			}
		}
	}
}