using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kyru.Network;
using System.IO;

namespace Kyru.Core
{
    internal abstract class KObject
    {
        internal KademliaId id;

        /// <summary>
        /// Reads the file from the harddisk
        /// </summary>
        /// <param name="f">A stream of the file where the object is in</param>
        public abstract void Read(FileStream f);

        /// <summary>
        /// Writes the file to the harddisk
        /// </summary>
        /// <param name="f">A stream of the file</param>
        public abstract void Write(FileStream f);
    }
}
