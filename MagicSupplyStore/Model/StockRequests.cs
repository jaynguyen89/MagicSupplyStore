using System;
using FileException;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class StockRequests : Products {
    public string Store { get; set; }
    public int Qty { get; set; }
    public int CurStock { get; set; }
    public bool Available { get; set; }

    public StockRequests() : base() { }

    public StockRequests(string id, string store, string item, int qty, int stock, bool avai) : base(id, item) {
        this.Store = store;
        this.Qty = qty;
        CurStock = stock;
        Available = avai;
    }

    // Read all stock requests from a text file and store it in an array
    public StockRequests[] retrieveStockRequests() {
        HelperView helperView = new HelperView();
        string[] requests = null;

        string currentDir = Directory.GetCurrentDirectory();
        string filePath = currentDir + "\\Files\\stock_requests.txt";

        bool found = false;

        try {
            requests = File.ReadAllLines(filePath);

            if (requests != null || requests.Length != 0)
                found = true;
        } catch (FileNotFoundException) {
            helperView.printFNFError();
        }

        StockRequests[] requestsArray = new StockRequests[requests.Length];

        if (found) {
            int i = 0; // to track a line in the file, where may contain a content mistake
            foreach (string request in requests) {
                string[] tokens = request.Split(',');

                string store = tokens[0];
                string item = tokens[1];
                int qty = int.Parse(tokens[2]);
                int stock = int.Parse(tokens[3]);
                bool avai = false;

                try {
                    if (int.Parse(tokens[4]) == 1)
                        avai = true;
                }
                catch (IndexOutOfRangeException) {
                    helperView.printFileContentError(i + 1);
                    return null;
                }
                catch (FormatException) {
                    helperView.printFileContentError(i + 1);
                    return null;
                }
                
                StockRequests aRequest = new StockRequests((i + 1).ToString(), store, item, qty, stock, avai);
                requestsArray[i] = aRequest;
                i++;
            }
            
            return requestsArray;
        }
        else
            return null;
    }

    /*This method updates the relevant files when a stock request is selected to process by store owner.
     All of the files are updated including: stock_requests.txt, owner_inventory.json, and [franchise]_inventory.json */
    public void processRequest(int id) {
        StockRequests[] requests = retrieveStockRequests();
        StockRequests request = new StockRequests();

        ProductLines productLines = new ProductLines();
        List<ProductLines> allProducts = productLines.retrieveAllProducts();

        List<string> requestFileLines = new List<string>();

        for (int i = 0; i < requests.Length; i++)
            if (int.Parse(requests[i].Id) == id) {
                request = requests[i];
                break;
            }

        int newQty = 0;
        for (int i = 0; i < allProducts.Count; i++)
            if (allProducts[i].ProductName.Equals(request.ProductName)) {
                newQty = allProducts[i].Quantity - request.Qty;
                allProducts[i].Quantity = newQty;
                break;
            }

        for (int i = 0; i < requests.Length; i++) {
            if (int.Parse(requests[i].Id) == id)
                continue;
            else {
                if (requests[i].Qty > newQty)
                    requests[i].Available = false;

                string line = requests[i].Store + "," + requests[i].ProductName + "," + requests[i].Qty + "," + requests[i].CurStock + "," + ((requests[i].Available) ? "1" : "0") + Environment.NewLine;
                requestFileLines.Add(line);
            }
        }

        string requestContent = "";
        foreach (string line in requestFileLines)
            requestContent += line;

        string currentDir = Directory.GetCurrentDirectory();
        string requestFilePath = currentDir + "\\Files\\stock_requests.txt";

        HelperView helperView = new HelperView();
        try {
            File.WriteAllText(requestFilePath, requestContent);
        }
        catch (FileNotFoundException) {
            helperView.printFNFError();
        }
        catch (DirectoryNotFoundException) {
            helperView.printFNFError();
        }

        JavaScriptSerializer jss = new JavaScriptSerializer();
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

        string processedStore = request.Store;
        int storeId = processedStore.Equals("Gryffindor Witchers") ? 1 : 
                      ((processedStore.Equals("Hufflepuff House") ? 2 : 
                      (processedStore.Equals("Ravenclaw Wizards") ? 3 : 4)));

        InventoryProducts inventory = new InventoryProducts();
        List<InventoryProducts> processedInventory = inventory.retrieveInventoryProducts(storeId, false);

        for (int i = 0; i < processedInventory.Count; i++)
            if (processedInventory[i].ProductName.Equals(request.ProductName)) {
                int newAmount = request.Qty + processedInventory[i].CurStock;
                processedInventory[i].CurStock = newAmount;
                processedInventory[i].Restock = false;
                break;
            }

        string fileName = (storeId == 1) ? "gryffindor_inventory.json" :
                          ((storeId == 2) ? "hufflepuff_inventory.json" :
                          ((storeId == 3) ? "ravenclaw_inventory.json" : "slytherin_inventory.json"));

        string processedFilePath = currentDir + "\\Files\\" + fileName;
        string processedContent = jss.Serialize(processedInventory);

        try {
            File.WriteAllText(processedFilePath, processedContent);
        }
        catch (FileNotFoundException) {
            helperView.printFNFError();
        }
        catch (DirectoryNotFoundException) {
            helperView.printFNFError();
        }

        helperView.congrat(2);
    }
}
