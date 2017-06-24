using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Proyecto_FAT16.FAT16
{
    public class FileAllocationTable
    {

        BootSector BS;
        MemoryStream FAT_Table;

        //32 bit FAT ID 
        byte[] FAT_ID = { 0xF8, 0xFF, 0xFF, 0x0F };

        //EOC (End of Chain)
        byte[] EOC = { 0xFF, 0xFF, 0xFF, 0x0F };

        //Utilizado para crear un nuevo FAT (File Allocation Table)
        public FileAllocationTable(BootSector bootSector)
        {
            BS = bootSector;
        }

        public FileAllocationTable(byte[] fat, BootSector bootSector)
        {
            FAT_Table = new MemoryStream(fat);
            BS = bootSector;
        }

        public int FATSectorForCluster(int cluster)
        {
            return BS.FATSectorNumber(cluster);
        }

        public int FATEntryOffset(int cluster)
        {
            return BS.FATEntryOffset(cluster);
        }

        public Int32 nextClusterInChain(int currentCluster)
        {
            int offset = currentCluster * 4;

            int cluster = -1;
            byte[] clusterBytes = new byte[4];

            if (currentCluster >= 2)
            {
                FAT_Table.Seek(offset, SeekOrigin.Begin);
                FAT_Table.Read(clusterBytes, 0, 4);
                if (!clusterBytes.SequenceEqual(EOC))
                {
                    BitArray bitArray = new BitArray(clusterBytes);
                    bitArray[31] = false;
                    bitArray[30] = false;
                    bitArray[29] = false;
                    bitArray[28] = false;
                    clusterBytes = new byte[4];
                    bitArray.CopyTo(clusterBytes, 0);
                    cluster = BitConverter.ToInt32(clusterBytes, 0);
                }
                else
                {
                    cluster = -1;
                }
            }

            return cluster;
        }

        public bool WriteNewFAT(FileStream stream)
        {
            MemoryStream memStream = new MemoryStream(12);

            memStream.Write(FAT_ID, 0, 4);
            memStream.Write(EOC, 0, 4);
            memStream.Write(FAT_ID, 0, 4);

            try
            {
                stream.Seek(BS.FATOffset(), SeekOrigin.Begin);
                stream.Write(memStream.ToArray(), 0, 12);
                stream.Seek(BS.FAT2Offset(), SeekOrigin.Begin);
                stream.Write(memStream.ToArray(), 0, 12);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
