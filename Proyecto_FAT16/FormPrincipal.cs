using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Proyecto_FAT16.FAT16;
using Proyecto_FAT16.Custom_Views;
using Proyecto_FAT16.Custom_Classes;

namespace Proyecto_FAT16
{
    public partial class FormPrincipal : Form
    {

        public static String DiscoAbierto;
        public static bool ImagenAbierta = false;
        public static byte NewsectorPorCluster; //Solo usado para formatear una imagen abierta
        public static String NewVolumeLabel; //Solo usado para formatear una imagen abierta
        public static long DiskSize; //Solo usado para formatear una imagen abierta

        //Formateo FAT16 y uso de Disco
        FileStream DiskStream;
        BootSector DiskBootSector;
        FSInfoSector DiskFSInfoSector;
        FileAllocationTable FAT_TABLE;

        //Directorios y Archivos
        FAT16Directory RootDirectory;
        int DirectorioActual = -1; //Solo usado como referencia al Directorio Actual en que estemos trabajando
        int DirectorioAnterior = -1;
        FAT16Directory DirActual;
        FAT16DirectoryTree F32DirectoryTree;
        private Stack<FAT16LFNEntry> TempLFNStack = new Stack<FAT16LFNEntry>();

        public FormPrincipal()
        {
            InitializeComponent();
            this.MaximumSize = this.MinimumSize = this.Size;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void abrirImagenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Imagen de Disco (*.img)|*.img";
            dialog.DefaultExt = ".img";
            dialog.AddExtension = true;

            DialogResult resultado = dialog.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                //Abrir Disco y extraer informacion
                try
                {
                    DiscoAbierto = dialog.FileName;

                    if (DiskStream != null)
                        DiskStream.Close();

                    DiskStream = new FileStream(DiscoAbierto, FileMode.Open, FileAccess.ReadWrite);

                    if (DiskStream != null)
                    {

                        //Abrir imagen
                        ImagenAbierta = true;
                        formatearToolStripMenuItem.Enabled = true;
                        cerrarImagenToolStripMenuItem.Enabled = true;
                        informacionToolStripMenuItem.Enabled = true;
                        toolStrip1.Enabled = true;

                        GetDiskBootSector();
                        GetDiskFSInfoSector();
                        GetDiskFileAllocationTable();
                        PopulateTree();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }


        private void salirToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void formatearToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (DiscoAbierto != null && ImagenAbierta)
            {
                FormatearImagen formatearImagenForm = new FormatearImagen();
                DialogResult resultado = formatearImagenForm.ShowDialog();

                if (resultado == DialogResult.OK && NewVolumeLabel != null)
                {
                    Formatear();
                }
            }
        }
        

        private void Formatear()
        {

            try
            {

                if (DiskStream != null)
                {

                    //Nulificar archivo
                    DiskStream.Seek(0, SeekOrigin.Begin);
                    DiskStream.SetLength(0);
                    DiskStream.Flush();
                    DiskStream.SetLength(DiskSize);
                    DiskStream.Flush();

                    //Escribir el BootSector
                    BootSector BootSector = new BootSector((int)DiskStream.Length, NewsectorPorCluster, NewVolumeLabel);
                    BootSector.WriteBootSector(DiskStream);

                    //Escribir FSInfoSector
                    FSInfoSector FSInfoSector = new FSInfoSector();
                    FSInfoSector.WriteInformationSector(DiskStream);

                    //Inicializar Tabla FAT - Offset 32 despues de los sectores reservados
                    FileAllocationTable NewFAT = new FileAllocationTable(BootSector);
                    NewFAT.WriteNewFAT(DiskStream);
                    FSInfoSector.UpdateFreeClusters(1, BootSector);
                    FSInfoSector.UpdateLastAllocatedCluster(2);
                    FSInfoSector.WriteInformationSector(DiskStream);

                    MessageBox.Show("La imagen de disco ha sido formateada!", "Formato Completo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GetDiskBootSector()
        {
            DiskStream.Seek(0, SeekOrigin.Begin);
            byte[] bTemp = new byte[512];
            DiskStream.Read(bTemp, 0, 512);
            DiskBootSector = new BootSector(bTemp);
            DiskBootSector.FATOffset();
            DiskBootSector.SectorOffsetForCluster(4);
        }

        private void GetDiskFSInfoSector()
        {
            DiskStream.Seek(512, SeekOrigin.Begin);
            byte[] bTemp = new byte[512];
            DiskStream.Read(bTemp, 0, 512);
            DiskFSInfoSector = new FSInfoSector(bTemp);

            //Actualizamos espacio libre aprox
            long spaceAvailableBytes = DiskFSInfoSector.GetFreeClusterCount() * DiskBootSector.GetBytesPerCluster();

            //GB
            if (spaceAvailableBytes >= (1024 * 1024 * 1024))
            {
                float tamano = spaceAvailableBytes / (1024 * 1024 * 1024);
                freeSpaceAvailable.Text = "" + tamano + " GB";
            }
            //MB
            else if (spaceAvailableBytes >= (1024 * 1024))
            {
                float tamano = spaceAvailableBytes / (1024 * 1024);
                freeSpaceAvailable.Text = "" + tamano + " MB";
            }
            //KB
            else if (spaceAvailableBytes >= 1024)
            {
                float tamano = spaceAvailableBytes / 1024;
                freeSpaceAvailable.Text = "" + tamano + " KB";
            }
            else
            {
                freeSpaceAvailable.Text = "" + spaceAvailableBytes + " bytes";
            }
        }

        private void GetDiskFileAllocationTable()
        {
            DiskStream.Seek(DiskBootSector.FATOffset(), SeekOrigin.Begin);
            byte[] tempFAT = new byte[DiskBootSector.SizeOfFAT() * DiskBootSector.GetBytesPerSector()];
            DiskStream.Read(tempFAT, 0, tempFAT.Length);
            FAT_TABLE = new FileAllocationTable(tempFAT, DiskBootSector);
        }

        private void PopulateTree()
        {

            ReadRootDirectory();
            UpdateTree();
            DirectoryTree.SelectedNode = DirectoryTree.TopNode;
        }

        private void UpdateTree()
        {

            DirectoryTree.Nodes.Clear();
            DirectoryTree.Nodes.Add(new TreeNode(DiskBootSector.GetVolumeLabel(), 0, 0));
            if (DiskStream != null && RootDirectory != null)
            {

                foreach (FAT16DirectoryTree directory in F32DirectoryTree.GetSubDirs())
                {
                    directory.AddToNode(DirectoryTree.TopNode);
                }
            }
        }

        private void ReadRootDirectory()
        {
            //Vamos a leer el Root Directory que empieza con el cluster #2
            RootDirectory = new FAT16Directory(2);

            ReadDirectory(RootDirectory.GetClusterStart(), RootDirectory);

            DirectoryTree.SelectedNode = DirectoryTree.TopNode;
            F32DirectoryTree = new FAT16DirectoryTree(DiskBootSector.GetVolumeLabel(), 2, new List<FAT16DirectoryTree>(), true);

            foreach (FAT16DirEntry directory in RootDirectory.GetEntries())
            {
                if (directory.esDir() && !directory.ignoreEntry)
                {
                    F32DirectoryTree.GetSubDirs().Add(new FAT16DirectoryTree(directory.NombreDeDir, directory.GetFirstCluster(), null, false));
                }
            }

            DirActual = RootDirectory;

        }

        private void ReadDirectory(int clusterStart, FAT16Directory dir)
        {
            int Cluster = clusterStart;

            while (Cluster != -1)
            {

                DiskStream.Seek(DiskBootSector.SectorOffsetForCluster(Cluster), SeekOrigin.Begin);

                byte[] dirCluster = new byte[DiskBootSector.GetBytesPerCluster()];
                DiskStream.Read(dirCluster, 0, DiskBootSector.GetBytesPerCluster());
                MemoryStream MS = new MemoryStream(dirCluster);

                ParseDirectoryCluster(dir, MS);

                Cluster = FAT_TABLE.nextClusterInChain(Cluster);
            }

            dir.ParseAllEntries();
            numeroDeArchivosLabel.Text = dir.NumeroDeArchivos().ToString();

            DirectorioActual = dir.GetClusterStart();
            DirectorioAnterior = dir.GetParentCluster();

            UpdateFileListView(dir);
        }

        private void UpdateFileListView(FAT16Directory directory)
        {
            FileListView.Items.Clear();
            foreach (FAT16DirEntry entry in directory.GetEntries())
            {
                if (!entry.ignoreEntry)
                    FileListView.Items.Add(ListViewFAT16Entry.FactoryMethod(entry));
            }

            if (DirectorioAnterior == -1)
                previosFolderButton.Enabled = false;
            else
                previosFolderButton.Enabled = true;

        }

        private void ParseDirectoryCluster(FAT16Directory dir, MemoryStream dirStream)
        {
            byte[] EntryByteBuffer = new byte[32];
            bool LFNFlag = false;

            while (dirStream.Read(EntryByteBuffer, 0, 32) > 0)
            {
                //0x00 y 0xE5 denotan que esa entrada esta libre
                if (EntryByteBuffer[0] != 0x00)
                {
                    if (EntryByteBuffer[11] == 0x0F)
                    {
                        //Entrada de directory LFN (Long File Name)
                        LFNFlag = true;

                        FAT16LFNEntry tempLFNEntry = new FAT16LFNEntry();

                        //Copiamos los datos de esa entrada de directory a una estructura LFN
                        tempLFNEntry.LDIR_Ord = EntryByteBuffer[0];
                        Array.Copy(EntryByteBuffer, 1, tempLFNEntry.LDIR_Name1, 0, 10);
                        tempLFNEntry.LDIR_ATTR = EntryByteBuffer[11];
                        tempLFNEntry.LDIR_Type = EntryByteBuffer[12];
                        tempLFNEntry.LDIR_Checksum = EntryByteBuffer[13];
                        Array.Copy(EntryByteBuffer, 14, tempLFNEntry.LDIR_Name2, 0, 12);
                        Array.Copy(EntryByteBuffer, 26, tempLFNEntry.LDIR_FstClustLO, 0, 2);
                        Array.Copy(EntryByteBuffer, 28, tempLFNEntry.LDIR_Name3, 0, 4);

                        TempLFNStack.Push(tempLFNEntry);
                    }
                    else
                    {
                        //Entrada de Directorio Normal
                        FAT16DirEntry tempDirEntry = new FAT16DirEntry();
                        if (EntryByteBuffer[0] == 0xE5)
                            tempDirEntry.ignoreEntry = true;
                        Array.Copy(EntryByteBuffer, 0, tempDirEntry.DIR_NAME, 0, 8);
                        Array.Copy(EntryByteBuffer, 8, tempDirEntry.DIR_EXT, 0, 3);
                        tempDirEntry.DIR_ATTR = EntryByteBuffer[11];
                        tempDirEntry.DIR_NTRES = EntryByteBuffer[12];
                        tempDirEntry.DIR_CrtTimeTenth = EntryByteBuffer[13];
                        Array.Copy(EntryByteBuffer, 14, tempDirEntry.DIR_CrtTime, 0, 2);
                        Array.Copy(EntryByteBuffer, 16, tempDirEntry.DIR_CrtDate, 0, 2);
                        Array.Copy(EntryByteBuffer, 18, tempDirEntry.DIR_LstAccDate, 0, 2);
                        Array.Copy(EntryByteBuffer, 20, tempDirEntry.DIR_FstClustHI, 0, 2);
                        Array.Copy(EntryByteBuffer, 22, tempDirEntry.DIR_WrtTime, 0, 2);
                        Array.Copy(EntryByteBuffer, 24, tempDirEntry.DIR_WrtDate, 0, 2);
                        Array.Copy(EntryByteBuffer, 26, tempDirEntry.DIR_FstClustLO, 0, 2);
                        Array.Copy(EntryByteBuffer, 28, tempDirEntry.DIR_FileSize, 0, 4);

                        //Clonar Stack de LFNs
                        if (LFNFlag && TempLFNStack.Count > 0)
                            tempDirEntry.ListaLFNEntries = new Stack<FAT16LFNEntry>(TempLFNStack);

                        if (TempLFNStack.Count > 0)
                            TempLFNStack.Clear();

                        //Agregar Entry a RootDirectory
                        dir.AddDirectory(tempDirEntry);
                    }
                }

                EntryByteBuffer = new byte[32];
            }

            //dir.ParseAllEntries();
        }

        private void cerrarImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DiskStream != null)
                {
                    DiskStream.Flush();
                    DiskStream.Close();
                    cerrarImagenToolStripMenuItem.Enabled = false;
                    formatearToolStripMenuItem.Enabled = false;
                    toolStrip1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        
        private void GuardarArchivo(String path)
        {
            path += "\\";
            Console.WriteLine("Guardara en Path : " + path);
            foreach (ListViewFAT16Entry item in FileListView.SelectedItems)
            {
                if (!item.GetEntry().esDir())
                {

                    Task t = Task.Factory.StartNew(() =>
                    {
                        item.obtenerArchivo(DiskBootSector, DiskStream, FAT_TABLE, path);
                    });

                }
            }
        }

        private void crearImagenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CrearImagen crearImagen = new CrearImagen();
            DialogResult resultado = crearImagen.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                DialogResult abrirImagen = MessageBox.Show("Desea abrir la nueva imagen de disco creada? -> " + DiscoAbierto, "Abrir Imagen de Disco", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (abrirImagen == DialogResult.Yes)
                {

                    //Abrir Disco y extraer informacion
                    try
                    {
                        if (DiskStream != null)
                            DiskStream.Close();

                        DiskStream = new FileStream(DiscoAbierto, FileMode.Open, FileAccess.ReadWrite);

                        if (DiskStream != null)
                        {

                            //Abrir la imagen creada
                            ImagenAbierta = true;
                            formatearToolStripMenuItem.Enabled = true;
                            cerrarImagenToolStripMenuItem.Enabled = true;
                            informacionToolStripMenuItem.Enabled = true;
                            toolStrip1.Enabled = true;

                            GetDiskBootSector();
                            GetDiskFSInfoSector();
                            GetDiskFileAllocationTable();
                            PopulateTree();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void informacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilesystemInformation Info = new FilesystemInformation(DiskBootSector, DiskFSInfoSector);
            Info.ShowDialog();
        }

  
        
    }
}