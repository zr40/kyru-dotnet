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

		private readonly Kademlia kademlia;

		internal const uint ProtocolVersion = 0;
		internal readonly KademliaId Id = KademliaId.RandomId;

		private readonly Dictionary<RequestIdentifier, RequestInformation> outstandingRequests = new Dictionary<RequestIdentifier, RequestInformation>();

		private struct RequestIdentifier
		{
			internal NodeInformation NodeInformation;
			internal ulong RequestId;
		}

		private struct RequestInformation
		{
			internal UdpMessage OutgoingMessage;
			internal bool SecondAttempt;
			internal DateTime SentAtTime;
		}

		internal Node() : this(12045)
		{
		}

		internal Node(int port)
		{
			kademlia = new Kademlia(this);
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

			var incomingMessage = Serializer.Deserialize<UdpMessage>(new MemoryStream(data));

			if (!incomingMessage.Validate(endPoint))
				return;

			var ni = new NodeInformation(endPoint, incomingMessage.SenderNodeId);

			if (incomingMessage.ResponseId != 0)
			{
				var identifier = new RequestIdentifier {NodeInformation = ni, RequestId = incomingMessage.ResponseId};
				if (!outstandingRequests.ContainsKey(identifier))
				{
					Console.WriteLine("Ignoring message from {0} with unknown response ID {1}", endPoint, incomingMessage.ResponseId);
					return;
				}
				var request = outstandingRequests[identifier];
				outstandingRequests.Remove(identifier);

				// the callback will deal with handling the actual response
				request.OutgoingMessage.ResponseCallback(incomingMessage);
			}

			if (incomingMessage.PingRequest != null)
			{
				IncomingPing(ni, incomingMessage);
			}
			else if (incomingMessage.FindNodeRequest != null)
			{
				IncomingFindNode(ni, incomingMessage);
			}
			else if (incomingMessage.FindValueRequest != null)
			{
				IncomingFindValue(ni, incomingMessage);
			}
			else if (incomingMessage.StoreRequest != null)
			{
				IncomingStore(ni, incomingMessage);
			}
			else if (incomingMessage.KeepObjectRequest != null)
			{
				IncomingKeepObject(ni, incomingMessage);
			}
		}

		/// <summary>Processes an incoming KeepObject request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingKeepObject(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			kademlia.HandleIncomingRequest(node, response);
			// TODO: reply.KeepObjectResponse
			// SendUdpMessage(response, node);

			throw new NotImplementedException();
		}

		/// <summary>Processes an incoming Store request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingStore(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			kademlia.HandleIncomingRequest(node, response);
			// TODO: reply.StoreResponse
			// SendUdpMessage(response, node);

			throw new NotImplementedException();
		}

		/// <summary>Processes an incoming FindValue request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingFindValue(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			kademlia.HandleIncomingRequest(node, response);
			// TODO: reply.FindValueResponse
			// SendUdpMessage(response, node);

			throw new NotImplementedException();
		}

		/// <summary>Processes an incoming FindNode request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingFindNode(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			kademlia.HandleIncomingRequest(node, response);
			// TODO: reply.FindNodeResponse
			// SendUdpMessage(response, node);

			throw new NotImplementedException();
		}

		/// <summary>Processes an incoming Ping request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingPing(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			kademlia.HandleIncomingRequest(node, response);
			SendUdpMessage(response, node);
		}

		private void TcpListen()
		{
			tcp.BeginAcceptTcpClient(OnTcpAccept, null);
		}

		private void OnTcpAccept(IAsyncResult ar)
		{
			var client = tcp.EndAcceptTcpClient(ar);
			TcpListen();

			new IncomingTcpConnection(client).Accept();
		}

		/// <summary>Creates a template reply UdpMessage with the ResponseId set based on the request message. Also notifies Kademlia about the request message.</summary>
		/// <param name="request">The incoming request message.</param>
		/// <returns>The created template reply message.</returns>
		private UdpMessage CreateUdpReply(UdpMessage request)
		{
			return new UdpMessage {ResponseId = request.RequestId};
		}

		/// <summary>Sends an UDP message to a given node.</summary>
		/// <param name="message">The message to be sent.</param>
		/// <param name="targetNode">The target node.</param>
		private void SendUdpMessage(UdpMessage message, NodeInformation targetNode)
		{
			var target = new IPEndPoint(targetNode.IpAddress, targetNode.Port);

			message.ProtocolVersion = ProtocolVersion;
			message.RequestId = Random.UInt64();
			message.SenderNodeId = Id;

			var s = new MemoryStream();
			Serializer.Serialize(s, message);

			Console.WriteLine("Sending message with length {0} with request ID {1:X16} to {2} (response ID {3:X16})", s.Length, message.RequestId, target, message.ResponseId);
			udp.Send(s.GetBuffer(), (int) s.Length, target);
		}
	}
}