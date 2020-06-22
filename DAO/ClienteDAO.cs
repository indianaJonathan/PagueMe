using MePague.Config;
using MePague.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MePague.DAO
{
    public class ClienteDAO
    {
        static Context ctx = SingletonContext.GetInstance();
        public static List<Cliente> getClientes()
        {
            List<Cliente> clientes = ctx.clientes.ToList();
            return clientes;
        }

        public static Cliente getCliente(int id)
        {
            Cliente cliente = ctx.clientes.FirstOrDefault(x => x.id == id);
            return cliente;
        }

        public static List<Cliente> searchClientePorNome(string nome)
        {
            return ctx.clientes.Where(x => x.nome.Contains(nome)).ToList();
        }

        public static List<Usuario> searchClientePorUsuario(string usuario)
        {
            return ctx.usuarios.Where(x => x.nome.Contains(usuario)).ToList();
        }

        public static bool updateCliente(Cliente c)
        {
            try
            {
                Cliente a = ctx.clientes.FirstOrDefault(x => x.id == c.id);
                a = c;
                ctx.SaveChanges();
                return true;
            } catch (Exception e)
            {
                throw e;
            }
        }

        public static bool deleteCliente(int id)
        {
            try
            {
                Cliente c = getCliente(id);
                ctx.clientes.Remove(c);
                ctx.SaveChanges();
                return true;
            } catch (Exception e)
            {
                throw e;
            }
        }

        public static bool addCliente(Cliente c)
        {
            try
            {
                ctx.clientes.Add(c);
                ctx.SaveChanges();
                return true;
            } catch (Exception e)
            {
                throw e;
            }
        }
    }
}