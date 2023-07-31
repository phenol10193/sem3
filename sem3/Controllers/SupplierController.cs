using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sem3.Models;
using System.Data;
using System.Data.SqlClient;

namespace sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        //private readonly string _connectionString = "Server=mydb.database.windows.net;Database=OnlineCaterer;User Id=Group4Catere;Password=@Hieu2104;";
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineCatere;Trusted_Connection=True;";

        // POST api/supplier/register
        [HttpPost("register")]
        public IActionResult RegisterSupplier([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (PhoneExists(supplier.PhoneNumber))
                {
                    return BadRequest("Phone number already exists.");
                }
                if (EmailExists(supplier.Email))
                {
                    return BadRequest("Email already exists.");
                }

                if (LoginNameExists(supplier.SLoginName))
                {
                    return BadRequest("Login name already exists.");
                }
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Supplier (SName, PhoneNumber, Address, Email, SLevel, Image, ACityId, SLoginGame, Password) " +
                                     "VALUES (@SName, @PhoneNumber, @Address, @Email, @SLevel, @Image, @ACityId, @SLoginName, @Password)";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // SQL Injection
                        cmd.Parameters.AddWithValue("@SName", supplier.SName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", supplier.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Address", supplier.Address);
                        cmd.Parameters.AddWithValue("@Email", supplier.Email);
                        cmd.Parameters.AddWithValue("@SLevel", supplier.SLevel);
                        cmd.Parameters.AddWithValue("@Image", supplier.UrlImage);
                        cmd.Parameters.AddWithValue("@ACityId", supplier.ACityId);
                        cmd.Parameters.AddWithValue("@SLoginName", supplier.SLoginName);
                        cmd.Parameters.AddWithValue("@Password", supplier.Password);

                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(supplier);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while registering the supplier.");
            }
        }
        // POST api/supplier/updateProfile
        [HttpPost("updateProfile")]
        public IActionResult UpdateSupplierProfile([FromForm] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //if (phoneexists(supplier.phonenumber, supplier.supplierid))
                //{
                //    return badrequest("phone number already exists.");
                //}

                //if (emailexists(supplier.email, supplier.supplierid))
                //{
                //    return badrequest("email already exists.");
                //}
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Suppliers SET SName = @SName, PhoneNumber = @PhoneNumber, Address = @Address, " +
                                   "Email = @Email, SLevel = @SLevel, UrlImage = @UrlImage,EventId = @EventId, ACityId = @ACityId " +
                                   "WHERE SupplierId = @SupplierId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // SQL Injection
                        cmd.Parameters.AddWithValue("@SName", supplier.SName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", supplier.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Address", supplier.Address);
                        cmd.Parameters.AddWithValue("@Email", supplier.Email);
                        cmd.Parameters.AddWithValue("@SLevel", supplier.SLevel);
                        cmd.Parameters.AddWithValue("@UrlImage", supplier.UrlImage);
                        cmd.Parameters.AddWithValue("@ACityId", supplier.ACityId);
                        cmd.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);

                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the supplier profile.");
            }
        }

        // DELETE api/supplier/deleteProfile
        [HttpPut("deleteProfile")]
        public IActionResult DeleteSupplierProfile()
        {
            try
            {
                return Ok("Supplier profile deleted successfully.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while deleting the supplier profile.");
            }
        }

        // GET api/supplier/{supplierId}
        [HttpGet("{supplierId}")]
        public async Task<IActionResult> GetSupplier(int supplierId)
        {
            try
            {
                Supplier supplier = await GetSupplierInfoAsync(supplierId);
                if (supplier == null)
                {
                    return NotFound("Supplier not found.");
                }
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                // log the error or handle it appropriately
                return StatusCode(500, "an error occurred while retrieving supplier information.");
            }
        }
        private async Task<Supplier> GetSupplierInfoAsync(int supplierId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT SupplierId, SName, PhoneNumber, Address, Email, SLevel, UrlImage FROM Supplier WHERE SupplierId = @SupplierId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        Supplier supplier = new Supplier
                        {
                            SupplierId = reader.GetInt32(0),
                            SName = reader.GetString(1),
                            PhoneNumber = reader.GetString(2),
                            Address = reader.GetString(3),
                            Email = reader.GetString(4),
                            SLevel = reader.GetString(5),
                            UrlImage = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                            Services = await GetSupplierServicesAsync(supplierId)
                        };

                        return supplier;
                    }

                    return null;
                }
            }
        }
        private async Task<List<Service>> GetSupplierServicesAsync(int supplierId)
        {
            List<Service> services = new List<Service>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT ServiceId, ServiceName, Description, UrlImage FROM Service WHERE SupplierId = @SupplierId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Service service = new Service
                        {
                            ServiceId = reader.GetInt32(0),
                            ServiceName = reader.GetString(1),
                            Description = reader.GetString(2),
                            UrlImage = !reader.IsDBNull(3) ? reader.GetString(3) : null
                        };

                        service.Rooms = await GetServiceRoomsAsync(service.ServiceId);
                        services.Add(service);
                    }
                }
            }

            return services;
        }
        private async Task<List<Room>> GetServiceRoomsAsync(int serviceId)
        {
            List<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT RoomId, RoomName, Capacity, RoomPrice FROM Room WHERE ServiceId = @ServiceId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Room room = new Room
                        {
                            RoomId = reader.GetInt32(0),
                            RoomName = reader.GetString(1),
                            Capacity = reader.GetInt32(2),
                            RoomPrice = reader.GetDouble(3)
                        };

                        rooms.Add(room);
                    }
                }
            }

            return rooms;
        }

        // GET api/supplier/feedback/{supplierId}
        [HttpGet("feedback/{supplierId}")]
        public async Task<IActionResult> GetSupplierFeedback(int supplierId)
        {
            try
            {
                List<Feedback> feedbacks = await GetSupplierFeedbackAsync(supplierId);
                return Ok(feedbacks);
        }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                return StatusCode(500, "An error occurred while retrieving supplier feedback.");
    }
}

        private async Task<List<Feedback>> GetSupplierFeedbackAsync(int supplierId)
        {
            List<Feedback> feedbacks = new List<Feedback>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT f.FeedbackId, f.SupplierId, f.CustomerId, f.Comment, f.FeedbackDate, 
                                c.UrlImage, c.FirstName 
                         FROM CustomerFeedback f
                         JOIN Customer c ON f.CustomerId = c.CustomerId
                         WHERE f.SupplierId = @SupplierId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Feedback feedback = new Feedback
                        {
                            FeedbackId = reader.GetInt32(0),
                            SupplierId = reader.GetInt32(1),
                            CustomerId = reader.GetInt32(2),
                            Comment = reader.GetString(3),
                            FeedbackDate = reader.GetDateTime(4),
                            UrlImage = !reader.IsDBNull(5) ? reader.GetString(5):null,
                            FirstName = reader.GetString(6)
                        };

                        feedbacks.Add(feedback);
                    }
                }
            }

            return feedbacks;
        }

        // GET api/menu
        [HttpGet("allMenu")]
        public async Task<IActionResult> GetAllMenu()
        {
            //try
            //{
                List<Menu> menuItems = await GetAllMenuAsync();
                return Ok(menuItems);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, "An error occurred while retrieving all menu items.");
            //}
        }

        private async Task<List<Menu>> GetAllMenuAsync()
        {
            List<Menu> menuItems = new List<Menu>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT M.MenuItemId, M.ItemName, M.Price, M.SupplierId, M.UrlImage, C.Name " +
                               "FROM SuppMenu M INNER JOIN Category C ON M.CategoryId = C.CategoryId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Menu menuItem = new Menu
                        {
                            MenuItemId = reader.GetInt32(0),
                            ItemName = reader.GetString(1),
                            Price = reader.GetDouble(2),
                            SupplierId = reader.GetInt32(3),
                            UrlImage = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                            Name = reader.GetString(5)
                        };

                        menuItems.Add(menuItem);
                    }
                }
            }

            return menuItems;
        }


        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetMenuByCategoryId(int categoryId)
        {
            try
            {
                List<Menu> menuItems = await GetMenuByCategoryAsync(categoryId);
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving menu items by category.");
            }
        }

        private async Task<List<Menu>> GetMenuByCategoryAsync(int categoryId)
        {
            List<Menu> menuItems = new List<Menu>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT M.MenuItemId, M.ItemName, M.Price, M.SupplierId, M.UrlImage, C.Name " +
                               "FROM SuppMenu M INNER JOIN Category C ON M.CategoryId = C.CategoryId " +
                               "WHERE C.CategoryId = @CategoryId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Menu menuItem = new Menu
                        {
                            MenuItemId = reader.GetInt32(0),
                            ItemName = reader.GetString(1),
                            Price = reader.GetDouble(2),
                            SupplierId = reader.GetInt32(3),
                            UrlImage = reader.GetString(4),
                            Name = reader.GetString(5)
                        };

                        menuItems.Add(menuItem);
                    }
                }
            }

            return menuItems;
        }

        // POST api/menu/insert
        [HttpPost("insertMenu")]
        public async Task<IActionResult> InsertMenu([FromForm] Menu menu, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Save the image and get the URL
                menu.UrlImage = await SaveImageAndGetUrlAsync(imageFile);

                await InsertMenuAsync(menu);

                return Ok(menu);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while inserting the menu.");
            }
        }

        private async Task InsertMenuAsync(Menu menu)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO SuppMenu (ItemName, Price, SupplierId, CategoryId, UrlImage) " +
                               "VALUES (@ItemName, @Price, @SupplierId, @CategoryId, @UrlImage)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ItemName", menu.ItemName);
                    cmd.Parameters.AddWithValue("@Price", menu.Price);
                    cmd.Parameters.AddWithValue("@SupplierId", menu.SupplierId);
                    cmd.Parameters.AddWithValue("@CategoryId", menu.CategoryId);
                    cmd.Parameters.AddWithValue("@UrlImage", menu.UrlImage);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task<string> SaveImageAndGetUrlAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            // Generate a unique file name to avoid conflicts
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            string imagePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Return the URL of the saved image
            return "/uploads/" + uniqueFileName;
        }

        // GET api/supplier/{supplierId}/services
        [HttpGet("{supplierId}/services")]
        public async Task<IActionResult> GetServices()
        {
            //try
            //{
            List<Service> services = new List<Service>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT s.ServiceId, s.ServiceName, s.Description, s.UrlImage, " +
                               "r.RoomId, r.RoomName, r.Capacity, r.RoomPrice " +
                               "FROM Service s " +
                               "LEFT JOIN Room r ON s.ServiceId = r.ServiceId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())

                    {
                        Dictionary<int, Service> serviceMap = new Dictionary<int, Service>();

                        while (await reader.ReadAsync())
                        {
                            int serviceId = reader.GetInt32(0);

                            if (!serviceMap.ContainsKey(serviceId))
                            {
                                var service = new Service
                                {
                                    ServiceId = serviceId,
                                    ServiceName = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    UrlImage = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                                    Rooms = new List<Room>()
                                };
                                serviceMap.Add(serviceId, service);
                                services.Add(service);
                            }

                            if (!reader.IsDBNull(4))
                            {
                                Room room = new Room
                                {
                                    RoomId = reader.GetInt32(4),
                                    RoomName = reader.GetString(5),
                                    Capacity = reader.GetInt32(6),
                                    RoomPrice = reader.GetDouble(7)
                                };
                                serviceMap[serviceId].Rooms.Add(room);
                            }
                        }
                    }
                }
            }

            return Ok(services);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, "An error occurred while fetching data.");
            //}
        }
        private bool PhoneExists(string phoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Supplier WHERE PhoneNumber = @PhoneNumber";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private bool EmailExists(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Supplier WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private bool LoginNameExists(string loginName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Supplier WHERE SLoginName = @SLoginName";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SLoginName", loginName);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}

