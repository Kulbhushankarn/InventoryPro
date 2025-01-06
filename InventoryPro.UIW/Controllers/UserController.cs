using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using InventoryPro.BM;
using InventoryPro.BM.ITF;
using InventoryPro.DL;
using InventoryPro.DL.ITF;
using InventoryPro.VO;

namespace InventoryPro.UIW.Controllers
{
    public class UserController : Controller
    {
        private readonly IclsUserBM _userBM;
        private readonly string _connectionString;

        // Parameterless constructor with manual instantiation
        public UserController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString;

            // Manually instantiate the dependencies
            IclsUserDL userDL = new clsUserDL();
            _userBM = new clsUserBM(userDL);
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ModelState.AddModelError("", "Username and password are required.");
                    return View();
                }

                // Hash the password (replace this with actual password hashing logic)
                string passwordHash = GeneratePasswordHash(password);

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Authenticate user using the username and hashed password
                    var user = _userBM.AuthenticateUser(connection, username, passwordHash);

                    if (user != null)
                    {
                        // Store user information in session
                        Session["UserId"] = user.UserId;
                        Session["Username"] = user.Username;
                        Session["Role"] = user.Role;

                        // Redirect to the dashboard or home page
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
            }

            return View();
        }


        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(clsUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        user.Username = user.Username;

                        user.Email = user.Email;

                        // Hash the password
                        user.PasswordHash = GeneratePasswordHash(user.PasswordHash);

                        // Set additional fields
                        user.Role = user.Role;

                        user.IsActive = true;

                        // Add the user to the database
                        _userBM.RegisterUser(connection, user);

                        // Redirect to login page after successful registration
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
            }

            return View(user);
        }

        // GET: User/Logout
        public ActionResult Logout()
        {
            // Clear the session
            Session.Clear();
            return RedirectToAction("Login");
        }

        // Helper method to hash the password
        private string GeneratePasswordHash(string password)
        {
            // Implement your password hashing logic here
            return password; // Replace with actual hashing logic
        }
    }
}
