using System;
using System.Collections.Generic;
using System.Linq;

public class CustomerView {
	public CustomerView() { }

    public void printCustomerMenu(int storeId, ShoppingCart shoppingCart) {
        string store = (storeId == 1) ? "Gryffindor Witchers" :
                       ((storeId == 2) ? "Hufflepuff House" :
                       ((storeId == 3) ? "Ravenclaw Wizards" : "Slytherin Manor"));

        Console.WriteLine("\n\n\n\tWelcome to Magic Supply Store (Retail - {0})", store);
        Console.WriteLine("==============================================================================\n");
        Console.WriteLine("Please enter a number of your task:\n");
        Console.WriteLine("\t1. Display Products\n\t2. Display Workshops");
        Console.WriteLine("\t3. Return to Main Menu\n\t4. Exit\n");

        cartSummary(shoppingCart); //Display a persistent shopping cart summary so customers know their progress
    }

    // prompt customer to select an option from customer menu
    public int getTask() {
        bool approval = false;
        int task = 0;

        while (!approval) {
            Console.Write("Enter an option: ");
            string option = Console.ReadLine();

            Helper taskHelper = new Helper(option);
            HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();

            if (!helperCtrl.checkInput(taskHelper))
                continue;
            else {
                task = int.Parse(option);

                if (task == 5) {
                    HelperView helperView = new HelperView();
                    helperView.printOutBoundError();
                }
                else
                    approval = true;
            }
        }
        return task;
    }

    //display products in a store, paging products and some additional information to help understanding the progress
    public void viewStoreProducts(int storeId, List<InventoryProducts> storeProducts, int page, int totalPages, ShoppingCart shoppingCart) {
        string store = (storeId == 1) ? "Gryffindor Witchers" :
                       ((storeId == 2) ? "Hufflepuff House" :
                       ((storeId == 3) ? "Ravenclaw Wizards" : "Slytherin Manor"));

        Console.WriteLine("\n{0} - Page {1} of {2}\n", store, page + 1, totalPages);
        Console.WriteLine("\nID\tProduct\t\t\t\tStock Level");
        Console.WriteLine("---------------------------------------------------------------");

        for (int i = page*5; i < page*5 + 5; i++) {
            if (i == storeProducts.Count)
                break;

            InventoryProducts aProduct = storeProducts.ElementAt(i);
            Console.WriteLine("{0}\t{1}\t\t{2}", aProduct.Id, aProduct.ProductName, aProduct.CurStock);
        }

        Console.WriteLine("");
        cartSummary(shoppingCart);
    }

    public void cartSummary(ShoppingCart shoppingCart) {
        try {
            if ((shoppingCart.Trolley.Count == 0 || shoppingCart.Trolley == null) && shoppingCart.Workshop == null)
                Console.WriteLine("Your shopping cart: <EMPTY>\n");
            else if ((shoppingCart.Trolley.Count == 0 || shoppingCart.Trolley == null) && shoppingCart.Workshop != null)
                Console.WriteLine("Your shopping cart: 0 items, Workshop Booking");
            else if (shoppingCart.Workshop != null)
                Console.WriteLine("Your shopping cart: {0} Items, Workshop Booking.\n", shoppingCart.Trolley.Count);
            else
                Console.WriteLine("Your shopping cart: {0} Items, No Workshop Booking.\n", shoppingCart.Trolley.Count);
        } catch (NullReferenceException) {
            Console.WriteLine("Your shopping cart: <EMPTY>\n");
        }
    }
}
