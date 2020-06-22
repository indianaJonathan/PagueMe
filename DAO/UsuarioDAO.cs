using MePague.Config;
using MePague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Web;
using System.Web.WebPages.Html;

namespace MePague.DAO
{
    public class UsuarioDAO
    {
        static Context ctx = SingletonContext.GetInstance();
        public static bool Add(Usuario u)
        {
            try
            {
                ctx.clientes.Add(u.cliente);
                ctx.usuarios.Add(u);
                ctx.SaveChanges();
                return true;
            } catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Usuario> getUsuarios()
        {
            List<Usuario> usuarios = ctx.usuarios.ToList();
            return usuarios;
        }

        public static Usuario getUsuario(int id)
        {
            return ctx.usuarios.FirstOrDefault(x => x.id == id);
        }

        public static bool updateUsuario(Usuario u)
        {
            try
            {
                Usuario a = ctx.usuarios.FirstOrDefault(x => x.id == u.id);
                a = u;
                ctx.SaveChanges();
                return true;
            } catch (Exception e)
            {
                throw e;
            }
        }

        public static bool deleteUsuario(int id)
        {
            try
            {
                Usuario u = getUsuario(id);
                ctx.usuarios.Remove(u);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Usuario searchUsuarioPorNome(string nome)
        {
            Usuario u = ctx.usuarios.FirstOrDefault(x => x.nome.Equals(nome));
            return u;
        }

        public static Usuario searchUsuarioPorCliente(int id)
        {
            return ctx.usuarios.FirstOrDefault(x => x.cliente.id == id);
        }

        public static List<Status> status()
        {
            List<Status> status = ctx.status.ToList();
            return status;
        }
    }
}