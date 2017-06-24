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

namespace Proyecto_FAT16
{
    public partial class FormatearImagen : Form
    {

        long disksize = 0;

        public FormatearImagen()
        {
            InitializeComponent();
            txtRuta.Text = FormPrincipal.DiscoAbierto;
            tamanoSectoresPorCluster.SelectedIndex = 0;
            disksize = new FileInfo(txtRuta.Text).Length;
            tamanoLabel.Text = "" + disksize + " bytes";
            CalcularTamanoCluster();
            this.MaximumSize = this.MinimumSize = this.Size;
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void btnFormatear_Click(object sender, EventArgs e)
        {

        }


        private void CalcularTamanoCluster()
        {
            //Calcular Megabytes
            if (AcceptableClusterSize(disksize, 32, 512))
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
            else if (AcceptableClusterSize(disksize, 16, 512))
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
            else if (AcceptableClusterSize(disksize, 8, 512))
            {
                List<String> ClusterSizes = new List<String>();
                ClusterSizes.Add("512");
                ClusterSizes.Add("1024");
                ClusterSizes.Add("2048");
                ClusterSizes.Add("4096");

                tamanoSectoresPorCluster.Items.Clear();
                tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
            }
            else if (AcceptableClusterSize(disksize, 4, 512))
            {
                List<String> ClusterSizes = new List<String>();
                ClusterSizes.Add("512");
                ClusterSizes.Add("1024");
                ClusterSizes.Add("2048");

                tamanoSectoresPorCluster.Items.Clear();
                tamanoSectoresPorCluster.Items.AddRange(ClusterSizes.ToArray());
            }
            else if (AcceptableClusterSize(disksize, 2, 512))
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
