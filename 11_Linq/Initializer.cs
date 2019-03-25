using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace _11_Linq
{
	public class Initializer : DropCreateDatabaseIfModelChanges<StoreContext>
	{
		public override void InitializeDatabase(StoreContext context)
		{
			Seed(context);
			base.InitializeDatabase(context);
		}

		protected override void Seed(StoreContext db)
		{
			Console.WriteLine("Initialize database...");

			var menu1 = new Menu { Name = "짜장", Price = 6000, IsHot = false };
			var menu2 = new Menu { Name = "우동", Price = 6000, IsHot = false };
			var menu3 = new Menu { Name = "짬뽕", Price = 7000, IsHot = true };
			var menu4 = new Menu { Name = "불짬뽕", Price = 8000, IsHot = true };
			var menu5 = new Menu { Name = "김밥", Price = 2000, IsHot = false };
			var menu6 = new Menu { Name = "멸추김밥", Price = 4000, IsHot = true };

			db.Menus.Add(menu1);
			db.Menus.Add(menu2);
			db.Menus.Add(menu3);
			db.Menus.Add(menu4);
			db.Menus.Add(menu5);
			db.Menus.Add(menu6);

			var customer1 = new Customer { Id = 1, Name = "중식 고객", };
			var customer2 = new Customer { Id = 2, Name = "한식 고객", };
			var customer3 = new Customer { Id = 3, Name = "순한맛 고객", };
			var customer4 = new Customer { Id = 4, Name = "매운맛 고객", };

			db.Customers.Add(customer1);
			db.Customers.Add(customer2);
			db.Customers.Add(customer3);
			db.Customers.Add(customer4);

			db.SaveChanges();

			db.Orders.Add(new Order
			{
				Id = 1,
				TableNo = 1,
				CustomerId = 1,       // 중식 고객
			});
			db.Order_Menu.AddRange(new List<Order_Menu>
			{
				new Order_Menu { OrderId = 1, MenuId = 1 },
				new Order_Menu { OrderId = 1, MenuId = 1 },
				new Order_Menu { OrderId = 1, MenuId = 3 },
			});

			db.Orders.Add(new Order
			{
				Id = 2,
				TableNo = 2,
				CustomerId = 2,       // 한식 고객
			});
			db.Order_Menu.AddRange(new List<Order_Menu>
			{
				new Order_Menu { OrderId = 2, MenuId = 5 },
				new Order_Menu { OrderId = 2, MenuId = 6 },
			});

			db.Orders.Add(new Order
			{
				Id = 3,
				TableNo = 3,
				CustomerId = 3,       // 순한맛 고객
			});
			db.Order_Menu.AddRange(new List<Order_Menu>
			{
				new Order_Menu { OrderId = 3, MenuId = 1 },
				new Order_Menu { OrderId = 3, MenuId = 2 },
				new Order_Menu { OrderId = 3, MenuId = 5 },
			});

			db.Orders.Add(new Order
			{
				Id = 4,
				TableNo = 4,
				CustomerId = 4,       // 매운맛 고객
			});
			db.Order_Menu.AddRange(new List<Order_Menu>
			{
				new Order_Menu { OrderId = 4, MenuId = 3 },
				new Order_Menu { OrderId = 4, MenuId = 4 },
				new Order_Menu { OrderId = 4, MenuId = 6 },
			});

			db.Orders.Add(new Order
			{
				Id = 5,
				TableNo = 9,
				CustomerId = 4,       // 매운맛 고객
			});
			db.Order_Menu.AddRange(
				db.Menus.Where(x => x.IsHot == true).ToList()
					.Select(x => new Order_Menu { OrderId = 5, MenuId = x.Id }).ToList());

			db.SaveChanges();

			base.Seed(db);
		}
	}
}
