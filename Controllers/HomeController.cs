using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheCoffeeShop.Models;

namespace TheCoffeeShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CoffeeShopDbContext _context;

    public HomeController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var accountId = HttpContext.Session.GetInt32("AccountId");
        if (accountId == null)
        {
            return RedirectToAction("Index", "Account");
        }

        // Lấy danh sách sản phẩm trong giỏ hàng
        var carts = _context.Carts.Where(c => c.AccountId == accountId).Include(c => c.Product).ToList();

        // Tính tổng số lượng sản phẩm trong giỏ hàng
        int cartTotalQuantity = (int)carts.Sum(item => item.Quality);

        // Đặt giá trị vào ViewBag để hiển thị trên nút giỏ hàng
        ViewBag.CartCount = cartTotalQuantity;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
