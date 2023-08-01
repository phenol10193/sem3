using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Message>> GetMessages()
        {
            var messages = new List<Message>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Message;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var message = new Message
                            {
                                MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                CContent = reader.GetString(reader.GetOrdinal("CContent")),
                                SentDate = reader.GetDateTime(reader.GetOrdinal("SentDate")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            messages.Add(message);
                        }
                    }

                }
            }

            return messages;
        }
        [HttpPost("insert")]
        public async Task<IActionResult> InsertCustInvoice([FromForm] Message message)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Message(CustomerId, CContent, SentDate) VALUES (@MenuItemId, @CustOrderSuppId)";
                                  
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", message.CustomerId);
                    command.Parameters.AddWithValue("@CContent", message.CContent);
                    command.Parameters.AddWithValue("@SentDate", message.SentDate);
                    

                    command.ExecuteNonQuery();

                }
            }
            return Ok(message);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMessage([FromForm] Message Message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Message SET CustomerId = @CustomerId, CContent = @CContent, SentDate=@SentDate " +
                                "WHERE MessageId = @MessageId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", Message.CustomerId);
                    cmd.Parameters.AddWithValue("@CContent", Message.CContent);
                    cmd.Parameters.AddWithValue("@SentDate", Message.SentDate);
                    cmd.Parameters.AddWithValue("@MessageId", Message.MessageId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Message SET Flag = @Flag WHERE MessageId = @MessageId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@MessageId", messageId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
