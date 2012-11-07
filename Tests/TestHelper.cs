using System.Net;

using Kyru.Network;
using Kyru.Network.UdpMessages;

namespace Tests
{
	internal static class TestHelper
	{
		internal static void RegisterFakeContact(Kademlia kademlia, NodeInformation ni)
		{
			var message = new UdpMessage();
			kademlia.HandleIncomingRequest(ni, message);

			if (message.ResponseCallback == null)
				return;

			var response = new UdpMessage();
			response.ResponseId = message.RequestId;
			response.SenderNodeId = ni.NodeId;
			message.ResponseCallback(response);
		}

		internal enum PrepareType
		{
			Local,
			Remote,
			Both,
			Overlapped,
		}

		internal static void PrepareFakeContacts(Kademlia kademlia, int contacts)
		{
			for (int i = 0; i < contacts; i++)
			{
				var id = KademliaId.RandomId;
				var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), id);

				RegisterFakeContact(kademlia, ni);
			}
		}

		internal static void PrepareFakeContacts(Kademlia kademlia1, Kademlia kademlia2, int contacts)
		{
			for (int i = 0; i < contacts; i++)
			{
				var id = KademliaId.RandomId;
				var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), id);

				RegisterFakeContact(kademlia1, ni);
			}
			for (int i = 0; i < contacts; i++)
			{
				var id = KademliaId.RandomId;
				var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), id);

				RegisterFakeContact(kademlia2, ni);
			}
		}

		internal static void PrepareOverlappedFakeContacts(Kademlia kademlia1, Kademlia kademlia2, int contacts)
		{
			for (int i = 0; i < contacts; i++)
			{
				var id = KademliaId.RandomId;
				var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), id);

				RegisterFakeContact(kademlia1, ni);
			}
			for (int i = 0; i < contacts; i++)
			{
				var id = KademliaId.RandomId;
				var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), id);

				RegisterFakeContact(kademlia2, ni);
			}
			for (int i = 0; i < contacts; i++)
			{
				var id = KademliaId.RandomId;
				var ni = new NodeInformation(new IPEndPoint(IPAddress.Loopback, 12345), id);

				RegisterFakeContact(kademlia1, ni);
				RegisterFakeContact(kademlia2, ni);
			}
		}
	}
}