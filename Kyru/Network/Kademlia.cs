using System;
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
			internal DateTime LastSeen;

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
			if (IsNewContact(sendingNode))
			{
				outgoingMessage.PingRequest = new PingRequest();
				outgoingMessage.ResponseCallback = message => AddContact(new NodeInformation(sendingNode.EndPoint, message.SenderNodeId));
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

		/// <summary>
		/// Removes a node that didn't respond from the Kademlia contact list.
		/// </summary>
		internal void RemoveNode(KademliaId nodeId)
		{
			var bucket = (node.Id - nodeId).KademliaBucket();
			buckets[bucket].RemoveAll(n => n.Node.NodeId == nodeId);
		}

		private void AddContact(NodeInformation contact)
		{
			var bucketIndex = (node.Id - contact.NodeId).KademliaBucket();
			var bucket = buckets[bucketIndex];

			var existingContact = bucket.FirstOrDefault(n => n.Node == contact);
			if (existingContact == null)
			{
				if (bucket.Count >= k)
				{
					// the bucket is full
					return;
				}
				Console.WriteLine("Kademlia: Adding contact {0} ({1})", contact.NodeId, contact.EndPoint);
				bucket.Add(new KnownNode(contact));
			}
			else
			{
				existingContact.LastSeen = DateTime.Now;
			}
		}

		/// <returns>Whether the contact needs to be pinged.</returns>
		private bool IsNewContact(NodeInformation contact)
		{
			var bucket = buckets[(node.Id - contact.NodeId).KademliaBucket()];
			if (bucket.Count >= k)
			{
				// the bucket is full
				return false;
			}
			return bucket.Find(n => n.Node == contact) == null;
		}
	}
}