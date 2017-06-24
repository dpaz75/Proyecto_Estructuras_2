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
using Proyecto_FAT16.Custom_Views;

namespace Proyecto_FAT16
{
    public partial class FileInfoForm : Form
    {
        public FileInfoForm(FAT16DirEntry entry)
        {
            InitializeComponent();

            this.MaximumSize = this.MinimumSize = this.Size;

            labelNombre.Text = entry.NombreDeDir;
            labelFechaCreacion.Text = entry.GetFechaCreacionString();
            labelFechaMod.Text = entry.GetFechaModificacionString();
            labelTamano.Text = entry.GetDirSizeString() + " bytes";
            labelCluster.Text = "" + entry.GetFirstCluster();
        }
    }
}
