using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Inlämning5.Classes;


namespace Inlämning5
{
    class Program
    {
        static void Main(string[] args)
        {
            var menuHandler = new MenuHandler();
            menuHandler.RunMenu();
        }
        
    }
}
