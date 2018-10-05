using Admin.Helppser;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Admin.Controllers
{
    public class VagaController : Controller
    {
        // GET: Vaga
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PublicarAgora(VagaViewModel vaga)
        {
            vaga.status = 1;
            vaga.DataEvento = DateTime.Now;
            if (SaveVaga(vaga))
                return RedirectToAction("Gerenciar");
            else
                return Json("Desculpe, o sistema encontrou um erro ao efetuar sua solicitação." +
                    " Entre em contato com nosso suporte técnico.");
        }

        [HttpPost]
        public ActionResult PublicarMaisTarde(VagaViewModel vaga)
        {
            vaga.status = 2;
            if (SaveVaga(vaga))
                return RedirectToAction("Gerenciar");
            else
                return Json("Desculpe, o sistema encontrou um erro ao efetuar sua solicitação. Entre em contato" +
                    " com nosso suporte técnico.");
        }
        
        public PartialViewResult ModalConfirmarVaga(VagaViewModel model)
        {
            return PartialView(model);
        }
        
        public ActionResult Teste()
        {
            var oportunidades = new List<VagaViewModel>
            {
                 new VagaViewModel()
                {
                    Nome = "Festa teste",
                    Id = 1,
                    Qtd = 3,
                    Valor = 100,
                    ProfissionalNome ="garçom"
                },


                new VagaViewModel()
                {
                    Nome = "Festa teste",
                    Id = 2,
                    Qtd = 3,
                    Valor = 100,
                    ProfissionalNome ="garçom"
                },
                    new VagaViewModel()
                {
                    Nome = "Festa teste",
                    Id = 3,
                    Qtd = 3,
                    Valor = 100,
                    ProfissionalNome ="garçom"
                }

            };

            return View(oportunidades);
        }

        public PartialViewResult _listarOportunidades()
        {
            //colocar o get de oportundiades por empresa
            //Passando o idempresa

            var oportunidades = new List<VagaViewModel>
            {
                 new VagaViewModel()
                {
                    Nome = "Festa teste",
                    Id = 1,
                    Qtd = 3,
                    Valor = 100,
                    ProfissionalNome ="garçom"
                },


                new VagaViewModel()
                {
                    Nome = "Festa teste",
                    Id = 2,
                    Qtd = 3,
                    Valor = 100,
                    ProfissionalNome ="garçom"
                },
                    new VagaViewModel()
                {
                    Nome = "Festa teste",
                    Id = 3,
                    Qtd = 3,
                    Valor = 100,
                    ProfissionalNome ="garçom"
                }

            };

            return PartialView(oportunidades);
        }

        public PartialViewResult _lsitarProfissionais()
        {
            return PartialView();
        }

        public PartialViewResult _vincularPorifissionais()
        {
            return PartialView();
        }

    


        private static bool SaveVaga(VagaViewModel vaga)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var jss = new JavaScriptSerializer();
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/WpOprtunidades/SalvarOportunidade/" + usuario.idCliente + "/" +
                    PixCoreValues.UsuarioLogado.IdUsuario;
                var data = jss.Serialize(Oportundiade.Convert(vaga));

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    if (string.IsNullOrEmpty(result)
                        || "null".Equals(result.ToLower()))
                    {
                        throw new Exception("Ouve um erro durante o processo.");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível salvar o usuário.", e);
            }
        }
        
        
    }
}