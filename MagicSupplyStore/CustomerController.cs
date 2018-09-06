using System;
using System.Collections.Generic;
using System.Linq;

public class CustomerCtrl {
    private Customer customer;
    private CustomerView customerView;

	public CustomerCtrl(Customer customer, CustomerView view) {
        this.customer = customer;
        customerView = view;
	}

    public void printCustomerMenu(int storeId, ShoppingCart shoppingCart) {
        customerView.printCustomerMenu(storeId, shoppingCart);
    }

    public int getTask() {
        return customerView.getTask();
    }

    //Prompt for store id when customer enter the store
    public int askStoreId() {
        HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();

        bool idCheck = false;
        string idInput = "";
        int id = 0;

        while (!idCheck) {
            idInput = helperCtrl.askStore(false); //false refers to a context to display a message
            Helper storeHelper = new Helper(idInput);

            idCheck = helperCtrl.checkInput(storeHelper);

            if (idCheck) {
                id = int.Parse(idInput);

                if (id == 5) {
                    idCheck = false;

                    HelperView helperView = new HelperView();
                    helperView.printOutBoundError();
                }
            }
        }
        return int.Parse(idInput);
    }

    // Read the inventory of a store into a List<> before displaying in the next step
    public void viewStoreProducts(int storeId, ShoppingCart shoppingCart) {
        InventoryProducts inventory = new InventoryProducts();
        List<InventoryProducts> storeProducts = inventory.retrieveInventoryProducts(storeId, false); //false refers to Threshold

        if (storeProducts == null || storeProducts.Count == 0)
            shoppingCart = null;
        else
            processPurchase(storeId, storeProducts, shoppingCart); //display store products and allow customer to make purchase
    }

    private void processPurchase(int storeId, List<InventoryProducts> storeProducts, ShoppingCart shoppingCart) {
        bool taskFinished = false; //determines if customer finished purchasing or booking
        while (!taskFinished) {
            int page = 0;
            int totalPages = (storeProducts.Count % 5 == 0) ? storeProducts.Count/5 : storeProducts.Count/5 + 1;

            while (page < totalPages) { //displays products in pages, also controls the program when customer enters "R" or "C" or "P"
                customerView.viewStoreProducts(storeId, storeProducts, page, totalPages, shoppingCart);

                bool taskCheck = false; //validates user input for product id or an action
                while (!taskCheck) {
                    HelperView helperView = new HelperView();
                    helperView.askRespond(false); //false refers to a context to display some message

                    string task = Console.ReadLine();
                    Helper taskHelper = new Helper(task);

                    HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();
                    int result = helperCtrl.checkTask(taskHelper);

                    if (result == 0) { //customer enter product id
                        int productId = int.Parse(task);

                        bool found = false; // check if the id input match a product in the current page
                        for (int i = page*5; i < page*5 + 5; i++)
                            if (int.Parse(storeProducts.ElementAt(i).Id) == productId) {
                                found = true;
                                break;
                            }

                        if (found) { //id input matches a product in the current page
                            int curStock = 0; // check the selected product for stock level
                            for (int i = 0; i < storeProducts.Count; i++)
                                if (int.Parse(storeProducts.ElementAt(i).Id) == productId) {
                                    curStock = storeProducts.ElementAt(i).CurStock;
                                    break;
                                }

                            if (curStock == 0) { // the selected product has no stock left
                                helperView.warning(2);
                                taskCheck = true;

                                helperView.waiting();
                            }
                            else { // the selected product has sufficient stock
                                string qty = "";
                                bool qtyCheck = false; 

                                while (!qtyCheck) {
                                    helperView.askQty(); //prompt for entering amount of item to purchase

                                    qty = Console.ReadLine();
                                    qtyCheck = helperCtrl.checkQty(qty, productId, storeProducts);
                                }

                                // Pick the selected product to prepare the shopping cart
                                InventoryProducts item = new InventoryProducts();
                                for (int i = 0; i < storeProducts.Count; i++)
                                    if (int.Parse(storeProducts.ElementAt(i).Id) == productId) {
                                        item = storeProducts.ElementAt(i);
                                        break;
                                    }

                                PurchasedItem aPurchase = new PurchasedItem(item.Id, item.ProductName, int.Parse(qty));

                                shoppingCart.Trolley.Add(aPurchase); //place product into shopping cart
                                helperView.congrat(1);
                                helperView.waiting();

                                taskCheck = true;
                            }
                        }
                        else { //id input matches nothing in the current page
                            helperView.printIDNotFound();
                            taskCheck = true;
                        }
                    }
                    else if (result == 1) { //customer enter an action
                        if (task.ToUpper().Equals("P")) { // go to next page
                            if (page == totalPages)
                                page = 0; // if the last page is displaying, revert to the first page
                            else
                                page += 1;

                            taskCheck = true;
                        }
                        else if (task.ToUpper().Equals("R")) { // return to customer menu
                            taskCheck = true;
                            page = totalPages;
                            taskFinished = true;
                        }
                        else { // redirect customer to check out
                            ShoppingCartView cartView = new ShoppingCartView();
                            ShoppingCartCtrl cartCtrl = new ShoppingCartCtrl(shoppingCart, cartView);

                            cartCtrl.checkOut(storeId);

                            //destroy shopping cart after customer checks out and data are updated in the system
                            shoppingCart.Trolley = null;
                            shoppingCart.Workshop = null;

                            taskCheck = true;
                            page = totalPages;
                            taskFinished = true;
                        }
                    }
                    else
                        helperView.printTaskError();
                }
            }
        }
    }

    //Read all workshop information of a store then display to customers
    public void displayWorkshops(int storeId, ShoppingCart shoppingCart) {
        WorkshopView wsv = new WorkshopView();
        Workshop ws = new Workshop();
        WorkshopCtrl wsCtrl = new WorkshopCtrl(ws, wsv);

        List<Workshop> workshops = wsCtrl.retrieveWorkshops(storeId); 

        if (workshops == null || workshops.Count == 0) { }
        else {
            wsCtrl.displayWorkshops(storeId, workshops, shoppingCart);
            wsCtrl.processBooking(storeId, workshops, shoppingCart); // allow customer to make a workshop booking
        }
    }

    private void wait(string task, int taskCheck) {
        if (taskCheck == 0 || (taskCheck == 1 && !string.Equals(task, "R"))) {
            HelperView helperView = new HelperView();
            helperView.waiting();
        }
    }
}
