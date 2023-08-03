using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustOderSuppController : ControllerBase
    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

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
                                CustOderSuppId = reader.GetInt32(reader.GetOrdinal("CustOderSuppId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                RoomId=reader.GetInt32(reader.GetOrdinal("RoomId")),
                                DeliveryDate = reader.GetDateTime(reader.GetOrdinal("DeliveryDate")),
                                VAT = reader.GetDouble(reader.GetOrdinal("VAT")),
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

                var commandText = "INSERT INTO CustOderSupp(CustomerId, RoomId, DeliveryDate, VAT, Status, NumPeople) VALUES  (@CustomerId, @RoomId, @DeliveryDate, @VAT, @Status, @NumPeople)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", custOrdSupp.CustomerId);
                    command.Parameters.AddWithValue("@RoomId", custOrdSupp.RoomId);
                    command.Parameters.AddWithValue("@DeliveryDate", custOrdSupp.DeliveryDate);
                    command.Parameters.AddWithValue("@VAT", custOrdSupp.VAT);
                    command.Parameters.AddWithValue("@Status", custOrdSupp.Status);
                    command.Parameters.AddWithValue("@NumPeople", custOrdSupp.NumPeople);
                    
                    command.ExecuteNonQuery();

                }
            }
            return Ok(custOrdSupp);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustOderSupp([FromForm] CustOderSupp CustOderSupp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE CustOderSupp SET CustomerId = @CustomerId, RoomId = @RoomId, DeliveryDate = @DeliveryDate, VAT = @VAT, Status = @Status, NumPeople=@NumPeople " +
                                "WHERE CustOderSuppId = @CustOderSuppId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", CustOderSupp.CustomerId);
                    cmd.Parameters.AddWithValue("@RoomId", CustOderSupp.RoomId);
                    cmd.Parameters.AddWithValue("@DeliveryDate", CustOderSupp.DeliveryDate);
                    cmd.Parameters.AddWithValue("@VAT", CustOderSupp.VAT);
                    cmd.Parameters.AddWithValue("@Status", CustOderSupp.Status);
                    cmd.Parameters.AddWithValue("@NumPeople", CustOderSupp.NumPeople);
                    cmd.Parameters.AddWithValue("@CustOderSuppId", CustOderSupp.CustOderSuppId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{custOderSuppId}")]
        public async Task<IActionResult> DeleteCustOderSupp(int custOderSuppId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE CustOderSupp SET Flag = @Flag WHERE CustOderSuppId = @CustOderSuppId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@CustOderSuppId", custOderSuppId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
