using System;
using System.IO;

namespace Kyru.Core
{
	internal sealed class Config
	{
		internal string storeDirectory;

		internal Config()
		{
			storeDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Kyru", "objects");
		}
	}
}