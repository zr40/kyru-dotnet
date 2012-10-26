using System;

using Kyru.Network;
using System.Security.Cryptography;
using System.IO;

namespace Kyru.Core
{
	internal class Chunk : KObject
	{
		internal readonly byte[] Data;

        /// <summary>
        /// The KObjectSet requires a constructor with no arguments. You should always follow this call with a Read call.
        /// </summary>
        internal Chunk() {
            
        }

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
            // NOTE: the conversion to int should not give problems because chunks should be 1 MiB or less
            f.Read(this.Data,0,(int)f.Length);
        }

        /// <summary>
        /// Writes the file to the harddisk
        /// </summary>
        /// <param name="f">A stream of the file</param>
        public override void Write(FileStream f)
        {
            // NOTE: the conversion to int should not give problems because chunks should be 1 MiB or less
            f.Write(this.Data, 0, (int)f.Length);
        }
	}
}