using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Kyru.Network;
using Kyru.Network.Objects;

namespace Kyru.Core
{
	/// <summary>
	/// This class keeps track of all kyru objects on the file system. Note that not all objects have to be in memory.
	/// </summary>
	internal class KObjectSet
	{
		private Config config;

		/// <summary>
		/// Currently not in use.
		/// The cache can give a list of some items that are in memory such that retrieving them is faster.
		/// </summary>
		private Dictionary<KademliaId, KyruObject> cache;

		private BinaryFormatter formatter = new BinaryFormatter();

		public KObjectSet(Config config)
		{
			this.config = config;
		}

		/// <summary>
		/// Retrieve an object from.
		/// </summary>
		/// <param name="id">The id of the object</param>
		/// <returns>The object, or null if he can't find it</returns>
		internal KyruObject Get(KademliaId id)
		{
			var idString = id.ToString();
			String path = Path.Combine(config.storeDirectory, idString + ".obj");
			FileStream fs = null;
			try
			{
				fs = new FileStream(path, FileMode.Open);
				var returnValue = (KyruObject) formatter.Deserialize(fs);
				fs.Close();
				return returnValue;
			}
			catch (FileNotFoundException)
			{
				return null;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
		}

		/// <summary>
		/// Method to retrieve a list of items.
		/// </summary>
		/// <param name="ids">A list of id's for the given item</param>
		/// <param name="strict">Wheter incomplete lists are allowed</param>
		/// <returns></returns>
		internal List<KyruObject> GetList(List<KademliaId> ids, bool strict = false)
		{
			var returnList = new List<KyruObject>();
			foreach (var id in ids)
			{
				var item = Get(id);
				if (item == null && strict == true)
					return null;

				if (item != null)
				{
					returnList.Add(item);
				}
			}
			return returnList;
		}

		/// <summary>
		/// Add an object to the file system.
		/// </summary>
		/// <param name="obj">The object to be added</param>
		internal void Store(KyruObject obj)
		{
			var idString = obj.ObjectId.ToString();
			String path = Path.Combine(config.storeDirectory, idString + ".obj");
			var fs = new FileStream(path, FileMode.Create);
			formatter.Serialize(fs, obj);
			fs.Close();
		}

		/// <summary>
		/// Deletes a file on the file system
		/// </summary>
		/// <param name="id">The id of the file to be deleted</param>
		internal void Delete(KademliaId id)
		{
			var idString = id.ToString();
			String path = Path.Combine(config.storeDirectory, idString + ".obj");
			File.Delete(path);
		}
	}
}