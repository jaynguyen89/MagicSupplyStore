using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public class InventoryProducts : Products {
    public int CurStock { get; set; }
    public bool Restock { get; set; }

	public InventoryProducts() : base() { }

    public InventoryProducts(string id, string name, int curStock, bool restock) : base(id, name) {
        this.CurStock = curStock;
        this.Restock = restock;
    }

    /*this method is used in writting the codes for franchise owner and customer
     * It read the inventory of a store and store each inventory item in a List<>*/
    public List<InventoryProducts> retrieveInventoryProducts(int storeId, bool context) { //context refers to Threshold
        HelperView helperView = new HelperView();

        string fileName = (storeId == 1) ? "gryffindor_inventory.json" : 
                          ((storeId == 2) ? "hufflepuff_inventory.json" : 
                          ((storeId == 3) ? "ravenclaw_inventory.json" : "slytherin_inventory.json"));

        string currentDir = Directory.GetCurrentDirectory();
        string filePath = currentDir + "\\Files\\" + fileName;

        List<InventoryProducts> inventory = new List<InventoryProducts>();
        List<InventoryProducts> inventoryThreshold = new List<InventoryProducts>();
        bool found = false;
        string jsonContent = "";
        JavaScriptSerializer jss = new JavaScriptSerializer();

        try {
            jsonContent = File.ReadAllText(filePath); // Read all json file content into a string

            if (jsonContent != null || jsonContent.Length != 0)
                found = true;
        } catch (FileNotFoundException) {
            helperView.printFNFError();
        }

        if (found) {
            bool success = false;
            try {
                inventory = jss.Deserialize<List<InventoryProducts>>(jsonContent); //save each inventory item to the list

                if (inventory != null || inventory.Count != 0)
                    success = true;
            } catch (ArgumentException) {
                helperView.printFileContentError(0);
            }

            if (success) {
                if (context) { // context == true --> extra step to filter inventory with Threshold
                    foreach (InventoryProducts product in inventory) {
                        if (product.Restock)
                            inventoryThreshold.Add(product);
                    }
                    return inventoryThreshold;
                }
                else
                    return inventory;
            }
            else
                return null;
        }
        else
            return null;
    }

    // Process an inventory request, it updates the contents of stock_requests.txt
    public void processRequest(int storeId, int id, List<InventoryProducts> inventory) {
        InventoryProducts request = new InventoryProducts();
        foreach (InventoryProducts product in inventory)
            if (int.Parse(product.Id) == id) {
                request = product;
                break;
            }

        string store = (storeId == 1) ? "Gryffindor Witchers" :
                       ((storeId == 2) ? "Hufflepuff House" :
                       ((storeId == 3) ? "Ravenclaw Wizards" : "Slytherin Manor"));

        StockRequests stockRequests = new StockRequests();
        StockRequests[] allRequests = stockRequests.retrieveStockRequests();

        int requestQuantity = 0;
        if (request.Restock == false)
            requestQuantity = 5;
        else
            requestQuantity = 10;

        List<string> requestFileLines = new List<string>();

        int newQty = 0;
        bool found = false;
        for (int i = 0; i < allRequests.Length; i++) { // iterate the file content to determine where to be updated
            string line = "";
            if (allRequests[i].ProductName.Equals(request.ProductName) && store.Equals(allRequests[i].Store)) {
                newQty = allRequests[i].Qty + requestQuantity;
                line = allRequests[i].Store + "," + allRequests[i].ProductName + "," + allRequests[i].CurStock + "," + newQty + "," + ((allRequests[i].Available) ? "1" : "0") + Environment.NewLine;
                found = true;
            }
            else
                line = allRequests[i].Store + "," + allRequests[i].ProductName + "," + allRequests[i].CurStock + "," + allRequests[i].Qty + "," + ((allRequests[i].Available) ? "1" : "0") + Environment.NewLine;

            requestFileLines.Add(line); //prepare new file content to overwrite the old file
        }

        if (!found) {
            string line = store + "," + request.ProductName + "," + request.CurStock + "," + requestQuantity + "," + 1 + Environment.NewLine;
            requestFileLines.Add(line);
        }

        string requestContent = "";
        foreach (string line in requestFileLines)
            requestContent += line;

        string currentDir = Directory.GetCurrentDirectory();
        string requestFilePath = currentDir + "\\Files\\stock_requests.txt";

        HelperView helperView = new HelperView();
        try {
            File.WriteAllText(requestFilePath, requestContent); //write to file
        }
        catch (FileNotFoundException) {
            helperView.printFNFError();
        }
        catch (DirectoryNotFoundException) {
            helperView.printFNFError();
        }

        helperView.congrat(2);
    }

    // Read all products from owner's inventory and franchise's inventory, then compare them to find new products
    public List<ProductLines> retrieveNewInventoryProducts(int storeId) {
        List<InventoryProducts> currentProducts = retrieveInventoryProducts(storeId, false);

        ProductLines productLines = new ProductLines();
        List<ProductLines> allProducts = productLines.retrieveAllProducts();

        List<ProductLines> newProducts = new List<ProductLines>();

        foreach (ProductLines product in allProducts) { // compare the 2 inventories
            int i = 0;
            foreach (InventoryProducts storeProduct in currentProducts) {
                if (product.Id.Equals(storeProduct.Id)) //common products
                    break;

                if (i == (currentProducts.Count - 1) && !product.Id.Equals(storeProduct.Id))
                    newProducts.Add(product);

                i++;
            }
        }
        return newProducts;
    }

    /*Pick a product from owner's inventory to add to franchise's inventory
     * Asumption: the default quantity to add product is 1/3 quantity of that product in owner's inventory
     * This method updates the following files: [franchise]_inventory.json, and owner_inventory.json*/
    public void addNewProducts(List<ProductLines> newProducts, int id, int storeId) {
        ProductLines aProduct = new ProductLines();
        foreach (ProductLines product in newProducts)
            if (int.Parse(product.Id) == id)
                aProduct = product; // pick the intended product from the list of new products

        int defaultQty = aProduct.Quantity / 3; // set default quantity

        ProductLines productLines = new ProductLines();
        List<ProductLines> allProducts = productLines.retrieveAllProducts();

        int newQty = 0;
        for (int i = 0; i < allProducts.Count; i++) // iterate owner's inventory to update new quantity
            if (allProducts[i].ProductName.Equals(aProduct.ProductName)) {
                newQty = allProducts[i].Quantity - defaultQty;
                allProducts[i].Quantity = newQty;
                break;
            }

        //Prepare the new product to add into the inventory
        InventoryProducts newInvenProduct = new InventoryProducts(id.ToString(), aProduct.ProductName, defaultQty, false);

        List<InventoryProducts> inventory = retrieveInventoryProducts(storeId, false);
        inventory.Add(newInvenProduct);

        //Update files after adding
        JavaScriptSerializer jss = new JavaScriptSerializer();
        string currentDir = Directory.GetCurrentDirectory();

        string fileName = (storeId == 1) ? "gryffindor_inventory.json" :
                          ((storeId == 2) ? "hufflepuff_inventory.json" :
                          ((storeId == 3) ? "ravenclaw_inventory.json" : "slytherin_inventory.json"));

        string inventoryFilePath = currentDir + "\\Files\\" + fileName;
        string inventoryContent = jss.Serialize(inventory);

        HelperView helperView = new HelperView();
        try {
            File.WriteAllText(inventoryFilePath, inventoryContent);
        }
        catch (FileNotFoundException) {
            helperView.printFNFError();
        }
        catch (DirectoryNotFoundException) {
            helperView.printFNFError();
        }

        string ownerFilePath = currentDir + "\\Files\\owner_inventory.json";
        string ownerContent = jss.Serialize(allProducts);

        try {
            File.WriteAllText(ownerFilePath, ownerContent);
        }
        catch (FileNotFoundException) {
            helperView.printFNFError();
        }
        catch (DirectoryNotFoundException) {
            helperView.printFNFError();
        }
    }
}
