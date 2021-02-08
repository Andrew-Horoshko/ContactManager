using ContactManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ContactManager.Classes;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        private readonly ILogger<HomeController> _logger;
        List<Person> contactsList = new List<Person>();
        public HomeController( IWebHostEnvironment appEnvironment ,ILogger<HomeController> logger)
        {
            _appEnvironment = appEnvironment;
            _logger = logger;
            con.ConnectionString = ContactManager.Properties.Resources.ConnectionString;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                     uploadedFile.CopyTo(fileStream);
                }
            }
            ContactModel obj = new ContactModel();

            obj.OpenFile(uploadedFile);
            FetchData();
            return View(contactsList);
        }
        private void FetchData()
        {
            if (contactsList.Count > 0) contactsList.Clear();
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP(100)[Id] ,  [Name],[Date of birth], [Married], [Phone] ,[Salary] FROM [Apps].[dbo].[Contact]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    contactsList.Add(new Person()
                    {
                        Id = dr["Id"].ToString(),
                        Name = dr["Name"].ToString(),
                        DateOfBirth = dr["Date of birth"].ToString(),
                        Married = dr["Married"].ToString(),
                        Phone = dr["Phone"].ToString(),
                        Salary = dr["Salary"].ToString()
                    });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
