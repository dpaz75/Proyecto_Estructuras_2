namespace Proyecto_FAT16
{
    partial class CrearImagen
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
            this.checkFormatear = new System.Windows.Forms.CheckBox();
            this.txtVolLabel = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnCrear = new System.Windows.Forms.Button();
            this.tamanoSectoresPorCluster = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sizeTypeCombobox = new System.Windows.Forms.ComboBox();
            this.tamanoDisco = new System.Windows.Forms.NumericUpDown();
            this.btnUbicacion = new System.Windows.Forms.Button();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tamanoDisco)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkFormatear);
            this.groupBox1.Controls.Add(this.txtVolLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnCancelar);
            this.groupBox1.Controls.Add(this.btnCrear);
            this.groupBox1.Controls.Add(this.tamanoSectoresPorCluster);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.sizeTypeCombobox);
            this.groupBox1.Controls.Add(this.tamanoDisco);
            this.groupBox1.Controls.Add(this.btnUbicacion);
            this.groupBox1.Controls.Add(this.txtRuta);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 298);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // checkFormatear
            // 
            this.checkFormatear.AutoSize = true;
            this.checkFormatear.Checked = true;
            this.checkFormatear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkFormatear.Location = new System.Drawing.Point(60, 153);
            this.checkFormatear.Name = "checkFormatear";
            this.checkFormatear.Size = new System.Drawing.Size(125, 17);
            this.checkFormatear.TabIndex = 41;
            this.checkFormatear.Text = "Dar Formato FAT16?";
            this.checkFormatear.UseVisualStyleBackColor = true;
            this.checkFormatear.CheckedChanged += new System.EventHandler(this.checkFormatear_CheckedChanged_1);
            // 
            // txtVolLabel
            // 
            this.txtVolLabel.Location = new System.Drawing.Point(241, 197);
            this.txtVolLabel.MaxLength = 11;
            this.txtVolLabel.Name = "txtVolLabel";
            this.txtVolLabel.Size = new System.Drawing.Size(106, 20);
            this.txtVolLabel.TabIndex = 40;
            this.txtVolLabel.Text = "SIN NOMBRE ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Volume Label (11 Caracteres Max ) :";
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(272, 246);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 38;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnCrear
            // 
            this.btnCrear.Location = new System.Drawing.Point(191, 246);
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.Size = new System.Drawing.Size(75, 23);
            this.btnCrear.TabIndex = 37;
            this.btnCrear.Text = "Crear";
            this.btnCrear.UseVisualStyleBackColor = true;
            this.btnCrear.Click += new System.EventHandler(this.btnCrear_Click);
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
            this.tamanoSectoresPorCluster.Location = new System.Drawing.Point(207, 170);
            this.tamanoSectoresPorCluster.Name = "tamanoSectoresPorCluster";
            this.tamanoSectoresPorCluster.Size = new System.Drawing.Size(140, 21);
            this.tamanoSectoresPorCluster.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Tamano de Cluster (Bytes)  :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Tamano de Disco :";
            // 
            // sizeTypeCombobox
            // 
            this.sizeTypeCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeTypeCombobox.FormattingEnabled = true;
            this.sizeTypeCombobox.Items.AddRange(new object[] {
            "MB",
            "GB"});
            this.sizeTypeCombobox.Location = new System.Drawing.Point(293, 84);
            this.sizeTypeCombobox.Name = "sizeTypeCombobox";
            this.sizeTypeCombobox.Size = new System.Drawing.Size(54, 21);
            this.sizeTypeCombobox.TabIndex = 33;
            // 
            // tamanoDisco
            // 
            this.tamanoDisco.Location = new System.Drawing.Point(207, 85);
            this.tamanoDisco.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.tamanoDisco.Name = "tamanoDisco";
            this.tamanoDisco.Size = new System.Drawing.Size(82, 20);
            this.tamanoDisco.TabIndex = 32;
            // 
            // btnUbicacion
            // 
            this.btnUbicacion.Location = new System.Drawing.Point(218, 55);
            this.btnUbicacion.Name = "btnUbicacion";
            this.btnUbicacion.Size = new System.Drawing.Size(129, 23);
            this.btnUbicacion.TabIndex = 31;
            this.btnUbicacion.Text = "Seleccionar ubicacion";
            this.btnUbicacion.UseVisualStyleBackColor = true;
            this.btnUbicacion.Click += new System.EventHandler(this.btnUbicacion_Click);
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(58, 29);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.ReadOnly = true;
            this.txtRuta.Size = new System.Drawing.Size(289, 20);
            this.txtRuta.TabIndex = 30;
            // 
            // CrearImagen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 323);
            this.Controls.Add(this.groupBox1);
            this.Name = "CrearImagen";
            this.Text = "CrearImagen";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tamanoDisco)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkFormatear;
        private System.Windows.Forms.TextBox txtVolLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.ComboBox tamanoSectoresPorCluster;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox sizeTypeCombobox;
        private System.Windows.Forms.NumericUpDown tamanoDisco;
        private System.Windows.Forms.Button btnUbicacion;
        private System.Windows.Forms.TextBox txtRuta;
    }
}