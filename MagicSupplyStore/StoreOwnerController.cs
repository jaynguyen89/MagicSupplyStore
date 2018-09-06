using System;

public class StoreOwnerCtrl {
    private StoreOwner owner;
    private StoreOwnerView ownerView;

	public StoreOwnerCtrl(StoreOwner owner, StoreOwnerView ownerView) {
        this.owner = owner;
        this.ownerView = ownerView;
    }

    public void printOwnerMenu() {
        ownerView.printOwnerMenu();
    }

    public int getTask() {
        return ownerView.getTask();
    }
}
