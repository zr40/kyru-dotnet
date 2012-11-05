using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kyru.Network;
using System.IO;
using ProtoBuf;

namespace Kyru.Core
{
	[ProtoContract]
	internal abstract class KObject
	{
		[ProtoMember(1)]
		internal KademliaId Id;
	}
}
