using Encrypt_Decrypt.Database;
using Encrypt_Decrypt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Encrypt_Decrypt.Controllers
{
    public class TextDetailController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        static int key1 = 17;
        static int key2 = 20;
        public int multiplicativeInverse(int a, int n)
        {
            int t1 = 0, t2 = 1;
            int r1, r2, r, t, multiplicative;
            if (a > n)
            {
                r1 = a;
                r2 = n;
            }
            else
            {
                r1 = n;
                r2 = a;
            }
            while (r2 > 0)
            {
                int q = r1 / r2;
                r = r1 - q * r2;
                t = t1 - q * t2;
                r1 = r2;
                r2 = r;
                t1 = t2;
                t2 = t;
            }
            t = t1;
            int gcd = r1;
            if (gcd == 1)
            {
                if (t < 0)
                {
                    multiplicative = t % n;
                }
                else
                {
                    multiplicative = t;
                }
            }
            else
            {
                return -1;
            }
            if (multiplicative < 0)
            {
                multiplicative = multiplicative + n;
            }
            return multiplicative;
        }

        [HttpGet]
        public HttpResponseMessage Decryption(string cipherText)
        {
            string originalText = "";
            int key1_inverse = multiplicativeInverse(key1, 26);
            for (int i = 0; i < cipherText.Length; i++)
            {
                int temp = ((key1_inverse * (cipherText[i] - 'a' - key2)) % 26);
                if (temp < 0)
                {
                    temp = temp + 26;
                }
                originalText = originalText +
                               (char)((temp + 'a'));
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, originalText);
            response.ReasonPhrase = originalText;
            response.Content = new StringContent(originalText, Encoding.Unicode);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage Encryption(string str)
        {
            string text = "";
            str = str.ToLower();
            foreach (char x in str)
            {
                int plain_text = (int)x - 97;
                int cipher_text = (plain_text * key1 + key2) % 26 + 97;
                text += (char)cipher_text;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, text);
            response.ReasonPhrase = text;
            response.Content = new StringContent(text, Encoding.Unicode);
            return response;
        }
        public IEnumerable<TextDetails> GetTextDetails()
        {
            return db.TextDetails.ToList();
        }

        public TextDetails GetTextDetail(int id)
        {
            return db.TextDetails.Find(id);
        }

        [HttpPost]
        public HttpResponseMessage AddTextDetails(TextDetails model)
        {
            try
            {
                db.TextDetails.Add(model);
                db.SaveChanges();
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateTextDetails(int id, TextDetails model)
        {
            try
            {
                if (id == model.id)
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    return response;
                }
                else
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotModified);
                    return response;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return response;
            }
        }


        public HttpResponseMessage DeleteUser(int id)
        {
            TextDetails td = db.TextDetails.Find(id);
            if (td != null)
            {
                db.TextDetails.Remove(td);
                db.SaveChanges();
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }
        }

    }
}
