using System;
using System.Collections.Generic;

public class InventoryProductsCtrl {
    private InventoryProducts inventoryProduct;
    private InventoryProductsView inventoryProductView;

	public InventoryProductsCtrl(InventoryProducts inventory, InventoryProductsView inventoryView) {
        this.inventoryProduct = inventory;
        this.inventoryProductView = inventoryView;
	}

    public void displayInventoryProducts(int storeId, bool context) {
        List<InventoryProducts> inventory = inventoryProduct.retrieveInventoryProducts(storeId, context);

        if (inventory == null || inventory.Count == 0) { }
        else {
            inventoryProductView.displayInventoryProducts(inventory);
            processRequest(storeId, inventory, context);
        }
    }

    // prompt user to select a product in their inventory to send a stock request to store owner
    private void processRequest(int storeId, List<InventoryProducts> inventory, bool context) {
        bool taskFinished = false;
        while (!taskFinished) {
            int id = 0;
            bool idCheck = false;
            bool execute = false;

            while (!idCheck) {
                HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();
                HelperView helperView = new HelperView();

                helperCtrl.askRequest(helperView, false);
                string idInput = Console.ReadLine();

                Helper idHelper = new Helper(idInput); //check if the input request id is valid within a context (Threshold)
                int result = helperCtrl.checkInventoryRequestID(idHelper, inventory, context);

                if (result == 0) {
                    helperView.printNotNumError();
                    continue;
                }
                else if (result == 2) {
                    helperView.printOutBoundError();
                    continue;
                }
                else if (result == 4) {
                    helperView.printIDNotFound();
                    continue;
                }
                else if (result == 3) {
                    helperView.printUnavailable(true); // a product in inventory still has enough stock
                    bool respondCheck = false;

                    while (!respondCheck) {
                        helperView.askRespond(true);
                        string respond = Console.ReadLine(); // user need to confirm they still want to request stock for that product

                        if (respond.Length != 1 && respond.Length != 2 && respond.Length != 3)
                            helperView.printEmptyError();
                        else if (respond.Equals("n") || respond.Equals("N") || respond.Equals("No") || respond.Equals("NO")) {
                            respondCheck = true;
                            idCheck = true;
                            taskFinished = true;
                        }
                        else if (respond.Equals("y") || respond.Equals("Y") || respond.Equals("Yes") || respond.Equals("YES")) {
                            id = int.Parse(idInput);
                            respondCheck = true;
                            idCheck = true;
                            execute = true;
                        }
                        else
                            helperView.printOutBoundError();
                    }

                }
                else {
                    id = int.Parse(idInput);
                    idCheck = true;
                    execute = true;
                }
            }

            if (execute) {
                inventoryProduct.processRequest(storeId, id, inventory);
                taskFinished = true;
            }
        }
    }

    //control the flow of task execution, display the new products and prompt user to select a product to add to their inventory
    public void addNewInventoryProducts(int storeId) {
        List<ProductLines> newProducts = inventoryProduct.retrieveNewInventoryProducts(storeId);

        if (newProducts == null || newProducts.Count == 0) { }
        else {
            ProductLinesView productLinesView = new ProductLinesView();
            productLinesView.displayAllProducts(newProducts, false);

            bool taskFinished = false;
            while (!taskFinished) {
                int id = 0;
                bool idCheck = false;

                while (!idCheck) {
                    HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();
                    HelperView helperView = new HelperView();

                    helperCtrl.askRequest(helperView, true);
                    string idInput = Console.ReadLine();

                    Helper idHelper = new Helper(idInput);
                    if (!helperCtrl.checkProductId(idHelper, newProducts))
                        continue;
                    else {
                        id = int.Parse(idInput);
                        idCheck = true;
                    }
                }

                inventoryProduct.addNewProducts(newProducts, id, storeId);
                taskFinished = true;
            }
        }
    }
}
