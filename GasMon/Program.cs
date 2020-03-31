using System;
using System.Runtime.InteropServices.ComTypes;

namespace GasMon
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World");
            ILocationsFetcher locationFetcher = new LocationsFetcher();
            var locationList = locationFetcher.LocationListMaker();
            Console.WriteLine(locationList[0].x);
        }
        
    }
}