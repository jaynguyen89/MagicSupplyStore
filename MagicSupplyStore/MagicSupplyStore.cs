using System;
using HelperCtrl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicSupplyStore {
    class MagicSupplyStore {
        static void Main(string[] args) {
            /*The Helper classes are used popularly in the codes to avoid rewriting the same things*/
            HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();
            HelperView helperView = new HelperView();            
            bool quit = false; //determines if users give a correct option in main menu

            while (!quit) {
                bool inputCheck = false;
                int input = 0;

                while (!inputCheck) {
                    helperCtrl.printWelcome(helperView);
                    helperCtrl.askPosition(helperView);
                    string choice = Console.ReadLine();

                    Helper mainHelper = new Helper(choice);

                    if (!helperCtrl.checkInput(mainHelper))
                        /*Waiting() method appears throughout the program to let users
                         * read the information before they wish to continue*/
                        helperView.waiting(); 
                    else {
                        input = int.Parse(choice);

                        if (input == 5) {
                            helperView.printOutBoundError();
                            helperView.waiting();
                        }
                        else
                            inputCheck = true;
                    }
                }

                //run the menu options
                switch (input) {
                    case 1: //Store owner section
                        StoreOwner owner = new StoreOwner();
                        StoreOwnerView ownerView = new StoreOwnerView();
                        StoreOwnerCtrl ownerCtrl = new StoreOwnerCtrl(owner, ownerView);

                        bool ownerTaskDone = false; //determines when to redirect owners back to main menu

                        while (!ownerTaskDone) {
                            ownerCtrl.printOwnerMenu();
                            int task = ownerCtrl.getTask();

                            Helper taskHelper = new Helper(task.ToString());

                            StockRequests stockRequests = new StockRequests();
                            StockRequestsView requestsView = new StockRequestsView();
                            StockRequestsCtrl requestsCtrl = new StockRequestsCtrl(stockRequests, requestsView);

                            //run owners menu
                            switch (task) {
                                case 1: //Display all stock requests to owner
                                    int requestCount = requestsCtrl.displayAllStockRequests();

                                    //allow owner to select a stock request to process
                                    if (requestCount != 0)
                                        requestsCtrl.processRequest(requestCount, false);

                                    helperCtrl.taskWaiting(taskHelper, helperView);
                                    break;
                                case 2: //Display stock requests in True/False context
                                    int[] infoArray = requestsCtrl.displayAllStockRequestsWithContext();

                                    /*infoArray[0] contains the number of stock requests
                                     * infoArray[1] contains True/False context, only requests in True context can be processed*/
                                    if (infoArray == null) { }
                                    else if (infoArray[1] == 1)
                                        requestsCtrl.processRequest(infoArray[0], true);

                                    helperCtrl.taskWaiting(taskHelper, helperView);
                                    break;
                                case 3: //Display all products in owner_inventory.json file
                                    ProductLines productLine = new ProductLines();
                                    ProductLinesView productLineView = new ProductLinesView();
                                    ProductLinesCtrl productLineCtrl = new ProductLinesCtrl(productLine, productLineView);

                                    productLineCtrl.displayAllProducts();

                                    helperCtrl.taskWaiting(taskHelper, helperView);
                                    break;
                                case 4: //return to main menu
                                    ownerTaskDone = true;
                                    break;
                                default: //quit and close the program
                                    helperCtrl.goodBye(helperView);

                                    ownerTaskDone = true;
                                    quit = true;
                                    break;
                            }
                        }
                        break;
                    case 2: //Franchise owner section
                        FranchiseOwner franOwner = new FranchiseOwner();
                        FranchiseOwnerView franView = new FranchiseOwnerView();
                        FranchiseOwnerCtrl franchiseCtrl = new FranchiseOwnerCtrl(franOwner, franView);

                        int storeId = franchiseCtrl.askStoreId();

                        bool franchiseTaskDone = false; //determines when to redirect franchise owner back to main menu

                        while (!franchiseTaskDone) {
                            franchiseCtrl.printFranchiseMenu(storeId);
                            int task = franchiseCtrl.getTask();

                            Helper taskHelper = new Helper(task.ToString());

                            InventoryProducts inventoryProduct = new InventoryProducts();
                            InventoryProductsView inventoryProductView = new InventoryProductsView();
                            InventoryProductsCtrl inventoryProductCtrl = new InventoryProductsCtrl(inventoryProduct, inventoryProductView);

                            //run franchise owner menu
                            switch (task) {
                                case 1: //Display all inventory products
                                    inventoryProductCtrl.displayInventoryProducts(storeId, false); //false determines displaying without Threshold

                                    helperCtrl.taskWaiting(taskHelper, helperView);
                                    break;
                                case 2: //Display inventory products Threshold
                                    inventoryProductCtrl.displayInventoryProducts(storeId, true); //true means Threashold

                                    helperCtrl.taskWaiting(taskHelper, helperView);
                                    break;
                                case 3: //Display new inventory products to add
                                    inventoryProductCtrl.addNewInventoryProducts(storeId);

                                    helperCtrl.taskWaiting(taskHelper, helperView);
                                    break;
                                case 4: //return to main menu
                                    franchiseTaskDone = true;
                                    break;
                                default: //quit and close the program
                                    helperCtrl.goodBye(helperView);

                                    franchiseTaskDone = true;
                                    quit = true;
                                    break;
                            }
                        }
                        break;
                    case 3: //customer section - customers can purchase multile items as well as make workshop booking at the same time
                        Customer customer = new Customer();
                        CustomerView customerView = new CustomerView();
                        CustomerCtrl customerCtrl = new CustomerCtrl(customer, customerView);

                        storeId = customerCtrl.askStoreId();

                        bool customerTaskDone = false; //determines when to redirect customers to main menu

                        List<PurchasedItem> trolley = new List<PurchasedItem>();
                        ShoppingCart shoppingCart = new ShoppingCart(trolley, null); //make the purchase persistent when customer switches menu options

                        while (!customerTaskDone) {
                            customerCtrl.printCustomerMenu(storeId, shoppingCart);
                            int task = customerCtrl.getTask();

                            Helper taskHelper = new Helper(task.ToString());

                            //run customer menu
                            switch (task) {
                                case 1: //Display store products
                                    customerCtrl.viewStoreProducts(storeId, shoppingCart);

                                    helperCtrl.taskWaiting(taskHelper, helperView);
                                    break;
                                case 2: //Display store workshops
                                    customerCtrl.displayWorkshops(storeId, shoppingCart);

                                    helperCtrl.taskWaiting(taskHelper, helperView);
                                    break;
                                case 3: // if customer returns to main menu while they have items in shopping cart, they need to confirm
                                    if (shoppingCart.Trolley != null || shoppingCart.Workshop != null) {
                                        helperView.warning(0);

                                        bool respondCheck = false;
                                        while (!respondCheck) {
                                            string respond = Console.ReadLine(); //read customer's confirmation

                                            if (respond.ToUpper().Equals("Y") || respond.ToUpper().Equals("YES")) {
                                                respondCheck = true;
                                                customerTaskDone = true;
                                            }
                                            else if (respond.ToUpper().Equals("N") || respond.ToUpper().Equals("NO"))
                                                respondCheck = true;
                                            else
                                                helperView.printOutBoundError();
                                        }
                                    }
                                    else
                                        customerTaskDone = true;

                                    break;
                                default: // if customer quit the program while they have items in shopping cart, they need to confirm
                                    if (shoppingCart.Trolley != null || shoppingCart.Workshop != null) {
                                        helperView.warning(0);

                                        bool respondCheck = false;
                                        while (!respondCheck) {
                                            string respond = Console.ReadLine();

                                            if (respond.ToUpper().Equals("Y") || respond.ToUpper().Equals("YES")) {
                                                respondCheck = true;
                                                customerTaskDone = true;
                                                quit = true;
                                            }
                                            else if (respond.ToUpper().Equals("N") || respond.ToUpper().Equals("NO"))
                                                respondCheck = true;
                                            else
                                                helperView.printOutBoundError();
                                        }
                                    }
                                    else {
                                        customerTaskDone = true;
                                        quit = true;
                                    }

                                    break;
                            }
                        }
                        break;
                    default:
                        helperCtrl.goodBye(helperView);
                        quit = true;
                        break;
                }
            }
                        
            System.Threading.Thread.Sleep(500);
            Environment.Exit(0);
            Console.ReadKey();
        }
    }
}
