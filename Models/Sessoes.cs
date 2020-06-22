using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MePague.Models
{
    [Table("sessoes")]
    public class Sessoes
    {
        [Key]
        public int sessao { get; set; }
        public Usuario usuario { get; set; }
    }
}