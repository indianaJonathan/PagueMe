using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MePague.Models
{
    [Table("despesas")]
    public class Despesa
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Sessão expirada!")]
        [Display(Name = "Dono")]
        public Cliente dono { get; set; }
        [Required(ErrorMessage = "Nome da despesa é obrigatório")]
        [Display(Name = "Despesa")]
        public string nome { get; set; }
        [Required(ErrorMessage = "Valor é obrigatório")]
        [Display(Name = "Valor total")]
        [Range(0.01, 99999999999.00)]
        public double valor { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        public List<Dependente> dependentes { get; set; }
        [Display(Name = "Tipo")]
        public string tipo { get; set; }

        public Despesa()
        {
            dono = new Cliente();
            nome = "";
            valor = 0;
            status = "Aberto";
            dependentes = new List<Dependente>();
            tipo = "U";
        }
    }
}