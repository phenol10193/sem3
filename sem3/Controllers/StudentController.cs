using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem3.Models;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpPost("insert")]
        public async Task<IActionResult> InsertStudent([FromForm] Student student)
        {
            string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Student (Name, Age, ImagePath) VALUES (@Name, @Age, @ImagePath); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@Age", student.Age);


                    // var file = student.ImagePath; // Get the uploaded file from the student object
                    var file = HttpContext.Request.Form.Files.FirstOrDefault();

                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                        // Generate a unique file name
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                        // Set the file path where the image will be saved on the server
                        var filePath = Path.Combine("uploads", uniqueFileName);

                        // Save the file to the s
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        command.Parameters.AddWithValue("@ImagePath", filePath);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    }
                    int insertedStudentId = Convert.ToInt32(await command.ExecuteScalarAsync());
                    student.StudentId = insertedStudentId;

                }
            }
            return Ok(student);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateStudent([FromForm]Student student)
        {
            string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Kiểm tra xem sinh viên có tồn tại trong cơ sở dữ liệu không
                var checkStudentExistsQuery = "SELECT COUNT(*) FROM Student WHERE StudentId = @StudentId";
                using (var checkStudentExistsCommand = new SqlCommand(checkStudentExistsQuery, connection))
                {
                    checkStudentExistsCommand.Parameters.AddWithValue("@StudentId", student.StudentId);
                    int studentCount = (int)await checkStudentExistsCommand.ExecuteScalarAsync();

                    // Nếu sinh viên không tồn tại, trả về mã lỗi 404 (Not Found)
                    if (studentCount == 0)
                    {
                        return NotFound("Student not found.");
                    }
                }

                // Thực hiện câu lệnh UPDATE để cập nhật thông tin của sinh viên
                var updateQuery = "UPDATE Student SET Name = @Name, Age = @Age, ImagePath = @ImagePath WHERE StudentId = @StudentId";
                using (var command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@Age", student.Age);
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

                        command.Parameters.AddWithValue("@ImagePath", filePath);
                    }
                    else
                    {
                        // Nếu không có ảnh mới được gửi, giữ nguyên ảnh cũ
                        command.Parameters.AddWithValue("@ImagePath", student.ImagePath);
                    }

                    command.Parameters.AddWithValue("@StudentId", student.StudentId);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok("Student updated successfully.");
        }

        [HttpGet("{studentId}")]
        public async Task<Student> GetStudent(int studentId)
        {
            Student student = null;
            string _connectionString = "Server=103.74.120.121;Database=students;User Id=tmp;Password=tmp@123;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Student WHERE StudentId = @StudentId;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {    
                        
                            student = new Student
                            {
                                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                                Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                
                               // ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath"))
                            };
                        }
                    }
                }
            }

            return student;
        }
        

        

    }
}
