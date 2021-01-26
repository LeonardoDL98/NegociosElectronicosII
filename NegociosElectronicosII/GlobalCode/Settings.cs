using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using NegociosElectronicosII.Models;

namespace NegociosElectronicosII
{
    public class Settings
    {
        #region Config

        private static DB_NE_Entitties _db;
        public static DB_NE_Entitties db
        {
            get
            {
                if (_db == null)
                    _db = new DB_NE_Entitties();
                return _db;
            }
            set
            {
                _db = value;
            }
        }

        #endregion

        //private static String GetValueFromSettings(String Key)
        //{
        //    return db.Tb_Settings.Where(x => x.Key == Key).First().Value;
        //}

        public static NE_Usuario LoggedUser
        {
            get
            {
                if (HttpContext.Current.Session["USR_Session"] == null)
                    return null;
                return (NE_Usuario)HttpContext.Current.Session["USR_Session"];
            }
            set
            {
                HttpContext.Current.Session["USR_Session"] = value;
            }
        }

        public static string ACCOUNT_SERVER
        {
            get
            {
                return WebConfigurationManager.AppSettings["ACCOUNT_SERVER"];
            }
        }

        public static string FROM
        {
            get
            {
                return WebConfigurationManager.AppSettings["FROM_SERVER"];
            }
        }

        public static string HOST_SERVER
        {
            get
            {
                return WebConfigurationManager.AppSettings["HOST_SERVER"];
            }
        }

        public static string PASSWORD_SERVER
        {
            get
            {
                return WebConfigurationManager.AppSettings["PASSWORD_SERVER"];
            }
        }

        public static int? PORT_SERVER
        {
            get
            {
                String PORT = WebConfigurationManager.AppSettings["PORT_SERVER"];
                if (String.IsNullOrEmpty(PORT))
                    return null;
                else
                    return Int32.Parse(PORT);
            }
        }
        public static string URL_TOConfirmEmail
        {
            get
            {
                return WebConfigurationManager.AppSettings["URL_TOConfirmEmail"];
            }
        }


        //Generate new code to new user
        public static string GetNewSuggestCode()
        {
            Int32 consecutive = db.NE_Usuario.Any() ? db.NE_Usuario.Max(x => x.UsuarioId) + 1 : 0;
            String CoreCode = consecutive.ToString().PadLeft(6, '0');
            return String.Format("X_{0}", CoreCode);
        }

        //public static Int32 GetPermission(int KEY)
        //{
        //    return db.NE_Usuario.Where(x => x.RolId == KEY).First().ID_Permission;
        //}

        public static Int32 NUMERO_DE_ITEMS_POR_PAGINA = 8;
        public static Dictionary<int,String> MESES = new Dictionary<int,string>{
            { 1,"Enero" },{ 2,"Febrero" },{ 3,"Marzo" },{ 4,"Abril" },{ 5,"Mayo" },{ 6,"Junio" }
            ,{ 7,"Julio" },{ 8,"Agosto" },{ 9,"Septiembre" },{ 10,"Octubre" },{ 11,"Noviembre" },{ 12,"Diciembre" }
        };
    }

    public class ACCION {
        public static int NUEVO_REGISTRO = 1;
        public static int INICIO_DE_SESION = 2;
        public static int COMPRA = 3;
        public static int AGREGAR_CARRO = 4;
        public static int ELIMINAR_CARRO = 5;
        public static int AGREGAR_LISTA_DESEOS = 8;
        public static int ELIMINAR_LISTA_DESEOS = 9;
        public static int BUSQUEDA = 10;
        public static int DETALLES_PRODUCTO = 11;
        public static int FILTRADO_CATEGORIA = 12;
        public static int FILTRADO_MARCA = 13;
    }
}