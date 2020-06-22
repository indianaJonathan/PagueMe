using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MePague.Models
{
    [Table("dependentes")]
    public class Dependente
    {
        [Key]
        public int id { get; set; }
        public int idDependente { get; set; }
        [Display(Name = "Dependente")]
        public Cliente dependente { get; set; }
        [Display(Name = "Valor")]
        public double valor { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }

        public Dependente()
        {
            dependente = new Cliente();
            valor = 0;
            status = "Pendente";
        }
    }
}