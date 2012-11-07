using System;

using Kyru.Network;

namespace Kyru.Core
{
	internal sealed class App
	{
		internal readonly Config Config;
		//internal KObjectSet objectSet;
		internal readonly LocalObjectStorage LocalObjectStorage;
		internal Session Session;
		internal readonly Node Node;

		internal App() : this(12045)
		{
		}

		internal App(ushort port)
		{
			Config = new Config();
			LocalObjectStorage = new LocalObjectStorage(Config);
			Node = new Node(port, this);
		}

		internal void Start()
		{
			Node.Start();
		}

		internal void Login(string username, string password)
		{
			Session = new Session(username, password, this);
		}

		internal int FindCopyCount(KademliaId objectId)
		{
			throw new NotImplementedException();
		}

		internal void GetAndDecryptAndStoreFile(string localPath /* TODO parameters: Session, File, Node, NetworkManager */)
		{
			throw new NotImplementedException();
		}
	}
}