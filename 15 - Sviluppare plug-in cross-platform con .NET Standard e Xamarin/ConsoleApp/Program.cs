using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = Plugin.PlatformService.CrossPlatformService.Current;
            var platform = service.GetPlatform();

            Console.WriteLine(platform);
            Console.WriteLine(DotNetStandardLib.Sample.Sum(12, 34));
            Console.ReadLine();
        }
    }
}
