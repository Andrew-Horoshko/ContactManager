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
using System.Data;
using System.Globalization;

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

            var contact =  obj.OpenFile(uploadedFile);
            con.Open();
            com.Connection = con;
            // com.CommandText = "INSERT INTO [Apps].[dbo].[Contact] ( Name , [Date of birth] , Married , Phone,Salary) VALUES("+ contact.Name +"," + contact.DateOfBirth +","+contact.Married +","+ contact.Phone+"," +contact.Salary +") ";
            com.CommandText = "INSERT INTO [Apps].[dbo].[Contact] ( Name , [Date of birth] , Married , Phone,Salary) VALUES( @Name, @DateOfBirth, @Married, @Phone, @Salary) ";
            com.Parameters.Add("@Name", SqlDbType.VarChar );
            com.Parameters["@Name"].Value = contact.Name;
            DateTime date = DateTime.ParseExact(contact.DateOfBirth,
                         "yyyy-MM-dd HH:mm:ss.fff",
                         CultureInfo.InvariantCulture);

            com.Parameters.Add("@DateOfBirth", SqlDbType.Date);
            com.Parameters["@DateOfBirth"].Value = date;
            com.Parameters.Add("@Married", SqlDbType.Bit);
            com.Parameters["@Married"].Value = contact.Married;
            com.Parameters.Add("@Phone", SqlDbType.VarChar);
            com.Parameters["@Phone"].Value = contact.Phone;
            com.Parameters.Add("@Salary", SqlDbType.Decimal);
            com.Parameters["@Salary"].Value = contact.Salary;
            com.ExecuteNonQuery(); 
            con.Close();
            FetchData();
            return View(contactsList);
        }
        /* string commandText = "UPDATE Sales.Store SET Demographics = @demographics "
        + "WHERE CustomerID = @ID;";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand command = new SqlCommand(commandText, connection);
        command.Parameters.Add("@ID", SqlDbType.Int);
        command.Parameters["@ID"].Value = customerID;
        command.Parameters.AddWithValue("@demographics", demoXml);*/
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
                /*   dr.Close();
                 com.CommandText = "INSERT INTO [Apps].[dbo].[Contact] (Id , Name , [Date of birth] , Married , Phone,Salary) VALUES(6, 'Marta', '01.01.1991', 1, '432242', 0) ";
                 com.ExecuteNonQuery();*/
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
