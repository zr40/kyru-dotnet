﻿using System;
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

		/// <summary>
		/// 	Create a sorted list of nodes of size k, containing the k nodes closest to the object ID. The list is sorted by distance to the object ID, closest first.
		/// </summary>
		/// <param name="ni"> Information about the other node </param>
		/// <param name="message"> Message from the other node </param>
		private void IncomingKeepObject(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			//TODO: reply.KeepObjectResponse
			SendUdpMessage(reply, ni);

			throw new NotImplementedException();
		}

		/// <summary>
		/// 	The STORE method allows nodes to store a value at another node.
		/// </summary>
		/// <param name="ni"> Information about the other node </param>
		/// <param name="message"> Message from the other node </param>
		private void IncomingStore(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			//TODO: reply.StoreResponse
			SendUdpMessage(reply, ni);

			throw new NotImplementedException();
		}

		/// <summary>
		/// 	The FIND_VALUE method acts like FIND_NODE, but if the node contains the value, it will be returned instead.
		/// </summary>
		/// <param name="ni"> Information about the other node </param>
		/// <param name="message"> Message from the other node </param>
		private void IncomingFindValue(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			//TODO: reply.FindValueResponse
			SendUdpMessage(reply, ni);

			throw new NotImplementedException();
		}

		/// <summary>
		/// 	The FIND_NODE method requests a node to return the k nodes closest to the given object ID that the node knows about.
		/// </summary>
		/// <param name="ni"> Information about the other node </param>
		/// <param name="message"> Message from the other node </param>
		private void IncomingFindNode(NodeInformation ni, UdpMessage message)
		{
			UdpMessage reply = CreateUdpReply(message);
			//TODO: reply.FindNodeResponse
			SendUdpMessage(reply, ni);

			throw new NotImplementedException();
		}

		/// <summary>
		/// 	The PING method requests a node to respond. If it responds, the node is known to be alive at this point.
		/// 	PING is also used to obtain the node ID from newly discovered nodes.
		/// </summary>
		/// <param name="ni"> Information about the other node </param>
		/// <param name="message"> Message from the other node </param>
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

			// TODO
		}

		private UdpMessage CreateUdpReply(UdpMessage request)
		{
			var response = new UdpMessage();
			response.ResponseId = request.RequestId;

			// The Kademlia object is interested in all incoming requests in order to maintain the contact list, and all valid requests create a response.
			// Kademlia might add a Ping request to the response, so this is the place to notify the kademlia object.
			kademlia.HandleRequest(request, response);

			return response;
		}

		/// <summary>
		/// 	Send a message.
		/// </summary>
		/// <param name="message"> The message to be sent </param>
		/// <param name="targetNode"> The node to send it to </param>
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