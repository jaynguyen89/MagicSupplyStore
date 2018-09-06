using System;
using System.Collections.Generic;

public class ShoppingCart {
    public List<PurchasedItem> Trolley { get; set; }
    public Workshop Workshop { get; set; }

    public ShoppingCart() { }

    public ShoppingCart(List<PurchasedItem> trolley, Workshop ws) {
        this.Trolley = trolley;
        this.Workshop = ws;
    }
}
