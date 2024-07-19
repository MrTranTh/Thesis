using Microsoft.AspNetCore.Mvc;

namespace Thesis.Models
{
	public class ShoppingCart
	{
		public List<ShoppingCartItems> Items { get; set; }

		public ShoppingCart() {
			this.Items = new List<ShoppingCartItems>();
		}

		public void AddToCart(ShoppingCartItems items, int quantity)
		{
			var checkExit = Items.FirstOrDefault(x => x.ProductId == items.ProductId);
			if(checkExit != null)
			{
				checkExit.Quantity += quantity;
				checkExit.TotalPrice = checkExit.Price * checkExit.Quantity;

			}
			else
			{
				Items.Add(items);
			}
		}

		public void Remove(int id)
		{
			var checkExit = Items.SingleOrDefault(x => x.ProductId == id);
			if(checkExit != null)
			{
				Items.Remove(checkExit);
			}
		}

		public void UpdateQuantity(int id, int quantity)
		{
			var checkExit = Items.SingleOrDefault(x => x.ProductId == id);
			if (checkExit != null)
			{
				checkExit.Quantity = quantity;
				checkExit.TotalPrice = checkExit.Price * checkExit.Quantity;
			}
		}

		public decimal getTotalPrice()
		{
			return Items.Sum(x => x.TotalPrice);
		}

		public int getTotalQuantity()
		{
			return Items.Sum(x => x.Quantity);
		}

		public void clearCart()
		{
			Items.Clear();
		}
	}

	public class ShoppingCartItems
	{
		public int ProductId {  get; set; }

		public string ProductName { get; set; }	

		public string ProductImage { get; set; }

		public int Quantity {  get; set; }

		public decimal Price { get; set; }

		public decimal TotalPrice { get; set; }

		public string CategoryName { get; set; }

		public int CategoryId {  get; set; }
	}
}
