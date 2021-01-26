using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NegociosElectronicosII.Models
{
    public class VentasDetalle
    {
        public int? vehiculoid { get; set; }
        public int? productoid { get; set; }
        public int? cantidad { get; set; }
        public int? total { get; set; }
    }

    public class LosMasBuscados {
        public Int32 CantidadDeVeces { get; set; }
        public String Busqueda { get; set; }
    }

    public class BusquedaModel {

        public Int32 TipoDeBusqueda { get; set; }
        public NE_Vehiculo Vehiculos = new NE_Vehiculo();
        public NE_Producto Productos = new NE_Producto();
    }
}