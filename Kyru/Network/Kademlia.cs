using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

using Kyru.Network.Operations;
using Kyru.Network.UdpMessages;
using Kyru.Utilities;

namespace Kyru.Network
{
	internal sealed class Kademlia : ITimerListener
	{
		internal const int k = 20;
		internal const int alpha = 3;

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

		internal int CurrentContactCount
		{
			get
			{
				return buckets.Sum(l => l.Count);
			}
		}

		public long EstimatedNetworkSize
		{
			get
			{
				long nodes = 0;
				bool full = false;
				long currentFull = 0;
				for (int i = 0; i < 160; i++)
				{
					if (full)
					{
						currentFull *= 2;
						nodes += currentFull;
					}
					else if (buckets[i].Count == k)
					{
						full = true;
						if (buckets[i].Count < k / 2)
							currentFull = buckets[i].Count * 2;
						else
							currentFull = k;
						nodes += currentFull;
					}
					else
					{
						nodes += buckets[i].Count;
					}
				}
				return nodes;
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
			if (sendingNode.NodeId == node.Id)
				return;

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

		/// <summary>
		/// Adds a node to the kademlia contacts.
		/// </summary>
		internal void AddNode(IPEndPoint ep)
		{
			var ping = new UdpMessage();
			ping.PingRequest = new PingRequest();
			ping.ResponseCallback = response => AddVerifiedNode(ep, response.SenderNodeId);
			node.SendUdpMessage(ping, ep, null);
		}

		/// <summary>
		/// Adds a known-good node to the kademlia contacts. This must only be called if the identity of the node is verified, such as with correct ping replies or a TCP connection.
		/// </summary>
		internal void AddVerifiedNode(IPEndPoint endPoint, KademliaId nodeId)
		{
			AddContact(new NodeInformation(endPoint, nodeId));
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
			if (node.Id == contact.NodeId)
				return;

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
				//this.Log("Adding contact {0} ({1})", contact.NodeId, contact.EndPoint);
				bucket.Add(new KnownNode(contact));

				if (CurrentContactCount == 1)
				{
					// populate the contacts list
					NodeLookup(node.Id, nodes =>
					                    {
						                    foreach (var n in nodes)
						                    {
							                    NodeLookup(n.NodeId, _ => { });
						                    }
					                    });
				}
			}
			else
			{
				existingContact.LastSeen = DateTime.Now;
			}
		}

		/// <returns>Whether the contact needs to be pinged.</returns>
		private bool IsNewContact(NodeInformation contact)
		{
			if (node.Id == contact.NodeId)
				return false;

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

			for (int i = bucketId; i >= 0; i--)
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

		internal void NodeLookup(KademliaId id, Action<List<NodeInformation>> done)
		{
			new Thread(new NodeLookup(node, id, done).ThreadStart).Start();
		}

		internal void ValueLookup(KademliaId id, Action<List<NodeInformation>> done)
		{
			new Thread(new ValueLookup(node, id, done).ThreadStart).Start();
		}
	}
}