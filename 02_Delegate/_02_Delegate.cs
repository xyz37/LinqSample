using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _02_Delegate
{
	class _02_Delegate
	{
		static void Main(string[] args)
		{
			_02_Delegate program = new _02_Delegate();

			Console.WriteLine("*** Basic Delegate 1 (반환값이 없음) ***");
			program.TestBasicDelegateWithoutReturn();

			Console.WriteLine();
			Console.WriteLine("*** Basic Delegate 2 (반환값이 있음) ***");
			program.TestBasicDelegateWithReturn();

			Console.WriteLine();
			Console.WriteLine("*** Basic Delegate 3 (MulticastDelegate) ***");
			program.TestBasicDelegate3_Multicast();

			Console.WriteLine();
			Console.WriteLine("*** Basic Event ***");
			program.TestBasicEvent();

			Console.WriteLine();
			Console.WriteLine("*** Event Class ***");
			program.TestEventClass();

			Console.ReadLine();
		}

		private delegate void Logger(string message);
		private enum LogHandlers { Default = 0, FileLogger, DbLogger };

		private void FileLogger(string message)
		{
			Console.WriteLine("Logging for file: {0}", message);
		}

		private void DbLogger(string message)
		{
			Console.WriteLine("Logging for Db  : {0}", message);
		}

		private void TestBasicDelegateWithoutReturn()
		{
			string message = "log message";

			BasicDelegateCore(LogHandlers.Default, message);
			BasicDelegateCore(LogHandlers.FileLogger, message);
			BasicDelegateCore(LogHandlers.DbLogger, message);
		}

		private void BasicDelegateCore(LogHandlers logHandler, string message)
		{
			Logger logger = null;

			switch (logHandler)
			{
				case LogHandlers.FileLogger:
					logger = FileLogger;

					break;
				case LogHandlers.DbLogger:
					logger = DbLogger;

					break;
				default:
					logger = Console.WriteLine;

					break;
			}

			logger(message);
		}


		private delegate int SumAction(int a, int b);

		private int Sum1(int a, int b)
		{
			return a + b;
		}

		private int Sum2(int a, int b)
		{
			return a * 2 + b * 2;
		}

		private void TestBasicDelegateWithReturn()
		{
			SumAction sum1 = new SumAction(Sum1);
			SumAction sum2 = new SumAction(Sum2);

			Console.WriteLine("sum1 (1, 3) = {0}", sum1(1, 3));
			Console.WriteLine("sum2 (1, 3) = {0}", sum2.Invoke(1, 3));
		}


		private void TestBasicDelegate3_Multicast()
		{
			Logger logger = Console.WriteLine;
			logger += FileLogger;
			logger += DbLogger;

			logger("All log handlers message");
		}


		private delegate void EventHandler2<TEventArgs>(object sender, EventArgs e)
			where TEventArgs : EventArgs;

		private void TestBasicEvent()
		{
			EventHandler2<EventArgs> handler = new EventHandler2<EventArgs>(TestHandler);

			handler(new object(), EventArgs.Empty);
		}

		private void TestHandler(object sender, EventArgs e)
		{
			Console.WriteLine("object:{0}, e:{1}", sender ?? "null", e);
			//Console.WriteLine("object:{0}, e:{1}", sender == null ? "null" : sender, e);
		}


		private void TestEventClass()
		{
			EventClass eventClass = new EventClass();

			eventClass.Started += new EventHandler<EventClass.StartedEventArgs>(OnEventClassStarted);
			eventClass.Started += OnEventClassStarted;

			eventClass.Start();
		}

		private void OnEventClassStarted(object sender, EventClass.StartedEventArgs e)
		{
			Console.WriteLine("Started on eventClass: {0}", e.Tag);
		}
	}
}
