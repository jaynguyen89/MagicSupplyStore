using System;
using FileException;
using System.IO;
using System.Collections;

public class StockRequestsView {
	public StockRequestsView() { }

    public void displayAllStockRequests(StockRequests[] requests) {
        Console.WriteLine("\nID\tStore\t\t\tProduct\t\t\t\tQuantity\tCurrent Stock\tStock Availability");
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------");

        if (requests == null || requests.Length == 0) {
            HelperView helperView = new HelperView();
            helperView.printNoRequests();
        }
        else {
            foreach (StockRequests request in requests) {
                string availability = (request.Available) ? "True" : "False";
                Console.WriteLine("{0}\t{1,10}\t{2}\t\t\t{3}\t\t{4}\t\t\t{5}", request.Id, request.Store, request.ProductName, request.Qty, request.CurStock, availability);
            }
        }
    }

    public bool askContext() {
        HelperView helperView = new HelperView();
        HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();

        string contextInput = "";
        bool contextCheck = false;

        while (!contextCheck) {
            helperView.askContext();
            contextInput = Console.ReadLine();

            Helper contextHelper = new Helper(contextInput);

            int result = helperCtrl.checkContext(contextHelper);
            if (result == 0) {
                helperView.printEmptyError();
                continue;
            }
            else if (result == 1) {
                contextCheck = true;
                return true;
            }
            else {
                contextCheck = true;
                return false;
            }
        }
        return false;
    }
}
