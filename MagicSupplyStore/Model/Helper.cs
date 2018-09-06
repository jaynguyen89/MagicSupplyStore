using System;
using System.Collections.Generic;
using System.Linq;

public class Helper {
    private string input;

    public Helper() { }

	public Helper(string input) {
        this.input = input;
	}

    public string getInput() {
        return input;
    }

    // Check main menu option
    public int checkInput() {
        if (input.Length != 1)
            return 0;
        else {
            int i = 0;
            bool success = int.TryParse(input, out i);

            return !success ? 1 : ((i < 1 || i > 5) ? 2 : 3);
        }
    }

    // check stock request id if it is a number and matches an item in stock request file
    public int checkStockRequestID(int requestCount, bool context) {
        int id = 0;
        bool success = int.TryParse(input, out id);

        if (!success) // id input is not a number
            return 0;
        else {
            StockRequests srObject = new StockRequests();
            StockRequests[] requests = srObject.retrieveStockRequests();

            foreach (StockRequests request in requests) { //iterate each stock request to check the id
                if (!context) {
                    if (int.Parse(request.Id) != id)
                        continue;
                    else
                        return (int.Parse(request.Id) == id && request.Available) ? 1 : 4;
                }
                else {
                    if (int.Parse(request.Id) != id)
                        continue;
                    else
                        return (int.Parse(request.Id) == id && request.Available) ? 1 : 3;
                }
            }
        }
        return 2;
    }

    //check if the id is a number and matches a product in the inventory, similar with the above method
    public int checkInventoryRequestID(List<InventoryProducts> inventory, bool context) {
        int id = 0;
        bool success = int.TryParse(input, out id);

        if (!success)
            return 0;
        else {
            foreach (InventoryProducts item in inventory) {
                if (!context) {
                    if (int.Parse(item.Id) != id)
                        continue;
                    else
                        return (int.Parse(item.Id) == id && item.Restock) ? 1 : 3;
                }
                else {
                    if (int.Parse(item.Id) != id)
                        continue;
                    else
                        return (int.Parse(item.Id) == id && item.Restock) ? 1 : 4;
                }
            }
        }
        return 2;
    }

    //Check the Threshold for displaying franchise inventory
    public int checkContext() {
        if (input.Length != 1 && input.Length != 4 && input.Length != 5)
            return 0;
        else if (input.Length == 1)
            return (input.Equals("T") || input.Equals("t")) ? 1 : ((input.Equals("F") || input.Equals("f")) ? 2 : 0);
        else if (input.Length == 4)
            return (input.Equals("True") || input.Equals("TRUE") || input.Equals("true")) ? 1 : 0;
        else
            return (input.Equals("False") || input.Equals("FALSE") || input.Equals("false")) ? 2 : 0;
    }

    //check the id when franchise owner specifies a new product to add to their inventory
    public int checkProductId(List<ProductLines> newProducts) {
        int id = 0;
        bool success = int.TryParse(input, out id);

        if (!success)
            return 0;
        else
            foreach (ProductLines product in newProducts)
                if (id == int.Parse(product.Id))
                    return 1;

        return 2;
    }

    /*check the options that a customer enters when they browse a store's products
     * option may be product id or an action*/
    public int checkTask() {
        int n = 0;
        bool success = int.TryParse(input, out n);

        if (success)
            return 0;
        else if (input.Length == 1) {
            if (string.Equals(input, "P") || string.Equals(input, "C") || string.Equals(input, "R"))
                return 1;
            else if (string.Equals(input, "p") || string.Equals(input, "c") || string.Equals(input, "r"))
                return 1;
            else
                return 2;
        }
        else
            return 2;
    }

    /*check the quantity that customer want to purchase a product,
     * customer can't purchase more than the available stock, and cant purchase a product having no stock*/
    public int checkQty(string qty, int productId, List<InventoryProducts> storeProducts) {
        int n;
        bool success = int.TryParse(qty, out n);

        if (!success)
            return 0;
        else {
            int curStock = 0;
            for (int i = 0; i < storeProducts.Count; i++)
                if (int.Parse(storeProducts.ElementAt(i).Id) == productId) {
                    curStock = storeProducts.ElementAt(i).CurStock;
                    break;
                }

            return (n > curStock) ? 2 : 1;
        }
    }

    //check if the workshop id entered by customer is a valid one
    //Asumption: each franchise store has their fixed number of workshops and fixed capacity for every workshop
    public bool checkWorkshopId() {
        HelperView view = new HelperView();

        if (input.Length == 1) {
            int n = 0;
            if (int.TryParse(input, out n)) {
                if (n == 0 || n > 6) {
                    view.printOutBoundError();
                    return false;
                }
                else
                    return true;
            }
            else {
                view.printNotNumError();
                return false;
            }
        }
        else {
            view.printNotNumError();
            return false;
        }
    }

    //check credit card number input when customer makes payment during checkout.
    //Asumption: credit card number takes form: '1111 2222 3333 4444'
    public bool checkCard() {
        HelperView view = new HelperView();
        string[] tokens = input.Split(' ');

        bool valid = false;
        if (tokens.Length != 4) { // should have 4 blocks of numbers
            view.printCardError();
            return false;
        }
        else {
            for (int i = 0; i < tokens.Length; i++) { //each block should be a number with length of 4
                int n = 0;
                if (tokens[i].Length != 4 || !int.TryParse(tokens[i], out n))
                {
                    view.printCardError();
                    return false;
                }

                if (tokens[i].Length == 4 && int.TryParse(tokens[i], out n) && i == tokens.Length - 1)
                    valid = true;
            }
        }
        return valid;
    }
}
