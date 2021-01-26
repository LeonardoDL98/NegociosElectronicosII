using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NegociosElectronicosII.Models
{
    public class OfertaModel
    {
        public OfertaModel() {
            this.ImagenesProducto = new List<NE_ProductoImagen>();
            this.ImagenesVehiculo = new List<NE_VehiculoImagen>();
        }

        public String Text { get; set; }
        public List<NE_ProductoImagen> ImagenesProducto { get; set; }
        public List<NE_VehiculoImagen> ImagenesVehiculo { get; set; }
        public bool IsProduct { get; set; }
        public Int32 ID { get; set; }
        public Int32 GetPositionRandom {
            get {
                Random random = new Random();
                return random.Next(0, 10);
            }
        }
    }
}