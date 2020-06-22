using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MePague.Models
{
    [Table("amizades")]
    public class Amizade
    {
        [Key]
        public int id { get; set; }
        public Cliente solicitante { get; set; }
        public Cliente solicitado { get; set; }
        public string status { get; set; }
    }
}