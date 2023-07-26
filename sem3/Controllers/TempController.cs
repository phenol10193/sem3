using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] temp tmp)
        {

            string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.OpenAsync();

                string query = "insert into temp(id,full_name) values(1,'B');";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    // SQL Injection
                    cmd.Parameters.AddWithValue("@FirstName", tmp.full_name);

                   
                    int insertedId = Convert.ToInt32(cmd.ExecuteScalarAsync());
                    tmp.Id = insertedId;
                }


                return Ok(tmp);

            }
        }
    }
}
