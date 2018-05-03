using plcdemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			var objeto = new Form1();
			int a = 100, b = 200;

			Console.WriteLine("La suma de {0} + {1} = {2}", a, b, objeto.Sumar(a, b));
			Console.ReadLine();
		}
	}
}
