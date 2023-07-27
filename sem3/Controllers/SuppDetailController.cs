using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppDetailController : ControllerBase
    {
        string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";
        [HttpGet("all")]
        public async Task<IEnumerable<SuppDetail>> GetSuppDetails()
        {
            var suppdetails = new List<SuppDetail>();
            

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM SuppDetail;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var suppdetail = new SuppDetail
                            {
                                SuppDetailId = reader.GetInt32(reader.GetOrdinal("SuppInvoiceId")),
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                NameDetail = reader.GetString(reader.GetOrdinal("ListRoom")),
                                NumPeople = reader.GetInt32(reader.GetOrdinal("NumPeople")),
                                CustomerCost = reader.GetFloat(reader.GetOrdinal("CustomerCost")),
                                SupplierCost= reader.GetFloat(reader.GetOrdinal("SupplierCost")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            suppdetails.Add(suppdetail);
                        }
                    }

                }
            }
            return suppdetails;
        }
        [HttpPost("insert")]

        public async Task<IActionResult> InsertSuppInvoice([FromForm] SuppDetail suppDetail)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO SuppDetail(SupplierId, NameDetail, NumPeople, CustomerCost, SupplierCost, Flag) VALUES  (@SupplierId, @NameDetail, @NumPeople, @CustomerCost, @SupplierCost, @Flag)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@SupplierId", suppDetail.SupplierId);
                    command.Parameters.AddWithValue("@NameDetail", suppDetail.NameDetail);
                    command.Parameters.AddWithValue("@NumPeople", suppDetail.NumPeople);
                    command.Parameters.AddWithValue("@CustomerCost", suppDetail.CustomerCost);
                    command.Parameters.AddWithValue("@SupplierCost", suppDetail.SupplierCost);
                    command.Parameters.AddWithValue("@Flag", suppDetail.Flag);
                    command.ExecuteNonQuery();

                }
            }
            return Ok(suppDetail);
        }
    }
}
