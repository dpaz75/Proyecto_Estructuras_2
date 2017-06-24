using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_FAT16.FAT16
{
    public class FAT16LFNEntry
    {

        public byte LDIR_Ord; //Offset 0 - 1 byte (Empieza con 0x40 | Num_Ordinal )
        public byte[] LDIR_Name1 = new byte[10]; //Offset 1 - 10 bytes;
        public byte LDIR_ATTR; //Offset 11 - 1 byte
        public byte LDIR_Type = 0x00; //Offset 12 - 1 byte
        public byte LDIR_Checksum; //Offset 13 - 1 byte
        public byte[] LDIR_Name2 = new byte[12]; //Offset 14 - 12 bytes
        public byte[] LDIR_FstClustLO = { 0x00, 0x00 }; //Offset 26 - 2 bytes
        public byte[] LDIR_Name3 = new byte[4]; //Offset 28 - 4 bytes

        public FAT16LFNEntry()
        {

        }

    }
}
