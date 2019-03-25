using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace _11_Linq
{
	[System.Diagnostics.DebuggerDisplay("Name:{Name}, Id:{Id}", Name = "Customer")]
	public class Customer
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string Region { get; set; }

		public virtual ICollection<Order_Menu> Orders { get; set; }
	}
}
