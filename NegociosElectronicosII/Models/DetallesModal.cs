using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NegociosElectronicosII.Models;

namespace NegociosElectronicosII.Models
{
    public class DetallesModal
    {
        public NE_Vehiculo vehiculo { get; set; }
        public NE_Producto producto { get; set; }
        public int tipo { get; set; }
    }
}