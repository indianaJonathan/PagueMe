using MePague.Config;
using MePague.Models;
using MePague.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace MePague.DAO
{
    public class AmizadeDAO
    {
        static Context ctx = SingletonContext.GetInstance();

        public static void Add(Amizade a)
        {
            ctx.amizades.Add(a);
            ctx.SaveChanges();
        }

        public static List<Amizade> getAmizades()
        {
            return ctx.amizades.ToList();
        }

        public static Amizade getAmizade(int id)
        {
            return ctx.amizades.FirstOrDefault(x => x.id == id);
        }

        public static List<Amizade> searchAmizadesAceitasPorCliente(int id)
        {
            List<Amizade> amizades = ctx.amizades.Where(x => x.solicitado.id == id && x.status.Equals(StatusSolicitacao.Aceito)).ToList();

            return amizades;
        }

        public static List<Amizade> searchAmizadesSolicitadasAceitasPorCliente(int id)
        {
            List<Amizade> amizades = ctx.amizades.Where(x => x.solicitante.id == id && x.status.Equals(StatusSolicitacao.Aceito)).ToList();

            return amizades;
        }

        public static List<Amizade> searchAmizadesSolicitadasPendentesPorCliente(int id)
        {
            List<Amizade> amizades = ctx.amizades.Where(x => x.solicitante.id == id && x.status.Equals(StatusSolicitacao.Pendente)).ToList();

            return amizades;
        }

        public static List<Amizade> searchAmizadesPendentesPorCliente(int id)
        {
            List<Amizade> amizades = ctx.amizades.Where(x => x.solicitado.id == id && x.status.Equals(StatusSolicitacao.Pendente)).ToList();

            return amizades;
        }

        public static void atualizarAmizade(Amizade a)
        {
            ctx.amizades.AddOrUpdate(a);
            ctx.SaveChanges();
        }

        public static void desfazerAmizade(Amizade a)
        {
            ctx.amizades.Remove(a);
            ctx.SaveChanges();
        }
    }
}