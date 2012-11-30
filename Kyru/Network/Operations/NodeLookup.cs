using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Kyru.Network.UdpMessages;

namespace Kyru.Network.Operations
{
	internal sealed class NodeLookup
	{
		private readonly Node node;
		private readonly KademliaId id;
		private readonly Action<List<NodeInformation>> done;
		private readonly AutoResetEvent ev = new AutoResetEvent(false);
		private readonly List<LookupListItem> nodeList = new List<LookupListItem>();
		private int pendingRequests;

		private sealed class LookupListItem : IComparable<LookupListItem>
		{
			private readonly KademliaId distance;
			internal readonly NodeInformation NodeInformation;
			internal bool Queried;

			internal LookupListItem(KademliaId distance, NodeInformation nodeInformation)
			{
				NodeInformation = nodeInformation;
				this.distance = distance;
			}

			public int CompareTo(LookupListItem other)
			{
				return distance.CompareTo(other.distance);
			}
		}

		internal NodeLookup(Node node, KademliaId id, Action<List<NodeInformation>> done)
		{
			this.node = node;
			this.id = id;
			this.done = done;
		}

		internal void ThreadStart()
		{
			// TODO matthijs: this has a valid reason; write it down
			Thread.Sleep(1000);

			foreach (var nearNode in node.Kademlia.NearestContactsTo(id, null))
			{
				nodeList.Add(new LookupListItem(id - nearNode.NodeId, nearNode));
			}

			nodeList.Sort();

			lock (nodeList)
			{
				while (pendingRequests != 0 || nodeList.Any(n => !n.Queried))
				{
					if (pendingRequests == Kademlia.α || nodeList.All(n => n.Queried))
					{
						try // unlock nodeList while waiting on the event
						{
							Monitor.Exit(nodeList);

							if (!ev.WaitOne((Node.TimeoutTicks * 2 + 1) * 1000))
							{
								throw new TimeoutException("BUG: neither ResponseCallback nor NoResponseCallback was called (" + node.Port + ")");
							}
						}
						finally
						{
							Monitor.Enter(nodeList);
						}

						if (nodeList.Count != 0)
							this.Warn("After wake, closest is {0}", id - nodeList[0].NodeInformation.NodeId);

						continue;
					}

					SendRequest(nodeList.First(n => !n.Queried));
				}

				done(nodeList.Select(n => n.NodeInformation).ToList());
			}
		}

		private void SendRequest(LookupListItem queryNode)
		{
			queryNode.Queried = true;

			this.Warn("{1} sending FindNode to {0}", queryNode.NodeInformation.Port, node.Port);

			var message = new UdpMessage();
			message.FindNodeRequest = new FindNodeRequest();
			message.FindNodeRequest.NodeId = id;
			message.ResponseCallback = response =>
			                           {
				                           var newNodes = response.FindNodeResponse.Nodes.Select(n => new LookupListItem(id - n.NodeId, n));
				                           lock (nodeList)
				                           {
					                           newNodes = newNodes.Where(n => nodeList.All(existing => existing.NodeInformation.NodeId != n.NodeInformation.NodeId));
					                           nodeList.AddRange(newNodes);
					                           nodeList.Sort();
					                           if (nodeList.Count > Kademlia.k)
					                           {
						                           nodeList.RemoveRange(Kademlia.k, nodeList.Count - Kademlia.k);
					                           }
					                           pendingRequests--;
				                           }
				                           ev.Set();
			                           };
			message.NoResponseCallback = () =>
			                             {
				                             lock (nodeList)
				                             {
					                             nodeList.Remove(queryNode);
					                             pendingRequests--;
				                             }
				                             ev.Set();
			                             };

			node.SendUdpMessage(message, queryNode.NodeInformation.EndPoint, queryNode.NodeInformation.NodeId);

			pendingRequests++;
		}
	}
}