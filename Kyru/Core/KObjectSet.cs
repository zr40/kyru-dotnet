using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Kyru.Network;
using System.Runtime.Serialization;

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
		private Dictionary<KademliaId, KObject> cache;
		private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

		public KObjectSet(Config config) {
			this.config = config;
		}

		/// <summary>
		/// Retrieve an object from.
		/// </summary>
		/// <param name="id">The id of the object</param>
		/// <returns>The object, or null if he can't find it</returns>
		internal T Get<T>(KademliaId id) where T : KObject , new()
		{
			var idString = id.ToString();
			String path = Path.Combine(config.storeDirectory, idString + ".obj");
			FileStream fs;
			try
			{
				fs = new FileStream(path, FileMode.Open);
			} catch (FileNotFoundException) {
				return null;
			}
			return (T)formatter.Deserialize(fs);
		}

		/// <summary>
		/// Method to retrieve a list of items.
		/// </summary>
		/// <param name="ids">A list of id's for the given item</param>
		/// <param name="strict">Wheter incomplete lists are allowed</param>
		/// <returns></returns>
		internal List<T> GetList<T>(List<KademliaId> ids, bool strict = false) where T : KObject, new()
		{
			List<T> returnList = new List<T>();
			foreach (var id in ids) {
				var item = Get<T>(id);
				if (item == null && strict == true)
					return null;

				if (item != null) {
					returnList.Add(item);
				}
			}
			return returnList;
		}

		/// <summary>
		/// Add an object to the file system.
		/// </summary>
		/// <param name="obj">The object to be added</param>
		internal void Store(KObject obj)
		{
			var idString = obj.Id.ToString();
			String path = Path.Combine(config.storeDirectory, idString + ".obj");
			FileStream fs = new FileStream(path, FileMode.Create);
			formatter.Serialize(fs, obj);
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
