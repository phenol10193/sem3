using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcityController : ControllerBase
    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";
        [HttpGet("all")]
        public async Task<IEnumerable<Acity>> GetAcitys()
        {
            var Acitys = new List<Acity>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Acity;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var city = new Acity
                            {
                                AcityId = reader.IsDBNull(reader.GetOrdinal("AcityId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AcityId")),
                                CityName = reader.IsDBNull(reader.GetOrdinal("CityName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CityName")),
                                ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ParentId")),
                                Flag = reader.IsDBNull(reader.GetOrdinal("Flag")) ? false : reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            Acitys.Add(city);
                        }
                    }

                }
            }
            return Acitys;
        }
        [HttpGet("AcityName")]
        public async Task<IEnumerable<Acity>> GetAcities()
        {
            var Acitys = new List<Acity>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT AcityId,CityName FROM Acity;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var city = new Acity
                            {
                                AcityId = reader.IsDBNull(reader.GetOrdinal("AcityId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AcityId")),
                                CityName = reader.IsDBNull(reader.GetOrdinal("CityName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CityName")),
                              
                            };
                            Acitys.Add(city);
                        }
                    }

                }
            }
            return Acitys;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertAcity([FromForm] Acity acity)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Acity(CityName, ParentId) VALUES  (@CityName, @ParentId )";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CityName", acity.CityName);
                    command.Parameters.AddWithValue("@ParentId", acity.ParentId);                    
                    command.ExecuteNonQuery();

                }
            }
            return Ok(acity);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAcity([FromForm] Acity Acity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Acity SET CityName = @CityName, ParentId = @ParentId " +
                                "WHERE AcityId = @AcityId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@AcityId", Acity.AcityId);
                    cmd.Parameters.AddWithValue("@CityName", Acity.CityName);
                    cmd.Parameters.AddWithValue("@ParentId", Acity.ParentId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{acityId}")]
        public async Task<IActionResult> DeleteAcity(int acityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Acity SET Flag = @Flag WHERE AcityId = @AcityId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@AcityId", acityId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
