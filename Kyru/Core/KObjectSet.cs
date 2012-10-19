using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Kyru.Network;

namespace Kyru.Core
{
    /// <summary>
    /// This class keeps track of all kyru objects on the file system. Note that not all objects have to be in memory.
    /// </summary>
    class KObjectSet
    {
        Dictionary<KademliaId, KObject> cache;

        /// <summary>
        /// Retrieve an object from.
        /// </summary>
        /// <param name="id">The id of the object</param>
        /// <returns>The object, or null if he can't find it</returns>
        public KObject Get(KademliaId id)
        {
            throw new NotImplementedException();
            // TODO: Try to get the object from the file system.
            //return null;
        }

        /// <summary>
        /// Method to retrieve a list of items.
        /// </summary>
        /// <param name="ids">A list of id's for the given item</param>
        /// <param name="strict">Wheter incomplete lists are allowed</param>
        /// <returns></returns>
        public List<KObject> GetList(List<KademliaId> ids, bool strict = false)
        {
            List<KObject> returnList = new List<KObject>();
            foreach (var id in ids) {
                var item = Get(id);
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
        public void Add(KObject obj)
        {
            
            throw new NotImplementedException();
        }

        private FileStream openFile(KademliaId id) {
            var base64id = id.ToString();
            throw new NotImplementedException();
        }
    }
}
