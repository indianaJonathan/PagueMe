namespace MePague.Migrations
{
    using MePague.DAO;
    using MePague.Models;
    using MePague.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MePague.Config.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MePague.Config.Context context)
        {
            Cliente c = new Cliente();
            c.id = 0001;
            c.nome = "Admin";
            c.email = "admin@pague.me";
            List<Usuario> usuarios = UsuarioDAO.getUsuarios();
            Usuario usuario = new Usuario();
            if (usuarios.Count == 0)
            {
                usuario.id = 0001;
                usuario.nome = "admin";
                usuario.senha = "admin";
                usuario.tipo = TipoUsuario.Administrador;
                usuario.cliente = c;
                context.usuarios.Add(usuario);
            }
            List<Status> statuses = context.status.ToList();
            Status status = new Status();
            if (statuses.Count == 0) {
                status.id = 0001;
                status.sigla = TipoUsuario.Administrador;
                status.nome = "Administrador";
                context.status.Add(status);
                Status status2 = new Status();
                status2.id = 0002;
                status2.sigla = TipoUsuario.UsuarioComum;
                status2.nome = "Usuario comum";
                context.status.Add(status2);
            }
            context.SaveChanges();
        }
    }
}
