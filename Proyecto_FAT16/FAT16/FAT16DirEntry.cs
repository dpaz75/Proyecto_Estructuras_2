using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Globalization;

namespace Proyecto_FAT16.FAT16
{
    public class FAT16DirEntry
    {

        public char[] DIR_NAME = new char[8]; //Offset 0 - 8 bytes. Primeros ocho caracteres del nombre del archivo con el formato 8.3
        public char[] DIR_EXT = new char[3]; //Offset 8 - 3 bytes. Ultimos 3 caracteres correspondientes a la extension del archivo en el formato 8.3
        public byte DIR_ATTR; //Offset 11 - 1 byte. Byte que nos dice el atributo del archivo.
        public byte DIR_NTRES = 0; //Offset 12 - 1 bytes. Reservado, tiene que ser 0
        public byte DIR_CrtTimeTenth; //Offset 13 - 1 bytes.
        public byte[] DIR_CrtTime = new byte[2]; //Offset 14 - 2 bytes.
        public byte[] DIR_CrtDate = new byte[2]; //Offset 16 - 2 bytes.
        public byte[] DIR_LstAccDate = new byte[2]; //Offset 18 - 2 bytes.
        public byte[] DIR_FstClustHI = new byte[2]; //Offset 20 - 2 bytes.
        public byte[] DIR_WrtTime = new byte[2]; //Offset 22 - 2 bytes.
        public byte[] DIR_WrtDate = new byte[2]; //Offset 24 - 2 bytes.
        public byte[] DIR_FstClustLO = new byte[2]; //Offset 26 - 2 bytes
        public byte[] DIR_FileSize = new byte[4]; //Offset 28 - 4 bytes

        //Real things
        public char[] DIR_FULL_NAME = new char[11];
        public String NombreDeDir;
        public DateTime FechaCreacion;
        public DateTime FechaUltimoAcceso;
        public DateTime FechaUltimaModificacion;
        public int PrimerCluster;
        public long FileSize;
        public bool esDirectorio = false;
        public bool ignoreEntry = false;
        public bool isPointingToParent = false;
        public int ParentCluster;

        public Stack<FAT16LFNEntry> ListaLFNEntries = new Stack<FAT16LFNEntry>();

        public FAT16DirEntry()
        {

        }

        public void ParseDirEntry()
        {

            DIR_FULL_NAME = DIR_NAME.Concat(DIR_EXT).ToArray();

            if (ListaLFNEntries.Count > 0)
            {
                NombreDeDir = ValidarNombreLFN(CheckSum(Encoding.ASCII.GetBytes(DIR_FULL_NAME)));
                if (NombreDeDir == null)
                {
                    NombreDeDir = "-ARCHIVO CORRUPTO-";
                }
            }
            else
            {

                //Parsear Nombre
                String nombre = new String(DIR_NAME);
                for (int i = nombre.Length - 1; i >= 0; i--)
                {
                    if (nombre[i] == ' ')
                        nombre = nombre.Substring(0, nombre.LastIndexOf(' '));
                }

                String ext = new String(DIR_EXT);
                String fullname;
                if (DIR_EXT.ToString() != "")
                    fullname = nombre;
                else
                    fullname = nombre + "." + ext;
                DIR_FULL_NAME = fullname.ToCharArray();

                NombreDeDir = new String(DIR_FULL_NAME);
            }

            //Comprobar de que tipo de archivo es
            if (DIR_ATTR == 0x10)
            {
                esDirectorio = true;
            }
            else if (DIR_ATTR == 0x20)
            {
                esDirectorio = false;
            }

            //Calcular Tamano o decir si es directorio
            FileSize = BitConverter.ToInt32(DIR_FileSize, 0);

            //Obtener Fechas de Creacion y Modificacion
            FechaCreacion = CalcularFecha(DIR_CrtDate, DIR_CrtTime);
            FechaUltimaModificacion = CalcularFecha(DIR_WrtDate, DIR_WrtTime);

            //Obtener el primer cluster de datos en la cadena de clusters
            PrimerCluster = BitConverter.ToInt16(DIR_FstClustHI, 0) + BitConverter.ToInt16(DIR_FstClustLO, 0);

            if (NombreDeDir == ".")
            {
                ignoreEntry = true;

            }
            else if (NombreDeDir == "..")
            {
                isPointingToParent = true;
                if (PrimerCluster == 0)
                {
                    ParentCluster = 2;
                    PrimerCluster = 2;
                }
                else
                {
                    ParentCluster = PrimerCluster;
                }
            }


        }

        public Int32 GetFirstCluster()
        {
            return PrimerCluster;
        }

        //Metodo para crear el checksum basado en el nombre en formato 8.3 segun la especificacion de Microsoft
        public byte CheckSum(byte[] pFcbName)
        {
            short FcbNameLen;
            byte Sum = 0;
            byte Compare = 0x01;
            int contadorChar = 0;
            for (FcbNameLen = 11; FcbNameLen != 0; FcbNameLen--)
            {
                byte[] operation = new byte[1];
                operation[0] = (byte)(Sum & Compare);
                bool booleano = BitConverter.ToBoolean(operation, 0);
                byte b;
                if (booleano)
                    b = 0x80;
                else
                    b = 0x00;

                Sum = (byte)(b + (Sum >> 1) + pFcbName[contadorChar]);
                contadorChar++;
            }
            return Sum;
        }


        private String ValidarNombreLFN(byte Checksum)
        {

            String nombre = null;

            //Copiar LFNs en otro Stack para no perderlo
            Stack<FAT16LFNEntry> StackClonado = new Stack<FAT16LFNEntry>(ListaLFNEntries);
            bool Corrupto = false;
            foreach (FAT16LFNEntry lfn in StackClonado)
            {
                if (lfn.LDIR_Checksum.Equals(Checksum))
                {
                    continue;
                }
                else
                {
                    Corrupto = true;
                    break;
                }
            }

            //Si todos los checksum dan con el nombre entonces procedemos a tomar el nombre del archivo a partir de los LFN
            if (!Corrupto)
            {
                foreach (FAT16LFNEntry lfn in StackClonado)
                {
                    String LDIR1 = Encoding.UTF8.GetString(lfn.LDIR_Name1);
                    foreach (char c in LDIR1)
                    //foreach(char c in lfn.LDIR_Name1)
                    {
                        if (c != '\0')
                        {
                            nombre += c;
                        }
                    }

                    String LDIR2 = Encoding.UTF8.GetString(lfn.LDIR_Name2);
                    foreach (char c in LDIR2)
                    //foreach (char c in lfn.LDIR_Name2)
                    {
                        if (c != '\0')
                        {
                            nombre += c;
                        }
                    }

                    String LDIR3 = Encoding.UTF8.GetString(lfn.LDIR_Name3);
                    foreach (char c in LDIR3)
                    //foreach (char c in lfn.LDIR_Name2)
                    {
                        if (c != '\0')
                        {
                            nombre += c;
                        }
                    }

                }

                byte[] nullCharacters = { 0xFF, 0xFF };
                char[] nullChars = Encoding.UTF8.GetChars(nullCharacters);

                for (int i = nombre.Length - 1; i >= 0; i--)
                {
                    if (nombre[i] == ' ')
                    {
                        nombre = nombre.Substring(0, nombre.LastIndexOf(' '));
                    }
                    else if (nombre[i] == nullChars[0])
                    {
                        nombre = nombre.Substring(0, nombre.LastIndexOf(nullChars[0]));
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Nombre Corrupto!!!!");
            }

            return nombre;
        }

        private DateTime CalcularFecha(byte[] Fecha, byte[] Tiempo)
        {

            DateTime fecha;

            //Fecha
            BitArray CreacionBits = new BitArray(Fecha);
            BitArray Ano = new BitArray(7);
            BitArray Mes = new BitArray(4);
            BitArray Dia = new BitArray(5);

            int ContadorBitsAno = 0;
            for (int i = 9; i <= 15; i++)
            {
                Ano[ContadorBitsAno] = CreacionBits[i];
                ContadorBitsAno++;
            }

            int ContadorBitsMes = 0;
            for (int i = 5; i <= 8; i++)
            {
                Mes[ContadorBitsMes] = CreacionBits[i];
                ContadorBitsMes++;
            }

            int ContadorBitsDia = 0;
            for (int i = 0; i <= 4; i++)
            {
                Dia[ContadorBitsDia] = CreacionBits[i];
                ContadorBitsDia++;
            }

            byte[] AnoByte = new byte[2];
            Ano.CopyTo(AnoByte, 0);
            int AnoCreacion = (1980 + BitConverter.ToInt16(AnoByte, 0));

            byte MesByte = 0;
            for (int i = 3; i >= 0; i--)
                MesByte = (byte)((MesByte << 1) | (Mes[i] ? 1 : 0));
            int MesCreacion = (MesByte);

            byte[] DiaByte = new byte[2];
            Dia.CopyTo(DiaByte, 0);
            int DiaCreacion = (BitConverter.ToInt16(DiaByte, 0));

            //Tiempo
            BitArray TiempoCreacionBits = new BitArray(Tiempo);
            BitArray Hora = new BitArray(5);
            BitArray Minutos = new BitArray(6);
            BitArray Segundos = new BitArray(5);

            int ContadorSegundos = 0;
            for (int i = 0; i <= 4; i++)
            {
                Segundos[ContadorSegundos] = TiempoCreacionBits[i];
                ContadorSegundos++;
            }

            int ContadorMinutos = 0;
            for (int i = 5; i <= 10; i++)
            {
                Minutos[ContadorMinutos] = TiempoCreacionBits[i];
                ContadorMinutos++;
            }

            int ContadorHora = 0;
            for (int i = 11; i <= 15; i++)
            {
                Hora[ContadorHora] = TiempoCreacionBits[i];
                ContadorHora++;
            }


            byte HoraByte = 0;
            for (int i = 4; i >= 0; i--)
                HoraByte = (byte)((HoraByte << 1) | (Hora[i] ? 1 : 0));
            int HoraCreacion = (HoraByte);

            byte MinutosByte = 0;
            for (int i = 5; i >= 0; i--)
                MinutosByte = (byte)((MinutosByte << 1) | (Minutos[i] ? 1 : 0));
            int MinutoCreacion = (MinutosByte);

            //Multiplicado por dos porque su granularidad es de 2 (Rango de 0 - 29)
            byte SegundosByte = 0;
            for (int i = 4; i >= 0; i--)
                SegundosByte = (byte)((SegundosByte << 1) | (Segundos[i] ? 1 : 0));
            int SegundosCreacion = (SegundosByte) * 2;

            String dia = "";
            if (DiaCreacion < 10)
            {
                dia = "0" + DiaCreacion;
            }
            else
            {
                dia = "" + DiaCreacion;
            }

            String mes = "";
            if (MesCreacion < 10)
            {
                mes = "0" + MesCreacion;
            }
            else
            {
                mes = "" + MesCreacion;
            }

            String hora = "";
            if (HoraCreacion < 10)
            {
                hora = "0" + HoraCreacion;
            }
            else
            {
                hora = "" + HoraCreacion;
            }

            String minutos = "";
            if (MinutoCreacion < 10)
            {
                minutos = "0" + MinutoCreacion;
            }
            else
            {
                minutos = "" + MinutoCreacion;
            }

            String segundos = "";
            if (SegundosCreacion < 10)
            {
                segundos = "0" + SegundosCreacion;
            }
            else
            {
                segundos = "" + SegundosCreacion;
            }

            String FormatoDate = String.Format("{0}/{1}/{2} {3}:{4}:{5}", dia, mes, AnoCreacion, hora, minutos, segundos);
            fecha = DateTime.ParseExact(FormatoDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            return fecha;

        }

        public String GetDirName()
        {
            return NombreDeDir;
        }

        public String GetFechaCreacionString()
        {
            return FechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
        }

        public DateTime GetFechaCreacion()
        {
            return FechaCreacion;
        }

        public String GetFechaModificacionString()
        {
            return FechaUltimaModificacion.ToString("dd/MM/yyyy H:mm:ss");
        }

        public DateTime GetFechaModificacion()
        {
            return FechaUltimaModificacion;
        }

        public long GetDirSize()
        {
            return FileSize;
        }

        public String GetDirSizeString()
        {
            return FileSize.ToString();
        }

        public String GetTipo()
        {
            if (esDirectorio)
                return "Directorio";
            else
                return "Archivo";
        }

        public bool esDir()
        {
            return esDirectorio;
        }
    }
}