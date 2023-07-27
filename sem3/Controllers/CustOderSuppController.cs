using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustOderSuppController : ControllerBase
    {
        string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

        [HttpGet("all")]
        public async Task<IEnumerable<CustOderSupp>> GetCustOderSupps()
        {

            var custOderSupp = new List<CustOderSupp>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM CustOderSupp;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var custordsupp = new CustOderSupp
                            {
                                CustOderSuppId = reader.GetInt32(reader.GetOrdinal("CustOrderSuppId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                SuppDetailId = reader.GetInt32(reader.GetOrdinal("SuppDetailId")),
                                DeliveryDate = reader.GetDateTime(reader.GetOrdinal("DeliveryDate")),
                                VAT = reader.GetFloat(reader.GetOrdinal("VAT")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                NumPeople = reader.GetInt32(reader.GetOrdinal("NumPeople")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            custOderSupp.Add(custordsupp);
                        }
                    }

                }
            }
            return custOderSupp;
        }
        [HttpPost("insert")]

        public async Task<IActionResult> InsertSuppInvoice([FromForm] CustOderSupp custOrdSupp)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO CustOderSupp(CustomerId, SuppDetailId, DeliveryDate, VAT, Status, NumPeople, Flag) VALUES  (@CustomerId, @SuppDetailId, @DeliveryDate, @VAT, @Status, @NumPeople, @Flag)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", custOrdSupp.CustomerId);
                    command.Parameters.AddWithValue("@SuppDetailId", custOrdSupp.SuppDetailId);
                    command.Parameters.AddWithValue("@DeliveryDate", custOrdSupp.DeliveryDate);
                    command.Parameters.AddWithValue("@VAT", custOrdSupp.VAT);
                    command.Parameters.AddWithValue("@Status", custOrdSupp.Status);
                    command.Parameters.AddWithValue("@NumPeople", custOrdSupp.NumPeople);
                    command.Parameters.AddWithValue("@Flag", custOrdSupp.Flag);
                    command.ExecuteNonQuery();

                }
            }
            return Ok(custOrdSupp);
        }

    }
}
