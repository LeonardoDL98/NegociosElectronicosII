using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NegociosElectronicosII.Models
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName ="Required",ErrorMessageResourceType =typeof(Recursos))]
        [Display(Name ="Email",ResourceType =(typeof(Recursos)))]
        public String Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Recursos))]
        [Display(Name = "Password", ResourceType = (typeof(Recursos)))]
        public String Password { get; set; }

    }
    public class RegisterModel
    {
        public String Nombre { get; set; }
      
        public String ApellidoPaterno { get; set; }
      
        public String ApellidoMAterno { get; set; }
        public String Sexo { get; set; }
        public String Edad { get; set; }
        public String Direccion { get; set; }
        public String Telefono { get; set; }
        public String Correo { get; set; }
        public String Contrasena { get; set; }
    }
}