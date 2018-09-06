using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

public class ProductLines : Products {
    public int Quantity { get; set; }

	public ProductLines() : base() { }

    public ProductLines(string id, string name, int qty) : base(id, name) {
        Quantity = qty;
    }

    //Read all products from owner_inventory.json into the List<>*/
    public List<ProductLines> retrieveAllProducts() {
        HelperView helperView = new HelperView();

        string currentDir = Directory.GetCurrentDirectory();
        string filePath = currentDir + "\\Files\\owner_inventory.json";

        List<ProductLines> products = new List<ProductLines>();
        bool found = false;
        string jsonContent = "";
        JavaScriptSerializer jss = new JavaScriptSerializer();

        try {
            jsonContent = File.ReadAllText(filePath);

            if (jsonContent != null || jsonContent.Length != 0)
                found = true;
        } catch (FileNotFoundException) {
            helperView.printFNFError();
        }

        if (found) {
            products = jss.Deserialize<List<ProductLines>>(jsonContent);
            return products;
        }
        else
            return null;
    }
}
