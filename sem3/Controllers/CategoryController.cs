using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

        [HttpGet("all")]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories= new List<Category>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Category;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var category = new Category
                            {
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ParentId = reader.GetInt32(reader.GetOrdinal("ParentId")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            categories.Add(category);
                        }
                    }

                }
            }
            return categories ;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertCategory([FromForm] Category category)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Category(Name, ParentId, Flag) VALUES  (@Name, @ParentId, @Flag)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Name", category.Name);
                    command.Parameters.AddWithValue("@ParentId", category.ParentId);
                    command.Parameters.AddWithValue("@Flag", category.Flag);
                    command.ExecuteNonQuery();

                }
            }
            return Ok(category);
        }
    }
}
