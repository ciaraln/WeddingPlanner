using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers
{
    public class LogRegController : Controller
    {
        public bool ErrorMessage { get; private set; }
        private WeddingPlannerContext _context;

        public LogRegController(WeddingPlannerContext context)
        {
            _context = context;
        }

        [HttpGet, Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        // POST registration page ///
        [HttpPost]
        [Route("Registration")]

        public IActionResult Registration(RegisterViewModel regUser)
        {
            System.Console.WriteLine("Are your working?");
            if (TryValidateModel(regUser))
            // if (ModelState.IsValid)
            {
                User exists = _context.users.Where(User => User.Email == regUser.Email).SingleOrDefault();
                // if statment that checks results from new user with the email 
                // if (_context.users.Where(u => u.Email == regUser.Email).ToList().Count() > 0)
                if (exists != null)
                {
                    //you have to make custom error message it does not exist in previous logic. 
                    ViewBag.err = "Email already exist, please try new one.";
                    return View("Index");
                }
                if (ModelState.IsValid)
                {

                    User newUser = new User
                    {
                        FirstName = regUser.FirstName,
                        LastName = regUser.LastName,
                        Email = regUser.Email,
                        Password = regUser.Password,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };

                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    // THE NEW UsER FROM ABOVE CI. 
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

                    _context.Add(newUser);
                    _context.SaveChanges();

                    User ConfirmedUser = _context.users.SingleOrDefault(u => u.Email == regUser.Email);
                    HttpContext.Session.SetInt32("CurrentUser", ConfirmedUser.userId);
                    // if all information is Correct, enter into account. 
                    return RedirectToAction("Dashboard", "Wedding");
                }
            }

            return View("Index");
        }

        [HttpGet, Route("Login")]
        public IActionResult Login()
        {
            return RedirectToAction("Dashboard", "Wedding");
        }

        [HttpPost]
        [Route("Login")]

        public IActionResult Login(LoginViewModel logUser)
        {
            System.Console.WriteLine("We are in LoggedIn!");

            // User ConfirmedUser = EmailExists(logUser.Email);
            User exists = _context.users.Where(User => User.Email == logUser.Email).SingleOrDefault();
            if (exists != null)
            {
                var Hasher = new PasswordHasher<User>();

                if (0 != Hasher.VerifyHashedPassword(exists, exists.Password, logUser.Password))
                {
                    System.Console.WriteLine("**********SUCCESSFUL LOG O(N)*****************");
                    HttpContext.Session.SetInt32("CurrentUser", exists.userId);
                    HttpContext.Session.SetString("Username", exists.FirstName);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard", "Wedding");
                }
                else
                {
                    IsLoggedIn();
                    ViewBag.LogInError = "Email and/or password incorrect";
                    return View("Index");
                }
            }
            else
            {
                IsLoggedIn();
                return View("Index");


                // HttpContext.Session.SetInt32("logUser", exists.userId);
            }
        }

        public User EmailExists(string Email)
        {
            User emailMatch = _context.users.SingleOrDefault(user => user.Email == Email);
            if (emailMatch != null)
            {
                return emailMatch;
            }
            else
            {
                return null;
            }
        }

        public bool IsLoggedIn()
        {
            if (HttpContext.Session.GetInt32("CurrentUser") == null)
            {
                // 1)do nothing but convert null to zero to home page. 
                @ViewBag.LoggedIn = false;
                HttpContext.Session.SetInt32("CurrentUser", 0);
                return false;
            }
            // 2) is there user id in session and using it
            else if (HttpContext.Session.GetInt32("CurrentUser") > 0)
            {
                @ViewBag.LoggedIn = true;
                return true;
            }
            // 3) catch weird scenarios and let you know that you're not logged in //
            else
            {
                @ViewBag.LoggedIn = false;
                return false;
            }
        }

        // LogOut to Login Page //
        //make sure to put /logout on cshtml.
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        private User GetInfo()
        {
            int? LoginId = HttpContext.Session.GetInt32("CurrentUser");
            if ((int)LoginId != 0)
            {
                User ConfirmedUser = _context.users.SingleOrDefault(User => User.userId == LoginId);
                @ViewBag.UserInfo = ConfirmedUser;
                return ConfirmedUser;

            }
            else
            {
                return null;
            }
        }
    }
}        