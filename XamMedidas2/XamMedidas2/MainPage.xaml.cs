using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamMedidas2.Modelos;

namespace XamMedidas2
{
    public partial class MainPage : ContentPage
    {
        public IEnumerable<MedidasNotasClase> MisMedidas;
        public static string Servidor;
        List<MedidasNotasClase> MisMedidasLista = new List<MedidasNotasClase>();

        public string data;
        public string PasaDatos;

        string MiDia;
        string MiMes;
        string MiAno;
        public static string MiMedidor;

 
        public MainPage()
        {
            InitializeComponent();
            DtpFecha.Date = DateTime.Now;
            //prueba
            Servidor = "No Encontrado";

            Ping MiPing = new Ping();
            if (MiPing.Send("192.168.1.200").Status == IPStatus.Success)
            {
                Servidor = "192.168.1.200";
            }
            else
            {
                Servidor = "213.98.73.215";
            }

            //Servidor = "213.98.73.215";
            //Servidor = "10.0.65.200";

            MiDia = DtpFecha.Date.Day.ToString("00");
            MiMes = DtpFecha.Date.Month.ToString("00");
            MiAno = DtpFecha.Date.Year.ToString().Substring(2);
            MiMedidor = TxtMedidor.Text;

            CargaMedidasAsync(MiMedidor, MiDia, MiMes, MiAno);

            MiLista.ItemSelected += MiLista_ItemSelected;
        }

        private async void MiLista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var MiMedidaSeleccionada = (MedidasNotasClase)e.SelectedItem;
            MedidasNotasClase MiMedida = (from p in MisMedidas
                                          where p.MedidasNotascodigonota == MiMedidaSeleccionada.MedidasNotascodigonota
                                          select p).Single();
            await Navigation.PushAsync(new DetallePage(MiMedida));

        }

        private void DtpFecha_DateSelected(object sender, DateChangedEventArgs e)
        {

            MiDia = DtpFecha.Date.Day.ToString("00");
            MiMes = DtpFecha.Date.Month.ToString("00");
            MiAno = DtpFecha.Date.Year.ToString().Substring(2);
            MiMedidor = TxtMedidor.Text;

            CargaMedidasAsync(MiMedidor, MiDia, MiMes, MiAno);
        }


        private async void CargaMedidasAsync(string MiMedidor, string MiDia, string MiMes, string Miano)
        {
            try
            {
                HttpClient httpclient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://" + Servidor + "/Reparto/Service1.svc/DameMedidas?MiMedidor=" + MiMedidor + "&MiDia=" + MiDia + "&MiMes=" + MiMes + "&MiAno=" + Miano);
                HttpResponseMessage response = await httpclient.SendAsync(request);

                MisMedidasLista.Clear();

                data = await response.Content.ReadAsStringAsync();
                MisMedidas = JsonConvert.DeserializeObject<IEnumerable<MedidasNotasClase>>(data);

                IEnumerable<MedidasNotasClase> MisClientes = (from p in MisMedidas select p).Distinct();

                int MiContador = 1;
                foreach (MedidasNotasClase MiCliente in MisClientes)
                {
                    MiCliente.MedidasNotasContador = MiContador;
                    MisMedidasLista.Add(MiCliente);
                    MiContador += 1;
                }
                PasaDatos = JsonConvert.SerializeObject(MisClientes);

                MiLista.ItemsSource = null;
                MiLista.ItemsSource = MisMedidasLista;

            }
            catch
            {

            }
        }

    }
}
