using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustOderMenuController : ControllerBase
    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<CustOderMenu>> GetCustOderMenus()
        {
            var custordermenus = new List<CustOderMenu>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM CustOderMenu;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var custordermenu = new CustOderMenu
                            {
                                CustOderMenuId = reader.GetInt32(reader.GetOrdinal("CustOderMenuId")),
                                MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                                CustOderSuppId = reader.GetInt32(reader.GetOrdinal("CustOderSuppId")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            custordermenus.Add(custordermenu);
                        }
                    }

                }
            }
            return custordermenus;
        }
        
        [HttpPost("insert")]
        public async Task<IActionResult> InsertCustOderMenu([FromForm] CustOderMenu custOrderMenu)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO CustOderMenu(MenuItemId, CustOderSuppId) VALUES  (@MenuItemId, @CustOderSuppId)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@MenuItemId", custOrderMenu.MenuItemId);
                    command.Parameters.AddWithValue("@CustOderSuppId", custOrderMenu.CustOderSuppId);                              
                    command.Parameters.AddWithValue("@Flag", custOrderMenu.Flag);
                    command.ExecuteNonQuery();

                }
            }
            return Ok(custOrderMenu);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustOderMenu([FromForm] CustOderMenu CustOderMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE CustOderMenu SET MenuItemId = @MenuItemId, CustOderSuppId = @CustOderSuppId" +
                                "WHERE CustOderMenuId = @CustOderMenuId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@MenuItemId", CustOderMenu.MenuItemId);
                    cmd.Parameters.AddWithValue("@CustOderSuppId", CustOderMenu.CustOderSuppId);
                    cmd.Parameters.AddWithValue("@CustOderMenuId", CustOderMenu.CustOderMenuId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{custOderMenuId}")]
        public async Task<IActionResult> DeleteCustOderMenu(int custOderMenuId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE CustOderMenu SET Flag = @Flag WHERE CustOderMenuId = @CustOderMenuId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@CustOderMenuId", custOderMenuId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
