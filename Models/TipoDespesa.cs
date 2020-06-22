using MePague.Utils;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MePague.Models
{
    public class TipoDespesa
    {
        public string nome { get; set; }
        public string sigla { get; set; }
        public TipoDespesa(string a)
        {
            if (a.Equals(MePague.Utils.TipoDespesa.Anual))
            {
                sigla = a;
                nome = "Anual";
            }
            else if(a.Equals(MePague.Utils.TipoDespesa.Mensal))
            {
                sigla = a;
                nome = "Mensal";
            }
            else if (a.Equals(MePague.Utils.TipoDespesa.Semanal))
            {
                sigla = a;
                nome = "Semanal";
            }
            else if (a.Equals(MePague.Utils.TipoDespesa.CobrancaUnica))
            {
                sigla = a;
                nome = "Cobrança única";
            }
        }
    }
}