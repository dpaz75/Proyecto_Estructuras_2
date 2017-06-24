using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Proyecto_FAT16.FAT16
{
    public class FAT16Directory
    {
        List<FAT16DirEntry> ListaDeArchivos;
        int clusterStart = -1;
        int parentCluster = -1;

        public FAT16Directory(int cluster)
        {
            ListaDeArchivos = new List<FAT16DirEntry>();
            clusterStart = cluster;
        }

        public void AddDirectory(FAT16DirEntry entry)
        {
            ListaDeArchivos.Add(entry);
        }

        public int GetClusterStart()
        {
            return clusterStart;
        }

        public int GetParentCluster()
        {
            return parentCluster;
        }

        public void ParseAllEntries()
        {
            foreach (FAT16DirEntry entry in ListaDeArchivos)
            {
                entry.ParseDirEntry();
                if (entry.isPointingToParent)
                    parentCluster = entry.ParentCluster;
            }

        }

        public Int32 NumeroDeArchivos()
        {
            return ListaDeArchivos.Count;
        }

        public List<FAT16DirEntry> GetEntries()
        {
            return ListaDeArchivos;
        }
    }
}
