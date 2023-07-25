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
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            customers.Add(customer);
                        }
                    }

                }
            }
            return customers;
        }
       
        
        [HttpPost("register")]
        public IActionResult RegisterCustomer([FromBody] Customer customer)
        {
            // Kiểm tra xem dữ liệu đăng ký khách hàng hợp lệ hay không.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
  
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Mở kết nối
                    connection.Open();
             
                    string query = "INSERT INTO Customers (FirstName, MiddleName, LastName, Gender, BirthDate, PhoneNumber, Email, Address, Type, LoginName, Password) " +
                                   "VALUES (@FirstName, @MiddleName, @LastName, @Gender, @BirthDate, @PhoneNumber, @Email, @Address, @Type, @LoginName, @Password)";

                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Thêm các tham số cho câu truy vấn để tránh lỗ hổng SQL Injection
                        cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        cmd.Parameters.AddWithValue("@MiddleName", customer.MiddleName);
                        cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                        cmd.Parameters.AddWithValue("@BirthDate", customer.BirthDate);
                        cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", customer.Email);
                        cmd.Parameters.AddWithValue("@Address", customer.Address);
                        cmd.Parameters.AddWithValue("@Type", customer.TypeCustomer);
                        cmd.Parameters.AddWithValue("@LoginName", customer.CLoginName);
                        cmd.Parameters.AddWithValue("@Password", customer.Password);

                        
                        cmd.ExecuteNonQuery();
                    }
                }

               
                return Ok(customer);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "An error occurred while registering the customer.");
            }
        }
    }

}

