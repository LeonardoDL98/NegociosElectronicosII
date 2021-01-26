using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NegociosElectronicosII.Models
{
    public class ExtendModel
    {
    }

    #region NE_Vehiculos
    [MetadataType(typeof(MetadataVehiculo))]
    public partial class NE_Vehiculo
    {

    }
    public class MetadataVehiculo
    {
        [Key]
        public int VehiculoId { get; set; }

        [Display(Name = "CategoriaId", ResourceType = (typeof(Recursos)))]
        public int CategoriaId { get; set; }

        [Display(Name = "MarcaId", ResourceType = (typeof(Recursos)))]
        public int MarcaId { get; set; }

        [Range(1000, 2020, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public int Modelo { get; set; }

        [Display(Name = "TransmisionId", ResourceType = (typeof(Recursos)))]
        public int TransmisionId { get; set; }

        [Display(Name = "ColorId", ResourceType = (typeof(Recursos)))]
        public int ColorId { get; set; }

        [Range(0, 100000000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        [Display(Name = "PrecioVenta", ResourceType = (typeof(Recursos)))]
        public decimal PrecioVenta { get; set; }

        [Range(0, 100000000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        [Display(Name = "PrecioCompra", ResourceType = (typeof(Recursos)))]
        public decimal PrecioCompra { get; set; }

        [MaxLength(1000, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        [Range(0, 100000000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Display(Name = "PrecioOFerta", ResourceType = (typeof(Recursos)))]
        public decimal PrecioOFerta { get; set; }

        [Display(Name = "MarcarComoOferta", ResourceType = (typeof(Recursos)))]
        public bool MarcarComoOferta { get; set; }

    }
    #endregion

    #region NE_Producto
    [MetadataType(typeof(MetadataProducto))]
    public partial class NE_Producto
    {
    }
    public class MetadataProducto
    {
        [Key]
        public int ProductoId { get; set; }

        [MaxLength(50, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Nombre { get; set; }

        [MaxLength(1000, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Descripcion { get; set; }

        [Display(Name = "MarcaId", ResourceType = (typeof(Recursos)))]
        public int MarcaId { get; set; }

        [Display(Name = "ColorId", ResourceType = (typeof(Recursos)))]
        public int ColorId { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public int Stock { get; set; }

        [Range(0, 100000000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        [Display(Name = "PrecioVenta", ResourceType = (typeof(Recursos)))]
        public decimal PrecioVenta { get; set; }

        [Range(0, 100000000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        [Display(Name = "PrecioCompra", ResourceType = (typeof(Recursos)))]
        public decimal PrecioCompra { get; set; }

        public bool Activo { get; set; }

        [Range(0, 100000000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Display(Name = "PrecioOFerta", ResourceType = (typeof(Recursos)))]
        public decimal PrecioOFerta { get; set; }

        [Display(Name = "MarcarComoOferta", ResourceType = (typeof(Recursos)))]
        public bool MarcarComoOferta { get; set; }
    }
    #endregion

    #region NE_Usuario
    [MetadataType(typeof(MetadataUsuario))]
    public partial class NE_Usuario
    {
        public string password { get; set; }

        public Int32 NumeroDeIngresosAlPortal { get; set; }
        public Int32 NumeroDeComprasRealizadas { get; set; }
        public Decimal TotalDeCompras { get; set; }
        public Int32 NumeroDeArticulosEnElCarro { get; set; }
        public Int32 NumeroDeArticulosEnListaDeDeseos { get; set; }
        public DateTime UltimoInicioDeSesion { get; set; }
    }

    public class MetadataUsuario
    {
        [Key]
        public int UsuarioId { get; set; }
        [MaxLength(50, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Nombre { get; set; }

        [MaxLength(50, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [MaxLength(50, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "SexoId", ResourceType = (typeof(Recursos)))]
        public int SexoId { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public int Edad { get; set; }

        [MaxLength(400, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Direccion { get; set; }

        [MaxLength(15, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(Recursos))]
        [MaxLength(360, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        [Display(Name = "Email", ResourceType = (typeof(Recursos)))]
        public string CorreoElectronico { get; set; }
        public bool Activo { get; set; }
        [Display(Name = "RolId", ResourceType = (typeof(Recursos)))]
        public int RolId { get; set; }

    }
    #endregion

    #region NE_Carrousell
    [MetadataType(typeof(MetadataCarrusell))]

    public partial class NE_Carrusel
    {
    }
    public class MetadataCarrusell
    {
        [Key]
        public int CarruselId { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public int Posicion { get; set; }

        [MaxLength(100, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Texto { get; set; }

        [MaxLength(1000, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Descripcion { get; set; }
        public string Ruta { get; set; }
        public string Extension { get; set; }
        public string NombreArchivo { get; set; }
        public bool Activo { get; set; }
    }
    #endregion

    #region NE_FAQS
    [MetadataType(typeof(MetadataFAQS))]

    public partial class NE_FAQS
    {
    }
    public class MetadataFAQS
    {
        [Key]
        public int FAQId { get; set; }

        [MaxLength(300, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Pregunta { get; set; }

        [MaxLength(1000, ErrorMessageResourceType = (typeof(Recursos)), ErrorMessageResourceName = "MaxLenght")]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public string Respuesta { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "Range", ErrorMessageResourceType = (typeof(Recursos)))]
        [Required(ErrorMessageResourceType = typeof(Recursos), ErrorMessageResourceName = "Required")]
        public int Orden { get; set; }
        public bool Activo { get; set; }
    }
    #endregion
}