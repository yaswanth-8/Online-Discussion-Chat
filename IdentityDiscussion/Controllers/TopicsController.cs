using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdentityDiscussion.Data;
using IdentityDiscussion.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityDiscussion.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public TopicsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
              return _context.Topics != null ? 
                          View(await _context.Topics.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Topics'  is null.");
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicId,TopicName")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,TopicName")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
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
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Topics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Topics'  is null.");
            }
            var topic = await _context.Topics.FindAsync(id);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Open(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var member = new Member { User = user, TopicId = id };

            var existingMember = _context.Members.FirstOrDefault(m => m.User == user && m.TopicId == id);
            if (existingMember == null)
            {
                _context.Members.Add(member);
                await _context.SaveChangesAsync();
            }

            var messages = _context.Messages
                         .Where(m => m.Member.TopicId == id)
                         .Select(m => new { Chat = m.Chat, UserEmail = m.Member.User.Email })
                         .ToList();

            ViewBag.Messages = messages;
            foreach (var message in messages)
            {
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
            }
            return View();
        }


        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Open(string message, int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var member = _context.Members.FirstOrDefault(m => m.User == user && m.TopicId == id);

            if (member != null)
            {
                var newMessage = new Message { MemId = member.MemId, Chat = message };
                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Open");
        }



        private bool TopicExists(int id)
        {
          return (_context.Topics?.Any(e => e.TopicId == id)).GetValueOrDefault();
        }
    }
}
