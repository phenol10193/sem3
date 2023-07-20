using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppDetailController : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IEnumerable<SuppDetail>> GetSuppDetails()
        {
            var suppdetails = new List<SuppDetail>();
            string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM SuppDetail;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var suppdetail = new SuppDetail
                            {
                                SuppDetailId = reader.GetInt32(reader.GetOrdinal("SuppInvoiceId")),
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                NameDetail = reader.GetString(reader.GetOrdinal("ListRoom")),
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
