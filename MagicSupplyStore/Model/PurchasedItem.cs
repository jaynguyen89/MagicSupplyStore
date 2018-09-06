using System;

public class PurchasedItem : Products {
    public int Quantity { get; set; }

    public PurchasedItem() : base() { }

    public PurchasedItem(string id, string name, int qty) : base(id, name) {
        this.Quantity = qty;
    }
}
