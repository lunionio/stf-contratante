using Admin.Helppser;
using Admin.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Admin.Controllers
{
    public class FaleConoscoController : Controller
    {
        // GET: FaleConosco
        public ActionResult Index()
        {
            ViewBag.email = PixCoreValues.UsuarioLogado.Login;
            return View();
        }

        [HttpPost]
        public ActionResult Enviar(FaleconoscoVIewModel model)
        {
            // Save(model);

            return Json("0000000");
        }

        private bool Save(FaleconoscoVIewModel model)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var jss = new JavaScriptSerializer();
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/Principal/DeletarUsuario/" + usuario.idCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                object envio = Helppers.FaleConosco.Convert(model);
                var data = jss.Serialize(envio);

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