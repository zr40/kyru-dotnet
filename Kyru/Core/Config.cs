using System;
using System.IO;

namespace Kyru.Core
{
	internal sealed class Config
	{
		internal string StoreDirectory;

		internal Config()
		{
			StoreDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Kyru", "objects");
		}
	}
}