using Encrypt_Decrypt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Encrypt_Decrypt.Database
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        {

        }
        public DbSet<TextDetails> TextDetails { get; set; }
    }
}