using System;

using Kyru.Network;
using System.Security.Cryptography;

namespace Kyru.Core
{
	internal class Chunk : KObject
	{
		internal readonly byte[] Data;

        internal Chunk(byte[] Data)
		{
            this.Data = Data;

            //A chunk object’s ID is the SHA1 of the entire object.
            SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Data);
            id = new KademliaId(hash);
		}

		internal void Load()
		{
			throw new NotImplementedException();
		}

		internal void Save()
		{
			throw new NotImplementedException();
		}
	}
}