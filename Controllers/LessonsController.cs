using Microsoft.AspNetCore.Mvc;
using MyfirstBlackMetalAlbum.com.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;




namespace MyfirstBlackMetalAlbum.com.Controllers
{
    public class LessonsController : Controller
    {
        private readonly MyFirstBlackMetalAlbumContext _context;

        public LessonsController(MyFirstBlackMetalAlbumContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get the username of the currently authenticated user
            var username = User.Identity.Name;

            // Query the database to get the user's full profile, including ProgressPercentage
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not found
            }

            // Pass the user's ProgressPercentage to the view
            ViewBag.ProgressPercentage = user.ProgressPercentage;
            return View(user);
        }

        public IActionResult Phase1Lesson1()
        {
            var viewModel = new UserCommentsViewModel
            {
                // Populate the necessary properties
                // Title = "Your Title",
                LessonId = 1,
                Comments = _context.UserComment
                    .Where(c => c.LessonId == 1)
                    .Select(c => new CommentViewModel { UserName = c.UserName, Text = c.Text, ProfilePicture = c.User.ProfileImage, CreatedAt = c.CreatedAt })
                    .ToList()
            };

            return View("~/Views/Lessons/Phase1/Lesson1.cshtml", viewModel);
        }

        public IActionResult Phase1Lesson2()
        {
            var viewModel = new UserCommentsViewModel
            {
                // Populate the necessary properties
                // Title = "Your Title",
                LessonId = 2,
                Comments = _context.UserComment
                    .Where(c => c.LessonId == 2)
                    .Select(c => new CommentViewModel { UserName = c.UserName, Text = c.Text })
                    .ToList()
            };

            return View("~/Views/Lessons/Phase1/Lesson2.cshtml", viewModel);
        }

        public IActionResult Phase1Lesson3()
        {
            var viewModel = new UserCommentsViewModel
            {
                // Populate the necessary properties
                // Title = "Your Title",
                LessonId = 3,
                Comments = _context.UserComment
                    .Where(c => c.LessonId == 3)
                    .Select(c => new CommentViewModel { UserName = c.UserName, Text = c.Text })
                    .ToList()
            };
            //RecalculateAllUsersProgress();
            return View("~/Views/Lessons/Phase1/Lesson3.cshtml", viewModel);
        }

        public IActionResult Phase1Lesson4()
        {
            var viewModel = new UserCommentsViewModel
            {
                // Populate the necessary properties
                // Title = "Your Title",
                LessonId = 4,
                Comments = _context.UserComment
                    .Where(c => c.LessonId == 4)
                    .Select(c => new CommentViewModel { UserName = c.UserName, Text = c.Text })
                    .ToList()
            };
            //RecalculateAllUsersProgress();
            return View("~/Views/Lessons/Phase1/Lesson4.cshtml", viewModel);
        }

        public IActionResult IntermediateLesson1()
        {
            var viewModel = new UserCommentsViewModel
            {
                // Populate the necessary properties
                // Title = "Your Title",
                LessonId = 5,
                Comments = _context.UserComment
                    .Where(c => c.LessonId == 5)
                    .Select(c => new CommentViewModel { UserName = c.UserName, Text = c.Text })
                    .ToList()
            };
            //RecalculateAllUsersProgress();
            return View("~/Views/Lessons/Intermediate/Lesson1.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteLesson(int lessonId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;

                var user = await _context.Users
                    .Include(u => u.UserProgresses)
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user != null)
                {
                    var lesson = await _context.Lessons.FindAsync(lessonId);
                    if (lesson != null)
                    {
                        var existingProgress = user.UserProgresses.FirstOrDefault(up => up.LessonId == lessonId);

                        if (existingProgress == null)
                        {
                            var totalLessons = _context.Lessons.Count(); // Total lessons
                            var beginnerLessonsCount = _context.Lessons.Count(l => l.DifficultyLevel == "Beginner");
                            var intermediateLessonsCount = _context.Lessons.Count(l => l.DifficultyLevel == "Intermediate");
                            var advancedLessonsCount = _context.Lessons.Count(l => l.DifficultyLevel == "Advanced");
                            var recordingLessonsCount = _context.Lessons.Count(l => l.DifficultyLevel == "Recording");
                            // Add more counts for other difficulty levels

                            var completedLessonsCount = user.UserProgresses.Count(); // Completed lessons by user
                            var completedBeginnerLessonsCount = user.UserProgresses.Count(up =>
                                _context.Lessons.Any(l => l.LessonId == up.LessonId && l.DifficultyLevel == "Beginner"));
                            var completedIntermediateLessonsCount = user.UserProgresses.Count(up =>
                                _context.Lessons.Any(l => l.LessonId == up.LessonId && l.DifficultyLevel == "Intermediate"));
                            var completedAdvancedLessonsCount = user.UserProgresses.Count(up =>
                                _context.Lessons.Any(l => l.LessonId == up.LessonId && l.DifficultyLevel == "Advanced"));
                            var completedRecordingLessonsCount = user.UserProgresses.Count(up =>
                                _context.Lessons.Any(l => l.LessonId == up.LessonId && l.DifficultyLevel == "Recording"));
                            // Add more counts for other completed lessons by difficulty levels

                            var percentagePerLesson = 100.0 / totalLessons;
                            var percentageToAdd = (int)Math.Round(percentagePerLesson); // Convert the double to an integer
                            var percentagePerBeginnerLesson = 100.0 / beginnerLessonsCount;
                            var percentageToAddBeginner = (int)Math.Round(percentagePerBeginnerLesson); // Convert the double to an integer
                            var percentagePerIntermediateLesson = 100.0 / intermediateLessonsCount;
                            var percentageToAddIntermediate = (int)Math.Round(percentagePerIntermediateLesson); // Convert the double to an integer
                            var percentagePerAdvancedLesson = 100.0 / advancedLessonsCount;
                            var percentageToAddAdvanced = (int)Math.Round(percentagePerAdvancedLesson); // Convert the double to an integer
                            var percentagePerRecordingLesson = 100.0 / recordingLessonsCount;
                            var percentageToAddRecording = (int)Math.Round(percentagePerRecordingLesson); // Convert the double to an integer


                            // Add more percentage calculations for other difficulty levels
                            // Increment user progress by the percentage value per lesson

                            if (lesson.DifficultyLevel == "Beginner")
                            {
                                user.BeginnerProgress += percentageToAdd;
                            }
                            else if (lesson.DifficultyLevel == "Intermediate")
                            {
                                user.IntermediateProgress += percentageToAdd;
                            }
                            // Add more conditions for other difficulty levels

                            var progressInDb = _context.UserProgress
     .FirstOrDefault(up => up.UserIntId == user.UserIntId && up.LessonId == lessonId);

                            if (progressInDb == null)
                            {
                                var newProgress = new UserProgress
                                {
                                    LessonId = lessonId,
                                    IsCompleted = true,
                                    UserIntId = user.UserIntId // Assigning the foreign key from Users table
                                };
                                _context.UserProgress.Add(newProgress);
                            }
                            else
                            {
                                // Check if the lesson is not already completed to avoid duplicating progress
                                if (!progressInDb.IsCompleted)
                                {
                                    progressInDb.IsCompleted = true;
                                    _context.UserProgress.Update(progressInDb);
                                }
                            }

                            await _context.SaveChangesAsync();

                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }



        private double CalculatePercentage(params double[] percentages)
            {
                return percentages.Sum();
            }




        }
    } 