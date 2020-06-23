﻿using System;
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
            var menuHelper = new MenuHandler();
            var jsonPath = menuHelper.SetPath("Produkter.json");
            if (File.Exists(jsonPath))
            {
                menuHelper.RunMenu(jsonPath);
                
            } 
            else
                Console.WriteLine("Input file not found");
        }
        
    }
}
