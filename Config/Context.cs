using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MePague.Models;

namespace MePague.Config
{
    public class Context : DbContext
    {
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Despesa> despesas { get; set; }
        public DbSet<Dependente> dependentes { get; set; }
        public DbSet<Amizade> amizades { get; set; }
        public DbSet<Status> status { get; set; }
        public DbSet<Sessoes> sessoes { get; set; }
    }
}