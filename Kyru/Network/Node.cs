using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Kyru.Core;
using Kyru.Network.TcpMessages;
using Kyru.Network.TcpMessages.Clients;
using Kyru.Network.UdpMessages;
using Kyru.Utilities;
using ProtoBuf;
using Random = Kyru.Utilities.Random;

namespace Kyru.Network
{
	internal sealed class Node : ITimerListener, IDisposable
	{
		internal ushort Port { get; private set; }
		private readonly KyruApplication app;
		private readonly UdpClient udp;
		private readonly TcpListener tcp;

		internal readonly Kademlia Kademlia;
		private bool running;

		internal const uint ProtocolVersion = 0;
		internal const int TimeoutTicks = 2 + 1;
		internal readonly KademliaId Id = KademliaId.RandomId;

		private readonly Dictionary<RequestIdentifier, RequestInformation> outstandingRequests = new Dictionary<RequestIdentifier, RequestInformation>();
		private readonly MetadataStorage metadataStorage;

		private struct RequestIdentifier
		{
			internal IPEndPoint EndPoint;
			internal ulong RequestId;
		}

		private sealed class RequestInformation
		{
			internal UdpMessage OutgoingMessage;
			internal bool SecondAttempt;
			internal uint Age;
			internal KademliaId NodeId;
		}

		internal Node(KyruApplication app) : this(12045, app)
		{
		}

		internal Node(ushort port, KyruApplication app)
		{
			Port = port;
			this.app = app;
			metadataStorage = new MetadataStorage(this);
			Kademlia = new Kademlia(this);

			udp = new UdpClient(port);
			tcp = new TcpListener(IPAddress.Any, port);
			KyruTimer.Register(this, 1);
		}

		internal void Start()
		{
			if (running)
				throw new InvalidOperationException("node has already been started");
			running = true;

			UdpListen();
			tcp.Start();
			TcpListen();

			this.Log("Node {0} started", tcp.LocalEndpoint);
		}

		private void UdpListen()
		{
			udp.BeginReceive(OnUdpReceive, null);
		}

		private void OnUdpReceive(IAsyncResult ar)
		{
			if (!running)
			{
				return;
			}
			var endPoint = new IPEndPoint(IPAddress.Any, 0);
			var data = udp.EndReceive(ar, ref endPoint);
			UdpListen();

			var incomingMessage = Serializer.Deserialize<UdpMessage>(new MemoryStream(data));

			if (!incomingMessage.Validate(endPoint))
				return;

			var ni = new NodeInformation(endPoint, incomingMessage.SenderNodeId);

			if (incomingMessage.ResponseId != 0)
			{
				var identifier = new RequestIdentifier {EndPoint = ni.EndPoint, RequestId = incomingMessage.ResponseId};

				lock (outstandingRequests)
				{
					if (!outstandingRequests.ContainsKey(identifier))
					{
						this.Warn("{2} from {0} has unknown response ID {1:X16}", endPoint, incomingMessage.ResponseId, incomingMessage.Inspect());
					}
					else
					{
						var request = outstandingRequests[identifier];

						if (request.NodeId != null && request.NodeId != incomingMessage.SenderNodeId)
						{
							this.Warn("In {0}, node ID from {1} does not match (expected {2}, received {3})", incomingMessage.Inspect(), endPoint, request.NodeId, incomingMessage.SenderNodeId);
						}
						else
						{
							outstandingRequests.Remove(identifier);

							// the callback will deal with handling the actual response
							request.OutgoingMessage.ResponseCallback(incomingMessage);
						}
					}
				}
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
			Kademlia.HandleIncomingRequest(node, response);

			response.KeepObjectResponse = new KeepObjectResponse();
			response.KeepObjectResponse.HasObject = app.LocalObjectStorage.KeepObject(request.KeepObjectRequest.ObjectId);

			SendUdpMessage(response, node);
		}

		/// <summary>Processes an incoming Store request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingStore(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			Kademlia.HandleIncomingRequest(node, response);

			metadataStorage.Store(request.StoreRequest.ObjectId, request.StoreRequest.Data);

			response.StoreResponse = new StoreResponse();
			SendUdpMessage(response, node);
		}

		/// <summary>Processes an incoming FindValue request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingFindValue(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			Kademlia.HandleIncomingRequest(node, response);

			response.FindValueResponse = new FindValueResponse();

			var metadata = metadataStorage.Get(request.FindValueRequest.ObjectId);
			if (metadata == null)
			{
				var contacts = Kademlia.NearestContactsTo(request.FindValueRequest.ObjectId, request.SenderNodeId);
				response.FindValueResponse.Nodes = contacts.ToArray();
			}
			else
			{
				response.FindValueResponse.Data = metadata;
			}

			SendUdpMessage(response, node);
		}

		/// <summary>Processes an incoming FindNode request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingFindNode(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			Kademlia.HandleIncomingRequest(node, response);

			var contacts = Kademlia.NearestContactsTo(request.FindNodeRequest.NodeId, node.NodeId);
			response.FindNodeResponse = new FindNodeResponse();
			response.FindNodeResponse.Nodes = contacts.ToArray();

			SendUdpMessage(response, node);
		}

		/// <summary>Processes an incoming Ping request.</summary>
		/// <param name="node">The sending node</param>
		/// <param name="request">The message received from the sending node</param>
		private void IncomingPing(NodeInformation node, UdpMessage request)
		{
			UdpMessage response = CreateUdpReply(request);
			Kademlia.HandleIncomingRequest(node, response);
			SendUdpMessage(response, node);
		}

		private void TcpListen()
		{
			tcp.BeginAcceptTcpClient(OnTcpAccept, null);
		}

		private void OnTcpAccept(IAsyncResult ar)
		{
			if (!running)
				return;

			var client = tcp.EndAcceptTcpClient(ar);
			TcpListen();

			new IncomingTcpConnection(app, client).Accept();
		}

		internal void GetObject(KademliaId objectId, Action<Error, byte[]> done)
		{
			new Thread(() => {
				Kademlia.NodeLookup(objectId,list => {
					GetObjectRecursive(list,objectId,done);
				});
			}).Start();
		}

		private void GetObjectRecursive(List<NodeInformation> targetNodes, KademliaId objectId, Action<Error, byte[]> done)
		{
			if (targetNodes.Count == 0){
				done(Error.NotFound, new byte[0]);
				return;
			}
			var targetNode = targetNodes[0];
			targetNodes.RemoveAt(0);
			new GetObjectClient(app, targetNode, objectId, (error, data) =>
			{
				switch (error)
				{
					case Error.NotFound:
						GetObjectRecursive(targetNodes, objectId, done);
						return;
					case Error.ObjectAlreadyStored:
					case Error.StoreRejected:
						done(error, new byte[0]);
						return;
					case Error.Success:
						done(error, data);
						return;
				}
			}).ThreadStart();
		}

		internal void GetObject(NodeInformation targetNode, KademliaId objectId, Action<Error, byte[]> done)
		{
			new Thread(new GetObjectClient(app, targetNode, objectId, done).ThreadStart).Start();
		}

		internal void StoreObject(NodeInformation targetNode, KademliaId objectId, byte[] bytes, Action<Error> done)
		{
			new Thread(new StoreObjectClient(app, targetNode, objectId, bytes, done).ThreadStart).Start();
		}

		/// <summary>Creates a template reply UdpMessage with the ResponseId set based on the request message. Also notifies Kademlia about the request message.</summary>
		/// <param name="request">The incoming request message.</param>
		/// <returns>The created template reply message.</returns>
		private static UdpMessage CreateUdpReply(UdpMessage request)
		{
			return new UdpMessage {ResponseId = request.RequestId};
		}

		/// <summary>Sends an UDP message to a given node.</summary>
		/// <param name="message">The message to be sent.</param>
		/// <param name="targetNode">The target node.</param>
		private void SendUdpMessage(UdpMessage message, NodeInformation targetNode)
		{
			SendUdpMessage(message, targetNode.EndPoint, targetNode.NodeId);
		}

		/// <summary>Sends an UDP message to a given node.</summary>
		/// <param name="message">The message to be sent.</param>
		/// <param name="target">The address of the target node.</param>
		/// <param name="targetNodeId">The target node ID.</param>
		internal void SendUdpMessage(UdpMessage message, IPEndPoint target, KademliaId targetNodeId)
		{
			message.ProtocolVersion = ProtocolVersion;
			message.RequestId = Random.UInt64();
			message.SenderNodeId = Id;

			var requestIdentifier = new RequestIdentifier {EndPoint = target, RequestId = message.RequestId};
			var requestInformation = new RequestInformation {OutgoingMessage = message, SecondAttempt = false, NodeId = targetNodeId};

			if (message.IsRequest)
			{
				lock (outstandingRequests)
				{
					outstandingRequests.Add(requestIdentifier, requestInformation);
				}
			}

			SendUdp(message, target);
		}

		private void SendUdp(UdpMessage message, IPEndPoint target)
		{
			var s = new MemoryStream();
			Serializer.Serialize(s, message);

			this.Log("Sending {4} with length {0} with request ID {1:X16} to {2} (response ID {3:X16})", s.Length, message.RequestId, target, message.ResponseId, message.Inspect());
			udp.Send(s.GetBuffer(), (int) s.Length, target);
		}

		public void Dispose()
		{
			running = false;
			udp.Close();
			tcp.Stop();

			this.Log("Node {0} stopped", tcp.LocalEndpoint);
		}

		public void TimerElapsed()
		{
			var toRemove = new List<RequestIdentifier>();
			lock (outstandingRequests)
			{
				foreach (var outstandingRequest in outstandingRequests)
				{
					var key = outstandingRequest.Key;
					var ri = outstandingRequest.Value;

					ri.Age++;

					if (ri.Age >= TimeoutTicks)
					{
						if (ri.SecondAttempt)
						{
							this.Warn("{0} does not respond to message {1:X8}", key.EndPoint, key.RequestId);
							toRemove.Add(key);
							if (ri.OutgoingMessage.NoResponseCallback != null)
								ri.OutgoingMessage.NoResponseCallback();
							if (ri.NodeId != null)
								Kademlia.RemoveNode(ri.NodeId);
						}
						else
						{
							this.Warn("Resending message {1:X8} to {0}", key.EndPoint, key.RequestId);
							ri.Age = 0;
							ri.SecondAttempt = true;
							SendUdp(ri.OutgoingMessage, key.EndPoint);
						}
					}
				}
				foreach (var key in toRemove)
				{
					outstandingRequests.Remove(key);
				}
			}
		}
	}
}