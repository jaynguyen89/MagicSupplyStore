using System;

public class Products {
    public string Id { get; set; }
    public string ProductName { get; set; }

    public Products() { }

	public Products(string id, string name) {
        this.Id = id;
        ProductName = name;
	}
}
