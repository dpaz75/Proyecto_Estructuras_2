using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_FAT16.Custom_Classes
{
    class FAT16DirectoryTree
    {

        String NombreDeFolder;
        int ClusterNumber;
        List<FAT16DirectoryTree> Subdirectorios;
        bool specialDir = false;

        public FAT16DirectoryTree()
        {
            NombreDeFolder = "";
            ClusterNumber = -1;
            Subdirectorios = null;
            specialDir = true;
        }

        public FAT16DirectoryTree(String nombre, int cluster, List<FAT16DirectoryTree> subdirs, bool special)
        {
            this.NombreDeFolder = nombre;
            this.ClusterNumber = cluster;
            this.Subdirectorios = subdirs;
            this.specialDir = special;
        }

        public bool esSpecialDir()
        {
            return specialDir;
        }

        public String GetDirName()
        {
            return NombreDeFolder;
        }

        public List<FAT16DirectoryTree> GetSubDirs()
        {
            return Subdirectorios;
        }

        public void AddToNode(TreeNode node)
        {
            if (this.esSpecialDir())
            {
                return;
            }
            else
            {
                int index = node.Nodes.Add(new TreeNode(NombreDeFolder, 1, 1));
                TreeNode newNode = node.Nodes[index];
                if (Subdirectorios != null)
                {
                    foreach (FAT16DirectoryTree dir in Subdirectorios)
                    {
                        dir.AddToNode(newNode);
                    }
                }
            }
        }

        public int GetCountOfSubdirs()
        {
            int contador = 0;

            foreach (FAT16DirectoryTree dir in Subdirectorios)
            {
                if (!dir.esSpecialDir())
                    contador++;
            }

            return contador;
        }

        public int GetClusterNumber()
        {
            return ClusterNumber;
        }


    }
}
