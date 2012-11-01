using System;
using System.Net;

using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract(SkipConstructor = true)]
	internal sealed class NodeInformation
	{
		[ProtoMember(1)]
		internal readonly KademliaId NodeId;

		[ProtoMember(2)]
		internal readonly uint IpAddress;

		[ProtoMember(3)]
		internal readonly ushort Port;

		internal IPEndPoint EndPoint
		{
			get
			{
				return new IPEndPoint(IpAddress, Port);
			}
		}

		public NodeInformation(IPEndPoint endPoint, KademliaId nodeId)
		{
			NodeId = nodeId;
			IpAddress = BitConverter.ToUInt32(endPoint.Address.GetAddressBytes(), 0);
			Port = (ushort) endPoint.Port;
		}

		#region Equality (generated code)

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is NodeInformation && Equals((NodeInformation) obj);
		}

		private bool Equals(NodeInformation other)
		{
			return Equals(NodeId, other.NodeId) && IpAddress == other.IpAddress && Port == other.Port;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (NodeId != null ? NodeId.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int) IpAddress;
				hashCode = (hashCode * 397) ^ Port.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(NodeInformation left, NodeInformation right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(NodeInformation left, NodeInformation right)
		{
			return !Equals(left, right);
		}

		#endregion
	}
}