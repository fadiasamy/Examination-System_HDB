using Examination_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Examination_System.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly MyApplicationContext _context;

        public UserController(MyApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Results()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var results = await _context.ExamResults
                .Where(r => r.UserId == userId)
                .Include(r => r.Exam)
                .ToListAsync();
            return View(results);
        }

        public async Task<IActionResult> GenerateExam()
        {
            var randomQuestions = await _context.Questions
                .Include(q => q.Answers)
                .OrderBy(q => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            return View(randomQuestions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExam(int[] questionIds, int[] selectedAnswers)
        {
            Console.WriteLine($"Question IDs: {string.Join(", ", questionIds)}");
            Console.WriteLine($"Selected Answers: {string.Join(", ", selectedAnswers)}");

            if (questionIds.Length != selectedAnswers.Length)
            {
                Console.WriteLine($"Question count: {questionIds.Length}, Answer count: {selectedAnswers.Length}");
                return BadRequest("Question count and answer count doesn't match.");
            }

            var questions = await _context.Questions
                .Include(q => q.Answers)
                .Where(q => questionIds.Contains(q.Question_Id))
                .ToListAsync();

            var correctAnswers = questions.ToDictionary(
                q => q.Question_Id,
                q => q.Answers.FirstOrDefault(a => a.Is_correct)?.Answer_Id
            );

            Console.WriteLine("Correct Answers:");
            foreach (var item in correctAnswers)
            {
                Console.WriteLine($"Question ID: {item.Key}, Correct Answer ID: {item.Value}");
            }

            int score = questionIds
                .Select((questionId, index) => new { questionId, selectedAnswer = selectedAnswers[index] })
                .Where(x => correctAnswers.TryGetValue(x.questionId, out var correctAnswerId) && x.selectedAnswer == correctAnswerId)
                .Sum(x => questions.First(q => q.Question_Id == x.questionId).Question_Score);

            Console.WriteLine($"Calculated Score: {score}");

            var newExam = new Exam();
            _context.Exams.Add(newExam);
            await _context.SaveChangesAsync();

            var examResult = new ExamResult
            {
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ExamId = newExam.Exam_Id,
                Score = score,
                ExamDate = DateTime.Now
            };

            _context.ExamResults.Add(examResult);
            await _context.SaveChangesAsync();

            return RedirectToAction("ExamResult", new { score });
        }

        public IActionResult ExamResult(int score)
        {
            ViewBag.Score = score;
            return View();
        }
    }
}
