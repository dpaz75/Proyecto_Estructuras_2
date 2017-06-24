using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Proyecto_FAT16.FAT16
{
    public class FSInfoSector
    {
        //Ofrece informacion adicional del sistema de archivos en la particion y usualmente esta ubicado en la posicion
        // 1 o el sectore siguiente al boot sector. Su posicion esta definida en el BIOS Parameter Block del Boot Sector

        byte[] FSInformationSectorSignature = { 0x52, 0x52, 0x61, 0x41 }; //Offset 0 - 4 bytes
        byte[] Reservado = new byte[480]; //Offset 4 - 480 bytes
        byte[] FSInformationSectorSignature2 = { 0x72, 0x72, 0x41, 0x61 }; //Offset 484 - 4 bytes
        byte[] LastNumberFreeClusters = { 0xFF, 0xFF, 0xFF, 0xFF }; //Offset 488 - 4 bytes
        byte[] NumberAllocatedDataCluster = { 0xFF, 0xFF, 0xFF, 0xFF }; //Offset 492 - 4 bytes
        byte[] Reservado2 = new byte[12]; //Offset 496 - 12 bytes
        byte[] FSInformationSectorSignature3 = { 0x00, 0x00, 0x55, 0xAA }; //Offset 508 - 4 bytes



        public FSInfoSector()
        {

        }

        public FSInfoSector(byte[] infoSector)
        {
            Buffer.BlockCopy(infoSector, 0, FSInformationSectorSignature, 0, 4);
            Buffer.BlockCopy(infoSector, 4, Reservado, 0, 480);
            Buffer.BlockCopy(infoSector, 484, FSInformationSectorSignature2, 0, 4);
            Buffer.BlockCopy(infoSector, 488, LastNumberFreeClusters, 0, 4);
            Buffer.BlockCopy(infoSector, 492, NumberAllocatedDataCluster, 0, 4);
            Buffer.BlockCopy(infoSector, 496, Reservado2, 0, 12);
            Buffer.BlockCopy(infoSector, 508, FSInformationSectorSignature3, 0, 4);
        }

        public Int32 GetLastAllocatedCluster()
        {
            return BitConverter.ToInt32(NumberAllocatedDataCluster, 0);
        }

        public Int32 GetFreeClusterCount()
        {
            return BitConverter.ToInt32(LastNumberFreeClusters, 0);
        }

        public void UpdateFreeClusters(int clustersWritten, BootSector BS)
        {
            int OldClusterCount = BS.CountOfClusters();
            int newClusterCount = OldClusterCount - clustersWritten;
            LastNumberFreeClusters = BitConverter.GetBytes(newClusterCount);
        }

        public void UpdateFreeClusters(int clustersWritten)
        {
            int newClusterCount = BitConverter.ToInt32(LastNumberFreeClusters, 0) - clustersWritten;
            LastNumberFreeClusters = BitConverter.GetBytes(newClusterCount);
        }

        public void UpdateLastAllocatedCluster(FileStream stream)
        {
            //Read FAT Table
        }

        public void UpdateLastAllocatedCluster(int ClusterNumber)
        {
            //Actualizar en el FSInfo Sector el ultimo cluster alocado
            NumberAllocatedDataCluster = BitConverter.GetBytes(2);
        }

        public bool WriteInformationSector(FileStream stream)
        {
            MemoryStream memStream = new MemoryStream(512);
            memStream.Write(FSInformationSectorSignature, 0, 4);
            memStream.Write(Reservado, 0, 480);
            memStream.Write(FSInformationSectorSignature2, 0, 4);
            memStream.Write(LastNumberFreeClusters, 0, 4);
            memStream.Write(NumberAllocatedDataCluster, 0, 4);
            memStream.Write(Reservado2, 0, 12);
            memStream.Write(FSInformationSectorSignature3, 0, 4);

            try
            {
                stream.Seek(512, SeekOrigin.Begin);
                stream.Write(memStream.ToArray(), 0, 512);
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
