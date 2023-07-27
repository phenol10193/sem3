using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

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

                var commandText = "INSERT INTO Message(CustomerId, CContent, SentDate, Flag) VALUES  (@MenuItemId, @CustOrderSuppId, @Flag)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", message.CustomerId);
                    command.Parameters.AddWithValue("@CContent", message.CContent);
                    command.Parameters.AddWithValue("@SentDate", message.SentDate);
                    command.Parameters.AddWithValue("@Flag", message.Flag);

                    command.ExecuteNonQuery();

                }
            }
            return Ok(message);
        }
    }
}
