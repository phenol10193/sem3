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
        string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

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
        public async Task<IActionResult> InsertCustInvoice([FromForm] CustOderMenu custOrderMenu)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO CustOderMenu(MenuItemId, CustOderSuppId, Flag) VALUES  (@MenuItemId, @CustOderSuppId, @Flag)";

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

    }
}
