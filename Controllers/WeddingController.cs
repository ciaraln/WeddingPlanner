using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner
{
    public class WeddingController : Controller
    {
         public bool ErrorMessage { get; private set; }
        private WeddingPlannerContext _context;
        public WeddingController(WeddingPlannerContext context)
        {
            _context = context;
        }
            [HttpGet , Route("Dashboard")]
            public IActionResult Dashboard()
            {
                System.Console.WriteLine("In Dashboard about to get username");
               
                var AreTheyLoggedIn = IsLoggedIn();
                if (AreTheyLoggedIn == true)
                {
                    int? LoginId = HttpContext.Session.GetInt32("CurrentUser");
                    if ((int)LoginId != 0)
                    {
                        User thisUser = GetUserInfo();
                        List<Wedding> AllWeddings = _context.weddings.Include(a => a.Guests).ToList();
                        ViewBag.UserInfo = thisUser;
                        ViewBag.userId = thisUser.userId;
                        ViewBag.AllWeddings = AllWeddings;

                    // ************************* */*********************************8//
                    foreach(var wedding in ViewBag.AllWeddings)
                    {
                        System.Console.WriteLine(wedding.Guests.Count);
                       
                            if(wedding.userId == ViewBag.userId)
                    {
                       System.Console.WriteLine("CHECK ONE");
                    }
                    else
                    {
                            bool attending = false;
                            foreach(var guest in wedding.Guests)
                        {
                                if(guest.userId == ViewBag.userId)
                            {
                                    attending = true;
                                    break;
                                }
                            }

                            if(attending)
                    {
                       System.Console.WriteLine(" CHECK TWO");
                    }
                    else
                    {
                       System.Console.WriteLine(" CHECK THREE");
                    }
                        }
        }

// ***********************************8//
                        User ConfirmedUser = GetUserInfo();
                    }
                    return View("Dashboard");
                }
                return RedirectToAction("Index");
            }


            [HttpPost, Route("addWedding")]
            public IActionResult addWedding(WeddingViewModel WeddingSignUp)
            {
                
                System.Console.WriteLine("****************Creating Wedding Man!!!!**************");

               

                TryValidateModel(WeddingSignUp);
        
                    if(ModelState.IsValid)
                    {
                    User UserInfo = GetUserInfo();
                     int? CurrentUserId = HttpContext.Session.GetInt32("CurrentUser");
                 
                        Wedding newWedding = new Wedding
                        {
                            WedderOne = WeddingSignUp.WedderOne,
                            WedderTwo = WeddingSignUp.WedderTwo,
                            Date = (DateTime)WeddingSignUp.Date,
                            Address = WeddingSignUp.Address,
                            userId = UserInfo.userId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        _context.Add(newWedding);
                        _context.SaveChanges();
                        HttpContext.Session.SetInt32("CurrentWedding", newWedding.WeddingId);
                        GetWeddingInfo();
                        return RedirectToAction("ViewWedding", new { id = newWedding.WeddingId});
                    }
                    else
                    {
                        System.Console.WriteLine("ERROR: Wedding NOT Created, Fail");
                        TempData["Message"] = "Something is invalid here..";
                        return View("NewWedding");
                    }
                }
    

            [HttpGet, Route("ANewWedding")]

            public IActionResult ANewWedding()
            {
                return View("NewWedding");
            }


            [HttpGet]
            [Route("ViewWedding/{id}")]
        public IActionResult ViewWedding(int id)
        {
            // Grab your guest then singleout the wedding///
            List<Wedding> viewableWedding = _context.weddings.Where(w => w.WeddingId == id).Include(w => w.Guests).ThenInclude(u => u.User).ToList();
            if(viewableWedding.Count == 1 ){
                System.Console.WriteLine("List of Guess for Wedding.");
                ViewBag.WeddingInfo = viewableWedding[0];
            }
            return View("ViewWedding");
        }


        private User GetUserInfo()
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

            private Wedding GetWeddingInfo()
            {
                int? WeddingId = HttpContext.Session.GetInt32("CurrentWedding");
                if((int)WeddingId != 0)
                {
                    Wedding ConfirmedWedding = _context.weddings.SingleOrDefault( Wedding => Wedding.WeddingId == WeddingId);
                    @ViewBag.WeddingInfo = ConfirmedWedding;
                    return ConfirmedWedding;
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

        [HttpGet, Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Wedding RemoveWedding = _context.weddings.SingleOrDefault(wed => wed.WeddingId == id);
            _context.weddings.Remove(RemoveWedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        
        [HttpGet, Route("RSVP/{id}")]
        public IActionResult RSVP(int id)
        {
            System.Console.WriteLine("*******  ADDING RSVP *******");
            int? CurrentUserId = HttpContext.Session.GetInt32("CurrentUser");

            GuestList AttendingGuest = new GuestList
            {
                WeddingId= id,
                userId = (int)CurrentUserId
            };
            System.Console.WriteLine("******* About to add Guest to Wedding *********");
            _context.GuestList.Add(AttendingGuest);
            _context.SaveChanges();


            System.Console.WriteLine("******* About to add Guest to Wedding *********");
            System.Console.WriteLine(AttendingGuest);
            System.Console.WriteLine("******* Guest HAS BEEN ADDED TO THE GuestList *********");

            return RedirectToAction("Dashboard");
        }
       
        [HttpGet, Route("unRSVP/{id}")]
        public IActionResult unRSVP(int id)
        {
            GuestList GuestObj = _context.GuestList.Include(a=> a.User).ThenInclude(b => b.Guests).SingleOrDefault( c => c.WeddingId == id);
            
            _context.GuestList.Remove(GuestObj);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        
    }
}


