using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NegociosElectronicosII.GlobalCode;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace NegociosElectronicosII.Controllers
{
    [NegociosII_Auth(Roles = "1")]
    public class EstadisticaController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Ganancias mensuales

        public PartialViewResult GananciaMensualParcial() {

            List<Int32> Anios = new List<int>();
            List<SelectListItem> fechas = new List<SelectListItem>();
            
            //Obtener la lista de los anios con ventas
            Anios = db.NE_Venta.Select(x => x.Fecha.Year).Distinct().ToList();
            if (!Anios.Contains(DateTime.Now.Year))
                Anios.Add(DateTime.Now.Year);

            //recorrer por anios
            foreach (var item in Anios)
            {
                foreach (var subitem in Settings.MESES)
                {
                    fechas.Add(new SelectListItem() { Value = subitem.Key.ToString() + "," + item.ToString(), Text = subitem.Value, Selected= (item==DateTime.Now.Year && DateTime.Now.Month== subitem.Key) });
                }
            }

            ViewBag.Fechas = fechas;

            return PartialView();
        }

        public PartialViewResult GananciaMensualDetalleParcial(Int32 Mes, Int32 Anio)
        {
            ViewBag.TotalVenta =
                db.NE_Venta.Where(x => x.Fecha.Month == Mes && x.Fecha.Year == Anio).Any()?
                db.NE_Venta.Where(x => x.Fecha.Month == Mes && x.Fecha.Year == Anio).Sum(x => x.TotalVenta) :0.0m;
            return PartialView();
        }

        #endregion

        #region Ganancias anuales

        public PartialViewResult GananciaAnualParcial()
        {

            List<Int32> Anios = new List<int>();
            List<SelectListItem> fechas = new List<SelectListItem>();

            //Obtener la lista de los anios con ventas
            Anios = db.NE_Venta.Select(x => x.Fecha.Year).Distinct().ToList();
            if (!Anios.Contains(DateTime.Now.Year))
                Anios.Add(DateTime.Now.Year);

            //recorrer por anios
            foreach (var item in Anios)
            {
                fechas.Add(new SelectListItem() {
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            }

            ViewBag.Fechas = fechas;

            return PartialView();
        }

        public PartialViewResult GananciaAnualDetalleParcial(Int32 Anio)
        {
            ViewBag.TotalVenta =
                db.NE_Venta.Where(x => x.Fecha.Year == Anio).Any() ?
                db.NE_Venta.Where(x => x.Fecha.Year == Anio).Sum(x => x.TotalVenta) : 0.0m;
            return PartialView();
        }

        #endregion

        #region Cantidad de vehiculos vendidos

        public PartialViewResult VehiculosVendidosPartial() {
            List<SelectListItem> Anios = new List<SelectListItem>();
            List<SelectListItem> Meses = new List<SelectListItem>();
            List<Int32> _Anios = new List<int>();

            _Anios = db.NE_Venta.Select(x => x.Fecha.Year).Distinct().ToList();
            if (!_Anios.Contains(DateTime.Now.Year))
                _Anios.Add(DateTime.Now.Year);

            Anios = _Anios.Select(x => new SelectListItem() { Text=x.ToString(), Value=x.ToString() }).ToList();

            foreach (var item in Settings.MESES)
                Meses.Add(new SelectListItem() {
                    Text = item.Value,
                    Value = item.Key.ToString(),
                    Selected = item.Key == DateTime.Now.Month
                });

            ViewBag.Anios = Anios;
            ViewBag.Meses = Meses;

            return PartialView();
        }

        public PartialViewResult VehiculosVendidosDetalleParcial(Int32 Mes, Int32 Anio) {

            //ViewBag.NumeroDeAutos= db.NE_Venta.Where(x => x.Fecha.Month == Mes && x.Fecha.Year == Anio).Count();
            ViewBag.NumeroDeAutos= db.NE_VentaDetalle.Any(x=>x.VehiculoId!=null && x.NE_Venta.Fecha.Month == Mes && x.NE_Venta.Fecha.Year == Anio)?
                db.NE_VentaDetalle.Where(x => x.VehiculoId != null && x.NE_Venta.Fecha.Month == Mes && x.NE_Venta.Fecha.Year == Anio).Sum(x => x.Cantidad) : 0;
            return PartialView();
        }

        #endregion

        #region Cantidad de articulos vendidos

        public PartialViewResult ArticulosVendidosPartial() {
            List<SelectListItem> Anios = new List<SelectListItem>();
            List<SelectListItem> Meses = new List<SelectListItem>();
            List<Int32> _Anios = new List<int>();

            _Anios = db.NE_Venta.Select(x => x.Fecha.Year).Distinct().ToList();
            if (!_Anios.Contains(DateTime.Now.Year))
                _Anios.Add(DateTime.Now.Year);

            Anios = _Anios.Select(x => new SelectListItem() { Text = x.ToString(), Value = x.ToString() }).ToList();

            foreach (var item in Settings.MESES)
                Meses.Add(new SelectListItem()
                {
                    Text = item.Value,
                    Value = item.Key.ToString(),
                    Selected = item.Key == DateTime.Now.Month
                });

            ViewBag.Anios = Anios;
            ViewBag.Meses = Meses;

            return PartialView();
        }

        public PartialViewResult ArticulosVendidosDetalleParcial(Int32 Mes, Int32 Anio)
        {
            //ViewBag.NumeroDeArticulos = db.NE_Venta.Where(x => x.Fecha.Month == Mes && x.Fecha.Year == Anio).Count();
            ViewBag.NumeroDeArticulos = db.NE_VentaDetalle.Any(x => x.ProductoId != null && x.NE_Venta.Fecha.Month == Mes && x.NE_Venta.Fecha.Year == Anio)?
                db.NE_VentaDetalle.Where(x => x.ProductoId != null && x.NE_Venta.Fecha.Month == Mes && x.NE_Venta.Fecha.Year == Anio).Sum(x=>x.Cantidad) : 0;
            return PartialView();
        }


        #endregion

        #region Usuarios

        public PartialViewResult FiltrosUsuariosParcial() {
            List<SelectListItem> Anios = new List<SelectListItem>();
            List<SelectListItem> Meses = new List<SelectListItem>();
            List<Int32> _Anios = new List<int>();

            _Anios = db.NE_Venta.Select(x => x.Fecha.Year).Distinct().ToList();
            if (!_Anios.Contains(DateTime.Now.Year))
                _Anios.Add(DateTime.Now.Year);

            Anios = _Anios.Select(x => new SelectListItem() { Text = x.ToString(), Value = x.ToString() }).ToList();

            foreach (var item in Settings.MESES)
                Meses.Add(new SelectListItem()
                {
                    Text = item.Value,
                    Value = item.Key.ToString(),
                    Selected = item.Key == DateTime.Now.Month
                });

            ViewBag.Anios = Anios;
            ViewBag.Meses = Meses;

            return PartialView();
        }

        public PartialViewResult FiltrosUsuariosDetalleParcial(Int32 Mes, Int32 Anio) {

            ViewBag.NumerosDeUsuariosLogueados= db.NE_Bitacora.Where(x => x.FechaDeRegistro.Month == Mes && x.FechaDeRegistro.Year == Anio && x.AccionId == ACCION.INICIO_DE_SESION).Count();
            ViewBag.NumerosDeUsuariosRegistrados = db.NE_Bitacora.Where(x => x.FechaDeRegistro.Month == Mes && x.FechaDeRegistro.Year == Anio && x.AccionId == ACCION.NUEVO_REGISTRO).Count();
            ViewBag.NumeroDeUsuariosQueHicieronCompras = db.NE_Bitacora.Where(x => x.FechaDeRegistro.Month == Mes && x.FechaDeRegistro.Year == Anio && x.AccionId == ACCION.COMPRA).Count();

            return PartialView();
        }

        #endregion

        #region Usuarios con mas ventas

        public PartialViewResult UsuariosConMasVentas() {
            Decimal total = 0.0m;
            List<Models.UsuarioReporteModel> model = new List<Models.UsuarioReporteModel>();

            foreach (var item in db.NE_Usuario.Where(x => x.RolId == 4))
            {
                total = db.NE_Venta.Any(x => x.UsuarioId == item.UsuarioId) ? db.NE_Venta.Where(x => x.UsuarioId == item.UsuarioId).Sum(x => x.TotalVenta) : 0;
                model.Add(new Models.UsuarioReporteModel() {
                    Total= total,
                    Usuario= item.Nombre + " " + item.ApellidoPaterno
                });
            }

            model = model.OrderByDescending(x => x.Total).Take(5).ToList();

            return PartialView(model);
        }

        #endregion

        #region Ventas anuales

        public PartialViewResult MostrarVentasAnuales() {
            List<Int32> Anios = new List<int>();
            List<SelectListItem> fechas = new List<SelectListItem>();

            //Obtener la lista de los anios con ventas
            Anios = db.NE_Venta.Select(x => x.Fecha.Year).Distinct().ToList();
            if (!Anios.Contains(DateTime.Now.Year))
                Anios.Add(DateTime.Now.Year);

            //recorrer por anios
            foreach (var item in Anios)
            {
                fechas.Add(new SelectListItem()
                {
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            }

            ViewBag.Fechas = fechas;

            return PartialView();
        }

        public PartialViewResult MostrarVentasAnualesDetalle(Int32 Anio)
        {
            Int32 NumeroArticulos = 0, NumeroVehiculos = 0; 
            Models.GraficaAnualModel model= new Models.GraficaAnualModel();

            foreach (var item in Settings.MESES)
            {
                model.Meses.Add(item.Value);
                NumeroArticulos = db.NE_VentaDetalle.Any(x => x.NE_Venta.Fecha.Year == Anio && x.NE_Venta.Fecha.Month == item.Key && x.ProductoId != null) ?
                    db.NE_VentaDetalle.Where(x => x.NE_Venta.Fecha.Year == Anio && x.NE_Venta.Fecha.Month == item.Key && x.ProductoId != null).Select(x => x.Cantidad).Sum():0;
                model.Articulos.Add(NumeroArticulos);

                NumeroVehiculos = db.NE_VentaDetalle.Any(x => x.NE_Venta.Fecha.Year == Anio && x.NE_Venta.Fecha.Month == item.Key && x.VehiculoId != null) ?
                    db.NE_VentaDetalle.Where(x => x.NE_Venta.Fecha.Year == Anio && x.NE_Venta.Fecha.Month == item.Key && x.VehiculoId != null).Select(x => x.Cantidad).Sum() : 0;
                model.Vehiculos.Add(NumeroVehiculos);
            }

            model.VehiculosJson= JsonConvert.SerializeObject(model.Vehiculos);
            model.ArticulosJson=JsonConvert.SerializeObject(model.Articulos);
            model.MesesJson=JsonConvert.SerializeObject(model.Meses);

            return PartialView(model);
        }

        #endregion

        #region Productos mas vendidos

        public PartialViewResult MostrarProductosMasVendidosAnuales()
        {
            List<Int32> Anios = new List<int>();
            List<SelectListItem> fechas = new List<SelectListItem>();

            //Obtener la lista de los anios con ventas
            Anios = db.NE_Venta.Select(x => x.Fecha.Year).Distinct().ToList();
            if (!Anios.Contains(DateTime.Now.Year))
                Anios.Add(DateTime.Now.Year);

            //recorrer por anios
            foreach (var item in Anios)
            {
                fechas.Add(new SelectListItem()
                {
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            }

            ViewBag.Fechas = fechas;

            return PartialView();
        }

        public PartialViewResult MostrarProductosMasVendidosDetalle(Int32 Anio)
        {
            Models.LoMasVendidoModel model = new Models.LoMasVendidoModel();
            Int32 Cantidad = 0;

            foreach (var item in db.NE_Producto)
            {
                if (db.NE_VentaDetalle.Any(x => x.ProductoId == item.ProductoId && x.NE_Venta.Fecha.Year == Anio))
                {
                    Cantidad = db.NE_VentaDetalle.Where(x => x.ProductoId == item.ProductoId && x.NE_Venta.Fecha.Year == Anio).Sum(x=>x.Cantidad);
                    model.ListModel.Add(new Models.VentasGraficoModel() {
                        Cantidad= Cantidad,
                        Objeto= item.Nombre 
                    });
                }
            }

            foreach (var item in db.NE_Vehiculo)
            {
                if (db.NE_VentaDetalle.Any(x => x.VehiculoId == item.VehiculoId && x.NE_Venta.Fecha.Year == Anio))
                {
                    Cantidad = db.NE_VentaDetalle.Where(x => x.VehiculoId == item.VehiculoId && x.NE_Venta.Fecha.Year == Anio).Sum(x => x.Cantidad);
                    model.ListModel.Add(new Models.VentasGraficoModel()
                    {
                        Cantidad = Cantidad,
                        Objeto = item.NombreVehiculo
                    });
                }
            }

            model.ListModel = model.ListModel.OrderByDescending(x => x.Cantidad).Take(5).ToList();

            Int32 i = 0;
            foreach (var item in model.ListModel)
            {
                model.Cantidad.Add(item.Cantidad);
                model.ArticuloVehiculo.Add(item.Objeto);
                var random = new Random();
                if(i==0)
                    model.Colores.Add("#6807f9");
                if (i == 1)
                    model.Colores.Add("#9852f9");
                if (i == 2)
                    model.Colores.Add("#c299fc");
                if (i == 3)
                    model.Colores.Add("#ffd739");
                if (i == 4)
                    model.Colores.Add("#293462");
                if (i == 5)
                    model.Colores.Add("#00818a");


                i++;
            }

            model.CantidadJson = JsonConvert.SerializeObject(model.Cantidad);
            model.ArticuloVehiculoJson = JsonConvert.SerializeObject(model.ArticuloVehiculo);
            model.ColoresJson= JsonConvert.SerializeObject(model.Colores);

            return PartialView(model);
        }

        #endregion

        #region Los mas buscados

        public PartialViewResult LosMasBuscados()
        {
            List<SelectListItem> Anios = new List<SelectListItem>();
            List<SelectListItem> Meses = new List<SelectListItem>();
            List<Int32> _Anios = new List<int>();

            _Anios = db.NE_Venta.Select(x => x.Fecha.Year).Distinct().ToList();
            if (!_Anios.Contains(DateTime.Now.Year))
                _Anios.Add(DateTime.Now.Year);

            Anios = _Anios.Select(x => new SelectListItem() { Text = x.ToString(), Value = x.ToString() }).ToList();

            foreach (var item in Settings.MESES)
                Meses.Add(new SelectListItem()
                {
                    Text = item.Value,
                    Value = item.Key.ToString(),
                    Selected = item.Key == DateTime.Now.Month
                });

            ViewBag.Anios = Anios;
            ViewBag.Meses = Meses;

            return PartialView();
        }

        public PartialViewResult LosMasBuscadosDetalle(int Mes, int Anio) {
            List<Models.VentasGraficoModel> listModel = new List<Models.VentasGraficoModel>();
            Int32 contador = 0;

            foreach (var item in db.NE_Producto)
            {
                contador = db.NE_ArticuloVehiculoVisto.Any(x => x.ID_Producto == item.ProductoId && x.RecordDate.Year==Anio && x.RecordDate.Month== Mes) ?
                    db.NE_ArticuloVehiculoVisto.Where(x => x.ID_Producto == item.ProductoId && x.RecordDate.Year == Anio && x.RecordDate.Month == Mes).Count() : 0;
                if (contador > 0)
                {
                    listModel.Add(new Models.VentasGraficoModel() {
                        Cantidad= contador,
                        Objeto= item.Nombre,
                    });
                }
            }

            foreach (var item in db.NE_Vehiculo)
            {
                contador = db.NE_ArticuloVehiculoVisto.Any(x => x.ID_Vehiculo == item.VehiculoId && x.RecordDate.Year == Anio && x.RecordDate.Month == Mes) ?
                    db.NE_ArticuloVehiculoVisto.Where(x => x.ID_Vehiculo == item.VehiculoId && x.RecordDate.Year == Anio && x.RecordDate.Month == Mes).Count() : 0;
                if (contador > 0)
                {
                    listModel.Add(new Models.VentasGraficoModel()
                    {
                        Cantidad = contador,
                        Objeto = item.NombreVehiculo,
                    });
                }
            }

            listModel = listModel.OrderByDescending(x => x.Cantidad).Take(6).ToList();
            contador = listModel.Sum(x => x.Cantidad);

            foreach (var item in listModel)
                item.Porcentaje = (item.Cantidad * 100) / contador;

            listModel = DesordenarLista(listModel);

            return PartialView(listModel);
        }

        #endregion

        #region Clientes Parcial

        public PartialViewResult ClientesParcial() {

            List<Models.NE_Usuario> clientes = db.NE_Usuario.Where(x => x.RolId == 4).ToList();
        
            foreach(var item in clientes)
            {
                item.NumeroDeIngresosAlPortal = db.NE_Bitacora.Where(x => x.AccionId == ACCION.NUEVO_REGISTRO && x.UsuarioId== item.UsuarioId).Count();
                item.NumeroDeComprasRealizadas=  db.NE_Bitacora.Where(x => x.AccionId == ACCION.COMPRA && x.UsuarioId == item.UsuarioId).Count();
                item.TotalDeCompras = db.NE_Venta.Any(x => x.UsuarioId == item.UsuarioId) ? db.NE_Venta.Where(x => x.UsuarioId == item.UsuarioId).Sum(x=>x.TotalVenta) :0;
                item.NumeroDeArticulosEnElCarro = db.NE_Carrito.Where(x => x.UsuarioId == item.UsuarioId).Count();
                item.NumeroDeArticulosEnListaDeDeseos= db.NE_ListaDeDeseos.Where(x => x.UsuarioId == item.UsuarioId).Count();
                item.UltimoInicioDeSesion = db.NE_Autenticacion.Any(x => x.UsuarioId == item.UsuarioId) ? db.NE_Autenticacion.Where(x => x.UsuarioId == item.UsuarioId).First().UltimoInicioSesion :default(DateTime) ;
            }

            return PartialView(clientes);
        }


        #endregion
    }
}