﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sem3.Models;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

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

        [HttpGet("Selectcustomer")]
        public async Task<IEnumerable<Customer>> GetCustomer()
        {
            var customers = new List<Customer>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT CustomerId, FirstName, MiddleName, LastName, PhoneNumber  FROM Customer;";

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
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                               
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
                                BirthOfDate = reader.GetDateTime(reader.GetOrdinal("BirthOfDate")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                TypeCustomer = reader.GetString(reader.GetOrdinal("TypeCustomer")),
                                CLoginName = reader.GetString(reader.GetOrdinal("CLoginName")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                UrlImage = reader.GetString(reader.GetOrdinal("UrlImage"))
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
            //try
            //{
            //    if (PhoneExists(customer.PhoneNumber))
            //    {
            //        return BadRequest("Phone number already exists.");
            //    }
            //    if (EmailExists(customer.Email))
            //    {
            //        return BadRequest("Email already exists.");
            //    }
            //    if (LoginNameExists(customer.CLoginName))
            //    {
            //        return BadRequest("Login name already exists.");
            //    }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO Customer (FirstName, MiddleName, LastName, Gender, BirthOfDate, PhoneNumber, Email, Address, TypeCustomer,UrlImage, CLoginName, Password) " +
                               "VALUES (@FirstName, @MiddleName, @LastName, @Gender, @BirthDate, @PhoneNumber, @Email, @Address, @TypeCustomer,@UrlImage, @CLoginName, @Password); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    string hashedPassword = HashPassword(password: customer.Password);
                    // SQL Injection
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", customer.MiddleName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                    cmd.Parameters.AddWithValue("@BirthDate", customer.BirthOfDate);
                    cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@TypeCustomer", customer.TypeCustomer);
                    cmd.Parameters.AddWithValue("@CLoginName", customer.CLoginName);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

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
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, "An error occurred while registering the customer.");
            //}
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
                               "Gender = @Gender, BirthDate = @BirthOfDate, PhoneNumber = @PhoneNumber, " +
                               "Email = @Email, Address = @Address, TypeCustomer = @TypeCustomer, " +
                               "CLoginName = @CLoginName, UrlImage = @UrlImage ";

                if (!string.IsNullOrEmpty(customer.Password))
                {
                    query += ", Password = @Password";
                    customer.Password = HashPassword(customer.Password);
                }

                query += " WHERE CustomerId = @CustomerId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", customer.MiddleName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                    cmd.Parameters.AddWithValue("@BirthDate", customer.BirthOfDate);
                    cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@TypeCustomer", customer.TypeCustomer);
                    cmd.Parameters.AddWithValue("@CLoginName", customer.CLoginName);
                    if (!string.IsNullOrEmpty(customer.Password))
                    {
                        cmd.Parameters.AddWithValue("@Password", customer.Password);
                    }
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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string CLoginName, [FromForm] string Password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT * FROM Customer WHERE CLoginName = @CLoginName AND Password = @Password;";

                    using (var command = new SqlCommand(query, connection))
                    {
                        string hashedPassword = HashPassword(Password);
                        command.Parameters.AddWithValue("@CLoginName", CLoginName);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Tìm thấy khách hàng trong cơ sở dữ liệu, đăng nhập thành công.
                                // Tiến hành tạo token JWT và trả về cho client.
                                var token = GenerateJwtToken(CLoginName);
                                return Ok(new { token });
                            }
                            else
                            {
                                // Không tìm thấy khách hàng trong cơ sở dữ liệu, đăng nhập thất bại.
                                return Unauthorized(new { message = "Đăng nhập không thành công." });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, "Lỗi trong quá trình xử lý.");
            }
        }

        private string GenerateJwtToken(string username)
        {
            var secretKey = "2023sem3";
            var issuer = "ss@123";
            var audience = "123mmm";
            var expirationDays = 7;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
    };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(expirationDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Chuyển đổi mật khẩu sang dạng byte[]
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Mã hóa mật khẩu và trả về dạng HEX (hexadecimal) string
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2")); // Chuyển byte sang dạng HEX
                }
                return builder.ToString();
            }
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

