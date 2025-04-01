﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCoffeeShop.Models;
using System.Security.Cryptography;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,PassWord,Email,Phone,Adress,FullName")] Account account)
        {
            // Lấy giá trị confirmPassword từ form
            string confirmPassword = Request.Form["confirmPassword"].ToString();

            // Kiểm tra xác nhận mật khẩu
            if (account.PassWord != confirmPassword)
            {
                ModelState.AddModelError("PassWord", "Mật khẩu và xác nhận mật khẩu không khớp");
                return View(account);
            }

            // Kiểm tra tên đăng nhập đã tồn tại chưa
            if (await _context.Accounts.AnyAsync(a => a.UserName == account.UserName))
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                return View(account);
            }

            // Kiểm tra email đã tồn tại chưa
            if (!string.IsNullOrEmpty(account.Email) && await _context.Accounts.AnyAsync(a => a.Email == account.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");
                return View(account);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Thiết lập các giá trị mặc định
                    account.Role = 0; // 0: Khách hàng, 1: Admin
                    account.IsBan = false;

                    // Mã hóa mật khẩu
                    account.PassWord = HashPassword(account.PassWord);

                    // Thêm tài khoản vào database
                    _context.Add(account);
                    await _context.SaveChangesAsync();

                    // Đăng nhập người dùng sau khi đăng ký
                    await LoginUserAsync(account);

                    // Chuyển hướng đến trang chủ
                    TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký. Vui lòng thử lại sau.");
                    // Log lỗi
                    Console.WriteLine(ex.Message);
                }
            }

            return View(account);
        }

        // Mã hóa mật khẩu
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // Đăng nhập người dùng
        private async Task LoginUserAsync(Account account)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
        new Claim(ClaimTypes.Name, account.UserName),
        new Claim(ClaimTypes.Email, account.Email ?? string.Empty),
        new Claim(ClaimTypes.Role, account.Role.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }





    }
}
