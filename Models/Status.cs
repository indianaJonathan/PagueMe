using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MePague.Models
{
    [Table("status")]
    public class Status
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Sigla")]
        public string sigla { get; set; }
        [Display(Name = "Status")]
        public string nome { get; set; }
    }
}