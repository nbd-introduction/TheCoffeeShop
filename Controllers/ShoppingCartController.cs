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
    public class ShoppingCartController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public ShoppingCartController(CoffeeShopDbContext context)
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
            return View(carts);


        }
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            if (product == null)
            {
                return NotFound();
            }
            var existingCart = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.AccountId == accountId);
            if (existingCart != null)
            {
                existingCart.Quality += quantity;
                existingCart.TotalPrice = product.ProductPrice * existingCart.Quality;
                _context.SaveChanges();
            }
            else
            {
                var cart = new Cart
                {
                    AccountId = accountId,
                    ProductId = productId,
                    Quality = quantity,
                    TotalPrice = product.ProductPrice * quantity
                };
                _context.Carts.Add(cart);
            }
                _context.SaveChanges();
                return RedirectToAction("Index", "ShoppingCart");
            int cartCount = (int)_context.Carts
                .Where(c => c.AccountId == accountId)
            .Sum(c => c.Quality); // Hoặc c.Quality

            return Json(new { success = true, cartCount });
        }
        public ActionResult RemoveFromCart(int productId)
        {
            // L?y AccountId t? session
            var accountId = HttpContext.Session.GetInt32("AccountId");

            if (accountId == null)
            {
                // N?u không có accountId, chuy?n h??ng ??n trang ??ng nh?p
                return RedirectToAction("Login", "Account");
            }

            // Tìm cart item d?a trên productId và accountId
            var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.AccountId == accountId);

            if (cartItem != null)
            {
                // L?y thông tin s?n ph?m ?? tính giá
                var product = _context.Products.Find(productId);

                if (product != null)
                {
                    // Xóa cart item
                    _context.Carts.Remove(cartItem);

                    // C?p nh?t t?ng giá cho các cart item còn l?i c?a user
                    var remainingCartItems = _context.Carts
                        .Where(c => c.AccountId == accountId)
                        .Include(c => c.Product)
                        .ToList();

                    foreach (var item in remainingCartItems)
                    {
                        // C?p nh?t TotalPrice cho m?i item d?a trên s? l??ng và giá s?n ph?m
                        item.TotalPrice = item.Quality * item.Product.ProductPrice;
                    }

                    // L?u thay ??i vào database
                    _context.SaveChanges();
                }
            }

            // Chuy?n h??ng v? trang gi? hàng
            return RedirectToAction("Index", "ShoppingCart");
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.AccountId == accountId);
            if (cartItem != null)
            {
                if(quantity < 1)
                   quantity = 1;
                
                cartItem.Quality = quantity;
                var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {

                    cartItem.TotalPrice = product.ProductPrice * quantity;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "ShoppingCart");
        }
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            int count = 0;

            if (accountId != null)
            {
                // Tính tổng số lượng sản phẩm trong giỏ hàng
                count = (int)_context.Carts
                    .Where(c => c.AccountId == accountId)
                    .Sum(c => c.Quality); // Hoặc c.Quality tùy thuộc vào tên thuộc tính trong model của bạn
            }

            return Json(new { count });
        }
    }
}
