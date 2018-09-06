using System;

public class FranchiseOwnerView {
	public FranchiseOwnerView() { }

    public void printFranchiseMenu(int storeId) {
        string store = (storeId == 1) ? "Gryffindor Witchers" : 
                       ((storeId == 2) ? "Hufflepuff House" : 
                       ((storeId == 3) ? "Ravenclaw Wizards" : "Slytherin Manor"));

        Console.WriteLine("\n\n\n\tWelcome to Magic Supply Store (Franchise Owner - {0})", store);
        Console.WriteLine("========================================================================================\n");
        Console.WriteLine("Please enter a number of your task:\n");
        Console.WriteLine("\t1. Display Inventory\n\t2. Display Inventory (Threshold)");
        Console.WriteLine("\t3. Add new Inventory Item\n\t4. Return to Main Menu\n\t5. Exit\n");
    }


    // prompt framchise owner to select an action from menu
    public int getTask() {
        bool approval = false;
        int task = 0;

        while (!approval) {
            Console.Write("Enter an option: ");
            string option = Console.ReadLine();

            Helper taskHelper = new Helper(option);
            HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();

            if (!helperCtrl.checkInput(taskHelper))
                continue;
            else {
                task = int.Parse(option);
                approval = true;
            }
        }
        return task;
    }
}
