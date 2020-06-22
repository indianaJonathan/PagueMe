using MePague.Config;
using MePague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace MePague.DAO
{
    public class DependenteDAO
    {
        static Context ctx = SingletonContext.GetInstance();

        public static void Add(Dependente d)
        {
            ctx.dependentes.Add(d);
            ctx.SaveChanges();
        }

        public static List<Dependente> getDependentes()
        {
            List<Dependente> dependentes = ctx.dependentes.ToList();
            return dependentes;
        }

        public static Dependente getDependente(int id)
        {
            return ctx.dependentes.FirstOrDefault(x => x.id == id);
        }

        public static void removeDependente(int id)
        {
            Dependente d = getDependente(id);
            ctx.dependentes.Remove(d);
            ctx.SaveChanges();
        }
    }
}