using System;
using System.Globalization;
using System.Linq;

using Random = Kyru.Utilities.Random;

namespace Kyru.Network
{
	internal sealed class KademliaId : IComparable<KademliaId>
	{
		internal const int Size = 160;

		private const int ArraySize = Size / 8;

		private readonly byte[] id;

		internal KademliaId(byte[] bytes)
		{
			if (bytes.Length != ArraySize)
				throw new InvalidOperationException("The array must of size " + ArraySize);

			id = bytes;
		}

		public static KademliaId operator -(KademliaId left, KademliaId right)
		{
			var result = new byte[ArraySize];
			for (int i = 0; i < ArraySize; i++)
			{
				result[i] = (byte) (left.id[i] ^ right.id[i]);
			}
			return result;
		}

		internal static KademliaId RandomId
		{
			get
			{
				return Random.Bytes(ArraySize);
			}
		}

		public static implicit operator byte[](KademliaId id)
		{
			return id.Bytes;
		}

		public static implicit operator KademliaId(byte[] bytes)
		{
			return new KademliaId(bytes);
		}

		internal byte[] Bytes
		{
			get
			{
				var bytes = new byte[ArraySize];
				id.CopyTo(bytes, 0);
				return bytes;
			}
		}

		public int CompareTo(KademliaId other)
		{
			for (int i = 0; i < ArraySize; i++)
			{
				var result = id[i].CompareTo(other.id[i]);
				if (result != 0) return result;
			}
			return 0;
		}

		public override string ToString()
		{
			return id.Aggregate("", (s, u) => s + u.ToString("X2"));
		}

		internal int KademliaBucket()
		{
			if (id.All(i => i == 0))
			{
				throw new InvalidOperationException("BUG: the local node must not be added as a Kademlia contact");
			}

			int bucket = 159;

			foreach (byte b in id)
			{
				for (int i = 7; i >= 0; i--)
				{
					if (b >> i != 0)
					{
						return bucket;
					}
					bucket--;
				}
			}

			throw new NotImplementedException();
		}

		#region Equality (generated code)

		private bool Equals(KademliaId other)
		{
			return id.SequenceEqual(other.id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is KademliaId && Equals((KademliaId) obj);
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			foreach (uint n in id)
			{
				hashCode = (hashCode * 397) ^ (int) n;
			}
			return hashCode;
		}

		public static bool operator ==(KademliaId left, KademliaId right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(KademliaId left, KademliaId right)
		{
			return !Equals(left, right);
		}

		#endregion

		public static KademliaId FromHex(string hex)
		{
			var bytes = new byte[ArraySize];
			for (int i = 0; i < ArraySize; i++)
			{
				bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
			}
			return bytes;
		}
	}
}