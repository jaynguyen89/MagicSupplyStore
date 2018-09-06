using System;
using System.Collections.Generic;

namespace HelperCtrl {
    class HelperCtrl {
        public HelperCtrl() { }

        public void printWelcome(HelperView helperView) {
            helperView.printWelcome();
        }

        public void askPosition(HelperView helperView) {
            helperView.askPosition();
        }

        public void askRequest(HelperView helperView, bool context) {
            helperView.askRequest(context);
        }

        public bool checkInput(Helper helper) {
            HelperView helperView = new HelperView();

            int result = helper.checkInput();
            if (result == 0) {
                helperView.printEmptyError();
                return false;
            }
            else if (result == 1) {
                helperView.printNotNumError();
                return false;
            }
            else if (result == 2) {
                helperView.printOutBoundError();
                return false;
            }
            else
                return true;
        }

        public bool checkStockRequestID(Helper helper, int requestCount, bool context) {
            HelperView helperView = new HelperView();

            int result = helper.checkStockRequestID(requestCount, context);
            if (result == 0) {
                helperView.printNotNumError();
                return false;
            }
            else if (result == 2) {
                helperView.printOutBoundError();
                return false;
            }
            else if (result == 3) {
                helperView.printIDNotFound();
                return false;
            }
            else if (result == 4) {
                helperView.printUnavailable(false);
                return false;
            }
            else
                return true;
        }

        public int checkInventoryRequestID(Helper helper, List<InventoryProducts> inventory, bool context) {
            return helper.checkInventoryRequestID(inventory, context);
        }

        public bool checkProductId(Helper helper, List<ProductLines> newProducts) {
            HelperView view = new HelperView();
            int result = helper.checkProductId(newProducts);

            if (result == 0) {
                view.printNotNumError();
                return false;
            }
            else if (result == 1)
                return true;
            else {
                view.printIDNotFound();
                return false;
            }
        }

        public string askStore(bool context) {
            HelperView helperView = new HelperView();
            helperView.askStore(context);

            string input = Console.ReadLine();
            return input;
        }

        public int checkContext(Helper helper) {
            return helper.checkContext();
        }

        public int checkTask(Helper taskHelper) {
            return taskHelper.checkTask();
        }

        public bool checkQty(string qty, int productId, List<InventoryProducts> storeProducts) {
            Helper helper = new Helper();
            int result = helper.checkQty(qty, productId, storeProducts);

            HelperView view = new HelperView();
            if (result == 0) {
                view.printNotNumError();
                return false;
            }
            else if (result == 1)
                return true;
            else {
                view.warning(1);
                return false;
            }
        }

        public void taskWaiting(Helper taskHelper, HelperView view) {
            if (int.Parse(taskHelper.getInput()) != 4 || int.Parse(taskHelper.getInput()) != 5)
                view.waiting();
        }

        public void goodBye(HelperView helperView) {
            helperView.goodBye();
        }
    }
}
