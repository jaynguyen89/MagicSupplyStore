using System;
using System.Collections.Generic;

public class WorkshopCtrl {
    public Workshop Workshop { get; set; }
    public WorkshopView WSView { get; set; }

	public WorkshopCtrl() { }

    public WorkshopCtrl(Workshop ws, WorkshopView wsv) {
        this.Workshop = ws;
        this.WSView = wsv;
    }

    public List<Workshop> retrieveWorkshops(int storeId) {
        return Workshop.retrieveWorkshops(storeId);
    }

    public void displayWorkshops(int storeId, List<Workshop> workshops, ShoppingCart cart) {
        WSView.displayWorkshops(storeId, workshops, cart);
    }

    //Allow customer to select a workshop id to make a booking
    public void processBooking(int storeId, List<Workshop> workshops, ShoppingCart cart) {
        HelperView view = new HelperView();
        bool bookingDone = false;

        while (!bookingDone) { // determines if customer finishes booking
            bool checkWsid = false;
            string wsId = ""; //select a workshop slot

            while (!checkWsid) {
                view.askWorkshopId();

                wsId = Console.ReadLine();
                Helper wsHelper = new Helper(wsId);

                checkWsid = wsHelper.checkWorkshopId();

                if (checkWsid) { // all workshops use the same checking method so the if statement is to double-check the workshop id
                    if (((storeId == 2 || storeId == 4) && int.Parse(wsId) > 4) || (storeId == 3 && int.Parse(wsId) == 6)) {
                        view.printOutBoundError();
                        checkWsid = false;
                    }
                }
            }

            int workshopId = int.Parse(wsId);
            Workshop aWorkshop = new Workshop();

            foreach (Workshop workshop in workshops)
                if (int.Parse(workshop.Id) == workshopId) {
                    aWorkshop = workshop; //Pick a workshop to add to the shopping cart
                    break;
                }

            if (aWorkshop.Capacity == aWorkshop.Reserved) { //check if the selected workshop is full
                view.printFullMessage();
                continue;
            }

            cart.Workshop = new Workshop(aWorkshop.Id, aWorkshop.Capacity, aWorkshop.Reserved + 1);
            view.congrat(0);

            bookingDone = true;
        }
    }
}
