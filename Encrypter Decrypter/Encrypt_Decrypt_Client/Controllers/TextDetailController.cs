using Encrypt_Decrypt_Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Encrypt_Decrypt_Client.Controllers
{
    public class TextDetailController : Controller
    {
      
        Uri baseAddress = new Uri("https://localhost:44357/api");
        Uri encrypt_baseAddress = new Uri("https://localhost:44357/encrypt");
        Uri decrypt_baseAddress = new Uri("https://localhost:44357/decrypt");

        HttpClient client;
        HttpClient encrypt_client;
        HttpClient decrypt_client;

        public TextDetailController()
        {
            client = new HttpClient();
            encrypt_client = new HttpClient();
            decrypt_client = new HttpClient();

            client.BaseAddress = baseAddress;
            encrypt_client.BaseAddress = encrypt_baseAddress;
            decrypt_client.BaseAddress = decrypt_baseAddress;
        }

        public ActionResult Index()
        {
            string message = TempData["Message"] as string;
            if (!String.IsNullOrEmpty(message))
            {
                TempData["Message"] = "Successfully Added to database";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Encryption(TextDetails td)
        {
            HttpResponseMessage response= client.GetAsync(encrypt_client.BaseAddress + "/textdetail/" + td.plaintext).Result;
            td.encryptedtext = response.ReasonPhrase;
            return View("Index", td);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decryption(TextDetails td)
        {
            HttpResponseMessage response = client.GetAsync(decrypt_client.BaseAddress + "/textdetail/" + (TempData["encrypt"]).ToString()).Result;
            td.decryptedtext = response.ReasonPhrase;
            td.encryptedtext = (TempData["encrypt"]).ToString();
            return View("Index", td);
        }

        [HttpPost]
        public ActionResult AddtoDatabse(TextDetails td)
        {
            td.encryptedtext = (TempData["encrypt"]).ToString();
            td.decryptedtext = (TempData["decrypt"]).ToString();
            string data = JsonConvert.SerializeObject(td);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/Json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/textdetail", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Successfully Added to database";
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult ShowAll()
        {
            string message = TempData["Message"] as string;
            string message2 = TempData["DeleteMessage"] as string;
            if (!String.IsNullOrEmpty(message))
            {
                TempData["Message"] = "Successfully Updated";
            }
            if (!String.IsNullOrEmpty(message2))
            {
                TempData["DeleteMessage"] = "Deleted Successfully";
            }
            List<TextDetails> modelList = new List<TextDetails>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/textdetail").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<TextDetails>>(data);
            }
            return View(modelList);
        }

        public ActionResult Edit(int id)
        {
            TextDetails model = new TextDetails();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/textdetail/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<TextDetails>(data);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TextDetails model)
        {
            HttpResponseMessage response = client.GetAsync(encrypt_client.BaseAddress + "/textdetail/" + model.plaintext).Result;
            model.encryptedtext = response.ReasonPhrase;
            response = client.GetAsync(decrypt_client.BaseAddress + "/textdetail/" + model.encryptedtext).Result;
            model.decryptedtext = response.ReasonPhrase;
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/Json");
            response = client.PutAsync(client.BaseAddress + "/textdetail/" + model.id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Successfully Updated";
                return RedirectToAction("ShowAll");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/textdetail/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteMessage"] = "Deleted Successfully";
                return RedirectToAction("ShowAll");
            }
            return RedirectToAction("ShowAll");
        }
    }
}   