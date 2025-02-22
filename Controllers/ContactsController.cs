using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Models;
using ContactsAPI.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static Azure.Core.HttpHeader;
using System.Collections.Generic;

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactInformationDbContext _context;
        private readonly IConfiguration _configuration;

        public ContactsController(IConfiguration configuration)
        {
            
            _configuration = configuration;
        }

        //Create a new contact
        [HttpPost]
        public JsonResult Create(Contact contact)
        {

            using (SqlConnection con = new(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Contacts (Name, Phone, Fax, eMail, Notes, LastUpdateDate, LastUpdateUserName)" +
                    "VALUES(@Name, @Phone, @Fax, @eMail, @Notes, @LastUpdateDate, @LastUpdateUserName)", con))
                {
                    cmd.Parameters.AddWithValue("@Name", contact.Name);
                    cmd.Parameters.AddWithValue("@Phone", contact.Phone);
                    cmd.Parameters.AddWithValue("@Fax", contact.Fax);
                    cmd.Parameters.AddWithValue("@eMail", contact.eMail);
                    cmd.Parameters.AddWithValue("@Notes", contact.Notes);
                    cmd.Parameters.AddWithValue("@LastUpdateDate", contact.LastUpdateDate);
                    cmd.Parameters.AddWithValue("@LastUpdateUserName", contact.LastUpdateUserName);

                    cmd.ExecuteNonQuery();
                }

            }


            return new JsonResult(Ok(contact));
        }

        //Edit Contact Information
        [HttpPut]
        public JsonResult Edit(Contact contact)
        {

            if (contact.ContactId == null || contact.ContactId == 0)
                return new JsonResult(NotFound());

            using (SqlConnection con = new(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Contacts SET Name = @Name, Phone = @Phone, Fax = @Fax, eMail = @eMail,Notes = @Notes, LastUpdateDate = @LastUpdateDate, LastUpdateUserName = @LastUpdateUserName " +
                    "WHERE ContactID = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", contact.ContactId);
                    cmd.Parameters.AddWithValue("@Name", contact.Name);
                    cmd.Parameters.AddWithValue("@Phone", contact.Phone);
                    cmd.Parameters.AddWithValue("@Fax", contact.Fax);
                    cmd.Parameters.AddWithValue("@eMail", contact.eMail);
                    cmd.Parameters.AddWithValue("@Notes", contact.Notes);
                    cmd.Parameters.AddWithValue("@LastUpdateDate", contact.LastUpdateDate);
                    cmd.Parameters.AddWithValue("@LastUpdateUserName", contact.LastUpdateUserName);

                    cmd.ExecuteNonQuery();
                }

            }


            return new JsonResult(Ok(contact));
        }

        //Retrieves all register contacts
        [HttpGet]
        public JsonResult GetAllContacts()
        {
            List<Contact> contactsModel = new List<Contact>();

            using (SqlConnection con = new(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ContactID, Name, Phone, Fax, eMail, Notes, LastUpdateDate, LastUpdateUserName FROM Contacts", con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        contactsModel.Add(new Contact
                        {
                            ContactId = Convert.ToInt32(reader["ContactID"]),
                            Name = reader["Name"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Fax = reader["Fax"].ToString(),
                            eMail = reader["eMail"].ToString(),
                            Notes = reader["Notes"].ToString(),
                            LastUpdateDate = Convert.ToDateTime(reader["LastUpdateDate"].ToString()),
                            LastUpdateUserName = reader["LastUpdateUserName"].ToString(),
                        });
                    }

                }
            }

            return new JsonResult(contactsModel);
        }
    }
}
