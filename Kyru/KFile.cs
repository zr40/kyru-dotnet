using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kyru
{
    class KFile
    {
        Byte[] Data;
        public string Name;
        public string Extension;

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
