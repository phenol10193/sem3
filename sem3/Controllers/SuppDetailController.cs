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
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";
        [HttpGet("all")]
        public async Task<IEnumerable<SuppDetail>> GetSuppDetails()
        {

            var SuppDetails = new List<SuppDetail>();

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
                            var SuppDetail = new SuppDetail
                            {
                                SuppDetailId = reader.GetInt32(reader.GetOrdinal("SuppDetailId")),
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                NameDetail = reader.GetString(reader.GetOrdinal("NameDetail")),
                                NumPeople = reader.GetInt32(reader.GetOrdinal("NumPeople")),
                                CustomerCost = reader.GetDouble(reader.GetOrdinal("CustomerCost")),
                                SupplierCost = reader.GetDouble(reader.GetOrdinal("SupplierCost")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag")),
                            };
                            SuppDetails.Add(SuppDetail);
                        }
                    }

                }
            }
            return SuppDetails;
        }
        [HttpGet("selectNameDetail")]
        public async Task<IEnumerable<SuppDetail>> GetSuppDetail()
        {
            var suppdetails = new List<SuppDetail>();


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT SuppDetailId, NameDetail FROM SuppDetail;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var suppdetail = new SuppDetail
                            {
                                SuppDetailId = reader.GetInt32(reader.GetOrdinal("SuppDetailId")),
                               
                                NameDetail = reader.GetString(reader.GetOrdinal("NameDetail")),
                                
                            };
                            suppdetails.Add(suppdetail);
                        }
                    }

                }
            }
            return suppdetails;
        }
        [HttpPost("insert")]

        public async Task<IActionResult> InsertSuppDetail([FromForm] SuppDetail suppDetail)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO SuppDetail(SupplierId, NameDetail, NumPeople, CustomerCost, SupplierCost ) VALUES  (@SupplierId, @NameDetail, @NumPeople, @CustomerCost, @SupplierCost )";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@SupplierId", suppDetail.SupplierId);
                    command.Parameters.AddWithValue("@NameDetail", suppDetail.NameDetail);
                    command.Parameters.AddWithValue("@NumPeople", suppDetail.NumPeople);
                    command.Parameters.AddWithValue("@CustomerCost", suppDetail.CustomerCost);
                    command.Parameters.AddWithValue("@SupplierCost", suppDetail.SupplierCost);
                   
                    command.ExecuteNonQuery();

                }
            }
            return Ok(suppDetail);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSuppDetail([FromForm] SuppDetail SuppDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE SuppDetail SET SupplierId = @SupplierId, NameDetail = @NameDetail, NumPeople = @NumPeople, CustomerCost = @CustomerCost, SupplierCost = @SupplierCost " +
                                "WHERE SuppDetailId = @SuppDetailId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SupplierId", SuppDetail.SupplierId);
                    cmd.Parameters.AddWithValue("@NameDetail", SuppDetail.NameDetail);
                    cmd.Parameters.AddWithValue("@NumPeople", SuppDetail.NumPeople);
                    cmd.Parameters.AddWithValue("@CustomerCost", SuppDetail.CustomerCost);
                    cmd.Parameters.AddWithValue("@SupplierCost", SuppDetail.SupplierCost);
                    cmd.Parameters.AddWithValue("@SuppDetailId", SuppDetail.SuppDetailId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{suppDetailId}")]
        public async Task<IActionResult> DeleteSuppDetail(int suppDetailId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE SuppDetail SET Flag = @Flag WHERE SuppDetailId = @SuppDetailId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@SuppDetailId", suppDetailId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
