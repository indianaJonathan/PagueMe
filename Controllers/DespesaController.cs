using MePague.DAO;
using MePague.Models;
using MePague.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MePague.Controllers
{
    public class DespesaController : Controller
    {
        static Despesa desp = new Despesa();
        public ActionResult Index()
        {
            if (Sessao.isLogged()) {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                if (!TipoUsuario.Administrador.Equals(u.tipo)) {
                    List<Despesa> despesas = DespesaDAO.searchDespesasPorDono(u.cliente.id);
                    List<Despesa> dependencias = DespesaDAO.searchDespesaPorDependente(u.cliente.id);
                    List<Despesa> solicitacoes = DespesaDAO.searchSolicitacaoPendentePorDependente(u.cliente.id);
                    ViewBag.Despesas = despesas;
                    ViewBag.Dependencias = dependencias;
                    ViewBag.DependenciaSolicitada = solicitacoes;
                    return View();
                } else
                {
                    ViewBag.Despesas = DespesaDAO.getDespesas();
                    ViewBag.Dependencias = new List<Despesa>();
                    ViewBag.DependenciaSolicitada = new List<Despesa>();
                    return View();
                }
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Add()
        {
            if (Sessao.isLogged())
            {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                List<Models.TipoDespesa> tiposDespesa = new List<Models.TipoDespesa>();
                tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Anual));
                tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Mensal));
                tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Semanal));
                tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.CobrancaUnica));
                IEnumerable<SelectListItem> tipos = new SelectList(tiposDespesa, "sigla", "nome");
                ViewBag.Tipos = tipos;
                return View();
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public ActionResult Add(Despesa d)
        {
            if (Sessao.isLogged())
            {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                if (ModelState.IsValid)
                {
                    d.dono = u.cliente;
                    d.status = "Aberto";
                    DespesaDAO.Add(d);
                    return RedirectToAction("Index", "Despesa");
                }
                ModelState.AddModelError("", "Falha ao adicoinar despesa!");
                List<Models.TipoDespesa> tiposDespesa = new List<Models.TipoDespesa>();
                tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Anual));
                tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Mensal));
                tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Semanal));
                tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.CobrancaUnica));
                IEnumerable<SelectListItem> tipos = new SelectList(tiposDespesa, "sigla", "nome");
                ViewBag.Tipos = tipos;
                return View(d);
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult AddDependent(int id)
        {
            if (Sessao.isLogged())
            {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                Despesa d = DespesaDAO.getDespesa(id);
                desp = d;
                ViewBag.Despesa = d;
                if ("A".Equals(u.tipo))
                {
                    ViewBag.Dependentes = new SelectList(ClienteDAO.getClientes(), "id", "nome");
                    return View();
                } else
                {
                    List<Cliente> clientes = new List<Cliente>();
                    List<Amizade> amizadesAceitas = AmizadeDAO.searchAmizadesAceitasPorCliente(u.cliente.id);
                    List<Amizade> amizadesSolicitadasAceitas = AmizadeDAO.searchAmizadesSolicitadasAceitasPorCliente(u.cliente.id);
                    List<Dependente> dependentes = d.dependentes;
                    foreach (Amizade a in amizadesAceitas)
                    {
                        if (d.dependentes.Count > 0) {
                            bool canAdd = true;
                            foreach (Dependente dep in dependentes) {
                                if (dep.dependente.id == a.solicitante.id) {
                                    canAdd = false;
                                }
                            }
                            if (canAdd)
                            {
                                Cliente c = a.solicitante;
                                clientes.Add(c);
                            }
                        }
                    }
                    foreach (Amizade a in amizadesSolicitadasAceitas)
                    {
                        if (d.dependentes.Count > 0)
                        {
                            bool canAdd = true;
                            foreach (Dependente dep in dependentes)
                            {
                                if (dep.dependente.id == a.solicitado.id)
                                {
                                    canAdd = false;
                                }
                            }
                            if (canAdd)
                            {
                                Cliente c = a.solicitado;
                                clientes.Add(c);
                            }
                        }
                    }
                    ViewBag.Dependentes = new SelectList(clientes, "id", "nome");
                    return View();
                }
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public ActionResult AddDependent(Dependente dependente)
        {
            if (Sessao.isLogged())
            {
                Despesa d = desp;
                Cliente c = ClienteDAO.getCliente(dependente.idDependente);
                dependente.dependente = c;
                double valorTotal = 0;
                double valorDependentes = 0;
                valorTotal = d.valor;
                foreach (Dependente dep in d.dependentes)
                {
                    valorDependentes += dep.valor;
                }
                double valorRestante = valorTotal - valorDependentes;
                ViewBag.ValorRestante = valorRestante;
                if (d.dependentes != null) {
                    d.dependentes.Add(dependente);
                } else
                {
                    List<Dependente> dependentes = new List<Dependente>();
                    dependentes.Add(dependente);
                    d.dependentes = dependentes;
                }
                DespesaDAO.updateDespesa(d);
                return Redirect(Url.Action("Edit", "Despesa") + "/" + d.id);
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult adicionarDependente(int idDependente, int idDespesa)
        {
            if (Sessao.isLogged())
            {
                Despesa despesa = DespesaDAO.getDespesa(idDespesa);
                Dependente dependente = new Dependente();
                dependente.dependente = ClienteDAO.getCliente(idDependente);
                dependente.idDependente = idDependente;
                despesa.dependentes.Add(dependente);
                DespesaDAO.updateDespesa(despesa);
                return Redirect(Url.Action("Edit", "Despesa") + "/" + idDespesa);
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult deleteDespesa(int id)
        {
            if (Sessao.isLogged())
            {
                DespesaDAO.deleteDespesa(id);
                return RedirectToAction("Index", "Despesa");
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Edit(int id)
        {
            if (Sessao.isLogged())
            {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                Despesa d = DespesaDAO.getDespesa(id);
                if (TipoUsuario.Administrador.Equals(u.tipo) || u.cliente.id == d.dono.id) {
                    List<Models.TipoDespesa> tiposDespesa = new List<Models.TipoDespesa>();
                    tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Anual));
                    tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Mensal));
                    tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.Semanal));
                    tiposDespesa.Add(new Models.TipoDespesa(Utils.TipoDespesa.CobrancaUnica));
                    IEnumerable<SelectListItem> tipos = new SelectList(tiposDespesa, "sigla", "nome");
                    ViewBag.Tipos = tipos;
                    desp = d;
                    double valorTotal = 0;
                    double valorDependentes = 0;
                    valorTotal = d.valor;
                    foreach (Dependente dep in d.dependentes)
                    {
                        valorDependentes += dep.valor;
                    }
                    double valorRestante = valorTotal - valorDependentes;
                    ViewBag.ValorRestante = valorRestante;
                    if (valorRestante > 0)
                    {
                        ViewBag.CanAdd = true;
                    }
                    else
                    {
                        ViewBag.CanAdd = false;
                    }
                    if (TipoUsuario.Administrador.Equals(u.tipo))
                    {
                        ViewBag.AmigosSolicitados = new List<Amizade>();
                        ViewBag.AmigosAceitos = AmizadeDAO.getAmizades();
                    } else {
                        ViewBag.AmigosSolicitados = AmizadeDAO.searchAmizadesSolicitadasAceitasPorCliente(u.cliente.id);
                        ViewBag.AmigosAceitos = AmizadeDAO.searchAmizadesAceitasPorCliente(u.cliente.id);
                    }
                    return View(d);
                }
                else
                {
                    return RedirectToAction("Index", "Despesa");
                }
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public ActionResult Edit(Despesa d)
        {
            if (Sessao.isLogged())
            {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                if (ModelState.IsValid) {
                    Despesa aux = DespesaDAO.getDespesa(d.id);
                    for(int i = 0; i < aux.dependentes.Count; i++)
                    {
                        if (aux.dependentes[i].valor != d.dependentes[i].valor)
                        {
                            aux.dependentes[i].valor = d.dependentes[i].valor;
                        }
                    }
                    aux = d;
                    DespesaDAO.updateDespesa(aux);
                    return RedirectToAction("Index", "Despesa");
                }
                ModelState.AddModelError("", "Falha ao atualizar despesa!");
                double valorTotal = 0;
                double valorDependentes = 0;
                valorTotal = d.valor;
                foreach (Dependente dep in d.dependentes)
                {
                    valorDependentes += dep.valor;
                }
                double valorRestante = valorTotal - valorDependentes;
                ViewBag.ValorRestante = valorRestante;
                if (valorRestante > 0)
                {
                    ViewBag.CanAdd = true;
                }
                else
                {
                    ViewBag.CanAdd = false;
                }
                if (TipoUsuario.Administrador.Equals(u.tipo))
                {
                    ViewBag.AmigosSolicitados = new List<Amizade>();
                    ViewBag.AmigosAceitos = AmizadeDAO.getAmizades();
                }
                else
                {
                    ViewBag.AmigosSolicitados = AmizadeDAO.searchAmizadesSolicitadasAceitasPorCliente(u.cliente.id);
                    ViewBag.AmigosAceitos = AmizadeDAO.searchAmizadesAceitasPorCliente(u.cliente.id);
                }
                return View(d);
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult removeDependente(int idDependente)
        {
            if (Sessao.isLogged()) {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                if (desp.dono.id == u.cliente.id || u.tipo.Equals(TipoUsuario.Administrador)) {
                    DespesaDAO.removeDependente(desp.id, idDependente);
                    DependenteDAO.removeDependente(idDependente);

                    return Redirect(Url.Action("Edit", "Despesa") + "/" + desp.id);
                }
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult aceitarParticipacao(int idDependente, int idDespesa)
        {
            if (Sessao.isLogged())
            {
                Dependente d = DespesaDAO.getDependente(idDespesa, idDependente);
                d.status = "Aceito";
                DespesaDAO.updateDependente(d);
                return RedirectToAction("Index", "Despesa");
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult recusarParticipacao(int idDependente, int idDespesa)
        {
            if (Sessao.isLogged())
            {
                Dependente d = DespesaDAO.getDependente(idDespesa, idDependente);
                d.status = "Recusado";
                DespesaDAO.updateDependente(d);
                return RedirectToAction("Index", "Despesa");
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult solicitarSaida(int idDependente)
        {
            if (Sessao.isLogged())
            {
                Dependente d = DespesaDAO.getDependentePorId(idDependente);
                d.status = "Saída solicitada";
                DespesaDAO.updateDependente(d);
                return RedirectToAction("Index", "Despesa");
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult recusarSaida(int idDependente, int idDespesa)
        {
            if (Sessao.isLogged())
            {
                Dependente d = DespesaDAO.getDependentePorId(idDependente);
                d.status = "Aceito";
                DespesaDAO.updateDependente(d);
                return Redirect(Url.Action("Edit", "Despesa") + "/" + idDespesa);
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult aceitarSaida(int idDependente, int idDespesa)
        {
            if (Sessao.isLogged())
            {
                DespesaDAO.removeDependente(idDespesa, idDependente);
                return Redirect(Url.Action("Edit", "Despesa") + "/" + idDespesa);
            }
            return RedirectToAction("Login", "Usuario");
        }
    }
}