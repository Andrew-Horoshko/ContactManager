using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Models
{
    public class ContactModel
    {
        public  string file { get; set; }

        public void OpenFile(IFormFile uploadedFile)
        {
            string path = @"wwwroot\Files\" + uploadedFile.FileName ;
             file = File.ReadAllText(path);
        }
    }
}
