using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheCoffeeShop.Models;

public class OrderController : Controller
{
    private readonly CoffeeShopDbContext _context;

    public OrderController(CoffeeShopDbContext context)
    {
        _context = context;
    }
    [Authorize(Roles = "3")]
    public async Task<IActionResult> AdminIndex()
    {
        // Lấy danh sách đơn hàng kèm theo thông tin liên quan
        var orders = await _context.Orders
            .Include(o => o.Account)
            .Include(o => o.Cart)
                .ThenInclude(c => c.Product)
            .ToListAsync();

        // Thống kê cho dashboard
        ViewBag.TotalOrders = orders.Count;
       

        return View(orders);
    }

   


    // GET: /Order
    public async Task<IActionResult> Index()
    {
        var accountId = HttpContext.Session.GetInt32("AccountId");
        if (accountId == null)
        {
            return RedirectToAction("Index", "Account");
        }

        var carts = await _context.Carts
            .Where(c => c.AccountId == accountId)
            .Include(c => c.Product)
            .ToListAsync();

        return View(carts);
    }

    // POST: /Order/Confirm
    [HttpPost]
    public async Task<IActionResult> Confirm()
    {
        var accountId = HttpContext.Session.GetInt32("AccountId");

        if (accountId == null)
        {
            return RedirectToAction("Index", "Account");
        }

        var carts = await _context.Carts
            .Where(c => c.AccountId == accountId)
            .Include(c => c.Product)
            .ToListAsync();

        if (!carts.Any())
        {
            return RedirectToAction("Index", "ShoppingCart");
        }

        foreach (var cartItem in carts)
        {
            var CartID = cartItem.CartId;
            if (CartID == null)
            {
                Console.WriteLine("CartID is null",CartID);
            }
            var order = new Order
            {
                AccountId = accountId,
                CartId = cartItem.CartId,
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
        }
        
        await _context.SaveChangesAsync();

        TempData["Message"] = "Đặt hàng thành công!";
        return RedirectToAction("ThankYou");
    }

    public IActionResult ThankYou()
    {
        return View();
    }
}
