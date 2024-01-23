using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyfirstBlackMetalAlbum.com.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MyfirstBlackMetalAlbum.com.Helpers;

namespace MyfirstBlackMetalAlbum.com.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyFirstBlackMetalAlbumContext _context;
        private readonly ProgressCalculationService _progressService;

        public UsersController(MyFirstBlackMetalAlbumContext context, ProgressCalculationService progressService)
        {
            _context = context;
            _progressService = progressService;
        }

        //delete this star to restore code
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'MyFirstBlackMetalAlbumContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details()
        {
            // Get the username of the currently authenticated user
            var username = User.Identity.Name;

            // Query the database to get the user's full profile, including UserProgress
                 var user = await _context.Users
                .Include(u => u.UserProgresses) // Include the UserProgress navigation property
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not found
            }

            // Ensure that UserProgress is not null to avoid null reference exceptions
            if (user.UserProgresses == null)
            {
        //        user.UserProgresses = new UserProgress(); // Initialize UserProgress if null
            }
            _progressService.RecalculateAllUsersProgress();
            return View(user); // Pass the user's details to the view
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    user.ProfileImage = memoryStream.ToArray();
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details"); // Redirect back to the user details page
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,PasswordHash,Email,RegistrationDate")] Users users, IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    users.ProfileImage = memoryStream.ToArray();
                }
            }

            _context.Add(users);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,PasswordHash,Email,RegistrationDate")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        } 

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        } */

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'MyFirstBlackMetalAlbumContext.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        // [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check user credentials in the database
                if (UserIsValid(model.Username, model.Password))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                // Add more claims as needed
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true // Make the authentication persistent
                        });

                    return RedirectToAction("Index", "Home"); // Redirect to the desired page after login
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }


        private bool UserIsValid(string username, string password)
        {
            // Replace this with your actual database query logic to validate the user
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

            // Check if a user with the given username and password was found in the database
            if (user != null)
            {
                return true; // User credentials are valid
            }

            return false; // User credentials are invalid
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Redirect to the desired page after logout
        }



       /* private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        } */
    }
} 
