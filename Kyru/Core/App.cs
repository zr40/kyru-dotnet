﻿using System;

using Kyru.Network;

namespace Kyru.Core
{
	internal sealed class App
	{
		internal readonly Config Config;
		//internal KObjectSet objectSet;
		internal readonly LocalObjectStorage LocalObjectStorage;
		internal Session Session;
		internal readonly Node node;

		internal App()
		{
			Config = new Config();
			LocalObjectStorage = new LocalObjectStorage(Config);
			node = new Node(this);
		}

		internal void Start()
		{
			node.Start();
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