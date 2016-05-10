using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hoond
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            Random rnd = new Random();
            string val1;
            string val2;

            #region Routes

            //----->       Empresa      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaEmpresa",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Empresa", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaEmpresa",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Empresa", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaEmpresa",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Empresa", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Icono      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaIcono",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Icono", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaIcono",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Icono", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaIcono",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Icono", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Menu      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaMenu",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Menu", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaMenu",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Menu", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaMenu",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Menu", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Rol      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaRol",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Rol", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaRol",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Rol", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaRol",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Rol", action = "Details", id = UrlParameter.Optional }
            );


            //----->       RolMenu      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaRolMenu",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "RolMenu", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaRolMenu",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "RolMenu", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaRolMenu",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "RolMenu", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Usuario      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaUsuario",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Usuario", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaUsuario",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Usuario", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaUsuario",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Usuario", action = "Details", id = UrlParameter.Optional }
            );


            //----->       UsuarioProvider      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaUsuarioProvider",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioProvider", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaUsuarioProvider",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioProvider", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaUsuarioProvider",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioProvider", action = "Details", id = UrlParameter.Optional }
            );


            //----->       UsuarioRol      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaUsuarioRol",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioRol", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaUsuarioRol",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioRol", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaUsuarioRol",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioRol", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Vista      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaVista",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Vista", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaVista",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Vista", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaVista",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Vista", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Almacen      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaAlmacen",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Almacen", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaAlmacen",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Almacen", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaAlmacen",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Almacen", action = "Details", id = UrlParameter.Optional }
            );


            //----->       AlmacenUbicacion      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaAlmacenUbicacion",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "AlmacenUbicacion", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaAlmacenUbicacion",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "AlmacenUbicacion", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaAlmacenUbicacion",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "AlmacenUbicacion", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Articulo      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaArticulo",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Articulo", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaArticulo",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Articulo", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaArticulo",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Articulo", action = "Details", id = UrlParameter.Optional }
            );


            //----->       ArticuloCaracteristica      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaArticuloCaracteristica",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "ArticuloCaracteristica", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaArticuloCaracteristica",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "ArticuloCaracteristica", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaArticuloCaracteristica",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "ArticuloCaracteristica", action = "Details", id = UrlParameter.Optional }
            );


            //----->       ArticuloPrecio      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaArticuloPrecio",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "ArticuloPrecio", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaArticuloPrecio",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "ArticuloPrecio", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaArticuloPrecio",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "ArticuloPrecio", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Caracteristica      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaCaracteristica",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Caracteristica", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaCaracteristica",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Caracteristica", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaCaracteristica",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Caracteristica", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Categoria      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaCategoria",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Categoria", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaCategoria",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Categoria", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaCategoria",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Categoria", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Coleccion      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaColeccion",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Coleccion", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaColeccion",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Coleccion", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaColeccion",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Coleccion", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Color      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaColor",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Color", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaColor",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Color", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaColor",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Color", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Marca      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaMarca",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Marca", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaMarca",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Marca", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaMarca",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Marca", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Mensajeria      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaMensajeria",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Mensajeria", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaMensajeria",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Mensajeria", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaMensajeria",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Mensajeria", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Presentacion      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaPresentacion",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Presentacion", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaPresentacion",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Presentacion", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaPresentacion",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Presentacion", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Proveedor      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaProveedor",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Proveedor", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaProveedor",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Proveedor", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaProveedor",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Proveedor", action = "Details", id = UrlParameter.Optional }
            );


            //----->       ProveedorContacto      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaProveedorContacto",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "ProveedorContacto", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaProveedorContacto",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "ProveedorContacto", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaProveedorContacto",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "ProveedorContacto", action = "Details", id = UrlParameter.Optional }
            );


            //----->       RazonSocial      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaRazonSocial",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "RazonSocial", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaRazonSocial",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "RazonSocial", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaRazonSocial",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "RazonSocial", action = "Details", id = UrlParameter.Optional }
            );


            //----->       SubCategoria      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaSubCategoria",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "SubCategoria", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaSubCategoria",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "SubCategoria", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaSubCategoria",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "SubCategoria", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Sucursal      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaSucursal",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Sucursal", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaSucursal",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Sucursal", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaSucursal",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Sucursal", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Talla      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTalla",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Talla", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTalla",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Talla", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTalla",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Talla", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoAlmacen      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoAlmacen",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoAlmacen", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoAlmacen",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoAlmacen", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoAlmacen",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoAlmacen", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoArticulo      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoArticulo",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoArticulo", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoArticulo",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoArticulo", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoArticulo",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoArticulo", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoArticuloCaracteristica      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoArticuloCaracteristica",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoArticuloCaracteristica", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoArticuloCaracteristica",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoArticuloCaracteristica", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoArticuloCaracteristica",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoArticuloCaracteristica", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoImporte      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoImporte",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoImporte", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoImporte",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoImporte", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoImporte",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoImporte", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoSucursal      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoSucursal",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoSucursal", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoSucursal",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoSucursal", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoSucursal",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoSucursal", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoUbicacion      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoUbicacion",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoUbicacion", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoUbicacion",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoUbicacion", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoUbicacion",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoUbicacion", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Ubicacion      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaUbicacion",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Ubicacion", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaUbicacion",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Ubicacion", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaUbicacion",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Ubicacion", action = "Details", id = UrlParameter.Optional }
            );


            //----->       UsuarioSucursal      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaUsuarioSucursal",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioSucursal", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaUsuarioSucursal",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioSucursal", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaUsuarioSucursal",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "UsuarioSucursal", action = "Details", id = UrlParameter.Optional }
            );


            //----->       ValorCaracteristica      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaValorCaracteristica",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "ValorCaracteristica", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaValorCaracteristica",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "ValorCaracteristica", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaValorCaracteristica",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "ValorCaracteristica", action = "Details", id = UrlParameter.Optional }
            );


            //----->       sysdiagrams      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "Editasysdiagrams",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "sysdiagrams", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Eliminasysdiagrams",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "sysdiagrams", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Consultasysdiagrams",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "sysdiagrams", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Divisa      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaDivisa",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Divisa", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaDivisa",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Divisa", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaDivisa",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Divisa", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Estatus      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaEstatus",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Estatus", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaEstatus",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Estatus", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaEstatus",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Estatus", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Provider      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaProvider",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Provider", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaProvider",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Provider", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaProvider",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Provider", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Ruta      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaRuta",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Ruta", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaRuta",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Ruta", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaRuta",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Ruta", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoEstatus      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoEstatus",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoEstatus", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoEstatus",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoEstatus", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoEstatus",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoEstatus", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoLogin      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoLogin",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoLogin", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoLogin",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoLogin", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoLogin",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoLogin", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoPermiso      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoPermiso",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoPermiso", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoPermiso",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoPermiso", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoPermiso",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoPermiso", action = "Details", id = UrlParameter.Optional }
            );


            //----->       TipoUsuarioSucursal      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaTipoUsuarioSucursal",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoUsuarioSucursal", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaTipoUsuarioSucursal",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoUsuarioSucursal", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaTipoUsuarioSucursal",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "TipoUsuarioSucursal", action = "Details", id = UrlParameter.Optional }
            );


            //----->       Factura      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaFactura",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "Factura", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaFactura",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "Factura", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaFactura",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "Factura", action = "Details", id = UrlParameter.Optional }
            );


            //----->       FacturaConcepto      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaFacturaConcepto",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "FacturaConcepto", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaFacturaConcepto",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "FacturaConcepto", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaFacturaConcepto",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "FacturaConcepto", action = "Details", id = UrlParameter.Optional }
            );


            //----->       LogModificacion      <------
            val1 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            val2 = String.Format("{0,15:N0}", rnd.Next()).Trim().Replace(",", "");
            routes.MapRoute(
                name: "EditaLogModificacion",
                url: "1" + val1 + "{id}" + val2,
                defaults: new { controller = "LogModificacion", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EliminaLogModificacion",
                url: "2" + val1 + "{id}" + val2,
                defaults: new { controller = "LogModificacion", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaLogModificacion",
                url: "3" + val1 + "{id}" + val2,
                defaults: new { controller = "LogModificacion", action = "Details", id = UrlParameter.Optional }
            );

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            

        }
    }
}