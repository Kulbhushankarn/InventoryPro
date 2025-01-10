using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
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
        public ActionResult Login(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                ModelState.AddModelError("", "Username, password, and role are required.");
                return View();
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Retrieve user details from the database
                    var user = _userBM.AuthenticateUser(connection, username, role);

                    if (user != null && VerifyPassword(password, user.PasswordHash))
                    {
                        // Set session variables
                        Session["UserId"] = user.UserId;
                        Session["Username"] = user.Username;
                        Session["Role"] = user.Role;

                        // Redirect to dashboard
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username, password, or role.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
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

                        // Hash the plain-text password
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
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Helper method to verify the password
        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            string hashedPlainTextPassword = GeneratePasswordHash(plainTextPassword);
            return hashedPlainTextPassword == hashedPassword;
        }
    }
}
