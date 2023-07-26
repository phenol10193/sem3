using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

        // POST api/supplier/register
        [HttpPost("register")]
        public IActionResult RegisterSupplier([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (PhoneExists(supplier.PhoneNumber))
                {
                    return BadRequest("Phone number already exists.");
                }
                if (EmailExists(supplier.Email))
                {
                    return BadRequest("Email already exists.");
                }

                if (LoginNameExists(supplier.SLoginName))
                {
                    return BadRequest("Login name already exists.");
                }
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Supplier (SName, PhoneNumber, Address, Email, SLevel, Image, ACityId, SLoginGame, Password) " +
                                     "VALUES (@SName, @PhoneNumber, @Address, @Email, @SLevel, @Image, @ACityId, @SLoginName, @Password)";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // SQL Injection
                        cmd.Parameters.AddWithValue("@SName", supplier.SName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", supplier.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Address", supplier.Address);
                        cmd.Parameters.AddWithValue("@Email", supplier.Email);
                        cmd.Parameters.AddWithValue("@SLevel", supplier.SLevel);
                        cmd.Parameters.AddWithValue("@Image", supplier.Image);
                        cmd.Parameters.AddWithValue("@ACityId", supplier.ACityId);
                        cmd.Parameters.AddWithValue("@SLoginName", supplier.SLoginName);
                        cmd.Parameters.AddWithValue("@Password", supplier.Password);

                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(supplier);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while registering the supplier.");
            }
        }
        // POST api/supplier/updateProfile
        [HttpPost("updateProfile")]
        public IActionResult UpdateSupplierProfile([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //if (phoneexists(supplier.phonenumber, supplier.supplierid))
                //{
                //    return badrequest("phone number already exists.");
                //}

                //if (emailexists(supplier.email, supplier.supplierid))
                //{
                //    return badrequest("email already exists.");
                //}
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Suppliers SET SName = @SName, PhoneNumber = @PhoneNumber, Address = @Address, " +
                                   "Email = @Email, SLevel = @SLevel, Image = @Image, ACityId = @ACityId " +
                                   "WHERE SupplierId = @SupplierId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // SQL Injection
                        cmd.Parameters.AddWithValue("@SName", supplier.SName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", supplier.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Address", supplier.Address);
                        cmd.Parameters.AddWithValue("@Email", supplier.Email);
                        cmd.Parameters.AddWithValue("@SLevel", supplier.SLevel);
                        cmd.Parameters.AddWithValue("@Image", supplier.Image);
                        cmd.Parameters.AddWithValue("@ACityId", supplier.ACityId);
                        cmd.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);

                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the supplier profile.");
            }
        }

        // DELETE api/supplier/deleteProfile
        [HttpDelete("deleteProfile")]
        public IActionResult DeleteSupplierProfile()
        {
            
            try
            {
                return Ok("Supplier profile deleted successfully.");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while deleting the supplier profile.");
            }
        }


        private bool PhoneExists(string phoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Supplier WHERE PhoneNumber = @PhoneNumber";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private bool EmailExists(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Supplier WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private bool LoginNameExists(string loginName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Supplier WHERE SLoginName = @SLoginName";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SLoginName", loginName);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}

