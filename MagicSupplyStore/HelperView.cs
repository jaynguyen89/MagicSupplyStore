using System;

public class HelperView {
	public HelperView() { }

    public void printWelcome() {
        Console.WriteLine("\n\n\n\tWelcome to Magic Supply Store");
        Console.WriteLine("================================================\n");
        Console.WriteLine("Please enter a number of your position:\n");
        Console.WriteLine("\t1. Store Owner\n\t2. Franchise Owner");
        Console.WriteLine("\t3. Customer\n\t4. Quit\n");
    }

    //public delegate void ask(string s);

    public void askPosition() {
        Console.Write("Your position: ");
    }

    public void askRespond(bool context) {
        if (context)
            Console.Write("\nType \'Yes / Y / y\' to continue, otherwise type \'No / N / n\': ");
        else {
            Console.WriteLine("\n[\'P\' Next Page | \'R\' Return to Menu | \'C\' Complete Transaction]\n");
            Console.Write("Enter Item ID to purchase OR Action: ");
        }
    }

    public void askQty() {
        Console.Write("Enter the amount you want to purchase: ");
    }

    public void askWorkshopId() {
        Console.Write("\nSelect a workshop ID to make booking: ");
    }

    public void askRequest(bool context) {
        string message = (context) ? "Enter Product ID to add: " : "Enter Request ID to process: ";
        Console.Write("\n{0}", message);
    }

    public void askContext() {
        Console.Write("\nEnter type of Requests to display: ");
    }

    public void askStore(bool context) {
        if (context)
            Console.Write("\nEnter your Store ID to continue: ");
        else {
            Console.WriteLine("\nSelect a store to go:");
            Console.WriteLine("\t1. Gryffindor Witchers\n\t2. Hufflepuff House\n\t3. Ravenclaw Wizards\n\t4. Slytherin Manor\n");
            Console.Write("Enter a Store ID: ");
        }
    }

    public void printEmptyError() {
        Console.WriteLine("Error! Too long or empty input.\n");
    }

    public void printNotNumError() {
        Console.WriteLine("Error! Please enter a number.\n");
    }

    public void printOutBoundError() {
        Console.WriteLine("Error! Input out of menu or irrelevant.\n");
    }

    public void printFNFError() {
        Console.WriteLine("\nError! File not found or Empty file.\n");
    }

    public void printFileContentError(int i) {
        string message = (i == 0) ? "Invalid JSON content detected" : "File content mistake detected on line " + i;
        Console.WriteLine("\nError! {0}.\n", message);
    }

    public void printIDNotFound() {
        Console.WriteLine("Error! ID not found in this context.\n");
    }

    public void printUnavailable(bool context) {
        if (!context)
            Console.WriteLine("Error! The selected request has insufficient stock.\n");
        else
            Console.WriteLine("\nThe selected Product still has enough stock.");
    }

    public void printNoRequests() {
        Console.WriteLine("\nTHERE IS TEMPORARILY NO STOCK REQUESTS IN THIS CONTEXT.\n");
    }

    public void printTaskError() {
        Console.WriteLine("\nError! Please enter an action or product ID.\n");
    }

    public void warning(int context) {
        if (context == 0) {
            Console.WriteLine("\nWarning! You have item(s) in your cart. This action will wipe the cart.");
            Console.Write("\'Yes\' to continue, \'No\' to keep your cart: ");
        }
        else if (context == 1)
            Console.WriteLine("\nSorry! The amount of purchase was too big for this product.");
        else
            Console.WriteLine("\nSorry! We currently have no stock for this product.");
    }

    public void printFullMessage() {
        Console.WriteLine("This workshop has been fully reserved. Please select another one.\n");
    }

    public void printCardError() {
        Console.WriteLine("The Credit Card Number was incorrect! Please try again.\n");
    }

    public void congrat(int context) {
        if (context == 0)
            Console.WriteLine("\nYooo! Your booking has been placed.");
        else if (context == 2)
            Console.WriteLine("\nWritting file contents completed. System has been updated.");
        else if (context == 3)
            Console.WriteLine("\nCongrate! Your purchase has been placed. Thank you!");
        else
            Console.WriteLine("\nItem has been added to your shopping cart.");
    }

    public void informPurchaseStatus(Guid guid) {
        Console.WriteLine("Congrate! Your purchase has been placed.");
        Console.WriteLine("Your workshop reference: {0}", guid.ToString().ToUpper());
    }

    public void waiting() {
        Console.Write("\nPress enter to continue ...");
        Console.ReadLine();
    }

    public void goodBye() {
        Console.WriteLine("\nGoodbye!");
    }
}
