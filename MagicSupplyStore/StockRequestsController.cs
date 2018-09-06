using System;

public class StockRequestsCtrl {
    private StockRequests requests;
    private StockRequestsView requestsView;

	public StockRequestsCtrl(StockRequests request, StockRequestsView view)	{
        requests = request;
        requestsView = view;
	}

    //Read all stock requests into an array then display them, return the number of stock requests
    public int displayAllStockRequests() {
        StockRequests[] stockRequests = requests.retrieveStockRequests();

        if (stockRequests == null || stockRequests.Length == 0)
            return 0;
        else {
            requestsView.displayAllStockRequests(stockRequests);
            return stockRequests.Length;
        }
    }

    //Prompt the store owner to enter a stock request id to process
    public void processRequest(int requestCount, bool context) {
        bool taskFinished = false;
        while (!taskFinished) {
            int id = 0;
            bool idCheck = false;

            while (!idCheck) {
                HelperCtrl.HelperCtrl helperCtrl = new HelperCtrl.HelperCtrl();
                HelperView helperView = new HelperView();

                helperCtrl.askRequest(helperView, false);
                string idInput = Console.ReadLine();

                Helper idHelper = new Helper(idInput);
                if (!helperCtrl.checkStockRequestID(idHelper, requestCount, context))
                    continue;
                else {
                    id = int.Parse(idInput);
                    idCheck = true;
                }
            }

            requests.processRequest(id); // process after validate the id
            taskFinished = true;
        }
    }

    //Context in this method refers to Threshold
    public int[] displayAllStockRequestsWithContext() {
        bool context = requestsView.askContext(); //Threshold was determined by user input

        StockRequests[] stockRequests = requests.retrieveStockRequests();
        StockRequests[] stockRequestsTrue = new StockRequests[0];
        StockRequests[] stockRequestsFalse = new StockRequests[0];

        if (stockRequests == null || stockRequests.Length == 0)
            return null;
        else
        {
            int i = 0, j = 0;
            foreach (StockRequests request in stockRequests) // iterate all stock requests to classify them into True/False Restock for Threshold display
            {
                if (request.Available)
                {
                    Array.Resize<StockRequests>(ref stockRequestsTrue, i + 1); //True Threshold
                    stockRequestsTrue[i] = request;
                    i++;
                }
                else
                {
                    Array.Resize<StockRequests>(ref stockRequestsFalse, j + 1); //False Threshold
                    stockRequestsFalse[j] = request;
                    j++;
                }
            }

            if (context) // return the relevant stock request list basing on the Threshold
                requestsView.displayAllStockRequests(stockRequestsTrue);
            else
                requestsView.displayAllStockRequests(stockRequestsFalse);

            int[] returningArray = new int[2];
            returningArray[0] = stockRequests.Length;
            returningArray[1] = (context) ? 1 : 0;

            return returningArray;
        }
    }
}

namespace FileException
{
    class NoFileException : System.IO.FileNotFoundException
    {
        public NoFileException(string message) : base(message + "\n") { }
    }
}
