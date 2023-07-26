using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

        [HttpGet("all")]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customers = new List<Customer>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Customer;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var customer = new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                TypeCustomer = reader.GetString(reader.GetOrdinal("TypeCustomer")),
                                CLoginName = reader.GetString(reader.GetOrdinal("CLoginName")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                // Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            customers.Add(customer);
                        }
                    }

                }
            }
            return customers;
        }
        [HttpGet("{CustomerId}")]
        public async Task<Customer> GetCustomer(int CustomerId)
        {
            Customer customer = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Customer WHERE CustomerId = @CustomerId;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", CustomerId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            customer = new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                BirthDate = reader.GetDateTime(reader.GetOrdinal( "BirthDate" )),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                TypeCustomer = reader.GetString(reader.GetOrdinal("TypeCustomer")),
                                CLoginName = reader.GetString(reader.GetOrdinal("CLoginName")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                UrlImage =  reader.GetString(reader.GetOrdinal("UrlImage"))
                            };
                        }
                    }
                }
            }

            return customer;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromForm] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (PhoneExists(customer.PhoneNumber))
                {
                    return BadRequest("Phone number already exists.");
                }
                if (EmailExists(customer.Email))
                {
                    return BadRequest("Email already exists.");
                }
                if (LoginNameExists(customer.CLoginName))
                {
                    return BadRequest("Login name already exists.");
                }

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "INSERT INTO Customer (FirstName, MiddleName, LastName, Gender, BirthDate, PhoneNumber, Email, Address, TypeCustomer,UrlImage, CLoginName, Password) " +
                                   "VALUES (@FirstName, @MiddleName, @LastName, @Gender, @BirthDate, @PhoneNumber, @Email, @Address, @TypeCustomer,@UrlImage, @CLoginName, @Password); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // SQL Injection
                        cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        cmd.Parameters.AddWithValue("@MiddleName", customer.MiddleName);
                        cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                        cmd.Parameters.AddWithValue("@BirthDate", customer.BirthDate);
                        cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", customer.Email);
                        cmd.Parameters.AddWithValue("@Address", customer.Address);
                        cmd.Parameters.AddWithValue("@TypeCustomer", customer.TypeCustomer);
                        cmd.Parameters.AddWithValue("@CLoginName", customer.CLoginName);
                        cmd.Parameters.AddWithValue("@Password", customer.Password);

                        var file = HttpContext.Request.Form.Files.FirstOrDefault();

                        if (file != null && file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);

                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                            var filePath = Path.Combine("uploads", uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            cmd.Parameters.AddWithValue("@UrlImage", filePath);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@UrlImage", DBNull.Value);
                        }

                        int insertedCustomerId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        customer.CustomerId = insertedCustomerId;

                    }

                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while registering the customer.");
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromForm] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //try
            //{
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "UPDATE Customer SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, " +
                                   "Gender = @Gender, BirthDate = @BirthDate, PhoneNumber = @PhoneNumber, " +
                                   "Email = @Email, Address = @Address, TypeCustomer = @TypeCustomer, " +
                                   "CLoginName = @CLoginName, Password = @Password, UrlImage = @UrlImage " +
                                   "WHERE CustomerId = @CustomerId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                        cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        cmd.Parameters.AddWithValue("@MiddleName", customer.MiddleName);
                        cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                        cmd.Parameters.AddWithValue("@BirthDate", customer.BirthDate);
                        cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", customer.Email);
                        cmd.Parameters.AddWithValue("@Address", customer.Address);
                        cmd.Parameters.AddWithValue("@TypeCustomer", customer.TypeCustomer);
                        cmd.Parameters.AddWithValue("@CLoginName", customer.CLoginName);
                        cmd.Parameters.AddWithValue("@Password", customer.Password);

                        // Kiểm tra xem khách hàng có gửi file ảnh mới không
                        if (customer.ImageFile != null && customer.ImageFile.Length > 0)
                        {
                            var fileName = Path.GetFileName(customer.ImageFile.FileName);
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                            var filePath = Path.Combine("uploads", uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await customer.ImageFile.CopyToAsync(stream);
                            }

                            cmd.Parameters.AddWithValue("@UrlImage", filePath);
                        }
                        else
                        {
                            // Nếu không có ảnh mới được gửi, giữ nguyên ảnh cũ
                            cmd.Parameters.AddWithValue("@UrlImage", customer.UrlImage);
                        }

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return Ok("Profile updated successfully.");
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, "An error occurred while updating the profile.");
            //}
        }

        private bool PhoneExists(string phoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Customer WHERE PhoneNumber = @PhoneNumber";
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
                string query = "SELECT COUNT(*) FROM Customer WHERE Email = @Email";
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
                string query = "SELECT COUNT(*) FROM Customer WHERE CLoginName = @CLoginName";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CLoginName", loginName);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }

}

