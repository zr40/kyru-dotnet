using System;
using System.Collections.Generic;

namespace Kyru.Core
{
	internal class KFile
	{
		public List<Chunk> Chunks;
		public string Extension;
		public string Name;

		public KFile(List<Chunk> chunks)
		{
			throw new NotImplementedException();
		}

		public List<Chunk> Split()
		{
			throw new NotImplementedException();
		}

		public void Rename(string newName)
		{
			throw new NotImplementedException();
		}

		public void Load(string Name)
		{
			throw new NotImplementedException();
		}

		public void Save(string Name)
		{
			throw new NotImplementedException();
		}
	}
}