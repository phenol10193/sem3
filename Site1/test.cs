using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // Chuỗi kết nối cơ sở dữ liệu SQL Server
        private readonly string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCatere;User Id=Group4Catere;Password=@Hieu2104;";

        // POST api/customer/register
        [HttpPost("register")]
        public IActionResult RegisterCustomer([FromBody] CustomerModel customer)
        {
            // Kiểm tra xem dữ liệu đăng ký khách hàng hợp lệ hay không.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Nếu dữ liệu hợp lệ, tiếp tục xử lý đăng ký khách hàng.
            try
            {
                // Kiểm tra số điện thoại đã tồn tại trong cơ sở dữ liệu hay chưa
                if (PhoneExists(customer.PhoneNumber))
                {
                    return BadRequest("Phone number already exists.");
                }

                // Kiểm tra email đã tồn tại trong cơ sở dữ liệu hay chưa
                if (EmailExists(customer.Email))
                {
                    return BadRequest("Email already exists.");
                }

                // Kiểm tra CLoginName đã tồn tại trong cơ sở dữ liệu hay chưa
                if (LoginNameExists(customer.LoginName))
                {
                    return BadRequest("Login name already exists.");
                }

                // Tạo kết nối đến cơ sở dữ liệu SQL Server và thực hiện lưu thông tin khách hàng.
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Tạo câu truy vấn SQL để chèn thông tin khách hàng vào bảng Customers
                    string query = "INSERT INTO Customers (FirstName, MiddleName, LastName, Gender, BirthDate, PhoneNumber, Email, Address, Type, LoginName, Password) " +
                                   "VALUES (@FirstName, @MiddleName, @LastName, @Gender, @BirthDate, @PhoneNumber, @Email, @Address, @Type, @LoginName, @Password)";

                    // Tạo đối tượng SqlCommand để thực thi câu truy vấn
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Thêm các tham số cho câu truy vấn để tránh lỗ hổng SQL Injection
                        cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        cmd.Parameters.AddWithValue("@MiddleName", customer.MiddleName);
                        cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                        cmd.Parameters.AddWithValue("@BirthDate", customer.BirthDate);
                        cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", customer.Email);
                        cmd.Parameters.AddWithValue("@Address", customer.Address);
                        cmd.Parameters.AddWithValue("@Type", customer.Type);
                        cmd.Parameters.AddWithValue("@LoginName", customer.LoginName);
                        cmd.Parameters.AddWithValue("@Password", customer.Password);

                        // Thực thi câu truy vấn
                        cmd.ExecuteNonQuery();
                    }
                }

                // Trả về thông tin khách hàng đã đăng ký
                return Ok(customer);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi khi thêm thông tin vào cơ sở dữ liệu
                return StatusCode(500, "An error occurred while registering the customer.");
            }
        }

        // Hàm kiểm tra số điện thoại đã tồn tại trong cơ sở dữ liệu
        private bool PhoneExists(string phoneNumber)
        {
            // Thực hiện kiểm tra số điện thoại trong cơ sở dữ liệu và trả về true nếu tồn tại, ngược lại trả về false.
            // ...

            // Trả về mặc định để chạy code, bạn cần thay thế chỗ này bằng kiểm tra thực tế trong cơ sở dữ liệu.
            return false;
        }

        // Hàm kiểm tra email đã tồn tại trong cơ sở dữ liệu
        private bool EmailExists(string email)
        {
            // Thực hiện kiểm tra email trong cơ sở dữ liệu và trả về true nếu tồn tại, ngược lại trả về false.
            // ...

            // Trả về mặc định để chạy code, bạn cần thay thế chỗ này bằng kiểm tra thực tế trong cơ sở dữ liệu.
            return false;
        }

        // Hàm kiểm tra CLoginName đã tồn tại trong cơ sở dữ liệu
        private bool LoginNameExists(string loginName)
        {
            // Thực hiện kiểm tra CLoginName trong cơ sở dữ liệu và trả về true nếu tồn tại, ngược lại trả về false.
            // ...

            // Trả về mặc định để chạy code, bạn cần thay thế chỗ này bằng kiểm tra thực tế trong cơ sở dữ liệu.
            return false;
        }
    }

    public class CustomerModel
    {
        // Các Data Annotations và thuộc tính của khách hàng (giống như đã trình bày trong câu trả lời trước).
    }
}


public class CustomerModel
{
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; }

    // MiddleName and LastName are optional, so no Data Annotation is needed.

    [Required(ErrorMessage = "Gender is required.")]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Birth Date is required.")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    public string Type { get; set; }

    [Required(ErrorMessage = "Login Name is required.")]
    public string LoginName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}
