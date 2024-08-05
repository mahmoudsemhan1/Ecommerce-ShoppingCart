using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models
{
	public class OrderHeader
	{
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }  // User details not validated

        public DateTime OrderTime { get; set; }
        public DateTime? ShippingDate { get; set; }  // Shipping date might not be known at the time of order
        public decimal TotalPrice { get; set; }

        public string? OrderStatus { get; set; }  // Status of the order
        public string? PaymentStatus { get; set; }  // Payment status

        public string? TrackingNumber { get; set; }  // Optional tracking number
        public string? Carrier { get; set; }  // Optional carrier name

        public DateTime? PaymentDate { get; set; }  // Payment date might not be known immediately

        // Stripe-related properties
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }  // Corrected spelling to "PaymentIntent"

        // User details
        public string? Name { get; set; }  // Customer's name
        public string? Address { get; set; }  // Corrected spelling to "Address"
        public string? City { get; set; }  // Customer's city
        public string? Phone { get; set; }  // Customer's phone number
    }



}

