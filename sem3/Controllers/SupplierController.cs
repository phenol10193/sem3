using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;
namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("selectSName")]
        public async Task<IEnumerable<Supplier>> GetSupplierName()
        {
            var suppliers = new List<Supplier>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT SupplierId,SName FROM Supplier;";

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

