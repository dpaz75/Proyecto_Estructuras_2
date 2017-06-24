using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Proyecto_FAT16.FAT16
{

    
    public class BootSector
    {
        byte[] JumpBoot = { 0xEB, 0x58, 0x90 }; //Offset 0 - 3 bytes
        char[] OEMName = { 'O', 'R', 'G', 'A', 'R', 'C', 'H', 'I' }; //Offset 3 - 8 bytes
        byte[] BytesPerSector = BitConverter.GetBytes(512); //Offset 11 - 2 bytes - Asumimos como standard en el proyecto que el disco tiene 512 bytes por sector
        byte SectorsPerCluser; //Offset 13 - 1 byte - Calcular en tiempo de formateo
        byte[] ReservedSectors = BitConverter.GetBytes(32); //Ofsset 14 - 2 bytes - Reservar 32 sectores como standard de Micro$oft
        byte NumeroDeFATS = 0x02; //Offset 16 - 1 byte - 2 FATs
        byte[] RootEntryCount = { 0x00, 0x00 }; //Offset 17 - 2 bytes - Tiene que ser 0 en FAT16
        byte[] TotalSectores16 = { 0x00, 0x00 }; //Offset 19 - 2 bytes - Tiene que ser 0 en FAT16
        byte Media = 0xF8; //Offset 21 - 1 byte
        byte[] FATSz16 = { 0x00, 0x00 }; //Offset 22 - 2 bytes - Tiene que ser 0 en FAT16
        byte[] SectorsPerTrack = BitConverter.GetBytes(32); //Offset 24 - 2 bytes - Valor 63 sectores por track o pista
        byte[] NumeroHeads = BitConverter.GetBytes(64); //Offset 26 - 2 bytes - Valor 255 heads o cabezales
        byte[] HiddenSectors = { 0x00, 0x00, 0x00, 0x00 }; //Offset 28 - 4 bytes - Es 0 en todos los dispositivos sin particiones o 1 sola particion
        byte[] TotalSectores32 = new byte[4]; //Offset 32 - 4 bytes
        byte[] FATSz32 = new byte[4]; //Offset 36 - 4 bytes
        byte[] ExtFlags = { 0x00, 0x00 }; //Offset 40 - 2 bytes
        byte[] FSVer = { 0x00, 0x00 }; //Offset 42 - 2 bytes
        byte[] RootCluster = BitConverter.GetBytes(2); //Offset 44 - 4 bytes - Cluster 2
        byte[] FSInfo = BitConverter.GetBytes(1); //Offset 48 - 2 bytes - Sector 1
        byte[] BackupBootSector = BitConverter.GetBytes(6); //Offset 50 - 2 bytes
        byte[] Reservado = new byte[12]; //Offset 52 - 12 bytes
        byte DriveNumber = 0x80; //Offset 64 - 1 byte
        byte Reservado1 = 0x00; //Offset 65 - 1 byte
        byte ExtBootSig = 0x29; //Offset 66 - 1 byte
        byte[] VolumeID = new byte[4]; //Offset 67 - 4 bytes
        byte[] VolumeLabel = new byte[11]; //Offset 71 - 11 bytes
        byte[] FilesystemType = new byte[8]; //Offset 82 - 8 bytes
        byte[] BootCode = new byte[420]; //Offset 90 - 420 bytes
        byte[] BootSignature = { 0x55, 0xAA }; //Offset 510 - 2 bytes

        //Constructor de nuevo boot sector
        public BootSector(int disksize, byte sectoresporcluster, String volumelabel)
        {

            //Calcular total sectores de disco = tamano de disco / 512 (tamano de sectores)
            int sectors = disksize / BitConverter.ToInt16(BytesPerSector, 0);
            Console.WriteLine("Disksize : " + disksize + "bytes");
            Console.WriteLine("Total Sectors : " + sectors);
            Console.WriteLine("Sectores por Cluster : " + sectoresporcluster);
            TotalSectores32 = BitConverter.GetBytes(sectors);

            //Calcular FAT32Sz de acuerdo al documento en http://read.pudn.com/downloads77/ebook/294884/FAT32%20Spec%20%28SDA%20Contribution%29.pdf
            int RootDirSectors = 0; //Siempre 0 en FAT32
            int TmpVal1 = BitConverter.ToInt32(TotalSectores32, 0) - (32 + RootDirSectors);
            int TmpVal2 = (256 * sectoresporcluster) + NumeroDeFATS;
            TmpVal2 = TmpVal2 / 2;
            int FATSz = (TmpVal1 + (TmpVal2 - 1)) / TmpVal2;
            FATSz32 = BitConverter.GetBytes(FATSz);

            // FATSz = (DskSize - BPB_ResvdSecCnt + (2 * BPB_SecPerClus)) / ((BPB_SecPerClus * (BPB_BytsPerSec / 4)) + 2)
            FATSz = (int)Math.Ceiling((double)(sectors - 32 + (2 * sectoresporcluster)) / ((sectoresporcluster * (512 / 4)) + 2));
            Console.WriteLine("New FAT Size : " + FATSz);
            FATSz32 = BitConverter.GetBytes(FATSz);
            //Dar el numer de sectores por cluster
            SectorsPerCluser = sectoresporcluster;

            //VolumeID
            Array.Copy(BitConverter.GetBytes(new Random().Next()), 0, VolumeID, 0, 4);

            //VolumeLabel a char
            VolumeLabel = Encoding.ASCII.GetBytes(volumelabel);

            //FileSystem Type
            FilesystemType = Encoding.ASCII.GetBytes("FAT16   ");

            //Llenar el Bootcode de 0s
            for (int i = 0; i < 420; i++)
            {
                BootCode[i] = 0x00;
            }


        }

        //Constructor de bootsector existente, aun no utilizado
        public BootSector(byte[] bSector)
        {
            Array.Copy(bSector, 0, JumpBoot, 0, 3);
            Array.Copy(bSector, 3, OEMName, 0, 8);
            Array.Copy(bSector, 11, BytesPerSector, 0, 2);
            SectorsPerCluser = bSector[13];
            Array.Copy(bSector, 14, ReservedSectors, 0, 2);
            NumeroDeFATS = bSector[16];
            Array.Copy(bSector, 17, RootEntryCount, 0, 2);
            Array.Copy(bSector, 19, TotalSectores16, 0, 2);
            Media = bSector[21];
            Array.Copy(bSector, 22, FATSz16, 0, 2);
            Array.Copy(bSector, 24, SectorsPerTrack, 0, 2);
            Array.Copy(bSector, 26, NumeroHeads, 0, 2);
            Array.Copy(bSector, 28, HiddenSectors, 0, 4);
            Array.Copy(bSector, 32, TotalSectores32, 0, 4);
            Array.Copy(bSector, 36, FATSz32, 0, 4);
            Array.Copy(bSector, 40, ExtFlags, 0, 2);
            Array.Copy(bSector, 42, FSVer, 0, 2);
            Array.Copy(bSector, 44, RootCluster, 0, 4);
            Array.Copy(bSector, 48, FSInfo, 0, 2);
            Array.Copy(bSector, 50, BackupBootSector, 0, 2);
            Array.Copy(bSector, 52, Reservado, 0, 12);
            DriveNumber = bSector[64];
            Reservado1 = bSector[65];
            ExtBootSig = bSector[66];
            Array.Copy(bSector, 67, VolumeID, 0, 4);
            Array.Copy(bSector, 71, VolumeLabel, 0, 11);
            Array.Copy(bSector, 82, FilesystemType, 0, 8);
            Array.Copy(bSector, 90, BootCode, 0, 420);
            Array.Copy(bSector, 510, BootSignature, 0, 2);
        }

        public Int32 SizeOfFAT()
        {
            //Console.WriteLine("Size of FAT : " + BitConverter.ToInt32(FATSz32, 0));
            return BitConverter.ToInt32(FATSz32, 0);
        }

        public Int32 FATOffset()
        {
            int Offset = (BitConverter.ToInt32(ReservedSectors, 0) * BitConverter.ToInt16(BytesPerSector, 0));
            return Offset;
        }

        public Int32 FAT2Offset()
        {
            int Offset = (BitConverter.ToInt32(ReservedSectors, 0) * BitConverter.ToInt16(BytesPerSector, 0) + (SizeOfFAT() * BitConverter.ToInt16(BytesPerSector, 0)));
            return Offset;
        }

        public Int32 DataSectors()
        {
            int DataSectors = BitConverter.ToInt32(TotalSectores32, 0) - (BitConverter.ToInt16(ReservedSectors, 0) + (NumeroDeFATS * BitConverter.ToInt32(FATSz32, 0)));
            Console.WriteLine("Total Data Sectors : " + DataSectors);
            return DataSectors;
        }

        public Int32 CountOfClusters()
        {
            int datasectors = DataSectors();
            Console.WriteLine("Count of Clusters : " + (datasectors / SectorsPerCluser));
            return datasectors / SectorsPerCluser;
        }

        public Int32 DataRegionOffset()
        {
            int Offset = ((BitConverter.ToInt16(ReservedSectors, 0) * BitConverter.ToInt16(BytesPerSector, 0)) + (NumeroDeFATS * BitConverter.ToInt32(FATSz32, 0) * BitConverter.ToInt16(BytesPerSector, 0)));
            //Console.WriteLine("Data Region Starts at Offset : " + Offset);
            return Offset;
        }

        public Int32 SectorOffsetForCluster(int clusterNumber)
        {
            int Offset = ((((clusterNumber - 2) * SectorsPerCluser) * BitConverter.ToInt16(BytesPerSector, 0)) + DataRegionOffset());
            //Console.WriteLine("Sector Offset in Memory : " + Offset);
            return Offset;
        }

        public Int32 FATSectorNumber(int clusterNumber)
        {
            if (clusterNumber >= 2 && clusterNumber <= CountOfClusters())
            {
                int FATOffset = clusterNumber * 4;
                return BitConverter.ToInt16(ReservedSectors, 0) + (FATOffset / BitConverter.ToInt16(BytesPerSector, 0));
            }
            else
            {
                return -1;
            }
        }

        public Int32 FATEntryOffset(int clusterNumber)
        {
            if (clusterNumber >= 2 && clusterNumber <= CountOfClusters())
            {
                int FATOffset = clusterNumber * 4;
                return FATOffset % BitConverter.ToInt16(BytesPerSector, 0);
            }
            else
            {
                return -1;
            }
        }

        public String GetVolumeID()
        {
            return BitConverter.ToInt32(VolumeID, 0).ToString();
        }

        public String GetVolumeLabel()
        {
            return Encoding.ASCII.GetString(VolumeLabel);
        }

        public Int16 GetBytesPerSector()
        {
            return BitConverter.ToInt16(BytesPerSector, 0);
        }

        public Int32 GetBytesPerCluster()
        {
            return BitConverter.ToInt16(BytesPerSector, 0) * SectorsPerCluser;
        }

        public int GetSectorsPerCluster()
        {
            return SectorsPerCluser;
        }

        public Int16 GetReservedSectors()
        {
            return BitConverter.ToInt16(ReservedSectors, 0);
        }

        public Int32 GetTotalSectoresFAT32()
        {
            return BitConverter.ToInt32(TotalSectores32, 0);
        }

        public Int16 GetRootCluster()
        {
            return BitConverter.ToInt16(RootCluster, 0);
        }

        public bool WriteBootSector(FileStream fs)
        {
            MemoryStream stream = new MemoryStream();
            stream.SetLength(512);
            stream.Write(JumpBoot, 0, 3);
            stream.Write(Encoding.ASCII.GetBytes(OEMName), 0, 8);
            stream.Write(BytesPerSector, 0, 2);
            stream.WriteByte(SectorsPerCluser);
            stream.Write(ReservedSectors, 0, 2);
            stream.WriteByte(NumeroDeFATS);
            stream.Write(RootEntryCount, 0, 2);
            stream.Write(TotalSectores16, 0, 2);
            stream.WriteByte(Media);
            stream.Write(FATSz16, 0, 2);
            stream.Write(SectorsPerTrack, 0, 2);
            stream.Write(NumeroHeads, 0, 2);
            stream.Write(HiddenSectors, 0, 4);
            stream.Write(TotalSectores32, 0, 4);
            stream.Write(FATSz32, 0, 4);
            stream.Write(ExtFlags, 0, 2);
            stream.Write(FSVer, 0, 2);
            stream.Write(RootCluster, 0, 4);
            stream.Write(FSInfo, 0, 2);
            stream.Write(BackupBootSector, 0, 2);
            stream.Write(Reservado, 0, 12);
            stream.WriteByte(DriveNumber);
            stream.WriteByte(Reservado1);
            stream.WriteByte(ExtBootSig);
            stream.Write(VolumeID, 0, 4);
            stream.Write(VolumeLabel, 0, 11);
            stream.Write(FilesystemType, 0, 8);
            stream.Write(BootCode, 0, 420);
            stream.Write(BootSignature, 0, 2);

            try
            {
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(stream.ToArray(), 0, 512);
                fs.Seek(3072, SeekOrigin.Begin);
                fs.Write(stream.ToArray(), 0, 512);
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
