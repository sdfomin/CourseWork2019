﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork2019.Data;
using CourseWork2019.Models;
using CourseWork2019.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseWork2019.Controllers
{

    [Authorize(Roles = "admin")]
    public class QuestionsController : Controller
    {
        private readonly QuizContext _context;

        public QuestionsController(QuizContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var quizContext = _context.Questions.Include(q => q.Rubric);
            return View(await quizContext.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Rubric)
                .FirstOrDefaultAsync(m => m.QuestionID == id);
            if (question == null)
            {
                return NotFound();
            }

            var answers = await _context.Answers.Where(q => q.QuestionID == id).ToListAsync();

            return View(new QuestionDetailsModel(question, answers));
        }

        // GET: Questions/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["RubricID"] = new SelectList(_context.Rubrics, "RubricID", "RubricName");
            return View();
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("QuestionID,QuestionName,QuestionText,RubricID")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RubricID"] = new SelectList(_context.Rubrics, "RubricID", "RubricName", question.RubricID);
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["RubricID"] = new SelectList(_context.Rubrics, "RubricID", "RubricName", question.RubricID);
            ViewData["QuestonID"] = id;
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionID,QuestionName,QuestionText,RubricID")] Question question)
        {
            if (id != question.QuestionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionID))
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
            ViewData["RubricID"] = new SelectList(_context.Rubrics, "RubricID", "RubricName", question.RubricID);
            return View(question);
        }

        //GET: Questions/AddAnswer/5
        public IActionResult AddAnswer(int? id)
        {
            if (id == null) return NotFound();
            ViewBag.ID = id;
            return View();
        }

        //POST: Questions/AddAnswer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnswer([Bind("AnswerText,IsCorrectAnswer,QuestionID")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Answers.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Rubric)
                .FirstOrDefaultAsync(m => m.QuestionID == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionID == id);
        }
    }
}
