using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppMenuController : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IEnumerable<SuppMenu>> GetSuppMenu()
        {
            var suppmenu = new List<SuppMenu>();
            string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM SuppMenu;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var suppMenu = new SuppMenu
                            {
                                MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                                ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                                Price = reader.GetFloat(reader.GetOrdinal("Price")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            suppmenu.Add(suppMenu);
                        }
                    }

                }
            }
            return suppmenu;
        }
    }
}
