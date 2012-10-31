﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Kyru.Network.Messages;

namespace Kyru.Network
{
	internal sealed class Kademlia
	{
		private const int k = 20;
		private const int α = 3;

		private class KnownNode
		{
			internal readonly NodeInformation Node;
			internal readonly DateTime LastSeen;

			internal KnownNode(NodeInformation node)
			{
				Node = node;
				LastSeen = DateTime.Now;
			}
		}

		private readonly Node node;

		private readonly List<KnownNode>[] buckets = new List<KnownNode>[KademliaId.Size];

		internal int CurrentContacts
		{
			get
			{
				return buckets.Sum(l => l.Count);
			}
		}

		internal Kademlia(Node node)
		{
			this.node = node;
			for (int i = 0; i < KademliaId.Size; i++)
			{
				buckets[i] = new List<KnownNode>();
			}
		}

		/// <summary>
		/// Notifies Kademlia about incoming request messages, to maintain the contact list. If necessary, a ping request is added to the response message.
		/// </summary>
		/// <param name="sendingNode">The node that sent us a request.</param>
		/// <param name="outgoingMessage">The message object that will be sent.</param>
		internal void HandleIncomingRequest(NodeInformation sendingNode, UdpMessage outgoingMessage)
		{
			if (PingRequired(sendingNode))
			{
				outgoingMessage.PingRequest = new PingRequest();
				outgoingMessage.ResponseCallback = message => AddContact(sendingNode);
				outgoingMessage.NoResponseCallback = () => RemoveContact(sendingNode);
			}
			else
			{
				AddContact(sendingNode);
			}
		}

		internal void AddNode(IPEndPoint ep)
		{
			var ping = new UdpMessage();
			ping.PingRequest = new PingRequest();
			ping.ResponseCallback = response => AddContact(new NodeInformation(ep, response.SenderNodeId));
			node.SendUdpMessage(ping, ep);
		}

		private void AddContact(NodeInformation contact)
		{
			if (!RemoveContact(contact))
				Console.WriteLine("Kademlia: Adding contact {0} ({1})", contact.NodeId, new IPEndPoint(contact.IpAddress, contact.Port));

			var bucket = (node.Id - contact.NodeId).KademliaBucket();
			if (buckets[bucket].Count >= k)
			{
				buckets[bucket].RemoveAt(0);
			}
			buckets[bucket].Add(new KnownNode(contact));
		}

		/// <returns>Whether the contact needs to be pinged.</returns>
		private bool PingRequired(NodeInformation contact)
		{
			var bucket = buckets[(node.Id - contact.NodeId).KademliaBucket()];
			return bucket.Find(n => n.Node == contact) == null;
		}

		/// <returns>Whether the contact was known.</returns>
		private bool RemoveContact(NodeInformation contact)
		{
			var bucket = (node.Id - contact.NodeId).KademliaBucket();
			return buckets[bucket].RemoveAll(n => n.Node == contact) != 0;
		}
	}
}