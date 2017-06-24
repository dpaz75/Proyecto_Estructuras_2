using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_FAT16.FAT16;

namespace Proyecto_FAT16
{
    public partial class FilesystemInformation : Form
    {

        BootSector BootSector;
        FSInfoSector FSInfoSector;

        public FilesystemInformation(BootSector BS, FSInfoSector FSInfo)
        {
            InitializeComponent();
            this.MaximumSize = this.MinimumSize = this.Size;
            BootSector = BS;
            FSInfoSector = FSInfo;

            //General
            this.volumeIDLabel.Text = BootSector.GetVolumeID();
            this.volumeLabel.Text = BootSector.GetVolumeLabel();

            //Boot Sector
            this.labelBPS.Text = BootSector.GetBytesPerSector().ToString();
            this.labelSPC.Text = BootSector.GetSectorsPerCluster().ToString();
            this.labelSectoresReservados.Text = BootSector.GetReservedSectors().ToString();
            this.labelTotalSectores.Text = BootSector.GetTotalSectoresFAT32().ToString();
            this.labelFATSize.Text = BootSector.SizeOfFAT().ToString();

            //FSInfo Sector
            this.lastAllocatedCluster.Text = FSInfoSector.GetLastAllocatedCluster().ToString();
            this.freeClusters.Text = FSInfoSector.GetFreeClusterCount().ToString();

            //File Allocation Table
            fatSize.Text = BootSector.SizeOfFAT().ToString();
            fat1Offset.Text = BootSector.FATOffset().ToString();
            fat2Offset.Text = BootSector.FAT2Offset().ToString();

        }



    }
}
