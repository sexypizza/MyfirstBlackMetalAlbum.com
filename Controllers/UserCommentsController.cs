using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyfirstBlackMetalAlbum.com.Models;

namespace MyfirstBlackMetalAlbum.com.Controllers
{
    public class UserCommentsController : Controller
    {
        private readonly MyFirstBlackMetalAlbumContext _context;

        public UserCommentsController(MyFirstBlackMetalAlbumContext context)
        {
            _context = context;
        }

        /* // GET: UserComments
         public async Task<IActionResult> Index()
         {
             var myFirstBlackMetalAlbumContext = _context.UserComment.Include(u => u.User);
             return View(await myFirstBlackMetalAlbumContext.ToListAsync());
         }

         // GET: UserComments/Details/5
         public async Task<IActionResult> Details(int? id)
         {
             if (id == null || _context.UserComment == null)
             {
                 return NotFound();
             }

             var userComments = await _context.UserComment
                 .Include(u => u.User)
                 .FirstOrDefaultAsync(m => m.CommentId == id);
             if (userComments == null)
             {
                 return NotFound();
             }

             return View(userComments);
         } */



        // The two methods you need to uncomment are create and create2

       //just delete the star below to uncomment them 
              
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCommentsViewModel viewModel)
        {
            if (User.Identity.IsAuthenticated) // Ensure the user is logged in
            {
                
                    Console.WriteLine("Model is valid");
                    var username = User.Identity.Name; // Retrieve the username of the logged-in user
                    var loggedInUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);



                var userComment = new UserComments
                    {
                        Text = viewModel.Text,
                        LessonId = viewModel.LessonId,
                        UserName = username, // Assign the username to the comment
                        UserIntId = loggedInUser.UserIntId,
                        CreatedAt = DateTime.Now // Set the creation date to the current date and time
                };

                    _context.UserComment.Add(userComment); // Add the user comment to the UserComment DbSet
                    await _context.SaveChangesAsync(); // Save changes to the database

                    // Fetch the comments specific to the lesson from the database
                    viewModel.Comments = _context.UserComment
                        .Where(c => c.LessonId == viewModel.LessonId)
                        .Select(c => new CommentViewModel 
                        { 
                            UserName = c.UserName, 
                            Text = c.Text,
                            ProfilePicture = c.User.ProfileImage, // Assuming UserComment has a navigation property to the User and ProfileImage is the property holding the picture
                            CreatedAt = DateTime.Now // Set the creation date to the current date and time
                        })
                        .ToList();

                    return View($"~/Views/Lessons/Phase1/Lesson{viewModel.LessonId}.cshtml", viewModel); // Render the page with the updated view model

            }
            // If the user is not authenticated or the model state is invalid, handle the scenario accordingly.
            // You can redirect the user to the login page or display an error message.
            return RedirectToAction("Login", "Users");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(UserCommentsViewModel viewModel)
        {
            if (User.Identity.IsAuthenticated) // Ensure the user is logged in
            {

                Console.WriteLine("Model is valid");
                var username = User.Identity.Name; // Retrieve the username of the logged-in user
                var userComment = new UserComments
                {
                    Text = viewModel.Text,
                    LessonId = viewModel.LessonId,
                    UserName = username // Assign the username to the comment
                };

                _context.UserComment.Add(userComment); // Add the user comment to the UserComment DbSet
                await _context.SaveChangesAsync(); // Save changes to the database

                // Fetch the comments specific to the lesson from the database
                viewModel.Comments = _context.UserComment
                    .Where(c => c.LessonId == viewModel.LessonId)
                    .Select(c => new CommentViewModel { UserName = c.UserName, Text = c.Text })
                    .ToList();

                return View($"~/Views/Lessons/Phase1/Lesson{viewModel.LessonId}.cshtml", viewModel); // Render the page with the updated view model

            }
            // If the user is not authenticated or the model state is invalid, handle the scenario accordingly.
            // You can redirect the user to the login page or display an error message.
            return RedirectToAction("Login", "Users");
        }




        /*


        // GET: UserComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserComment == null)
            {
                return NotFound();
            }

            var userComments = await _context.UserComment.FindAsync(id);
            if (userComments == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", userComments.UserId);
            return View(userComments);
        }

        // POST: UserComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Text,Timestamp,UserId")] UserComments userComments)
        {
            if (id != userComments.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userComments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCommentsExists(userComments.CommentId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", userComments.UserId);
            return View(userComments);
        }

        // GET: UserComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserComment == null)
            {
                return NotFound();
            }

            var userComments = await _context.UserComment
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (userComments == null)
            {
                return NotFound();
            }

            return View(userComments);
        }

        // POST: UserComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserComment == null)
            {
                return Problem("Entity set 'MyFirstBlackMetalAlbumContext.UserComment'  is null.");
            }
            var userComments = await _context.UserComment.FindAsync(id);
            if (userComments != null)
            {
                _context.UserComment.Remove(userComments);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCommentsExists(int id)
        {
          return (_context.UserComment?.Any(e => e.CommentId == id)).GetValueOrDefault();
        }
    } */
    }
}
