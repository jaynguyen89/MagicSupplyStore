using System;
using System.Collections.Generic;

public class ProductLinesCtrl {
    private ProductLines productLine;
    private ProductLinesView productLineView;

	public ProductLinesCtrl(ProductLines productLine, ProductLinesView productLineView) {
        this.productLine = productLine;
        this.productLineView = productLineView;
	}

    public void displayAllProducts() {
        List<ProductLines> productLines = productLine.retrieveAllProducts();

        if (productLines == null || productLines.Count == 0) { }
        else
            productLineView.displayAllProducts(productLines, true);
    }
}
