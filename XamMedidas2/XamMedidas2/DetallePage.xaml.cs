using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using XamMedidas2.Modelos;

using Plugin.Media;
using Plugin.Media.Abstractions;

namespace XamMedidas2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetallePage : ContentPage
    {

        private class ImagenClase
        {
            public string ImagenPath { get; set; }
            public string ImagenNombre { get; set; }
        };
        private List<ImagenClase> imagenes = new List<ImagenClase>();

        public DetallePage(MedidasNotasClase MiMedida)
        {
            InitializeComponent();
            TxtCuentaClientes.Text = MiMedida.MedidasNotasContador.ToString();
            TxtCleinte.Text = MiMedida.MedidasNotasnombre;
            TxtDireccion.Text = MiMedida.MedidasNotasdomicilio;
            TxtPoblacion.Text = MiMedida.MedidasNotasnomzona;
            TxtTelefono1.Text = MiMedida.MedidasNotastelefono1;
            TxtTelefono2.Text = MiMedida.MedidasNotastelefono2;
            TxtObservaciones.Text = MiMedida.MedidasNotasobservaciones;
        }




        private void ButCamara_Clicked(object sender, EventArgs e)
        {
            var MiRespuesta = DisplayAlert("ñlñ","lñ","hjkhjkhj");
            //await CrossMedia.Current.Initialize();

            //if (!CrossMedia.Current.IsTakePhotoSupported && !CrossMedia.Current.IsPickPhotoSupported)
            //{
            //    await DisplayAlert("Mensaje..", "Captura de foto no soportada", "ok");
            //    return;
            //}
            //else
            //{

            //    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            //    {
            //        Directory = "Images",
            //        Name = DateTime.Now + "_test.jpg"
            //    });
            //    if (file == null) return;
            //    await DisplayAlert("Path...:", file.Path, "ok");
            //}


            //if (CrossMedia.Current.IsCameraAvailable &&
            //    CrossMedia.Current.IsTakePhotoSupported)
            //{
            //    var source = await Application.Current.MainPage.DisplayActionSheet(
            //        "Seleccionar Origen De Imagen",
            //        "Cancelar",
            //        null,
            //        "Galeria",
            //        "Camara");

            //    if (source == "Cancelar")
            //    {
            //        this.file = null;
            //        return;
            //    }

            //    if (source == "Camara")
            //    {
            //        this.file = await CrossMedia.Current.TakePhotoAsync(
            //            new StoreCameraMediaOptions
            //            {
            //                Directory = "Sample",
            //                Name = "test.jpg",
            //                PhotoSize = PhotoSize.Small,
            //            }
            //        );
            //    }
            //    else
            //    {
            //        this.file = await CrossMedia.Current.PickPhotoAsync();
            //    }
            //}
            //else
            //{
            //    this.file = await CrossMedia.Current.PickPhotoAsync();
            //}

            //if (this.file != null)
            //{
            //    this.imageSource = ImageSource.FromStream(() =>
            //    {
            //        var stream = file.GetStream();
            //        return stream;
            //    });
            //}
        }

        private void ImgDireccion_Clicked(object sender, EventArgs e)
        {
            string MiDireccioN = SacaDireccionN(TxtDireccion.Text);
            var uri = new Uri("http://maps.google.com/maps?q=" + MiDireccioN + " + ," + TxtPoblacion.Text);
            Device.OpenUri(uri);
        }
        private void ImgTelefono1_Clicked(object sender, EventArgs e)
        {
            string MiTelefono = TxtTelefono1.Text.Trim();
            Device.OpenUri(new Uri("tel:" + MiTelefono));
        }
        private void ImgTelefono2_Clicked(object sender, EventArgs e)
        {
            string MiTelefono = TxtTelefono2.Text.Trim();
            Device.OpenUri(new Uri("tel:" + MiTelefono));
        }
        private string SacaDireccionN(string MiDireccion)
        {
            try
            {
                string MiDireccionN;
                System.Text.StringBuilder direccion = new System.Text.StringBuilder();

                string Tipo = "";
                string calle = "";
                string numero = ";";
                int primero = 0;
                int segundo = 0;
                int tercero = 0;
                int cuarto = 0;
                int contador = 0;
                byte[] ascii = Encoding.UTF8.GetBytes(MiDireccion);
                foreach (byte codigo in ascii)
                {
                    if (primero == 0 && (codigo < 65 || codigo > 90) && (codigo < 96 || codigo > 123))
                        primero = contador;
                    else
                        if (segundo == 0 && ((codigo < 65 || codigo > 90) && (codigo < 96 || codigo > 123) && codigo != 32))
                    {
                        segundo = contador;
                        if (codigo > 47 && codigo < 58) tercero = contador;
                    }
                    else
                            if (tercero == 0 && (codigo > 47 && codigo < 58))
                        tercero = contador;
                    else
                                if (cuarto == 0 && tercero != 0 && (codigo < 48 || codigo > 57))
                        cuarto = contador + 1;
                    contador += 1;
                }
                if (cuarto == 0 && contador == direccion.Length) cuarto = contador;
                Tipo = MiDireccion.Substring(0, primero);
                calle = MiDireccion.Substring(primero + 1, segundo - primero - 1);
                numero = MiDireccion.Substring(tercero - 1, cuarto - tercero);

                if (Tipo.Substring(0, 1) == "A") Tipo = "Avenida";
                if (Tipo.Substring(0, 1) == "C") Tipo = "Calle";
                if (Tipo.Substring(0, 1) == "P") Tipo = "Plaza";
                if (Tipo.Substring(0, 1) == "U") Tipo = "Urbanizacion";
                //MiDireccionN = Tipo + " " + calle + ", " + numero + "," + MiPoblacion;
                MiDireccionN = calle + ", " + numero;
                return MiDireccionN;
            }
            catch
            { return ""; }
        }

        private async void ButGaleria_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {

                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
            });
            
            if (file == null) return;

            var MiImagenNombre = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
            //Debug.WriteLine("Answer: " + answer);

            ImagenClase imagen = new ImagenClase();
            imagen.ImagenPath = file.Path.ToString();
            imagen.ImagenNombre = MiImagenNombre.ToString();
            imagenes.Add(imagen);

            MiLista.ItemsSource = null;
            MiLista.ItemsSource = imagenes;



            //MiImagen.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});

        }
    }
}