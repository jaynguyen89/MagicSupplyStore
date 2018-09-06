using System;

public class ShoppingCartView {
	public ShoppingCartView() { }

    public void informInvoice(double invoice) {
        Console.WriteLine("\nYour bill for this purchase is: ${0}", Math.Round(invoice, 2));
        Console.Write("\nPlease enter a Credit Card Number to process: ");
    }
}
