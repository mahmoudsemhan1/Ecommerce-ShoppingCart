using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Utilities;
using MyShop.DataAccess.Implementation;
using MyShop.Entities.Models;
using MyShop.Entities.Models.ViewModels;
using MyShop.Entities.Repositiories;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ShoppingCart _shoppingCart;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int TotalCarts { get; set; }

        public CartController(IUnitOfWork unitOfWork, ShoppingCart shoppingCart)
        {
            _unitOfWork = unitOfWork;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Index()
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var product = _unitOfWork.product.GetAll();


            ShoppingCartVM = new ShoppingCartVM()
            {
                CartItemsList = _shoppingCart.GetCartItems(u => u.ApplicationUserId == claim.Value)
            };
            foreach (var item in ShoppingCartVM.CartItemsList)
            {
                ShoppingCartVM.TotalCarts += (item.Quantity * item.Product.Price);
            }

            return View(ShoppingCartVM);
        }

        [HttpGet]
        public IActionResult GetCartItemCount()
        {

            int count = _shoppingCart.GetItemCount(); // Method to get cart item count
            return Json(new { count });
        }
        [HttpGet]
        public IActionResult Plus(int CartItemId)
        {
            var CartItem = _shoppingCart.GetFirstOrDefualt(u => u.CartItemId == CartItemId);
            {
                CartItem.Quantity += 1;
                _unitOfWork.Complete();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Minus(int CartItemId)
        {
            var CartItem = _shoppingCart.GetFirstOrDefualt(u => u.CartItemId == CartItemId);

            if (CartItem.Quantity <= 1)
            {
                _shoppingCart.Remove(CartItem);
                _unitOfWork.Complete();

                return RedirectToAction("Index", "Home", new { area = "" });

            }
            else
            {
                CartItem.Quantity -= 1;
            }
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Remove(int CartItemId)
        {
            var CartItem = _shoppingCart.GetFirstOrDefualt(u => u.CartItemId == CartItemId);
            _shoppingCart.Remove(CartItem);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Summary()
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var product = _unitOfWork.product.GetAll();


            ShoppingCartVM = new ShoppingCartVM()
            {
                CartItemsList = _shoppingCart.GetCartItems(u => u.ApplicationUserId == claim.Value),
                OrderHeader = new()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefualt(x => x.Id == claim.Value);
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Adress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var item in ShoppingCartVM.CartItemsList)
            {
                ShoppingCartVM.TotalCarts += (item.Quantity * item.Product.Price);
            }

            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult PostSummary(ShoppingCartVM shoppingCartVM)
        {
            var claimIdentitiy = (ClaimsIdentity)User.Identity;
            var claim = claimIdentitiy.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM.CartItemsList = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,IncludeWord:"Product");
               

            shoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
            shoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
            shoppingCartVM.OrderHeader.OrderTime = DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
            shoppingCartVM.OrderHeader.Phone = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.UserName;


            foreach (var item in shoppingCartVM.CartItemsList)
            {
                shoppingCartVM.OrderHeader.TotalPrice += (item.Quantity * item.Product.Price);
            }
            _unitOfWork.orderHeader.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Complete();
            foreach (var item in shoppingCartVM.CartItemsList)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = shoppingCartVM.OrderHeader.Id,
                    Price=item.Product.Price,
                    Quantity=item.Quantity
                  
                };
                _unitOfWork.orderDetails.Add(orderDetails);
                _unitOfWork.Complete();
            }

            var domain = "https://localhost:44301/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + "Customer/Cart/index"
            };

            foreach (var item in shoppingCartVM.CartItemsList)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), // Amount in cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Quantity
                });
            }

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);
            shoppingCartVM.OrderHeader.SessionId = session.Id;
            _unitOfWork.Complete();

            return Redirect(session.Url);  // This will redirect the user to the Stripe Checkout page
        }
    }
}