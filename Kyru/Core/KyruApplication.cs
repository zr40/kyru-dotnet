using Kyru.Network;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Threading;

namespace Kyru.Core
{
	internal sealed class KyruApplication
	{
		internal readonly Config Config;
		internal readonly LocalObjectStorage LocalObjectStorage;
		internal Session Session;
		internal readonly Node Node;

		internal KyruApplication() : this(12045)
		{
		}

		internal KyruApplication(ushort port)
		{
			Config = new Config();
			LocalObjectStorage = new LocalObjectStorage(Config);
			Node = new Node(port, this);
		}

		internal void Start()
		{
			Node.Start();
		}
	}
}