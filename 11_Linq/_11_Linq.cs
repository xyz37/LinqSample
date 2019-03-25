using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace _11_Linq
{
	class _11_Linq
	{
		static void Main(string[] args)
		{
			_11_Linq program = new _11_Linq();

			Console.WriteLine("*** Linq to Entities ***");
			program.TestLinqToEntityFramework();

			Console.WriteLine();
			Console.WriteLine("\r\n*** Linq to DataSet ***");
			program.TestLinqToDataSet();

			Console.WriteLine();
			Console.WriteLine("\r\n*** Linq to Object ***");
			program.TestLinqToObject();

			Console.WriteLine();
			Console.WriteLine("\r\n*** Linq to Object(Extension Method) ***");
			program.TestLinqToObjectExtension();

			Console.WriteLine();
			Console.WriteLine("\r\n*** Linq to Xml ***");
			program.TestLinqToXml();

			Console.ReadLine();
		}


		private void TestLinqToEntityFramework()
		{
			using (var db = new StoreContext())
			{
				//db.Database.Delete();
				//new Initializer().InitializeDatabase(db);
				List<Order_Menu> orderMenu = db.Order_Menu.ToList();
				Console.WriteLine("\r\n[주문 내역]");
				var orderList = from om in orderMenu
								group om by om.Order.Id
								into om
								select new
								{
									Name = om.Max(o => string.Format("{0}: Table[{1}]", o.Order.Customer.Name, o.Order.TableNo)),
									Menus = om.Select(o => o.Menu)
								};
				foreach (var order in orderList)
				{
					Console.WriteLine(order.Name);
					foreach (var menu in order.Menus)
					{
						Console.WriteLine("\t{0}", menu);
					}
				}

				Console.WriteLine("\r\n[고객 이름 역순 정렬]");
				var customerOrder = from order in db.Orders
									orderby order.Customer.Name descending
									select order.Customer;
				foreach (var customer in customerOrder)
				{
					Console.WriteLine(customer.Name);
				}

				Console.WriteLine("\r\n[고객별 금액]");
				var sumByCustomer = from om in orderMenu
									group om by om.Order.Customer.Name
									into byCustomer
									select new
									{
										Name = byCustomer.Key,
										Sum = byCustomer.Sum(o => o.Menu.Price),
									};
				foreach (var sum in sumByCustomer)
				{
					Console.WriteLine("{0}\t:{1,7:#,##0}", sum.Name, sum.Sum);
				}
			}
		}


		private void TestLinqToDataSet()
		{
			DataTable customers;
			DataTable menus;
			DataTable orders;
			DataTable orderMenu;
			using (var db = new StoreContext())
			{
				customers = db.Customers.ToDataTable<Customer>("Customer");
				menus = db.Menus.ToDataTable<Menu>("Menu");
				orders = db.Orders.ToDataTable<Order>("Order");
				orderMenu = db.Order_Menu.ToDataTable<Order_Menu>("Order_Menu");
			}

			Console.WriteLine("\r\n[주문 내역]");
			var orderList = from om in orderMenu.Rows.Cast<DataRow>()
							join o in orders.Rows.Cast<DataRow>()
							on om["OrderId"] equals o["Id"]
							join c in customers.Rows.Cast<DataRow>()
							on o["CustomerId"] equals c["Id"]
							select new
							{
								OrderId = om["OrderId"],
								MenuId = om["MenuId"],
								TableNo = o["TableNo"],
								CustomerName = c["Name"],
							}
							into omj
							group omj by omj.OrderId
							into omg
							select new
							{
								OrderId = omg.Key,
								TableNo = omg.Max(x => x.TableNo),
								CustomerName = omg.Max(x => x.CustomerName),
								Menus = from m in menus.Rows.Cast<DataRow>()
										join mId in omg.Select(x => x.MenuId)
											on m["Id"] equals mId
										select new
										{
											Menu = string.Format("Id:{0}, Name:{1}, Price:{2,7:#,##0}, IsHot:{3}", m["Id"], m["Name"], m["Price"], m["IsHot"]),
											Price = Convert.ToInt32(m["Price"]),
										},
							};
			foreach (var order in orderList)
			{
				Console.WriteLine("{0}: Table[{1}]", order.CustomerName, order.TableNo);
				foreach (var menu in order.Menus)
				{
					Console.WriteLine("\t{0}", menu.Menu);
				}
			}

			Console.WriteLine("\r\n[고객 이름 역순 정렬]");
			var customerOrder = from order in orders.Rows.Cast<DataRow>()
								join customer in customers.Rows.Cast<DataRow>()
									on order["CustomerId"] equals customer["Id"]
								orderby customer["Name"] descending
								select customer["Name"];
			foreach (var customer in customerOrder)
			{
				Console.WriteLine(customer);
			}

			Console.WriteLine("\r\n[고객별 금액]");
			var sumByCustomer = from ol in orderList
								group ol by ol.CustomerName
								into byCustomer
								select new
								{
									Name = byCustomer.Key,
									Sum = byCustomer.Sum(c => c.Menus.Sum(x => x.Price)),
								};
			foreach (var sum in sumByCustomer)
			{
				Console.WriteLine("{0}\t:{1,7:#,##0}", sum.Name, sum.Sum);
			}
		}


		private void TestLinqToObject()
		{
			List<Customer> customers;
			List<Menu> menus;
			List<Order> orders;
			List<Order_Menu> orderMenu;
			using (var db = new StoreContext())
			{
				customers = db.Customers.ToList();
				menus = db.Menus.ToList();
				orders = db.Orders.ToList();
				orderMenu = db.Order_Menu.ToList();
			}

			Console.WriteLine("\r\n[주문 내역]");
			var orderList = from om in orderMenu
							join o in orders
							on om.OrderId equals o.Id
							join c in customers
							on o.CustomerId equals c.Id
							select new
							{
								OrderId = om.OrderId,
								MenuId = om.MenuId,
								TableNo = o.TableNo,
								CustomerName = c.Name,
							}
							into omj
							group omj by omj.OrderId
							into omg
							select new
							{
								OrderId = omg.Key,
								TableNo = omg.Max(x => x.TableNo),
								CustomerName = omg.Max(x => x.CustomerName),
								Menus = from m in menus
										join mId in omg.Select(x => x.MenuId)
											on m.Id equals mId
										select new
										{
											Menu = string.Format("Id:{0}, Name:{1}, Price:{2,7:#,##0}, IsHot:{3}", m.Id, m.Name, m.Price, m.IsHot),
											Price = Convert.ToInt32(m.Price),
										},
							};
			foreach (var order in orderList)
			{
				Console.WriteLine("{0}: Table[{1}]", order.CustomerName, order.TableNo);
				foreach (var menu in order.Menus)
				{
					Console.WriteLine("\t{0}", menu.Menu);
				}
			}

			Console.WriteLine("\r\n[고객 이름 역순 정렬]");
			var customerOrder = from order in orders
								join customer in customers
									on order.CustomerId equals customer.Id
								orderby customer.Name descending
								select customer.Name;
			foreach (var customer in customerOrder)
			{
				Console.WriteLine(customer);
			}

			Console.WriteLine("\r\n[고객별 금액]");
			var sumByCustomer = from ol in orderList
								group ol by ol.CustomerName
								into byCustomer
								select new
								{
									Name = byCustomer.Key,
									Sum = byCustomer.Sum(c => c.Menus.Sum(x => x.Price)),
								};
			foreach (var sum in sumByCustomer)
			{
				Console.WriteLine("{0}\t:{1,7:#,##0}", sum.Name, sum.Sum);
			}
		}


		private void TestLinqToObjectExtension()
		{
			List<Customer> customers;
			List<Menu> menus;
			List<Order> orders;
			List<Order_Menu> orderMenu;
			using (var db = new StoreContext())
			{
				customers = db.Customers.ToList();
				menus = db.Menus.ToList();
				orders = db.Orders.ToList();
				orderMenu = db.Order_Menu.ToList();
			}

			Console.WriteLine("\r\n[주문 내역]");
			var orderList = orderMenu
				.Join(orders,
					om => om.OrderId,
					order => order.Id,
					(om, o) => new
					{
						OrderId = om.OrderId,
						MenuId = om.MenuId,
						TableNo = o.TableNo,
						CustomerId = o.CustomerId,
					})
				.Join(customers,
					om => om.CustomerId,
					customer => customer.Id,
					(om, c) => new
					{
						OrderId = om.OrderId,
						MenuId = om.MenuId,
						TableNo = om.TableNo,
						CustomerName = c.Name,
					})
				.GroupBy(k => k.OrderId)
				.Select(omg => new
				{
					OrderId = omg.Key,
					TableNo = omg.Max(x => x.TableNo),
					CustomerName = omg.Max(x => x.CustomerName),
					Menus = menus
						.Join(omg.Select(x => x.MenuId),
							menu => menu.Id,
							menuId => menuId,
							(m, menuId) => new
							{
								Menu = string.Format("Id:{0}, Name:{1}, Price:{2,7:#,##0}, IsHot:{3}", m.Id, m.Name, m.Price, m.IsHot),
								Price = Convert.ToInt32(m.Price),
							}),
				});
			foreach (var order in orderList)
			{
				Console.WriteLine("{0}: Table[{1}]", order.CustomerName, order.TableNo);
				foreach (var menu in order.Menus)
				{
					Console.WriteLine("\t{0}", menu.Menu);
				}
			}

			Console.WriteLine("\r\n[고객 이름 역순 정렬]");
			var customerOrder = orders.Join(customers,
					order => order.CustomerId,
					customer => customer.Id,
					(o, c) => c.Name)
					.OrderByDescending(c => c);
			foreach (var customer in customerOrder)
			{
				Console.WriteLine(customer);
			}

			Console.WriteLine("\r\n[고객별 금액]");
			var sumByCustomer = orderList
				.GroupBy(ol => ol.CustomerName)
				.Select(byCustomer => new
				{
					Name = byCustomer.Key,
					Sum = byCustomer.Sum(c => c.Menus.Sum(x => x.Price)),
				});
			foreach (var sum in sumByCustomer)
			{
				Console.WriteLine("{0}\t:{1,7:#,##0}", sum.Name, sum.Sum);
			}
		}


		private void TestLinqToXml()
		{
			// 참고1: https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/concepts/linq/linq-to-xml-overview
			// 참고2: https://www.dotnetcurry.com/linq/564/linq-to-xml-tutorials-examples
			XElement menus =
				new XElement("Menus",
					new XElement("Menu",
						new XElement("Id", 1),
						new XElement("Name", "짜장"),
						new XElement("Price", 6000),
						new XElement("IsHot", false)
					),
					new XElement("Menu",
						new XElement("Id", 2),
						new XElement("Name", "우동"),
						new XElement("Price", 6000),
						new XElement("IsHot", false)
					),
					new XElement("Menu",
						new XElement("Id", 3),
						new XElement("Name", "짬뽕"),
						new XElement("Price", 7000),
						new XElement("IsHot", true)
					),
					new XElement("Menu",
						new XElement("Id", 4),
						new XElement("Name", "불짬뽕"),
						new XElement("Price", 8000),
						new XElement("IsHot", true)
					),
					new XElement("Menu",
						new XElement("Id", 5),
						new XElement("Name", "김밥"),
						new XElement("Price", 2000),
						new XElement("IsHot", false)
					),
					new XElement("Menu",
						new XElement("Id", 6),
						new XElement("Name", "멸추김밥"),
						new XElement("Price", 4000),
						new XElement("IsHot", true)
					)
				);
			XDocument xdoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));

			xdoc.Add(menus);
			//XmlFileControl(menus, xdoc);

			Console.WriteLine("\r\n[매운 메뉴별 금액 합]");
			var sumByHot = from menu in menus.Descendants("Menu")
						   group menu by (bool)menu.Element("IsHot")
						   into m
						   select new
						   {
							   IsHot = m.Key,
							   대표메뉴 = m.Max(x => (string)x.Element("Name")),
							   Price = m.Sum(x => (int)x.Element("Price")),
						   };

			foreach (var sum in sumByHot)
			{
				Console.WriteLine("{0}: {1}\t:{2,7:#,##0}",
					sum.IsHot == true ? "맵다" : "일반",
					sum.대표메뉴,
					sum.Price);
			}
		}

		private static void XmlFileControl(XElement menus, XDocument xdoc)
		{
			const string XML_FILE_NAME = @".\Menus.xml";

			Console.WriteLine(menus);

			if (File.Exists(XML_FILE_NAME) == true)
			{
				File.Delete(XML_FILE_NAME);
			}

			xdoc.Save(XML_FILE_NAME);

			var readDoc = XDocument.Load(XML_FILE_NAME);

			Console.WriteLine(readDoc);
		}
	}
}
