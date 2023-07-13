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
        [HttpGet("all")]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customers = new List<Customer>();
            string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

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
    }
}
