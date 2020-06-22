using MePague.Config;
using MePague.Models;
using MePague.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Deployment.Internal;
using System.Linq;
using System.Web;

namespace MePague.DAO
{
    public class DespesaDAO
    {
        static Context ctx = SingletonContext.GetInstance();
        public static bool Add(Despesa d)
        {
            try
            {
                ctx.despesas.Add(d);
                ctx.SaveChanges();
                return true;
            } catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Despesa> getDespesas()
        {
            List<Despesa> ds = ctx.despesas.ToList();
            return ds;
        }

        public static Despesa getDespesa(int id)
        {
            Despesa d = ctx.despesas.FirstOrDefault(x => x.id == id);
            return d;
        }

        public static List<Despesa> searchDespesasPorDono(int id)
        {
            return ctx.despesas.Where(x => x.dono.id == id).ToList();
        }

        public static void addDependent(int idDespesa, Dependente d)
        {
            Despesa despesa = getDespesa(idDespesa);
            despesa.dependentes.Add(d);
            ctx.despesas.AddOrUpdate(despesa);
            ctx.SaveChanges();
        }

        public static void deleteDespesa(int id)
        {
            Despesa d = getDespesa(id);
            ctx.despesas.Remove(d);
            ctx.SaveChanges();
        }

        public static List<Dependente> getDependentes(int id)
        {
            return ctx.despesas.FirstOrDefault(x => x.id == id).dependentes;
        }

        public static Dependente getDependente(int idDespesa, int idDependente)
        {
            List<Dependente> dependentes = ctx.despesas.FirstOrDefault(x => x.id == idDespesa).dependentes;
            Dependente dependente = new Dependente();
            foreach (Dependente d in dependentes)
            {
                if (d.dependente.id == idDependente)
                {
                    dependente = d;
                }
            }
            return dependente;
        }

        public static Dependente getDependentePorId(int idDependente)
        {
            return ctx.dependentes.FirstOrDefault(x => x.id == idDependente);
        }

        public static void removeDependente(int idDespesa, int idDependente)
        {
            Despesa despesa = getDespesa(idDespesa);
            Dependente dependente = DependenteDAO.getDependente(idDependente);
            List<Dependente> dependentes = despesa.dependentes;
            dependentes.Remove(dependente);
            despesa.dependentes = dependentes;
            ctx.despesas.AddOrUpdate(despesa);
            ctx.dependentes.Remove(dependente);
            ctx.SaveChanges();
        }

        public static List<Despesa> searchDespesaPorDependente(int id)
        {
            List<Despesa> ds = new List<Despesa>();
            List<Despesa> despesas = ctx.despesas.ToList();
            foreach (Despesa d in despesas)
            {
                if (d.dependentes != null) {
                    List<Dependente> dependentes = d.dependentes;
                    foreach (Dependente c in dependentes)
                    {
                        if (c.dependente.id == id && c.status.Equals("Aceito"))
                        {
                            ds.Add(d);
                        }
                    }
                } else
                {
                    ds = new List<Despesa>();
                }
            }
            return ds;
        }

        public static List<Despesa> searchSolicitacaoPendentePorDependente(int id)
        {
            List<Despesa> ds = new List<Despesa>();
            List<Despesa> despesas = ctx.despesas.ToList();
            foreach (Despesa d in despesas)
            {
                if (d.dependentes != null)
                {
                    List<Dependente> dependentes = d.dependentes;
                    foreach (Dependente c in dependentes)
                    {
                        if (c.dependente.id == id && c.status.Equals("Pendente"))
                        {
                            ds.Add(d);
                        }
                    }
                }
                else
                {
                    ds = new List<Despesa>();
                }
            }
            return ds;
        }

        public static void updateDependente(Dependente d)
        {
            ctx.dependentes.AddOrUpdate(d);
            ctx.SaveChanges();
        }

        public static void updateDespesa(Despesa d)
        {
            ctx.despesas.AddOrUpdate(d);
            ctx.SaveChanges();
        }
    }
}