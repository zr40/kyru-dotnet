using System;
using System.Collections.Generic;

namespace Kyru.Core
{
	internal class KFile
	{
		internal List<Chunk> Chunks;
		internal string Extension;
		internal string Name;

		internal KFile(List<Chunk> chunks)
		{
			throw new NotImplementedException();
		}

		internal List<Chunk> Split()
		{
			throw new NotImplementedException();
		}

		internal void Rename(string newName)
		{
			throw new NotImplementedException();
		}

		internal void Load(string Name)
		{
			throw new NotImplementedException();
		}

		internal void Save(string Name)
		{
			throw new NotImplementedException();
		}
	}
}