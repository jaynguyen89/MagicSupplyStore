using System;

public class StoreOwnerView {
	public StoreOwnerView() { }

    public void printOwnerMenu() {
        Console.WriteLine("\n\n\n\tWelcome to Magic Supply Store (Store Owner)");
        Console.WriteLine("=============================================================\n");
        Console.WriteLine("Please enter a number of your task:\n");
        Console.WriteLine("\t1. Display all Stock Requests\n\t2. Display Stock Requests (True/False)");
        Console.WriteLine("\t3. Display all Product Lines\n\t4. Return to Main Menu\n\t5. Exit\n");
    }

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
