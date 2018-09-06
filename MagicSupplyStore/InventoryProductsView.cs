using System;
using System.Collections.Generic;

public class InventoryProductsView {
	public InventoryProductsView() { }

    public void displayInventoryProducts(List<InventoryProducts> inventory) {
        Console.WriteLine("\nID\tProduct\t\t\t\tCurrent Stock\t\tRestock");
        Console.WriteLine("------------------------------------------------------------------------------");

        if (inventory == null || inventory.Count == 0) {
            HelperView helperView = new HelperView();
            helperView.printNoRequests();
        }
        else {
            foreach (InventoryProducts product in inventory) {
                string restock = (product.Restock) ? "True" : "False";
                Console.WriteLine("{0}\t{1}\t\t{2}\t\t{3}", product.Id, product.ProductName, product.CurStock, restock);
            }
        }
    }
}
