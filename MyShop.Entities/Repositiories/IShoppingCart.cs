using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositiories
{
	public interface IShoppingCart:IGenericRepositiory<CartItem>
	{
		 void AddItem(string userId, int itemId, int quantity);
		IEnumerable<CartItem> GetCartItems(Expression<Func<CartItem, bool>>? perdicate = null);
		void ClearCart();
        int GetCartItemCount();


    }
}
