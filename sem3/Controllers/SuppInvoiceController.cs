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
    }
}
