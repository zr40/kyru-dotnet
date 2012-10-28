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

			var message = Serializer.Deserialize<UdpMessage>(new MemoryStream(data));

			if (!message.Validate(endPoint))
				return;

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

				// the callback will deal with handling the actual response
				request.ResponseCallback(message);
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

		/// <summary>Processes an incoming KeepObject request.</summary>
		/// <param name="ni">The sending node</param>
		/// <param name="message">The message received from the sending node</param>
		private void IncomingKeepObject(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			//TODO: reply.KeepObjectResponse
			SendUdpMessage(reply, ni);

			throw new NotImplementedException();
		}

		/// <summary>Processes an incoming Store request.</summary>
		/// <param name="ni">The sending node</param>
		/// <param name="message">The message received from the sending node</param>
		private void IncomingStore(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			//TODO: reply.StoreResponse
			SendUdpMessage(reply, ni);

			throw new NotImplementedException();
		}

		/// <summary>Processes an incoming FindValue request.</summary>
		/// <param name="ni">The sending node</param>
		/// <param name="message">The message received from the sending node</param>
		private void IncomingFindValue(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			//TODO: reply.FindValueResponse
			SendUdpMessage(reply, ni);

			throw new NotImplementedException();
		}

		/// <summary>Processes an incoming FindNode request.</summary>
		/// <param name="ni">The sending node</param>
		/// <param name="message">The message received from the sending node</param>
		private void IncomingFindNode(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			//TODO: reply.FindNodeResponse
			SendUdpMessage(reply, ni);

			throw new NotImplementedException();
		}

		/// <summary>Processes an incoming Ping request.</summary>
		/// <param name="ni">The sending node</param>
		/// <param name="message">The message received from the sending node</param>
		private void IncomingPing(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			SendUdpMessage(reply, ni);
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
			var response = new UdpMessage();
			response.ResponseId = request.RequestId;

			// The Kademlia object is interested in all incoming requests in order to maintain the contact list, and all valid requests create a response.
			// Kademlia might add a Ping request to the response, so this is the place to notify the kademlia object.
			kademlia.HandleRequest(request, response);

			return response;
		}

		/// <summary>Sends an UDP message to a given node.</summary>
		/// <param name="message">The message to be sent.</param>
		/// <param name="targetNode">The target node.</param>
		private void SendUdpMessage(UdpMessage message, NodeInformation targetNode)
		{
			var target = new IPEndPoint(targetNode.IpAddress, targetNode.Port);

			message.ProtocolVersion = ProtocolVersion;
			message.RequestId = Random.UInt64();
			message.SenderNodeId = id;

			var s = new MemoryStream();
			Serializer.Serialize(s, message);

			Console.WriteLine("Sending message with length {0} with request ID {1:X16} to {2} (response ID {3:X16})", s.Length, message.RequestId, target, message.ResponseId);
			udp.Send(s.GetBuffer(), (int) s.Length, target);
		}
	}
}