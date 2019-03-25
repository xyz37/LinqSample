using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace _11_Linq
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int TableNo { get; set; }

		public virtual Customer Customer { get; set; }
		public virtual ICollection<Menu> Menus { get; set; }
	}
}
