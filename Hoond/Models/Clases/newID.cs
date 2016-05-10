using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hoond.Models
{
    public class newID
    {
        public decimal CalculaId(Int32 tipo = 1)
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
    }
}