using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustInvoiceController : ControllerBase
    {
        string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

        [HttpGet("all")]
        public async Task<IEnumerable<CustInvoice>> GetCustInvoices()
        {
            var custInvoices = new List<CustInvoice>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM CustInvoice;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var custinvoice = new CustInvoice
                            {
                                InvoiceId = reader.GetInt32(reader.GetOrdinal("InvoiceId")),
                                InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                                CustOderSuppId = reader.GetInt32(reader.GetOrdinal("CustOderSuppId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                VAT = reader.GetFloat(reader.GetOrdinal("VAT")),
                                ListRoom=reader.GetString(reader.GetOrdinal("ListRoom")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            custInvoices.Add(custinvoice);
                        }
                    }

                }
            }
            return custInvoices;
        }
        [HttpPost("insert")]
        public async Task<IActionResult> InsertCustInvoice([FromForm] CustInvoice custInvoice)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO CustInvoice(InvoiceDate, CustOderSuppId, CustomerId, VAT, ListRoom, Flag) VALUES  (@InvoiceDate, @CusOderSuppId, @CustomerId, @VAT, @ListRoom, @Flag)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@InvoiceDate", custInvoice.InvoiceDate);
                    command.Parameters.AddWithValue("@CustOderSuppId", custInvoice.CustOderSuppId);
                    command.Parameters.AddWithValue("@CustomerId", custInvoice.CustomerId);
                    command.Parameters.AddWithValue("@VAT", custInvoice.VAT);
                    command.Parameters.AddWithValue("@ListRoom", custInvoice.ListRoom);
                    command.Parameters.AddWithValue("@Flag", custInvoice.Flag);
                    command.ExecuteNonQuery();

                }
            }
            return Ok(custInvoice);
        }

    }
}
