using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Encrypt_Decrypt_Client.Models
{
    public class TextDetails
    {
        [Key]
        public int id { get; set; }
        public string plaintext { get; set; }
        public string encryptedtext { get; set; }
        public string decryptedtext { get; set; }
    }
}