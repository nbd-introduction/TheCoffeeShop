using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCoffeeShop.Models;

namespace TheCoffeeShop.Controllers
{
    public class BlogController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public BlogController(CoffeeShopDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> UserIndex(string searchString, string category)
        {
            // Lấy danh sách bài viết kèm thông tin tác giả và sản phẩm liên quan
            var blogs = _context.Blogs
                .Include(b => b.Account)
                .Include(b => b.Product)
                .OrderByDescending(b => b.BlogDate)
                .AsQueryable();

            // Xử lý tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchString))
            {
                blogs = blogs.Where(b =>
                    b.BlogName.Contains(searchString) ||
                    b.BlogContent.Contains(searchString));
            }

            // Xử lý lọc theo danh mục nếu có
            // Lưu ý: Trong thực tế, bạn cần thêm trường Category vào model Blog
            // Đây chỉ là ví dụ, bạn có thể điều chỉnh theo cấu trúc dữ liệu thực tế
            if (!string.IsNullOrEmpty(category) && category != "all")
            {
                // Giả sử có trường Category trong Blog
                // blogs = blogs.Where(b => b.Category == category);
            }

            return View("UserIndex", await blogs.ToListAsync());
        }


        // GET: Blog
        public async Task<IActionResult> Index()
        {
            var coffeeShopDbContext = _context.Blogs.Include(b => b.Account).Include(b => b.Product);
            return View(await coffeeShopDbContext.ToListAsync());
        }

        // GET: Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Account)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blog/Create
        [Authorize(Roles = "2, 3")]
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> Create([Bind("BlogId,BlogName,BlogDate,BlogContent,BlogImage,AccountId,ProductId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", blog.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", blog.ProductId);
            return View(blog);
        }

        // GET: Blog/Edit/5
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", blog.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", blog.ProductId);
            return View(blog);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,BlogName,BlogDate,BlogContent,BlogImage,AccountId,ProductId")] Blog blog)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", blog.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", blog.ProductId);
            return View(blog);
        }

        // GET: Blog/Delete/5
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Account)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
    }
}
