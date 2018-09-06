using System;

public class FranchiseOwnerCtrl {
    private FranchiseOwner franOwner;
    private FranchiseOwnerView franView;

	public FranchiseOwnerCtrl(FranchiseOwner owner, FranchiseOwnerView view) {
        franOwner = owner;
        franView = view;
	}

    public void printFranchiseMenu(int storeId) {
        franView.printFranchiseMenu(storeId);
    }

    public int getTask() {
        return franView.getTask();
    }

    // prompt franchise owner to enter their store id
    public int askStoreId() {
        HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();

        bool idCheck = false;
        string idInput = "";
        int id = 0;

        while (!idCheck) {
            idInput = helperCtrl.askStore(true);
            Helper storeHelper = new Helper(idInput);

            idCheck = helperCtrl.checkInput(storeHelper);

            if (idCheck) {
                id = int.Parse(idInput);

                if (id == 5) {
                    idCheck = false;

                    HelperView helperView = new HelperView();
                    helperView.printOutBoundError();
                }
            }
        }
        return int.Parse(idInput);
    }
}
