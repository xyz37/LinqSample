using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace _11_Linq
{
	[System.Diagnostics.DebuggerDisplay("{Order.Customer.Name}, {Menu.Name}({Menu.Price:#,##0})​", Name = "Order_Menu")]
	public class Order_Menu
	{
		[Key]
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int MenuId { get; set; }

		[ForeignKey("OrderId")]
		public virtual Order Order { get; set; }
		[ForeignKey("MenuId")]
		public virtual Menu Menu { get; set; }
	}
}
