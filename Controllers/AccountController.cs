using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCoffeeShop.Models;

namespace TheCoffeeShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public AccountController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(String username, string password)
        {
            var account = _context.Accounts.FirstOrDefault(acc => acc.UserName == username && acc.PassWord == password);
            if (account != null)
            {
                HttpContext.Session.SetInt32("AccountId", account.AccountId) ;
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }


        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,UserName,PassWord,Role,Email,Phone,Adress,FullName,IsBan")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        
        

       
    }
}
