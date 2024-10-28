using Examination_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Examination_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly MyApplicationContext _context;

        public AdminController(MyApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Questions()
        {
            var questions = await _context.Questions.Include(q => q.Answers).ToListAsync();
            return View(questions);
        }

        public IActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(Question question)
        {
            ModelState.Remove("Answers");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Questions.Add(question);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Questions));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred while creating the question: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please ensure all required fields are filled out correctly.");
            }

            return View(question);
        }

        public async Task<IActionResult> EditQuestion(int id)
        {

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Question_Id == id);

            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(int id, Question question)
        {
            if (id != question.Question_Id)
            {
                return NotFound();
            }
            ModelState.Remove("Question_Text");

            if (ModelState.IsValid)
            {
                var existingQuestion = await _context.Questions
                    .Include(q => q.Answers)
                    .FirstOrDefaultAsync(q => q.Question_Id == question.Question_Id);

                if (existingQuestion != null)
                {
                    existingQuestion.Question_Text = question.Question_Text;
                    existingQuestion.Question_Score = question.Question_Score;

                    _context.Answers.RemoveRange(existingQuestion.Answers);
                    foreach (var answer in question.Answers)
                    {
                        answer.Question_Id = question.Question_Id;
                        _context.Answers.Add(answer);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Questions));
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            ModelState.AddModelError("", "Please ensure all required fields are filled out correctly.");
            return View(question);
        }

        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost, ActionName("DeleteQuestion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestionConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Questions)); 
        }


        public async Task<IActionResult> CreateAnswer(int questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                return NotFound();
            }

            ViewBag.QuestionId = questionId;
            return View(new Answer { Question_Id = questionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnswer(Answer answer)
        {
            var existingQuestion = await _context.Questions.FindAsync(answer.Question_Id);
            if (existingQuestion == null)
            {
                ModelState.AddModelError("", "The specified question does not exist.");
                return View(answer);
            }

            answer.Question = existingQuestion;

            var existingAnswers = await _context.Answers
                .Where(a => a.Question_Id == answer.Question_Id).ToListAsync();

            if (existingAnswers.Count >= 5)
            {
                ModelState.AddModelError("", "Each question can only have up to 5 answers.");
                return View(answer);
            }

            if (answer.Is_correct)
            {
                foreach (var ans in existingAnswers)
                {
                    ans.Is_correct = false;
                    _context.Answers.Update(ans);
                }
            }

            _context.Answers.Add(answer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occurred while saving changes. Please try again.");
                return View(answer);
            }

            return RedirectToAction("EditQuestion", new { id = answer.Question_Id });
        }

        public async Task<IActionResult> EditAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnswer(int id, Answer answer)
        {
            if (id != answer.Answer_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (answer.Is_correct)
                {
                    var answers = await _context.Answers
                        .Where(a => a.Question_Id == answer.Question_Id && a.Is_correct)
                        .ToListAsync();

                    foreach (var ans in answers)
                    {
                        ans.Is_correct = false;
                    }
                }
                _context.Entry(answer).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Answers.Any(a => a.Answer_Id == answer.Answer_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("EditQuestion", new { id = answer.Question_Id });
            }
            return View(answer);
        }


        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        [HttpPost, ActionName("DeleteAnswer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAnswerConfirmed(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditQuestion", new { id = answer.Question_Id });
        }
    }
}
