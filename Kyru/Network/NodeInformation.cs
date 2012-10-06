using System;
using System.Net;

using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class NodeInformation
	{
		[ProtoMember(1)]
		internal KademliaId NodeId;

		[ProtoMember(2)]
		internal uint IpAddress;

		[ProtoMember(3)]
		internal ushort Port;

		public NodeInformation(IPEndPoint endPoint, KademliaId nodeId)
		{
			NodeId = nodeId;
			IpAddress = BitConverter.ToUInt32(endPoint.Address.GetAddressBytes(), 0);
			Port = (ushort) endPoint.Port;
		}
	}
}