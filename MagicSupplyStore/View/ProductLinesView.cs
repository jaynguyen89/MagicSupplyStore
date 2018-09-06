using System;
using System.Collections.Generic;

public class ProductLinesView {
	public ProductLinesView() {	}

    public void displayAllProducts(List<ProductLines> productLines, bool context) {
        if (!context)
            Console.WriteLine("\nNew Products List\n");

        Console.WriteLine("\nID\tProduct\t\t\t\tCurrent Stock");
        Console.WriteLine("------------------------------------------------------------------------------");

        foreach (ProductLines productLine in productLines)
            Console.WriteLine("{0}\t{1}\t\t\t{2}", productLine.Id, productLine.ProductName, productLine.Quantity);
    }
}
