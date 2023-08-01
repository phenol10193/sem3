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
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

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

                var commandText = "INSERT INTO CustInvoice(InvoiceDate, CustOderSuppId, CustomerId, VAT, ListRoom) VALUES  (@InvoiceDate, @CustOderSuppId, @CustomerId, @VAT, @ListRoom)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@InvoiceDate", custInvoice.InvoiceDate);
                    command.Parameters.AddWithValue("@CustOderSuppId", custInvoice.CustOderSuppId);
                    command.Parameters.AddWithValue("@CustomerId", custInvoice.CustomerId);
                    command.Parameters.AddWithValue("@VAT", custInvoice.VAT);
                    command.Parameters.AddWithValue("@ListRoom", custInvoice.ListRoom);
                    
                    command.ExecuteNonQuery();

                }
            }
            return Ok(custInvoice);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustInvoice([FromForm] CustInvoice CustInvoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE CustInvoice SET InvoiceDate = @InvoiceDate, CustOderSuppId = @CustOderSuppId, CustomerId = @CustomerId, VAT = @VAT, ListRoom = @ListRoom" +
                                "WHERE InvoiceId = @InvoiceId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@InvoiceDate", CustInvoice.InvoiceDate);
                    cmd.Parameters.AddWithValue("@CustOderSuppId", CustInvoice.CustOderSuppId);
                    cmd.Parameters.AddWithValue("@CustomerId", CustInvoice.CustomerId);
                    cmd.Parameters.AddWithValue("@VAT", CustInvoice.VAT);
                    cmd.Parameters.AddWithValue("@ListRoom", CustInvoice.ListRoom);
                    cmd.Parameters.AddWithValue("@InvoiceId", CustInvoice.InvoiceId);                  
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{invoiceId}")]
        public async Task<IActionResult> DeleteCustInvoice(int invoiceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE CustInvoice SET Flag = @Flag WHERE InvoiceId = @InvoiceId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }

}
