using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppInvoiceController : ControllerBase
    {

        [HttpGet("all")]
        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            var suppliers = new List<Supplier>();
            string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

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
                                Slever = reader.GetString(reader.GetOrdinal("Slever")),
                                ACityId = reader.GetInt32(reader.GetOrdinal("ACityId")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            suppliers.Add(supplier);
                        }
                    }

                }
            }
            return suppliers;
        }
    }
}
