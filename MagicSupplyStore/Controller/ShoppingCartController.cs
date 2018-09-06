using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public class ShoppingCartCtrl {
    const double DISCOUNT = 0.9;
    ShoppingCart Cart { get; set; }
    ShoppingCartView CartView { get; set; }

	public ShoppingCartCtrl() { }

    public ShoppingCartCtrl(ShoppingCart shoppingCart, ShoppingCartView cartView) {
        this.Cart = shoppingCart;
        this.CartView = cartView;
    }

    /*control all activities when customer completes purchase and wants to check out the system.
     * This method calculates the bill then prompt customer to enter payment info.
     * When payment is successful, it issues a unique reference code if customer did booking a workshop.
     * Then it updates the following files: [franchise]_inventory.json, and [franchise]_workshop.txt*/
    public void checkOut(int storeId) {
        Dictionary<int, int> unitPrices = new Dictionary<int, int>();
        double invoice = 0.0;

        //Read product prices into a dictionary
        string currentDir = Directory.GetCurrentDirectory();
        string filePath = currentDir + "\\Files\\unit_price.txt";

        bool found = false;
        string[] prices = null;

        HelperView helperView = new HelperView();
        try {
            prices = File.ReadAllLines(filePath);

            if (prices != null || prices.Length != 0)
                found = true;
        }
        catch (FileNotFoundException) {
            helperView.printFNFError();
        }

        if (found) {
            foreach (string price in prices) {
                string[] tokens = price.Split(',');

                unitPrices.Add(int.Parse(tokens[0]), int.Parse(tokens[1]));
            }

            //calculate the invoice
            foreach (PurchasedItem item in Cart.Trolley)
                invoice += item.Quantity * unitPrices[int.Parse(item.Id)];

            //check and calculate the invoice after discount
            if (Cart.Workshop != null)
                invoice *= DISCOUNT;

            //prompt customer for a payment
            if (invoice != 0) {
                bool inputCheck = false;
                while (!inputCheck) {
                    CartView.informInvoice(invoice);

                    string card = Console.ReadLine();
                    Helper helper = new Helper(card);

                    //issue a unique reference code for workshop
                    if (!helper.checkCard())
                        continue;
                    else {
                        Guid guid;
                        if (Cart.Workshop != null) {
                            guid = Guid.NewGuid();
                            helperView.informPurchaseStatus(guid);
                            inputCheck = true;
                        }
                        else {
                            helperView.congrat(3);
                            inputCheck = true;
                        }
                    }
                }
            }
            else { // in case customer only has workshop booking but no purchase
                Guid guid;
                if (Cart.Workshop != null) {
                    guid = Guid.NewGuid();
                    helperView.informPurchaseStatus(guid);
                }
                else
                    helperView.congrat(3);
            }

            //update the files
            string fileName = (storeId == 1) ? "gryffindor_inventory.json" :
                          ((storeId == 2) ? "hufflepuff_inventory.json" :
                          ((storeId == 3) ? "ravenclaw_inventory.json" : "slytherin_inventory.json"));

            string inventoryFilePath = currentDir + "\\Files\\" + fileName;
            InventoryProducts inventory = new InventoryProducts();

            List<InventoryProducts> workingInventory = inventory.retrieveInventoryProducts(storeId, false);
            for (int i = 0; i < workingInventory.Count; i++)
                foreach (PurchasedItem item in Cart.Trolley)
                    if (workingInventory[i].Id.Equals(item.Id)) {
                        int newQty = workingInventory[i].CurStock - item.Quantity;
                        workingInventory[i].CurStock = newQty;
                        break;
                    }
            
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string inventoryContent = jss.Serialize(workingInventory);

            try {
                File.WriteAllText(inventoryFilePath, inventoryContent);
            }
            catch (FileNotFoundException) {
                helperView.printFNFError();
            }
            catch (DirectoryNotFoundException) {
                helperView.printFNFError();
            }

            Workshop workshop = new Workshop();
            List<Workshop> workshops = workshop.retrieveWorkshops(storeId);

            for (int i = 0; i < workshops.Count; i++)
                if (workshops[i].Id.Equals(Cart.Workshop.Id)) {
                    workshops[i].Reserved = Cart.Workshop.Reserved;
                    break;
                }

            string workshopContent = "";
            foreach (Workshop ws in workshops) {
                string line = ws.Id + "," + ws.Capacity + "," + ws.Reserved + Environment.NewLine;

                workshopContent += line;
            }

            string storeName = (storeId == 1) ? "gryffindor_" :
                          ((storeId == 2) ? "hufflepuff_" :
                          ((storeId == 3) ? "ravenclaw_" : "slytherin_"));

            string workshopFilePath = currentDir + "\\Files\\" + storeName + "workshops.txt";
            try {
                File.WriteAllText(workshopFilePath, workshopContent);
            }
            catch (FileNotFoundException) {
                helperView.printFNFError();
            }
            catch (DirectoryNotFoundException) {
                helperView.printFNFError();
            }
        }
    }
}
