using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MePague.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo nome é obrigatório!")]
        public string nome { get; set; }
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Campo e-mail é obrigatório!")]
        public string email { get; set; }
    }
}