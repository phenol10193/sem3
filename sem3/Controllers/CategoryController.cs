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

        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = new List<Category>();

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
            return categories;
        }
        [HttpGet("Categories")]
        public async Task<IEnumerable<Category>> GetCategoriesName()
        {
            var categories = new List<Category>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT CategoryId,Name FROM Category;";

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
                               
                            };
                            categories.Add(category);
                        }
                    }

                }
            }
            return categories;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertCategory([FromForm] Category category)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Category(Name, ParentId) VALUES  (@Name, @ParentId)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Name", category.Name);
                    command.Parameters.AddWithValue("@ParentId", category.ParentId);
                   
                    command.ExecuteNonQuery();

                }
            }
            return Ok(category);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromForm] Category Category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //try
            //{
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Category SET Name = @Name, ParentId = @ParentId WHERE CategoryId = @CategoryId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", Category.CategoryId);
                    cmd.Parameters.AddWithValue("@Name", Category.Name);
                    cmd.Parameters.AddWithValue("@ParentId", Category.ParentId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Category SET Flag = @Flag WHERE CategoryId = @CategoryId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");
        
        }
    }
}

