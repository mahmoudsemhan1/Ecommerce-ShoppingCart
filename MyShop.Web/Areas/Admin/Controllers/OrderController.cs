using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Utilities;
using MyShop.Entities.Models;
using MyShop.Entities.Models.ViewModels;
using MyShop.Entities.Repositiories;
using Stripe;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("AdminRole")]


    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

    

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Fetch all order headers
            var orderHeaders = _unitOfWork.orderHeader.GetAll().ToList();
            // Fetch all order details
            var orderDetails = _unitOfWork.orderDetails.GetAll().ToList();
            var users=_unitOfWork.ApplicationUser.GetAll().ToList();

            // Create a list of OrderVM, each containing a single OrderHeader and its related OrderDetails
            var orders = orderHeaders.Select(orderHeader => new OrderVM
            {
                orderHeader = orderHeader,
                OrderDetails = orderDetails.Where(od => od.OrderHeaderId == orderHeader.Id),
                ApplicationUsers=orderHeader.ApplicationUser
            }).ToList();

            return View(orders); // Pass the list of OrderVM to the view
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            // Fetch the order header with the related ApplicationUser details
            var orderHeader = _unitOfWork.orderHeader.GetFirstOrDefualt(
                 o => o.Id == id,
                IncludeWord: "ApplicationUser"
            );

            if (orderHeader == null)
            {
                return NotFound(); // Return 404 if the order is not found
            }

            // Fetch order details for the specific order
            var orderDetails = _unitOfWork.orderDetails.GetAll(
                od => od.OrderHeaderId == id,IncludeWord:"Product"
            ).ToList();

            // Create the view model
            var orderVM = new OrderVM
            {
                orderHeader = orderHeader,
                OrderDetails = orderDetails,
         
            };

            return View(orderVM); // Pass the OrderVM to the view
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrder(OrderVM orderVM)
        {
           
            var orderFromDb = _unitOfWork.orderHeader.GetFirstOrDefualt(u => u.Id == orderVM.orderHeader.Id);

            if (orderFromDb == null)
            {
                return NotFound();
            }

            // Update the fields
           // orderFromDb.Name = orderVM.orderHeader.Name; //why admin change name ??? 
            orderFromDb.Phone = orderVM.orderHeader.Phone;
            orderFromDb.Address = orderVM.orderHeader.Address;
            orderFromDb.City = orderVM.orderHeader.City;
            if (!string.IsNullOrEmpty(orderVM.orderHeader.Carrier))
            {
                orderFromDb.Carrier = orderVM.orderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(orderVM.orderHeader.TrackingNumber))
            {
                orderFromDb.TrackingNumber = orderVM.orderHeader.TrackingNumber;
            }

            _unitOfWork.orderHeader.update(orderFromDb);
            _unitOfWork.Complete();

            TempData["Update"] = "Data has been updated successfully";
            return RedirectToAction("Details", "Order", new { orderVM.orderHeader.Id });
        }
        [HttpPost]
        public IActionResult StartProcess(OrderVM orderVM)
        {

            _unitOfWork.orderHeader.UpdateOrderStatus(orderVM.orderHeader.Id, SD.Proccessing, null);
            _unitOfWork.Complete();

         

            TempData["Update"] = "Order Status has been updated successfully";
            return RedirectToAction("Index", "Order", new { orderVM.orderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartShip(OrderVM orderVM)
        {
            var orderFromDb = _unitOfWork.orderHeader.GetFirstOrDefualt(u => u.Id == orderVM.orderHeader.Id);
            orderFromDb.TrackingNumber = orderVM.orderHeader.TrackingNumber;
            orderFromDb.Carrier = orderVM.orderHeader.Carrier;
            orderFromDb.OrderStatus = SD.Shipped;
            orderFromDb.ShippingDate = DateTime.Now;

            _unitOfWork.orderHeader.update(orderFromDb);
            _unitOfWork.Complete();

            TempData["Update"] = "Order  has shipped  successfully";
            return RedirectToAction("Details", "Order", new { orderVM.orderHeader.Id });
        }
        [HttpPost]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            var orderFromDb = _unitOfWork.orderHeader.GetFirstOrDefualt(u => u.Id == orderVM.orderHeader.Id);

            if (orderFromDb.PaymentStatus == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent=orderFromDb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(option);

                
                    _unitOfWork.orderHeader.UpdateOrderStatus(orderFromDb.Id, SD.Cancelled, SD.Refund);


            }
            else
            {
                _unitOfWork.orderHeader.UpdateOrderStatus(orderFromDb.Id, SD.Cancelled, SD.Refund);

            }
            _unitOfWork.Complete();




            TempData["Update"] = "Order  has been Cancelled successfully";
            return RedirectToAction("Index", "Order", new { orderVM.orderHeader.Id });
        }

    } 
}




