using ContactManager.Classes;
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
        public  string[] fileData { get; set; }

        public Person OpenFile(IFormFile uploadedFile)
        {
            string path = @"wwwroot\Files\" + uploadedFile.FileName ;
            fileData = File.ReadAllText(path).Split(';');
           return  new Person() {
                Name = fileData[0] ,
                DateOfBirth = fileData[1] , 
                Married = fileData[2] , 
                Phone = fileData[3] , 
                Salary = fileData[4]  };

        }
    }
}
