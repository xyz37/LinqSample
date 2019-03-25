using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace _11_Linq
{
	public class StoreContext : DbContext
	{
		public StoreContext()
		{
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<StoreContext>());
		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Menu> Menus { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Order_Menu> Order_Menu { get; set; }

	}
}
