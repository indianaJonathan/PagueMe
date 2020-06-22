using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MePague.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Nome do usuário é obrigatório!")]
        [Display(Name = "Nome de usuário")]
        public string nome { get; set; }
        [Required(ErrorMessage = "Senha é obrigatório!")]
        [Display(Name = "Senha")]
        public string senha { get; set; }
        public Cliente cliente { get; set; }
        [Display(Name = "Tipo")]
        [StringLength(1, ErrorMessage = "Categoria inválida!")]
        public string tipo { get; set; }
    }
}