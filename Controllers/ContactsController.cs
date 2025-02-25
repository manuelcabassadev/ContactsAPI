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
        private readonly IConfiguration _configuration;

        public ContactsController(IConfiguration configuration)
        {
            
            _configuration = configuration;
        }

        //Create a new contact
        [HttpPost]
        public IActionResult Post([FromBody]Contact model)
        {

            try
            {
                using (SqlConnection con = new(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Contacts (Name, Phone, Fax, Email, Notes, LastUpdateDate, LastUpdateUserName)" +
                        "VALUES(@Name, @Phone, @Fax, @Email, @Notes, @LastUpdateDate, @LastUpdateUserName)", con))
                    {
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@Phone", model.Phone);
                        cmd.Parameters.AddWithValue("@Fax", model.Fax);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Notes", model.Notes);
                        cmd.Parameters.AddWithValue("@LastUpdateDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@LastUpdateUserName", model.LastUpdateUserName);

                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok("Contact created.");
        }

        //Edit Contact Information
        [HttpPut]
        public IActionResult EditContact(Contact model)
        {

            try
            {
                if (model.ContactId == 0)
                    return new JsonResult(NotFound());

                using (SqlConnection con = new(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE Contacts SET Name = @Name, Phone = @Phone, Fax = @Fax, Email = @Email,Notes = @Notes, LastUpdateDate = @LastUpdateDate, LastUpdateUserName = @LastUpdateUserName " +
                        "WHERE ContactID = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", model.ContactId);
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@Phone", model.Phone);
                        cmd.Parameters.AddWithValue("@Fax", model.Fax);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Notes", model.Notes);
                        cmd.Parameters.AddWithValue("@LastUpdateDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@LastUpdateUserName", model.LastUpdateUserName);

                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok("Contact edited");
        }

        //Retrieves all register contacts
        [HttpGet]
        public JsonResult GetContacts()
        {
            List<Contact> contactsModel = new List<Contact>();

            try
            {
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
                                Email = reader["eMail"].ToString(),
                                Notes = reader["Notes"].ToString(),
                                LastUpdateDate = Convert.ToDateTime(reader["LastUpdateDate"].ToString()),
                                LastUpdateUserName = reader["LastUpdateUserName"].ToString(),
                            });
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                return new JsonResult(BadRequest(ex.Message));
            }

            return new JsonResult(contactsModel);
        }
    }
}
