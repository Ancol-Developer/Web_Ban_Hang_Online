namespace BanHangOnline.Models
{
	public class ShoppingCart
	{
		public List<ShoppingCartItem> Items { get; set; }
        public ShoppingCart()
        {
			this.Items = new List<ShoppingCartItem>();
		}

		public void AddToCard(ShoppingCartItem item, int quantity)
		{
			var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
			if (checkExits is not null)
			{
				checkExits.Quantity += quantity;
				checkExits.TotalPrice = checkExits.Price* checkExits.Price;
			}
			else
			{
				Items.Add(item);
			}
		}

		public void Remove(int id)
		{
			var checkExits = Items.FirstOrDefault(x => x.ProductId == id);
			if (checkExits is not null)
			{
				Items.Remove(checkExits);
			}
		}

		public decimal GetTotalPrice()
		{
			return Items.Sum(x => x.TotalPrice);
		}

		public decimal GetTotalQuantity()
		{
			return Items.Sum(x => x.Quantity);
		}
		
		public void ClearCart()
		{
			Items.Clear();
		}

		public void UpdateQuantity(int id, int quantity)
		{
			var checkExits = Items.FirstOrDefault(x => x.ProductId == id);
			if (checkExits is not null)
			{
				checkExits.Quantity = quantity;
				checkExits.TotalPrice = checkExits.Price * checkExits.Price;
			}
		}
    }

	public class ShoppingCartItem
	{
		public int ProductId { get; set; }
		public string? ProductName { get; set; }
		public string? Alias { get; set; }
		public string? CategoryName { get; set; }
		public string? ProductImage { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal TotalPrice { get; set; }

	}
}
