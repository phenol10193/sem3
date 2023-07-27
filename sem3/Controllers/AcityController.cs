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
        string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";
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
                            var acity= new Acity
                            {
                                AcityId = reader.GetInt32(reader.GetOrdinal("AcityId")),
                                CityName = reader.GetString(reader.GetOrdinal("CityName")),
                                ParentId = reader.GetInt32(reader.GetOrdinal("ParentId")),                             
                            };
                            Acitys.Add(acity);
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

                var commandText = "INSERT INTO Acity(CityName, ParentId, Flag) VALUES  (@CityName, @ParentId , @Flag)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CityName", acity.CityName);
                    command.Parameters.AddWithValue("@ParentId", acity.ParentId);  
                    command.Parameters.AddWithValue("@Flag", acity.Flag);
                    command.ExecuteNonQuery();

                }
            }
            return Ok(acity);
        }
    }
}
