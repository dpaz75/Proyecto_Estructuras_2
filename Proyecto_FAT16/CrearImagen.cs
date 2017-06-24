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

namespace Proyecto_FAT16
{
    public partial class CrearImagen : Form
    {
        public CrearImagen()
        {
            InitializeComponent();
            this.MaximumSize = this.MinimumSize = this.Size;
            this.sizeTypeCombobox.SelectedIndex = 0;
            tamanoSectoresPorCluster.SelectedIndex = 0;
        }

        private void btnUbicacion_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.OverwritePrompt = true;
            dialog.Filter = "Imagenes de disco (*.img)|*img";
            dialog.DefaultExt = ".img";
            dialog.AddExtension = true;

            DialogResult resultado = dialog.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                txtRuta.Text = dialog.FileName;
            }
        }
       

        private void sizeTypeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sizeTypeCombobox.SelectedIndex == 0)
            {
                tamanoDisco.Minimum = 33;
                tamanoDisco.Value = 33;
            }
            else
            {
                tamanoDisco.Minimum = 1;
                tamanoDisco.Value = 1;
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (txtRuta.Text.Length == 0 || !txtRuta.Text.EndsWith(".img"))
            {
                MessageBox.Show("Debe de indicar una ruta y nombre de archivo para crear la nueva imagen de disco.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    //Crear en MB
                    if (sizeTypeCombobox.SelectedIndex == 0)
                    {
                        using (var fileStream = new FileStream(txtRuta.Text, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            long length = (long)tamanoDisco.Value * 1024 * 1024;
                            fileStream.SetLength(length);
                            if (checkFormatear.Checked)
                                Formatear(fileStream);
                            fileStream.Close();
                            FormPrincipal.DiscoAbierto = txtRuta.Text;
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    //Crear en GB
                    else
                    {
                        using (var fileStream = new FileStream(txtRuta.Text, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            long length = (long)tamanoDisco.Value * 1024 * 1024 * 1024;
                            fileStream.SetLength(length);
                            if (checkFormatear.Checked)
                                Formatear(fileStream);
                            fileStream.Close();
                            FormPrincipal.DiscoAbierto = txtRuta.Text;
                            this.DialogResult = DialogResult.OK;
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    this.DialogResult = DialogResult.Abort;
                }
                finally
                {
                    this.Close();
                }
            }
        }

       

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkFormatear_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkFormatear.Checked)
            {
                tamanoSectoresPorCluster.Enabled = true;
            }
            else
            {
                tamanoSectoresPorCluster.Enabled = false;
            }
        }

                
        private void Formatear(FileStream stream)
        {
            try
            {

                byte SectoresPorCluster = 0x00;

                //Obtener el numero de sectores por cluster
                switch (tamanoSectoresPorCluster.SelectedIndex)
                {
                    case 0:
                        //1 sector por cluster = Clusters de 512 bytes
                        SectoresPorCluster = 0x01;
                        break;
                    case 1:
                        //2 sectores por cluster = Clusters de 1 KiB
                        SectoresPorCluster = 0x02;
                        break;
                    case 2:
                        //4 sectores por cluster = Clusters de 2 KiB
                        SectoresPorCluster = 0x04;
                        break;
                    case 3:
                        //8 sectores por cluster = Clusters de 4 KiB
                        SectoresPorCluster = 0x08;
                        break;
                    case 4:
                        //16 sectores por cluster = Clusters de 8 KiB
                        SectoresPorCluster = 0x10;
                        break;
                    case 5:
                        //32 sectores por cluster = Clusters de 16KiB
                        SectoresPorCluster = 0x20;
                        break;
                    default:
                        SectoresPorCluster = 0x01;
                        break;
                }

                //Escribir el BootSector
                BootSector BootSector = new BootSector((int)stream.Length, SectoresPorCluster, txtVolLabel.Text);
                BootSector.WriteBootSector(stream);

                //Escribir FSInfoSector
                FSInfoSector FSInfoSector = new FSInfoSector();
                FSInfoSector.WriteInformationSector(stream);

                //Inicializar Tabla FAT - Offset 32, despues de los sectores reservados
                FileAllocationTable NewFAT = new FileAllocationTable(BootSector);
                NewFAT.WriteNewFAT(stream);
                FSInfoSector.UpdateFreeClusters(1, BootSector);
                FSInfoSector.UpdateLastAllocatedCluster(2);
                FSInfoSector.WriteInformationSector(stream);

                MessageBox.Show("La imagen de disco ha sido creada y formateada!", "Formato Completo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void tamanoDisco_ValueChanged(object sender, EventArgs e)
        {
            CalcularTamanoCluster();
        }

        private void CalcularTamanoCluster()
        {
            if (sizeTypeCombobox.SelectedIndex == 0)
            {
                //Calcular Megabytes
                if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024, 32, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");
                    ClusterSizes.Add("2048");
                    ClusterSizes.Add("4096");
                    ClusterSizes.Add("8192");
                    ClusterSizes.Add("16384");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024, 16, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");
                    ClusterSizes.Add("2048");
                    ClusterSizes.Add("4096");
                    ClusterSizes.Add("8192");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024, 8, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");
                    ClusterSizes.Add("2048");
                    ClusterSizes.Add("4096");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024, 4, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");
                    ClusterSizes.Add("2048");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024, 2, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }

            }
            else
            {
                //Calcular en GigaBytes
                if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024 * 1024, 32, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");
                    ClusterSizes.Add("2048");
                    ClusterSizes.Add("4096");
                    ClusterSizes.Add("8192");
                    ClusterSizes.Add("16384");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024 * 1024, 16, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");
                    ClusterSizes.Add("2048");
                    ClusterSizes.Add("4096");
                    ClusterSizes.Add("8192");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024 * 1024, 8, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");
                    ClusterSizes.Add("2048");
                    ClusterSizes.Add("4096");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024 * 1024, 4, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");
                    ClusterSizes.Add("2048");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else if (AcceptableClusterSize((long)tamanoDisco.Value * 1024 * 1024 * 1024, 2, 512))
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");
                    ClusterSizes.Add("1024");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
                else
                {
                    List<String> ClusterSizes = new List<String>();
                    ClusterSizes.Add("512");

                    tamanoSectoresPorCluster.Items.Clear();
                    tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
                }
            }

            tamanoSectoresPorCluster.SelectedIndex = tamanoSectoresPorCluster.Items.Count - 1;
        }

        private bool AcceptableClusterSize(long diskSizeBytes, int clustersize, int bytespersector)
        {

            long resultado = (diskSizeBytes / bytespersector) / clustersize;
            Console.WriteLine("Resultado es : " + resultado + " con tamano de disco : " + diskSizeBytes + " y Cluster Size (Sectores) : " + clustersize);

            if (resultado >= 65525)
                return true;
            else
                return false;
        }

       
        
        

    }
}
