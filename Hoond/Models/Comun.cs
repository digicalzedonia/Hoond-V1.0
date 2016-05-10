using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;

namespace Hoond.Models
{
    public class Comun
    {
        private HoondDBEntities db = new HoondDBEntities();
        public string getMd5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public int TipoPer(decimal idRol, string Controller, string Action)
        {
            var Query1 = (from me in db.Menu
                          join vi in db.Vista on me.idVista equals vi.idVista
                          join rm in db.RolMenu on me.idMenu equals rm.idMenu
                          where rm.activo == true && rm.idRol == idRol && vi.controller == Controller && vi.action == Action
                          select new { rm.idTipoPermiso }).ToList();
            if (Query1.Count != 0)
                return Convert.ToInt32(Query1[0].idTipoPermiso);
            else
                return 0;
        }
        public List<SelectListItem> GetCampos(string Campos, string Descripcion)
        {
            string[] campo = Campos.Split(',');
            string[] desc = Descripcion.Split(',');
            List<SelectListItem> Items = new List<SelectListItem>();
            SelectListItem Item = new SelectListItem();
            Item.Text = "--Seleccione--";
            Item.Value = "0";
            Item.Selected = true;
            Items.Add(Item);
            for (int i = 0; i < campo.Length; i++)
            {
                Item = new SelectListItem();
                Item.Text = desc[i];
                Item.Value = campo[i];
                Items.Add(Item);
            }
            return Items;
        }
        public List<SelectListItem> GetPages()
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            SelectListItem Item = new SelectListItem();
            Item.Text = "25";
            Item.Value = "25";
            Item.Selected = true;
            Items.Add(Item);
            Item = new SelectListItem();
            Item.Text = "50";
            Item.Value = "50";
            Items.Add(Item);
            Item = new SelectListItem();
            Item.Text = "75";
            Item.Value = "75";
            Items.Add(Item);
            Item = new SelectListItem();
            Item.Text = "100";
            Item.Value = "100";
            Items.Add(Item);
            Item = new SelectListItem();
            Item.Text = "Ver Todo";
            Item.Value = "0";
            Items.Add(Item);
            return Items;
        }
        public int DefaultPage()
        {
            return 25;
        }
        public decimal GetId(int tipo)
        {
            decimal resultado = 0;
            Random r = new Random(DateTime.Now.Millisecond);
            int a = r.Next(1, 9);
            switch (tipo)
            {
                case 1:
                    resultado = (Convert.ToDecimal(DateTime.Now.ToString("yyMMddHHmmssfff")) * 10) + a;
                    break;
                case 2:
                    resultado = (Convert.ToDecimal(DateTime.Now.ToString("yyMMddHHmmssfffffff")) * 10) + a;
                    break;
            }
            return resultado;
        }
        public bool CreateLog(string tabla, string accion, decimal id, decimal idUsuario)
        {
            LogModificacion Item = new LogModificacion();
            Item.accion = accion;
            Item.fecha = DateTime.Now;
            Item.id = id;
            Item.idLogModificacion = GetId(2);
            Item.idUsuario = idUsuario;
            Item.tabla = tabla;
            db.LogModificacion.Add(Item);
            db.SaveChanges();
            return true;
        }
        public static string ConvertDataTableToHTMLTable(DataTable dt)
        {
            string ret = "";
            ret = "<table id=" + (char)34 + "TableAuto" + (char)34 + ">";
            ret += "<tr>";
            foreach (DataColumn col in dt.Columns)
            {
                ret += "<td class=" + (char)34 + "tdColumnHeader" + (char)34 + ">" + col.ColumnName + "</td>";
            }
            ret += "</tr>";
            foreach (DataRow row in dt.Rows)
            {
                ret += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ret += "<td class=" + (char)34 + "tdCellData" + (char)34 + ">" + row[i].ToString() + "</td>";
                }
                ret += "</tr>";
            }
            ret += "</table>";
            return ret;
        }
        public Dictionary<string, string> ValidaSession(ref UserLogin User, string controller, string action, bool vEscritura)
        {
            Dictionary<string, string> url = new Dictionary<string, string>();
            url["Controller"] = string.Empty;
            url["Action"] = string.Empty;
            url["msgError"] = string.Empty;
            if (User == null)
            {
                url["Controller"] = "Login";
                url["Action"] = "Login";
            }
            else
            {
                if (User.idEmpresa == 0)
                {
                    url["Controller"] = "Config";
                    url["Action"] = "Index";
                }
                else
                {
                    int Per = TipoPer(User.idRol, controller, action);
                    if (Per == 0)
                    {
                        url["Controller"] = "Home";
                        url["Action"] = "Index";
                        url["msgError"] = app_GlobalResources.Content.msg_AccesoDenegado;
                    }
                    if (vEscritura)
                    {
                        if (Per == 1)
                        {
                            url["Controller"] = controller;
                            url["Action"] = "Index";
                            url["msgError"] = app_GlobalResources.Content.msg_SinPrivilegio;
                        }
                    }
                }
            }
            return url;
        }
        public Dictionary<string, string> ValidaSession(ref UserLogin User)
        {
            Dictionary<string, string> url = new Dictionary<string, string>();
            url["Controller"] = string.Empty;
            url["Action"] = string.Empty;
            url["msgError"] = string.Empty;
            if (User == null)
            {
                url["Controller"] = "Login";
                url["Action"] = "Login";
            }
            else
            {
                if (User.idEmpresa == 0)
                {
                    url["Controller"] = "Config";
                    url["Action"] = "Index";
                }
            }
            return url;
        }

    }
}