﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppMenuController : ControllerBase

    {
        string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<SuppMenu>> GetSuppMenu()
        {
            var suppmenu = new List<SuppMenu>();

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
                                UrlImage = reader.GetString(reader.GetOrdinal("UrlImage"))
                                Flag = reader.GetBoolean(reader.GetOrdinal("Flag"))
                            };
                            suppmenu.Add(suppMenu);
                        }
                    }

                }
            }
            return suppmenu;
        }

        [HttpPost("insert")]

        public async Task<IActionResult> InsertSuppMenu([FromForm] SuppMenu suppMenu)

        {
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
            
                var commandText = "INSERT INTO SuppMenu(ItemName, Price, CategoryId, SupplierId, UrlImage) VALUES  (@ItemName, @Price, @CategoryId, @SupplierId, @UrlImage)";
                
                using (var command =new SqlCommand(commandText,connection) )
                {
                    command.Parameters.AddWithValue("@ItemName", suppMenu.ItemName);
                    command.Parameters.AddWithValue("@Price", suppMenu.Price);
                    command.Parameters.AddWithValue("@CategoryId", suppMenu.CategoryId);
                    command.Parameters.AddWithValue("@SupplierId", suppMenu.SupplierId);
                    var file = HttpContext.Request.Form.Files.FirstOrDefault();

                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                        var filePath = Path.Combine("uploads", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        command.Parameters.AddWithValue("@UrlImage", filePath);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@UrlImage", DBNull.Value);
                    }
                    command.ExecuteNonQuery();

               }
            }
            return Ok(suppMenu);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSuppMenu([FromForm] SuppMenu SuppMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE SuppMenu SET ItemName = @ItemName, Price = @Price, CategoryId = @CategoryId, SupplierId = @SupplierId, UrlImage =@UrlImage" +
                                "WHERE MenuItemId = @MenuItemId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ItemName", SuppMenu.ItemName);
                    cmd.Parameters.AddWithValue("@Price", SuppMenu.Price);
                    cmd.Parameters.AddWithValue("@CategoryId", SuppMenu.CategoryId);
                    cmd.Parameters.AddWithValue("@SupplierId", SuppMenu.SupplierId);
                    cmd.Parameters.AddWithValue("@MenuItemId", SuppMenu.MenuItemId);
                    // Kiểm tra xem khách hàng có gửi file ảnh mới không
                    if (SuppMenu.ImageFile != null && SuppMenu.ImageFile.Length > 0)
                    if (SuppMenu.ImageFile != null && SuppMenu.ImageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(SuppMenu.ImageFile.FileName);
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        var filePath = Path.Combine("uploads", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await SuppMenu.ImageFile.CopyToAsync(stream);
                        }

                        cmd.Parameters.AddWithValue("@UrlImage", filePath);
                    }
                    else
                    {
                        // Nếu không có ảnh mới được gửi, giữ nguyên ảnh cũ
                        cmd.Parameters.AddWithValue("@UrlImage", SuppMenu.UrlImage);
                    }
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" updated successfully.");

        }
        [HttpPut("{menuItemId}")]
        public async Task<IActionResult> DeleteSuppMenu(int menuItemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE SuppMenu SET Flag = @Flag WHERE MenuItemId = @MenuItemId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Flag", true);
                    cmd.Parameters.AddWithValue("@MenuItemId", menuItemId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok(" delete successfully.");

        }
    }
}
