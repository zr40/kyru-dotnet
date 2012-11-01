﻿using System;
using System.Linq;

using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract(SkipConstructor = true),Serializable]
	internal sealed class KademliaId
	{
		internal const int Size = 160;

		private const int ArraySize = Size / 8;

		[ProtoMember(1)]
		private readonly byte[] id;

		public KademliaId(byte[] bytes)
		{
			if (bytes.Length != ArraySize)
				throw new Exception("The array must of size " + ArraySize);

			id = bytes;
		}

		public static KademliaId operator -(KademliaId left, KademliaId right)
		{
			var result = new byte[ArraySize];
			for (int i = 0; i < ArraySize; i++)
			{
				result[i] = (byte) (left.id[i] ^ right.id[i]);
			}
			return new KademliaId(result);
		}

		internal static KademliaId RandomId
		{
			get
			{
				var bytes = Random.Bytes(ArraySize);

				return new KademliaId(bytes);
			}
		}

		internal byte[] Bytes
		{
			get
			{
				var bytes = new byte[ArraySize];
				id.CopyTo(bytes, ArraySize);
				return bytes;
			}
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
	}
}