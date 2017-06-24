using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_FAT16.FAT16;
using System.IO;


namespace Proyecto_FAT16.Custom_Views
{
    public class ListViewFAT16Entry : ListViewItem
    {
        String ListViewItemName;
        String Tipo;
        String TamanoArchivo;
        String FechaCreacion;
        String FechaModificacion;
        String ClusterNumber;
        FAT16DirEntry Entry;

        public ListViewFAT16Entry(FAT16DirEntry entry, String[] datos)
            : base(datos)
        {
            Entry = entry;
            ListViewItemName = datos[0];
            Tipo = datos[1];
            TamanoArchivo = datos[2];
            FechaCreacion = datos[3];
            FechaModificacion = datos[4];
            ClusterNumber = datos[5];

            if (entry.esDirectorio)
            {
                this.ImageIndex = 1;
            }
            else
            {
                this.ImageIndex = 3;
            }

        }

        public static ListViewFAT16Entry FactoryMethod(FAT16DirEntry entry)
        {
            String[] datos = new String[6];
            datos[0] = entry.GetDirName();
            datos[1] = entry.GetTipo();

            long FileSize = entry.GetDirSize();

            //KB
            if (FileSize >= (1024 * 1024 * 1024))
            {
                float tamano = FileSize / (1024 * 1024 * 1024);
                datos[2] = "" + tamano + " GB";
            }
            //MB
            else if (FileSize >= (1024 * 1024))
            {
                float tamano = FileSize / (1024 * 1024);
                datos[2] = "" + tamano + " MB";
            }
            //GB
            else if (FileSize >= 1024)
            {
                float tamano = FileSize / 1024;
                datos[2] = "" + tamano + " KB";
            }
            else
            {
                datos[2] = "" + FileSize + " bytes";
            }

            datos[3] = entry.GetFechaCreacionString();
            datos[4] = entry.GetFechaModificacionString();
            datos[5] = entry.GetFirstCluster().ToString();
            return new ListViewFAT16Entry(entry, datos);
        }

        public FAT16DirEntry GetEntry()
        {
            return Entry;
        }

        public void obtenerArchivo(BootSector BS, FileStream stream, FileAllocationTable FAT, String path)
        {
            List<byte[]> bytes = new List<byte[]>();
            MemoryStream ms = new MemoryStream();
            byte[] archivo = null;

            int Cluster = Entry.GetFirstCluster();

            while (Cluster != -1)
            {
                //Leer archivo
                byte[] tempByte = new byte[BS.GetBytesPerSector()];
                stream.Seek(BS.SectorOffsetForCluster(Cluster), SeekOrigin.Begin);
                stream.Read(tempByte, 0, BS.GetBytesPerSector());
                bytes.Add(tempByte);

                //Obtener el siguiente cluster
                Cluster = FAT.nextClusterInChain(Cluster);
            }

            if (bytes.Count > 0)
            {
                foreach (byte[] b in bytes)
                {
                    ms.Write(b, 0, b.Length);
                }

                ms.Seek(0, SeekOrigin.Begin);
                archivo = new byte[Entry.FileSize];
                ms.Read(archivo, 0, archivo.Length);

                Console.WriteLine(ms.Length);
            }

            if (archivo != null)
            {
                File.WriteAllBytes(path + Entry.NombreDeDir, archivo);
                MessageBox.Show("Archivo(s) extraido(s) exitosamente!", "Extraccion de Archivo(s)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Archivo devuelto en cero!", "Error al extraer archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}