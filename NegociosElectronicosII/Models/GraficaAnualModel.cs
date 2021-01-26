using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NegociosElectronicosII.Models
{
    public class GraficaAnualModel
    {
        public GraficaAnualModel() {
            this.Articulos = new List<int>();
            this.Vehiculos = new List<int>();
            this.Meses = new List<string>();
        }

        public List<String> Meses { get; set; }
        public List<Int32> Vehiculos { get; set; }
        public List<Int32> Articulos{ get; set; }

        public String MesesJson { get; set; }
        public String VehiculosJson { get; set; }
        public String ArticulosJson { get; set; }
    }

    public class LoMasVendidoModel {

        public LoMasVendidoModel()
        {
            this.ArticuloVehiculo = new List<string>();
            this.Cantidad = new List<int>();
            this.ListModel = new List<VentasGraficoModel>();
            this.Colores = new List<string>();
        }

        public List<String> ArticuloVehiculo { get; set; }
        public List<String> Colores { get; set; }
        public List<Int32> Cantidad { get; set; }

        public List<VentasGraficoModel> ListModel { get; set; }
        public string ArticuloVehiculoJson { get; set; }
        public string CantidadJson { get; set; }
        public string ColoresJson { get; set; }
    }

    public class VentasGraficoModel
    {
        public string Objeto { get; set; }
        public Int32 Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }
}