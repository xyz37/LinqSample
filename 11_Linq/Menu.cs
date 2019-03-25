using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace _11_Linq
{
	public class Menu
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }
		public bool IsHot { get; set; }

		public override string ToString()
		{
			return string.Format("Id:{0}, Name:{1}, Price:{2,7:#,##0}, IsHot:{3}", Id, Name, Price, IsHot);
		}
	}
}
