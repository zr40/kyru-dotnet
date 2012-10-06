using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Kyru.Network.Messages;

using ProtoBuf;

namespace Kyru.Network
{
	internal sealed class Node
	{
		private readonly UdpClient udp;
		private readonly TcpListener tcp;

		private const int ProtocolVersion = 0;
		private readonly KademliaId id = KademliaId.RandomId;

		private readonly Dictionary<RequestIdentifier, RequestInformation> outstandingRequests = new Dictionary<RequestIdentifier, RequestInformation>();

		private struct RequestIdentifier
		{
			internal NodeInformation NodeInformation;
			internal ulong RequestId;
		}

		private struct RequestInformation
		{
			internal UdpMessage Message;
			internal bool SecondAttempt;
			internal Action<UdpMessage> ResponseCallback;
			internal DateTime SentAtTime;
		}

		internal Node() : this(12045)
		{
		}

		internal Node(int port)
		{
			udp = new UdpClient(port);
			tcp = new TcpListener(IPAddress.Any, port);

			UdpListen();
			TcpListen();
		}

		private void UdpListen()
		{
			udp.BeginReceive(OnUdpReceive, null);
		}

		private void OnUdpReceive(IAsyncResult ar)
		{
			var endPoint = new IPEndPoint(IPAddress.Any, 0);
			var data = udp.EndReceive(ar, ref endPoint);
			UdpListen();

			var message = Serializer.Deserialize<UdpMessage>(new MemoryStream(data));

			if (message.ProtocolVersion != ProtocolVersion)
			{
				Console.WriteLine("Ignoring message from {0} with unknown protocol version {1}", endPoint, message.ProtocolVersion);
				return;
			}

			var ni = new NodeInformation(endPoint, message.SenderNodeId);

			if (message.ResponseId != 0)
			{
				var identifier = new RequestIdentifier {NodeInformation = ni, RequestId = message.ResponseId};
				if (!outstandingRequests.ContainsKey(identifier))
				{
					Console.WriteLine("Ignoring message from {0} with unknown response ID {1}", endPoint, message.ResponseId);
					return;
				}

				var request = outstandingRequests[identifier];
				outstandingRequests.Remove(identifier);

				// responses may not contain any requests besides ping
				if (message.FindNodeRequest != null)
				{
					Console.WriteLine("Ignoring response from {0} with response ID {1} containing a find node request", endPoint, message.ResponseId);
					return;
				}
				if (message.FindValueRequest != null)
				{
					Console.WriteLine("Ignoring response from {0} with response ID {1} containing a find value request", endPoint, message.ResponseId);
					return;
				}
				if (message.KeepObjectRequest != null)
				{
					Console.WriteLine("Ignoring response from {0} with response ID {1} containing a keep object request", endPoint, message.ResponseId);
					return;
				}
				if (message.StoreRequest != null)
				{
					Console.WriteLine("Ignoring response from {0} with response ID {1} containing a store request", endPoint, message.ResponseId);
					return;
				}

				// the callback will deal with handling the actual response
				request.ResponseCallback(message);
			}

			var requests = 0;
			if (message.FindNodeRequest != null)
			{
				requests++;
			}
			if (message.FindValueRequest != null)
			{
				requests++;
			}
			if (message.KeepObjectRequest != null)
			{
				requests++;
			}
			if (message.PingRequest != null)
			{
				requests++;
			}
			if (message.StoreRequest != null)
			{
				requests++;
			}

			if (requests > 1)
			{
				Console.WriteLine("Ignoring message from {0} with request ID {1} containing multiple requests", endPoint, message.ResponseId);
				return;
			}
			if (requests == 0 && message.ResponseId == 0)
			{
				Console.WriteLine("Ignoring empty message from {0} with request ID {1}.", endPoint, message.ResponseId);
				return;
			}

			if (message.PingRequest != null)
			{
				IncomingPing(ni, message);
			}
			else if (message.FindNodeRequest != null)
			{
				IncomingFindNode(ni, message);
			}
			else if (message.FindValueRequest != null)
			{
				IncomingFindValue(ni, message);
			}
			else if (message.StoreRequest != null)
			{
				IncomingStore(ni, message);
			}
			else if (message.KeepObjectRequest != null)
			{
				IncomingKeepObject(ni, message);
			}
		}

		private void IncomingKeepObject(NodeInformation ni, UdpMessage message)
		{
			throw new NotImplementedException();
		}

		private void IncomingStore(NodeInformation ni, UdpMessage message)
		{
			throw new NotImplementedException();
		}

		private void IncomingFindValue(NodeInformation ni, UdpMessage message)
		{
			throw new NotImplementedException();
		}

		private void IncomingFindNode(NodeInformation ni, UdpMessage message)
		{
			throw new NotImplementedException();
		}

		private void IncomingPing(NodeInformation ni, UdpMessage message)
		{
			throw new NotImplementedException();
		}

		private void TcpListen()
		{
			tcp.BeginAcceptTcpClient(OnTcpAccept, null);
		}

		private void OnTcpAccept(IAsyncResult ar)
		{
			var client = tcp.EndAcceptTcpClient(ar);
			TcpListen();

			// TODO
		}

		private void SendUdpMessage(UdpMessage message, IPEndPoint target)
		{
			message.ProtocolVersion = ProtocolVersion;
			message.RequestId = Random.UInt64();

			var s = new MemoryStream();
			Serializer.Serialize(s, message);

			Console.WriteLine("Sending message with length {0} with request ID {1:X16} to {2} (response ID {3:X16})", s.Length, message.RequestId, target, message.ResponseId);
			udp.Send(s.GetBuffer(), (int) s.Length, target);
		}
	}
}