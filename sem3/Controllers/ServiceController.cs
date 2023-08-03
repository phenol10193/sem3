using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Service>> GetServices()
        {
            var Services = new List<Service>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Service;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var Service = new Service
                            {
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                ServiceName = reader.GetString(reader.GetOrdinal("ServiceName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            Services.Add(Service);
                        }
                    }

                }
            }

            return Services;
        }
        [HttpGet("selectServiceName")]
        public async Task<IEnumerable<Service>> GetService()
        {
            var Services = new List<Service>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT ServiceId, ServiceName FROM Service;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var Service = new Service
                            {
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                ServiceName = reader.GetString(reader.GetOrdinal("ServiceName")),

                            };
                            Services.Add(Service);
                        }
                    }

                }
            }

            return Services;
        }
        [HttpPost("insert")]
        public async Task<IActionResult> InsertCustInvoice([FromForm] Service Service)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Service(ServiceName, Description, SupplierId) VALUES (@ServiceName, @Description, @SupplierId)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@ServiceName", Service.ServiceName);
                    command.Parameters.AddWithValue("@Description", Service.Description);
                    command.Parameters.AddWithValue("@SupplierId", Service.SupplierId);
                   

                    command.ExecuteNonQuery();

                }
            }
            return Ok(Service);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateService([FromForm] Service Service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Service SET ServiceName = @ServiceName, Description = @Description, SupplierId=@SupplierId " +
                                "WHERE ServiceId = @ServiceId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ServiceName", Service.ServiceName);
                    cmd.Parameters.AddWithValue("@Description", Service.Description);
                    cmd.Parameters.AddWithValue("@SupplierId", Service.SupplierId);
                    cmd.Parameters.AddWithValue("@ServiceId", Service.ServiceId);
                    
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{ServiceId}")]
        public async Task<IActionResult> DeleteService(int ServiceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Service SET Flag = @Flag WHERE ServiceId = @ServiceId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@ServiceId", ServiceId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
