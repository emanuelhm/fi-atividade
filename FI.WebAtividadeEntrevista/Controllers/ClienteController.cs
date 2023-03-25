using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Reflection;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
             var bo = new BoCliente();

            if (!Utility.Util.IsCPFValid(model.CPF))
            {
                ModelState.AddModelError("CPF", "Digite um CPF válido");
            }
            else if (bo.VerificarExistencia(model.CPF))
            {
                ModelState.AddModelError("CPF", "CPF já cadastrado");
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                
                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    CPF = model.CPF,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                foreach(var beneficiario in model.Beneficiarios)
                {
                    bo.IncluirOuAlterarBeneficiario(beneficiario.Id, new Beneficiario() 
                    {
                        CPF = beneficiario.CPF,
                        Nome = beneficiario.Nome,
                        IdCliente = model.Id,
                    });
                }
           
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!Utility.Util.IsCPFValid(model.CPF))
            {
                ModelState.AddModelError("CPF", "Digite um CPF válido");
            }
            else if (bo.VerificarExistencia(model.CPF, model.Id))
            {
                ModelState.AddModelError("CPF", "CPF já cadastrado");
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    CPF = model.CPF,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                foreach (var beneficiario in model.Beneficiarios)
                {
                    bo.IncluirOuAlterarBeneficiario(beneficiario.Id, new Beneficiario()
                    {
                        CPF = beneficiario.CPF,
                        Nome = beneficiario.Nome,
                        IdCliente = model.Id,
                    });
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    CPF = cliente.CPF,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Beneficiarios = cliente.Beneficiarios.Select(b => new BeneficiarioModel() 
                    {
                        Id = b.Id,
                        CPF = b.CPF,
                        Nome = b.Nome,
                    }).ToList(),
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public string IsCPFValid(long? id, string cpf)
        {
            var bo = new BoCliente();

            if (!Utility.Util.IsCPFValid(cpf))
            {
                return "Digite um CPF válido";
            }
            else if (bo.VerificarExistencia(cpf, id))
            {
                return "CPF já cadastrado";
            }

            return null;
        }
    }
}