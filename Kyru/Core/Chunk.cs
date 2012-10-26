using System;

using Kyru.Network;
using System.Security.Cryptography;
using System.IO;

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

        /// <summary>
        /// Reads the file from the harddisk
        /// </summary>
        /// <param name="f">A stream of the file where the object is in</param>
        public override void Read(FileStream f)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the file to the harddisk
        /// </summary>
        /// <param name="f">A stream of the file</param>
        public override void Write(FileStream f)
        {
            throw new NotImplementedException();
        }
	}
}