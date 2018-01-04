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
using ExifLib;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;



namespace metadata
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog carpeta = new FolderBrowserDialog();

            DialogResult resultado = carpeta.ShowDialog();

            if (!string.IsNullOrWhiteSpace(carpeta.SelectedPath))
            {
                //string[] files = Directory.GetFiles(carpeta.SelectedPath);
                textBox1.Text = carpeta.SelectedPath;

                //MessageBox.Show("Files found: " + files.Length.ToString(), "Message");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog salida = new OpenFileDialog();
            salida.DefaultExt = "txt";
            salida.InitialDirectory = textBox1.Text;
            salida.CheckFileExists = false;
            if (salida.ShowDialog() == DialogResult.OK)
                {
                textBox2.Text = salida.FileName;
                }
        }
        private void posicion_google(List<double> latitud, List<double> longitud)
        {
            gMapControl1.Overlays.Clear();
            gMapControl1.MapProvider = GoogleMapProvider.Instance;
            //gMapControl1.MapProvider = GoogleSatelliteMapProvider.Instance;
            //get tiles from server only
            gMapControl1.Manager.Mode = AccessMode.ServerOnly;
            //not use proxy
            GMapProvider.WebProxy = null;
            gMapControl1.Position = new PointLatLng(latitud[0], longitud[0]);
            GMapOverlay transparencia = new GMapOverlay("transparencia");
            for (int i = 0; i < longitud.Count; i++) {
                GMarkerGoogle marcador = new GMarkerGoogle(new PointLatLng(latitud[i], longitud[i]), GMarkerGoogleType.green);
                transparencia.Markers.Add(marcador);
            }
            //GMarkerGoogle marcador1 = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(textBox1.Text) - .5, Convert.ToDouble(textBox2.Text) - .5), GMarkerGoogleType.green);
            //GMarkerGoogle marcador2 = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(textBox1.Text) + .5, Convert.ToDouble(textBox2.Text) + .5), GMarkerGoogleType.green);
            //GMarkerGoogle marcador3 = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(textBox1.Text) + 1, Convert.ToDouble(textBox2.Text) + 1), GMarkerGoogleType.green);
            //GMarkerGoogle marcador4 = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(textBox1.Text) + 1, Convert.ToDouble(textBox2.Text) - 1.5), GMarkerGoogleType.red);
            //transparencia.Markers.Add(marcador1);
            //transparencia.Markers.Add(marcador2);
            //transparencia.Markers.Add(marcador3);
            //transparencia.Markers.Add(marcador4);
            gMapControl1.Overlays.Add(transparencia);

            //zoom min/max; default both = 2
            //gMapControl1.Refresh();
            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 20;
            //set zoom
            gMapControl1.Zoom = 10;
            gMapControl1.ZoomAndCenterMarkers("transparencia");
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;

        }
        private int extrmetadata(string directorio, string archivodestino)
        {
            int cantidad = 0;
            List<double> latitudes = new List<double>();
            List<double> longitudes = new List<double>();

            if (!Directory.Exists(directorio))
            {
                return 0;
            }
            StreamWriter sw = new StreamWriter(archivodestino);
            richTextBox1.Clear();
            foreach (string archivo in Directory.GetFiles(directorio, "*.jpg"))
            {

                double[] latitud;
                double[] longitud;
                string latitudref, longitudref;
                double altitud;
                double latitudFormato, longitudFormato;
                DateTime fechacreacion;
                try
                {
                    ExifLib.ExifReader lector = new ExifReader(archivo);
                    if (lector.GetTagValue<Double[]>(ExifTags.GPSLatitude, out latitud) && lector.GetTagValue<Double[]>(ExifTags.GPSLongitude, out longitud))
                    {
                        lector.GetTagValue(ExifTags.DateTime, out fechacreacion);
                        lector.GetTagValue(ExifTags.GPSLatitudeRef, out latitudref);
                        lector.GetTagValue(ExifTags.GPSLongitudeRef, out longitudref);
                        lector.GetTagValue(ExifTags.GPSAltitude, out altitud);
                        latitudFormato = latitud[0] + latitud[1] / 60 + latitud[2] / 3600;
                        longitudFormato = longitud[0] + longitud[1] / 60 + longitud[2] / 3600;
                        
                        if (latitudref == "S")
                        {
                            latitudFormato = latitudFormato * -1;
                        }

                        if (longitudref == "W")
                        {
                            longitudFormato = longitudFormato * -1;
                        }

                        //Console.WriteLine("{0}\t{10}\t{4} {1}:{2}:{3}\t{8} {5}:{6}:{7}\tAltitud: {9}", Path.GetFileName(archivo), latitud[0], latitud[1], latitud[2], latitudref, longitud[0], longitud[1], longitud[2], longitudref, altitud, fechacreacion);
                        if (radioButton1.Checked)
                        {
                            sw.WriteLine("{0}\t{4} {1}:{2}:{3}\t{8} {5}:{6}:{7}\tAltitud: {9}\t{10}", Path.GetFileName(archivo), latitud[0], latitud[1], latitud[2], latitudref, longitud[0], longitud[1], longitud[2], longitudref, altitud, fechacreacion);
                            richTextBox1.AppendText(string.Format("{0}\t{4} {1}:{2}:{3}\t{8} {5}:{6}:{7}\tAltitud: {9}\t{10}\r\n", Path.GetFileName(archivo), latitud[0], latitud[1], latitud[2], latitudref, longitud[0], longitud[1], longitud[2], longitudref, altitud, fechacreacion));
                        }
                        else
                        {
                            sw.WriteLine("{0}\t{1}\t{2}\tAltitud: {3}\t{4}", Path.GetFileName(archivo), latitudFormato, longitudFormato, altitud, fechacreacion);
                            richTextBox1.AppendText(string.Format("{0}\t{1}\t{2}\tAltitud: {3}\t{4}\r\n", Path.GetFileName(archivo), latitudFormato, longitudFormato, altitud, fechacreacion));
                        }
                        cantidad++;
                        //Console.WriteLine(Path.GetFileName(archivo), latitudFormato, longitudFormato, latitudes.Count);
                        latitudes.Add(latitudFormato);
                        longitudes.Add(longitudFormato);

                    }
                    else { sw.WriteLine("{0} no cuenta con datos de geolocalizacion", Path.GetFileName(archivo)); }
                }
                catch
                {
                }
            }
            posicion_google(latitudes, longitudes);
            sw.Flush();
            sw.Close();
            
            //Console.ReadKey();
            return cantidad;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int cantidad;
            if (!((textBox1.Text == string.Empty) || (textBox2.Text == string.Empty)))
            {
                cantidad = extrmetadata(textBox1.Text, textBox2.Text);
                label4.Text = Convert.ToString(cantidad);

            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            //System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            //ToolTip1.SetToolTip(button1, "Seleccione la carpeta con las fotos cuya geolocalizaion quiere extraer");

        }
        private void button2_MouseHover(object sender, EventArgs e)
        {
            //System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            //ToolTip1.SetToolTip(button2, "Seleccione el archivo que guardara la geolocalizacion");

        }
    }
}
