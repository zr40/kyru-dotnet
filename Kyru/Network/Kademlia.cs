using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Kyru.Network.Messages;

namespace Kyru.Network
{
	internal sealed class Kademlia : ITimerListener
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

		/// <remarks>Used by tests</remarks>
		private KnownNode FirstContact()
		{
			var nodes = new List<KnownNode>();
			foreach (var bucket in buckets)
			{
				nodes.AddRange(bucket);
			}
			return nodes[0];
		}

		internal Kademlia(Node node)
		{
			this.node = node;
			for (int i = 0; i < KademliaId.Size; i++)
			{
				buckets[i] = new List<KnownNode>();
			}

			KyruTimer.Register(this, 60);
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
			node.SendUdpMessage(ping, ep, null);
		}

		/// <summary>
		/// Removes a node that didn't respond from the Kademlia contact list.
		/// </summary>
		internal void RemoveNode(KademliaId nodeId)
		{
			var bucket = (node.Id - nodeId).KademliaBucket();
			if (buckets[bucket].RemoveAll(n => n.Node.NodeId == nodeId) != 0)
			{
				this.Log("Removing contact {0}", nodeId);
			}
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
				this.Log("Adding contact {0} ({1})", contact.NodeId, contact.EndPoint);
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

		public void TimerElapsed()
		{
			foreach (var bucket in buckets)
			{
				foreach (var contact in bucket)
				{
					if ((DateTime.Now - contact.LastSeen).TotalHours > 1)
					{
						CheckAlive(contact, bucket);
					}
				}
			}
		}

		private void CheckAlive(KnownNode contact, List<KnownNode> bucket)
		{
			var ping = new UdpMessage();
			ping.PingRequest = new PingRequest();
			ping.ResponseCallback = response => contact.LastSeen = DateTime.Now;
			ping.NoResponseCallback = () => bucket.Remove(contact);
			node.SendUdpMessage(ping, contact.Node.EndPoint, null);
		}

		internal List<NodeInformation> NearestContactsTo(KademliaId nearToId, KademliaId ignoreId)
		{
			var bucketId = nearToId.KademliaBucket();

			var contacts = new List<NodeInformation>();
			if (GetKContacts(contacts, bucketId, ignoreId))
				return contacts;

			for (int i = bucketId - 1; i >= 0; i--)
			{
				if (GetKContacts(contacts, i, ignoreId))
					return contacts;
			}

			for (int i = bucketId + 1; i < KademliaId.Size; i++)
			{
				if (GetKContacts(contacts, i, ignoreId))
					return contacts;
			}

			return contacts;
		}

		private bool GetKContacts(List<NodeInformation> contacts, int bucket, KademliaId ignoreId)
		{
			foreach (var contact in buckets[bucket])
			{
				if (contact.Node.NodeId == ignoreId)
					continue;

				contacts.Add(contact.Node);
				if (contacts.Count == k)
					return true;
			}
			return false;
		}
	}
}