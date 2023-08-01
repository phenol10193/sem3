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
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<SuppInvoice>> GetSuppInvoices()
        {

            var suppinvoices = new List<SuppInvoice>();
            string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM SuppInvoice;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var suppinvoice = new SuppInvoice
                            {
                                SuppInvoiceId = reader.GetInt32(reader.GetOrdinal("SuppInvoiceId")),
                                SuppInvoiceDate = reader.GetDateTime(reader.GetOrdinal("SuppInvoiceDate")),
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                ListRoom = reader.GetString(reader.GetOrdinal("ListRoom")),
                                PersonInvoice = reader.GetString(reader.GetOrdinal("PersonInvoice")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            suppinvoices.Add(suppinvoice);
                        }
                    }

                }
            }
            return suppinvoices;
        }
        [HttpPost("insert")]

        public async Task<IActionResult> InsertSuppInvoice([FromForm] SuppInvoice suppInvoice)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO SuppInvoice(SupInvoiceDate, SupplierId, ListRoom, PersonInvoice, Flag) VALUES  (@SupInvoiceDate, @SupplierId, @ListRoom, @PersonInvoice, @Flag)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@SuppInvoiceDate", suppInvoice.SuppInvoiceDate);
                    command.Parameters.AddWithValue("@SupplierId", suppInvoice.SupplierId);
                    command.Parameters.AddWithValue("@ListRoom", suppInvoice.ListRoom);
                    command.Parameters.AddWithValue("@PersonInvoice", suppInvoice.PersonInvoice);
                    command.Parameters.AddWithValue("@Flag", suppInvoice.Flag);
                    command.ExecuteNonQuery();

                }
            }
            return Ok(suppInvoice);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSuppInvoice([FromForm] SuppInvoice SuppInvoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE SuppInvoice SET SuppInvoiceDate = @SuppInvoiceDate, SupplierId = @SupplierId, ListRoom = @ListRoom, PersonInvoice = @PersonInvoice" +
                                "WHERE SuppInvoiceId = @SuppInvoiceId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SuppInvoiceDate", SuppInvoice.SuppInvoiceDate);
                    cmd.Parameters.AddWithValue("@SupplierId", SuppInvoice.SupplierId);
                    cmd.Parameters.AddWithValue("@ListRoom", SuppInvoice.ListRoom);
                    cmd.Parameters.AddWithValue("@PersonInvoice", SuppInvoice.PersonInvoice);
                    cmd.Parameters.AddWithValue("@SuppInvoiceId", SuppInvoice.SuppInvoiceId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
    }
}
