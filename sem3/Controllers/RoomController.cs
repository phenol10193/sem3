using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Room>> GetRooms()
        {
            var Rooms = new List<Room>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Room;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var Room = new Room
                            {
                                RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                RoomName = reader.GetString(reader.GetOrdinal("RoomName")),
                                Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                RoomPrice = reader.GetFloat(reader.GetOrdinal("RoomPrice")),
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            Rooms.Add(Room);
                        }
                    }

                }
            }

            return Rooms;
        }
        [HttpPost("insert")]
        public async Task<IActionResult> InsertCustInvoice([FromForm] Room Room)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Room(RoomName, Capacity, RoomPrice, ServiceId) VALUES (@RoomName, @Capacity, @RoomPrice, @ServiceId)";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@RoomName", Room.RoomName);
                    command.Parameters.AddWithValue("@Capacity", Room.Capacity);
                    command.Parameters.AddWithValue("@RoomPrice", Room.RoomPrice);
                    command.Parameters.AddWithValue("@ServiceId", Room.ServiceId);

                    command.ExecuteNonQuery();

                }
            }
            return Ok(Room);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRoom([FromForm] Room Room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Room SET RoomName = @RoomName, Capacity = @Capacity, RoomPrice=@RoomPrice, ServiceId=@ServiceId " +
                                "WHERE RoomId = @RoomId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@RoomName", Room.RoomName);
                    cmd.Parameters.AddWithValue("@Capacity", Room.Capacity);
                    cmd.Parameters.AddWithValue("@RoomPrice", Room.RoomPrice);
                    cmd.Parameters.AddWithValue("@ServiceId", Room.ServiceId);
                    cmd.Parameters.AddWithValue("@RoomId", Room.RoomId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{RoomId}")]
        public async Task<IActionResult> DeleteRoom(int RoomId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Room SET Flag = @Flag WHERE RoomId = @RoomId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@RoomId", RoomId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
