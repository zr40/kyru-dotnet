using System;
using Kyru.Network;

namespace Kyru.Core
{
	internal class App
	{
        internal Config config;
        internal KObjectSet objectSet;
        internal Session session;
        internal Node node;

		internal void Start()
		{
            config = new Config();
            objectSet = new KObjectSet(config);
            node = new Node();
		}

        internal void Login(string username, string password)
        {
            session = new Session(username, password, config);
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