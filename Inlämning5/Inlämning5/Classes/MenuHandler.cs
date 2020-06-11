using System;


namespace Inlämning4.Classes
{
    public class MenuHandler
    {
        internal static void DisplayMenu()
        {
            Console.WriteLine("Choose an action from the menu:");
            Console.WriteLine("_______________________________");
            Console.WriteLine(" 1.  Add a product");
            Console.WriteLine(" 2.  Update a product");
            Console.WriteLine(" 3.  Delete a product");
            Console.WriteLine(" 4.  Find product availability");
            Console.WriteLine(" 5.  Modify product availability");
            Console.WriteLine(" 6.  Search products by likelihood");
            Console.WriteLine(" 7.  Search products by price threshold");
            Console.WriteLine(" 8.  List products available by manufacturers");
            Console.WriteLine(" 9.  Remove a shop from stock");
            Console.WriteLine(" 0.  Exit the program");
        }
    }
}
