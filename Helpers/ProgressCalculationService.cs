using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyfirstBlackMetalAlbum.com.Models;

namespace MyfirstBlackMetalAlbum.com.Helpers
{
        public class ProgressCalculationService
        {
            private readonly MyFirstBlackMetalAlbumContext _context;

            public ProgressCalculationService(MyFirstBlackMetalAlbumContext context)
            {
                _context = context;
            }

        // Method to recalculate progress for all users
        public void RecalculateAllUsersProgress()
        {
            var allUsers = _context.Users.ToList();
            foreach (var user in allUsers)
            {
                RecalculateProgress(user.Username);
            }
        }

        public async Task ManuallyAddLessonAndRecalculate(Lesson newLesson)
        {
            // Your code to manually add the lesson to the database
            _context.Lessons.Add(newLesson);
            await _context.SaveChangesAsync();

            // After manually adding the lesson, recalculate progress for all users
            var allUsers = _context.Users.ToList();
            foreach (var user in allUsers)
            {
                RecalculateProgress(user.Username);
            }
        }


        // Method to recalculate progress for a specific user
        private void RecalculateProgress(string username)
        {
            var user = _context.Users
                .Include(u => u.UserProgresses)
                .FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                var totalLessons = _context.Lessons.Count(); // Total lessons
                var completedLessonsCount = user.UserProgresses.Count(); // Completed lessons by user

                var percentagePerLesson = 100.0 / totalLessons;
                var percentageToAdd = (int)Math.Round(percentagePerLesson); // Convert the double to an integer

                // Increment user progress by the percentage value per lesson
                user.ProgressPercentage = completedLessonsCount * percentageToAdd;

                // Save changes to the database
                _context.SaveChanges();
            }
        }
    }

    }

