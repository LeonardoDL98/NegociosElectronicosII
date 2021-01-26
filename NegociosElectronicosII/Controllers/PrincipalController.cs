using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.Models;
using CastanionUtils.Email;

namespace NegociosElectronicosII.Controllers
{
    public class PrincipalController : BaseController
    {
        /// <summary>
        /// Pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Vista del carrusel
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CarruselParcial()
        {
            List<OfertaModel> ofertas = new List<OfertaModel>();
            List<String> LosMasBuscadosAux = new List<string>();
            List<String> LosMasBuscados = new List<string>();
            List<LosMasBuscados> AgrupamientoLosMasBuscados = new List<LosMasBuscados>();
            List<BusquedaModel> busquedas = new List<BusquedaModel>();

            String Aux = String.Empty;

            //se obtienen las imagenes del carrusel
            List<NE_Carrusel> ImagenesCarrusel = db.NE_Carrusel.OrderBy(x => x.Posicion).ToList();
            try
            {
                //obtener los mas buscados
                LosMasBuscados= db.NE_Bitacora.Where(x => x.AccionId == ACCION.DETALLES_PRODUCTO).Select(x => x.Descripcion).ToList();
                foreach (var item in LosMasBuscados)
                {        
                    Int32 inicioDeCadena = item.IndexOf("'");
                    Aux = item.Substring(inicioDeCadena, item.Length-inicioDeCadena);
                    Aux = Aux.Replace("'", "").Replace("'", "");
                    LosMasBuscadosAux.Add(Aux);
                }
                AgrupamientoLosMasBuscados = LosMasBuscadosAux.GroupBy(x => x)
                    .Select(x => new LosMasBuscados()
                    {
                        Busqueda = x.Key,
                        CantidadDeVeces = x.Count()
                    }).ToList().OrderByDescending(x => x.CantidadDeVeces).ToList();

                LosMasBuscados = AgrupamientoLosMasBuscados.Select(x => x.Busqueda).Take(5).ToList();

                List<NE_Vehiculo> vehiculos = db.NE_Vehiculo.Where(x => LosMasBuscados.Contains(x.NE_Marca.Marca + " "+ x.NombreVehiculo + " " + x.Modelo.ToString() )).ToList();
                List<NE_Producto> productos = db.NE_Producto.Where(x => LosMasBuscados.Contains(x.NE_Marca.Marca + " " + x.Nombre)).ToList();

                foreach (var item in vehiculos)
                {
                    busquedas.Add(new BusquedaModel() {
                        Productos= null,
                        TipoDeBusqueda= 1,
                        Vehiculos= item
                    });
                }

                foreach (var item in productos)
                {
                    busquedas.Add(new BusquedaModel()
                    {
                        Productos = item,
                        TipoDeBusqueda = 2,
                        Vehiculos = null
                    });
                }

                //si hay objetos con oferta, lo agregamos a la lista
                if (db.NE_Producto.Any(x => x.MarcarComoOferta))
                {
                    List<OfertaModel> ofertaProducto = db.NE_Producto.Where(x => x.MarcarComoOferta).Select(x => new OfertaModel()
                    {
                        IsProduct = true,
                        Text = x.Nombre,
                        ID= x.ProductoId
                    }).ToList();

                    foreach (var item in ofertaProducto) 
                        if (db.NE_ProductoImagen.Any(x => x.ProductoId == item.ID))
                            item.ImagenesProducto.AddRange(db.NE_ProductoImagen.Where(x => x.ProductoId == item.ID));
                    
                    ofertas.AddRange(ofertaProducto);
                }

                //si hay autos con oferta, lo agregamos a la lista
                if (db.NE_Vehiculo.Any(x => x.MarcarComoOferta))
                {
                    List<OfertaModel> ofertaVehiculo = db.NE_Vehiculo.Where(x => x.MarcarComoOferta).Select(x => new OfertaModel()
                    {
                        IsProduct = true,
                        Text = x.NombreVehiculo + " " + x.Descripcion,
                        ID= x.VehiculoId
                    }).ToList();

                    foreach (var item in ofertaVehiculo)
                        if (db.NE_VehiculoImagen.Any(x => x.VehiculoId == item.ID))
                            item.ImagenesVehiculo.AddRange(db.NE_VehiculoImagen.Where(x => x.VehiculoId == item.ID));

                    ofertas.AddRange(ofertaVehiculo);
                }


                //ordenar aleatoriamente en el arreglo
                ofertas = DesordenarLista(ofertas);

                //obtener la listas de marcas disponibles
                ViewBag.Marcas = db.NE_Marca.ToList();
                //agregar ofertas a ViewBag
                ViewBag.Ofertas = ofertas.Take(4).ToList();
                //Lista de los mas buscados
                ViewBag.LosMasBuscados = busquedas;

                return PartialView(ImagenesCarrusel);
            }
            catch (Exception ex)
            {
                return PartialView(ImagenesCarrusel);
            }
        }

        public PartialViewResult ListamasBuscados()
        {
            ViewBag.Marcas = db.NE_Marca.ToList();
            return PartialView();
        }

        [HttpPost]
        public JsonResult EnviarComentarios(string email, string comentario) {
            String Message = String.Empty;
            //fill template
            String template = db.NE_EmailTemplate.Where(x => x.Name == "Comments").First().EmailTemplate;

            foreach (var item in db.NE_Usuario.Where(x => x.RolId == 1))
            {
                template = String.Format(template, item.Nombre + " " + item.ApellidoPaterno ,email,comentario , "carsold22141024@gmail.com");
                //create Instance
                Mail mail = new Mail()
                {
                    AccountServer = Settings.ACCOUNT_SERVER,
                    Subject = "Nuevo comentario",
                    From = Settings.FROM,
                    Host = Settings.HOST_SERVER,
                    PasswordServer = Settings.PASSWORD_SERVER,
                    Body = template,
                    To = new List<string>() { item.CorreoElectronico },
                    Port = Settings.PORT_SERVER
                };
                mail.Send();
            }

            Message = "Gracias por enviar tu comentario, para nuestro equipo es importante tu opinion. Por lo cual atenderemos tu solicitud, y garantizamos que recibiras una respuesta dentro de las proximas 24 horas.";
            return Json(new { Message= Message },JsonRequestBehavior.DenyGet);
        }

        #region Vehiculos

        /// <summary>
        /// Vista parcial del vehiculo
        /// </summary>
        /// <returns></returns>
        public PartialViewResult VehiculoParcial()
        {
            return PartialView();
        }

        /// <summary>
        /// Vista del fiiltro del vehiculo
        /// </summary>
        /// <returns></returns>
        public PartialViewResult FiltroVehiculoParcial(Int32? ID_Categoria, Int32? ID_Marca)
        {
            ViewBag.Marcas = db.NE_Marca.ToList();
            ViewBag.Color = db.NE_Color.ToList();
            ViewBag.Categoria = db.NE_Categoria.ToList();
            ViewBag.Transmision = db.NE_Transmision.ToList();
            ViewBag.ID_Categoria = ID_Categoria;
            ViewBag.ID_Marca = ID_Marca;

            List<NE_VehiculoImagen> ImagenesVehiculo = db.NE_VehiculoImagen.ToList();
            return PartialView(ImagenesVehiculo);
        }

        public PartialViewResult GridVehiculoParcial(Int32 Pagina,String Marcas,String Color, String Categoria, String Transmision, Decimal PrecioInicial, Decimal PrecioFinal, String Busqueda=null)
        {
            List<NE_VehiculoImagen> ImagenesVehiculo = new List<NE_VehiculoImagen>();
            try
            {
                //Se crean listas vacias en donde vaciaremos los strings de la vista
                List<Int32> Ids_Marcas = new List<int>();
                List<Int32> Ids_Color = new List<int>();
                List<Int32> Ids_Categorias = new List<int>();
                List<Int32> Ids_Transmision = new List<int>();

                //obtener precio inicial y precio final max y min
                Decimal Max = db.NE_Vehiculo.Max(x => x.MarcarComoOferta ? x.PrecioOFerta : x.PrecioVenta);
                Decimal Min = db.NE_Vehiculo.Min(x => x.MarcarComoOferta ? x.PrecioOFerta : x.PrecioVenta);

                //si la variable marcas viene como null o vacio entonces toma el arreglo del catalogo
                if (!String.IsNullOrEmpty(Marcas))
                    Ids_Marcas = Marcas.Split(',').ToList().Select(Int32.Parse).ToList();
                else
                    Ids_Marcas = db.NE_Marca.Select(x => x.MarcaId).ToList();

                //si la variable color viene como null o vacio entonces toma el arreglo del catalogo
                if (!String.IsNullOrEmpty(Color))
                    Ids_Color = Color.Split(',').ToList().Select(Int32.Parse).ToList();
                else
                    Ids_Color = db.NE_Color.Select(x => x.ColorId).ToList();

                //si la variable categorias viene como null o vacio entonces toma el arreglo del catalogo
                if (!String.IsNullOrEmpty(Categoria))
                    Ids_Categorias = Categoria.Split(',').ToList().Select(Int32.Parse).ToList();
                else
                    Ids_Categorias = db.NE_Categoria.Select(x => x.CategoriaId).ToList();

                //si la variable categorias viene como null o vacio entonces toma el arreglo del catalogo
                if (!String.IsNullOrEmpty(Transmision))
                    Ids_Transmision = Transmision.Split(',').ToList().Select(Int32.Parse).ToList();
                else
                    Ids_Transmision = db.NE_Transmision.Select(x => x.TransmisionId).ToList();


                //obtenemos el filtro de los vehiculos seleccionados y lo convertimos a lista
                if (String.IsNullOrEmpty(Busqueda))
                    ImagenesVehiculo = db.NE_VehiculoImagen
                        .Where(x =>
                            x.NE_Vehiculo.Activo
                            && Ids_Marcas.Contains(x.NE_Vehiculo.MarcaId)
                            && Ids_Color.Contains(x.NE_Vehiculo.ColorId)
                            && Ids_Categorias.Contains(x.NE_Vehiculo.CategoriaId)
                            && Ids_Transmision.Contains(x.NE_Vehiculo.TransmisionId)
                            && (
                                    (x.NE_Vehiculo.MarcarComoOferta ? x.NE_Vehiculo.PrecioOFerta : x.NE_Vehiculo.PrecioVenta) > (PrecioInicial < 0 ? Min : PrecioInicial)
                                    &&
                                    (x.NE_Vehiculo.MarcarComoOferta ? x.NE_Vehiculo.PrecioOFerta : x.NE_Vehiculo.PrecioVenta) < (PrecioFinal <= 0 ? Max : PrecioFinal)
                                )
                        ).Distinct()
                        .ToList();
                else
                {
                    ImagenesVehiculo = db.NE_VehiculoImagen.Where(x =>
                        (x.NE_Vehiculo.NE_Categoria.Categoria.Contains(Busqueda) ||
                        x.NE_Vehiculo.NombreVehiculo.Contains(Busqueda)||
                        x.NE_Vehiculo.NE_Color.Color.Contains(Busqueda) ||
                        x.NE_Vehiculo.Descripcion.Contains(Busqueda) ||
                        x.NE_Vehiculo.NE_Marca.Marca.Contains(Busqueda) ||
                        x.NE_Vehiculo.Modelo.ToString().Contains(Busqueda) ||
                        x.NE_Vehiculo.NE_Transmision.Transmision.ToString().Contains(Busqueda) ||
                       (x.NE_Vehiculo.MarcarComoOferta ? x.NE_Vehiculo.PrecioOFerta.ToString() : x.NE_Vehiculo.PrecioVenta.ToString()).Contains(Busqueda))
                       && x.NE_Vehiculo.Activo
                    ).ToList();
                    //NE_Bitacora bitacora = new NE_Bitacora()
                    //{
                    //    AccionId = ACCION.BUSQUEDA,
                    //    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Busco en la página '" + Busqueda + "'",
                    //    FechaDeRegistro = DateTime.Now,
                    //    UsuarioId = Settings.LoggedUser.UsuarioId,
                    //};
                    //db.NE_Bitacora.Add(bitacora);
                    //db.SaveChanges();
                }

                //Distinct


                //Se obtiene el total de los vehiculos
                Int32 TotalDeVehiculos = ImagenesVehiculo.Select(x => x.VehiculoId).Distinct().Count();
                //Se obtiene el total de paginas a pintar
                Int32 NumeroDePaginas = (TotalDeVehiculos / Settings.NUMERO_DE_ITEMS_POR_PAGINA) + 1;
                //Se obtiene el registro del primer salto de pagina
                Int32 skip = ((Pagina - 1) * Settings.NUMERO_DE_ITEMS_POR_PAGINA);

                //Se filtra por el paginado de la pagina
                ImagenesVehiculo = ImagenesVehiculo.Skip(skip).Take(Settings.NUMERO_DE_ITEMS_POR_PAGINA).ToList();

                //Se pintan en los Viebags los montos, para usarlos en la vista
                ViewBag.TotalDeVehiculos = TotalDeVehiculos;
                ViewBag.NumeroDePaginas = NumeroDePaginas;
                ViewBag.PaginaActual = Pagina;
            }
            catch (Exception ex)
            {
            }
            return PartialView(ImagenesVehiculo);
        }

        #endregion

        #region Articulos

        public PartialViewResult ArticuloParcial()
        {
            return PartialView();
        }

        public PartialViewResult FiltroArticuloParcial()
        {
            ViewBag.Marcas = db.NE_Marca.ToList();
            ViewBag.Color = db.NE_Color.ToList();; 
            List<NE_ProductoImagen> ImagenesVehiculo = db.NE_ProductoImagen.ToList();
            return PartialView(ImagenesVehiculo);
        }

        public PartialViewResult GridArticuloParcial(Int32 Pagina, String Marcas, String Color, Decimal PrecioInicial, Decimal PrecioFinal, String Busqueda = null)
        {
            List<NE_ProductoImagen> ImagenesArticulo = new List<NE_ProductoImagen>();

            //Se crean listas vacias en donde vaciaremos los strings de la vista
            List<Int32> Ids_Marcas = new List<int>();
            List<Int32> Ids_Color = new List<int>();

            //obtener precio inicial y precio final max y min
            Decimal Max = db.NE_Producto.Max(x => x.MarcarComoOferta ? x.PrecioOFerta : x.PrecioVenta);
            Decimal Min = db.NE_Producto.Min(x => x.MarcarComoOferta ? x.PrecioOFerta : x.PrecioVenta);

            //si la variable marcas viene como null o vacio entonces toma el arreglo del catalogo
            if (!String.IsNullOrEmpty(Marcas))
                Ids_Marcas = Marcas.Split(',').ToList().Select(Int32.Parse).ToList();
            else
                Ids_Marcas = db.NE_Marca.Select(x => x.MarcaId).ToList();

            //si la variable color viene como null o vacio entonces toma el arreglo del catalogo
            if (!String.IsNullOrEmpty(Color))
                Ids_Color = Color.Split(',').ToList().Select(Int32.Parse).ToList();
            else
                Ids_Color = db.NE_Color.Select(x => x.ColorId).ToList();

            //obtenemos el filtro de los Articuloss seleccionados y lo convertimos a lista
            if (String.IsNullOrEmpty(Busqueda))
                ImagenesArticulo = db.NE_ProductoImagen
                .Where(x =>
                    x.NE_Producto.Activo
                    && Ids_Marcas.Contains(x.NE_Producto.MarcaId)
                    && Ids_Color.Contains(x.NE_Producto.ColorId)
                    && (
                            (x.NE_Producto.MarcarComoOferta ? x.NE_Producto.PrecioOFerta : x.NE_Producto.PrecioVenta) > (PrecioInicial < 0 ? Min : PrecioInicial)
                            &&
                            (x.NE_Producto.MarcarComoOferta ? x.NE_Producto.PrecioOFerta : x.NE_Producto.PrecioVenta) < (PrecioFinal <= 0 ? Max : PrecioFinal)
                        )
                )
                .ToList();
            else
                ImagenesArticulo = db.NE_ProductoImagen.Where(x =>
                    (x.NE_Producto.NE_Color.Color.Contains(Busqueda) ||
                    x.NE_Producto.Nombre.Contains(Busqueda)||
                    x.NE_Producto.Descripcion.Contains(Busqueda) ||
                    x.NE_Producto.NE_Marca.Marca.Contains(Busqueda) ||
                   (x.NE_Producto.MarcarComoOferta ? x.NE_Producto.PrecioOFerta.ToString() : x.NE_Producto.PrecioVenta.ToString()).Contains(Busqueda)
                   )
                   && x.NE_Producto.Activo
                ).ToList();

            //Se obtiene el total de los vehiculos
            Int32 TotalDeArticulos = ImagenesArticulo.Select(x=>x.ProductoId).Distinct().Count();
            //Se obtiene el total de paginas a pintar
            Int32 NumeroDePaginas = (TotalDeArticulos / Settings.NUMERO_DE_ITEMS_POR_PAGINA) + 1;
            //Se obtiene el registro del primer salto de pagina
            Int32 skip = ((Pagina - 1) * Settings.NUMERO_DE_ITEMS_POR_PAGINA);

            //Se filtra por el paginado de la pagina
            ImagenesArticulo= ImagenesArticulo.Skip(skip).Take(Settings.NUMERO_DE_ITEMS_POR_PAGINA).ToList();

            //Se pintan en los Viebags los montos, para usarlos en la vista
            ViewBag.TotalDeArticulos = TotalDeArticulos;
            ViewBag.NumeroDePaginas = NumeroDePaginas;
            ViewBag.PaginaActual = Pagina;
            return PartialView(ImagenesArticulo);
            //List<NE_ProductoImagen> ImagenesVehiculo = db.NE_ProductoImagen.ToList();
            //return PartialView(ImagenesVehiculo);
        }

        public ActionResult ListaDeDeseos() {
            if (Settings.LoggedUser != null)
            {
                List<NE_ListaDeDeseos> deseado = db.NE_ListaDeDeseos.Where(x => x.UsuarioId == Settings.LoggedUser.UsuarioId).ToList();
                return View(deseado);
            }
            else
                return View();
        }

        public ActionResult CarritoDeCompras() {
            List<NE_Carrito> carro = new List<NE_Carrito>();
            Decimal TotalSinDescuentosCarro = 0, TotalConDescuentosCarro = 0, DiferenciaCarro = 0, TotalCarro = 0,
                TotalSinDescuentosArticulos = 0, TotalConDescuentosArticulos = 0, DiferenciaArticulos = 0, TotalArticulos = 0;

            if (Settings.LoggedUser != null)
            {
                carro = db.NE_Carrito.Where(x => x.UsuarioId == Settings.LoggedUser.UsuarioId).ToList();
                TotalSinDescuentosArticulos = carro.Where(x => x.ProductoId != null).Sum(x => x.NE_Producto.PrecioVenta);
                TotalSinDescuentosCarro = carro.Where(x => x.VehiculoId != null).Sum(x => x.NE_Vehiculo.PrecioVenta);
                TotalConDescuentosArticulos = carro.Where(x => x.ProductoId != null).Sum(x => x.NE_Producto.MarcarComoOferta ? x.NE_Producto.PrecioOFerta : x.NE_Producto.PrecioVenta);
                TotalConDescuentosCarro = carro.Where(x => x.VehiculoId != null).Sum(x => x.NE_Vehiculo.MarcarComoOferta ? x.NE_Vehiculo.PrecioOFerta : x.NE_Vehiculo.PrecioVenta);

            }

            ViewBag.TotalSinDescuentos = TotalSinDescuentosArticulos + TotalSinDescuentosCarro;
            ViewBag.TotalConDescuentos = TotalConDescuentosArticulos + TotalConDescuentosCarro;
            ViewBag.Descuento = ViewBag.TotalSinDescuentos - ViewBag.TotalConDescuentos;
            

            return View(carro);
        }
        public ActionResult Pedidos()
        {
            List<NE_VentaDetalle> venta = new List<NE_VentaDetalle>();
            NE_Venta nE_venta = db.NE_Venta.Where(x => x.UsuarioId == Settings.LoggedUser.UsuarioId).First();
            venta = db.NE_VentaDetalle.Where(x => x.VentaId == nE_venta.VentaId).ToList();
            return View(venta);
        }

            public ActionResult FaqsPrincipal()
        {
            return View(db.NE_FAQS.ToList());
        }

        #endregion
    }
}