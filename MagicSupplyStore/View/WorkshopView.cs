using System;
using System.Collections.Generic;

public class WorkshopView {
	public WorkshopView() { }

    public void displayWorkshops(int storeId, List<Workshop> workshops, ShoppingCart cart) {
        string store = (storeId == 1) ? "Gryffindor Witchers" :
                       ((storeId == 2) ? "Hufflepuff House" :
                       ((storeId == 3) ? "Ravenclaw Wizards" : "Slytherin Manor"));

        Console.WriteLine("\n{0} - Workshops", store);
        Console.WriteLine("\nID\tCapacity\t\tCurrent Bookings");
        Console.WriteLine("---------------------------------------------------------------");

        foreach (Workshop ws in workshops)
            Console.WriteLine("{0}\t{1}\t\t\t{2}", ws.Id, ws.Capacity, ws.Reserved);

        CustomerView cusView = new CustomerView();
        Console.WriteLine("");
        cusView.cartSummary(cart); //the shopping cart information are always kept updated to the customer
    }
}
