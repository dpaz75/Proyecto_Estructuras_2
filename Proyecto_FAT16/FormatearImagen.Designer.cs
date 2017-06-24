namespace Proyecto_FAT16
{
    partial class FormatearImagen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tamanoLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtVolLabel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFormatear = new System.Windows.Forms.Button();
            this.tamanoSectoresPorCluster = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tamanoLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnCancelar);
            this.groupBox1.Controls.Add(this.txtVolLabel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnFormatear);
            this.groupBox1.Controls.Add(this.tamanoSectoresPorCluster);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtRuta);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(29, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(311, 233);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // tamanoLabel
            // 
            this.tamanoLabel.AutoSize = true;
            this.tamanoLabel.Location = new System.Drawing.Point(152, 81);
            this.tamanoLabel.Name = "tamanoLabel";
            this.tamanoLabel.Size = new System.Drawing.Size(41, 13);
            this.tamanoLabel.TabIndex = 17;
            this.tamanoLabel.Text = "0 bytes";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Tamano Archivo : ";
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(217, 200);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 15;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // txtVolLabel
            // 
            this.txtVolLabel.Location = new System.Drawing.Point(190, 159);
            this.txtVolLabel.MaxLength = 11;
            this.txtVolLabel.Name = "txtVolLabel";
            this.txtVolLabel.Size = new System.Drawing.Size(106, 20);
            this.txtVolLabel.TabIndex = 14;
            this.txtVolLabel.Text = "SIN NOMBRE ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Volume Label (11 Caracteres Max ) :";
            // 
            // btnFormatear
            // 
            this.btnFormatear.Location = new System.Drawing.Point(136, 200);
            this.btnFormatear.Name = "btnFormatear";
            this.btnFormatear.Size = new System.Drawing.Size(75, 23);
            this.btnFormatear.TabIndex = 12;
            this.btnFormatear.Text = "Formatear";
            this.btnFormatear.UseVisualStyleBackColor = true;
            // 
            // tamanoSectoresPorCluster
            // 
            this.tamanoSectoresPorCluster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tamanoSectoresPorCluster.FormattingEnabled = true;
            this.tamanoSectoresPorCluster.Items.AddRange(new object[] {
            "512 bytes",
            "1 KiB",
            "2 KiB",
            "4 KiB",
            "8 KiB",
            "16 KiB"});
            this.tamanoSectoresPorCluster.Location = new System.Drawing.Point(155, 119);
            this.tamanoSectoresPorCluster.Name = "tamanoSectoresPorCluster";
            this.tamanoSectoresPorCluster.Size = new System.Drawing.Size(141, 21);
            this.tamanoSectoresPorCluster.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Tamano de Cluster ( Bytes ) :";
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(60, 44);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.ReadOnly = true;
            this.txtRuta.Size = new System.Drawing.Size(236, 20);
            this.txtRuta.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Imagen :";
            // 
            // FormatearImagen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 299);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormatearImagen";
            this.Text = "Formatear Imagen";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label tamanoLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtVolLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFormatear;
        private System.Windows.Forms.ComboBox tamanoSectoresPorCluster;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.Label label1;
    }
}