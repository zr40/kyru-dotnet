using System;
using System.Net;
using System.Text;

using ProtoBuf;

namespace Kyru.Network.Messages
{
	[ProtoContract]
	internal sealed class UdpMessage
	{
		[ProtoMember(1)]
		internal uint ProtocolVersion;

		[ProtoMember(2)]
		internal KademliaId SenderNodeId;

		[ProtoMember(3)]
		internal ulong RequestId;

		[ProtoMember(4)]
		internal ulong ResponseId;

		[ProtoMember(5)]
		internal PingRequest PingRequest;

		[ProtoMember(6)]
		internal FindNodeRequest FindNodeRequest;

		[ProtoMember(7)]
		internal FindNodeResponse FindNodeResponse;

		[ProtoMember(8)]
		internal FindValueRequest FindValueRequest;

		[ProtoMember(9)]
		internal FindValueResponse FindValueResponse;

		[ProtoMember(10)]
		internal StoreRequest StoreRequest;

		[ProtoMember(11)]
		internal StoreResponse StoreResponse;

		[ProtoMember(12)]
		internal KeepObjectRequest KeepObjectRequest;

		[ProtoMember(13)]
		internal KeepObjectResponse KeepObjectResponse;

		internal Action<UdpMessage> ResponseCallback;

		internal Action NoResponseCallback;

		internal string Inspect()
		{
			var sb = new StringBuilder();
			if (FindNodeRequest != null)
			{
				sb.Append("FindNode request");
			}
			if (FindValueRequest != null)
			{
				sb.Append("FindValue request");
			}
			if (StoreRequest != null)
			{
				sb.Append("Store request");
			}
			if (KeepObjectRequest != null)
			{
				sb.Append("KeepObject request");
			}

			if (FindNodeResponse != null)
			{
				sb.Append("FindNode response");
			}
			if (FindValueResponse != null)
			{
				sb.Append("FindValue response");
			}
			if (StoreResponse != null)
			{
				sb.Append("Store response");
			}
			if (KeepObjectResponse != null)
			{
				sb.Append("KeepObject response");
			}

			if (PingRequest != null)
			{
				if (sb.Length != 0)
				{
					sb.Append(" with ");
				}
				sb.Append("Ping request");
			}

			if (sb.Length == 0)
			{
				if (ResponseId == 0)
				{
					sb.Append("empty message");
				}
				else
				{
					sb.Append("Ping response");
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// 	Check for various types of errors in the message format. If there is an error it will be printed to the console.
		/// </summary>
		/// <param name="endPoint"> (For debugging purposes), a string containing the address where the message is from. </param>
		/// <returns> true when no error is found </returns>
		internal bool Validate(IPEndPoint endPoint)
		{
			if (Node.ProtocolVersion != ProtocolVersion)
			{
				Console.WriteLine("Ignoring message from {0} with unknown protocol version {1}", endPoint, ProtocolVersion);
				return false;
			}

			if (ResponseId != 0)
			{
				// responses may not contain any requests besides ping
				if (FindNodeRequest != null)
				{
					Console.WriteLine("Ignoring response from {0} with response ID {1:X16} containing a find node request", endPoint, ResponseId);
					return false;
				}
				if (FindValueRequest != null)
				{
					Console.WriteLine("Ignoring response from {0} with response ID {1:X16} containing a find value request", endPoint, ResponseId);
					return false;
				}
				if (KeepObjectRequest != null)
				{
					Console.WriteLine("Ignoring response from {0} with response ID {1:X16} containing a keep object request", endPoint, ResponseId);
					return false;
				}
				if (StoreRequest != null)
				{
					Console.WriteLine("Ignoring response from {0} with response ID {1:X16} containing a store request", endPoint, ResponseId);
					return false;
				}
			}

			var requests = 0;
			if (FindNodeRequest != null)
			{
				requests++;
			}
			if (FindValueRequest != null)
			{
				requests++;
			}
			if (KeepObjectRequest != null)
			{
				requests++;
			}
			if (PingRequest != null)
			{
				requests++;
			}
			if (StoreRequest != null)
			{
				requests++;
			}

			if (requests > 1)
			{
				Console.WriteLine("Ignoring message from {0} with request ID {1:X16} containing multiple requests", endPoint, ResponseId);
				return false;
			}
			if (requests == 0 && ResponseId == 0)
			{
				Console.WriteLine("Ignoring empty message from {0} with request ID {1:X16}.", endPoint, ResponseId);
				return false;
			}
			return true;
		}
	}
}