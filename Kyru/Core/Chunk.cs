using System;

using Kyru.Network;
using System.Security.Cryptography;
using System.IO;
using ProtoBuf;

namespace Kyru.Core
{
	[ProtoContract]
	internal class Chunk : KObject
	{
		[ProtoMember(1)]
		internal readonly byte[] Data;

        /// <summary>
        /// The KObjectSet requires a constructor with no arguments. You should always follow this call with a Read call.
        /// </summary>
		public Chunk() {
            
		}

		internal Chunk(byte[] Data)
		{
			this.Data = Data;

			//A chunk object’s ID is the SHA1 of the entire object.
			SHA1 sha1 = SHA1.Create();
			byte[] hash = sha1.ComputeHash(Data);
			Id = new KademliaId(hash);
		}
	}
}