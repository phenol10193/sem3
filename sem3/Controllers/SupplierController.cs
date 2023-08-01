using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            var suppliers = new List<Supplier>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Supplier;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var supplier = new Supplier
                            {
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                SName = reader.GetString(reader.GetOrdinal("SName")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                SLevel = reader.GetString(reader.GetOrdinal("SLevel")),
                                ACityId = reader.GetInt32(reader.GetOrdinal("ACityId")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag")),
                                Image=reader.GetString(reader.GetOrdinal("Image")),
                                SLoginName = reader.GetString(reader.GetOrdinal("SLoginName")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),

                            };
                            suppliers.Add(supplier);
                        }
                    }

                }
            }
            return suppliers;
        }
        [HttpPost("insert")]

        public async Task<IActionResult> InsertSupplier([FromForm] Supplier suppliers)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Supplier(SName, PhoneNumber, Address, Email, SLevel, ACityId, Flag, Image, SLoginName, Password) VALUES  (@SName, @PhoneNumber, @Address, @Email, @SLevel, @ACityId, @Flag, @Image, @SLoginName, @Password)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@SName", suppliers.SName);
                    command.Parameters.AddWithValue("@PhoneNumber", suppliers.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", suppliers.Address);
                    command.Parameters.AddWithValue("@Email", suppliers.Email);
                    command.Parameters.AddWithValue("@Slevel", suppliers.SLevel);
                    command.Parameters.AddWithValue("@ACityId", suppliers.ACityId);
                    command.Parameters.AddWithValue("@Flag", suppliers.Flag);
                    command.Parameters.AddWithValue("@Image", suppliers.Image);
                    command.Parameters.AddWithValue("@SLoginName", suppliers.SLoginName);
                    command.Parameters.AddWithValue("@Password", suppliers.Password);
                    command.ExecuteNonQuery();

                }
            }
            return Ok(suppliers);
        }
    }
}
