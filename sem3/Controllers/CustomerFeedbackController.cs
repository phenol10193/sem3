using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerFeedbackController : ControllerBase
    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<CustomerFeedback>> GetCustomerFeedbacks()
        {
            var CustomerFeedbacks = new List<CustomerFeedback>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM CustomerFeedback;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var CustomerFeedback = new CustomerFeedback
                            {
                                FeedbackId = reader.GetInt32(reader.GetOrdinal("FeedbackId")),
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                Comment = reader.GetString(reader.GetOrdinal("Comment")),
                                FeedbackDate= reader.GetDateTime(reader.GetOrdinal("FeedbackDate")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            CustomerFeedbacks.Add(CustomerFeedback);
                        }
                    }

                }
            }

            return CustomerFeedbacks;
        }
        [HttpPost("insert")]
        public async Task<IActionResult> InsertCustInvoice([FromForm] CustomerFeedback CustomerFeedback)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO CustomerFeedback(SupplierId, CustomerId, Comment, FeedbackDate) VALUES (@SupplierId, @CustomerId, @Comment, @FeedbackDate)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@SupplierId", CustomerFeedback.SupplierId);
                    command.Parameters.AddWithValue("@CustomerId", CustomerFeedback.CustomerId);
                    command.Parameters.AddWithValue("@Comment", CustomerFeedback.Comment);
                    command.Parameters.AddWithValue("@FeedbackDate", CustomerFeedback.FeedbackDate);


                    command.ExecuteNonQuery();

                }
            }
            return Ok(CustomerFeedback);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustomerFeedback([FromForm] CustomerFeedback CustomerFeedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE CustomerFeedback SET SupplierId = @SupplierId, CustomerId = @CustomerId, Comment=@Comment, FeedbackDate=@FeedbackDate " +
                                "WHERE FeedbackId = @FeedbackId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SupplierId", CustomerFeedback.SupplierId);
                    cmd.Parameters.AddWithValue("@CustomerId", CustomerFeedback.CustomerId);
                    cmd.Parameters.AddWithValue("@Comment", CustomerFeedback.Comment);
                    cmd.Parameters.AddWithValue("@FeedbackDate", CustomerFeedback.FeedbackDate);
                    cmd.Parameters.AddWithValue("@FeedbackId", CustomerFeedback.FeedbackId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{feedbackId}")]
        public async Task<IActionResult> DeleteCustomerFeedback(int feedbackId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE CustomerFeedback SET Flag = @Flag WHERE FeedbackId = @FeedbackId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@FeedbackId", feedbackId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
