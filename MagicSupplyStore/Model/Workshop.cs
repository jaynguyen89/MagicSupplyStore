using System;
using System.Collections.Generic;
using System.IO;

public class Workshop {
    public string Id { get; set; }
    public int Capacity { get; set; }
    public int Reserved { get; set; }

    public Workshop() { }

	public Workshop(string id, int cap, int res) {
        this.Id = id;
        this.Capacity = cap;
        this.Reserved = res;
	}

    //Read all workshop information into a List<> from a [franchise]_workshop.txt file
    public List<Workshop> retrieveWorkshops(int storeId) {
        HelperView helperView = new HelperView();
        string[] workshops = null;

        string storeName = (storeId == 1) ? "gryffindor_" :
                          ((storeId == 2) ? "hufflepuff_" :
                          ((storeId == 3) ? "ravenclaw_" : "slytherin_"));

        string currentDir = Directory.GetCurrentDirectory();
        string filePath = currentDir + "\\Files\\" + storeName + "workshops.txt";

        bool found = false;

        try {
            workshops = File.ReadAllLines(filePath);

            if (workshops != null || workshops.Length != 0)
                found = true;
        }
        catch (FileNotFoundException) {
            helperView.printFNFError();
        }

        List<Workshop> Workshops = new List<Workshop>();
        if (found) {
            foreach (string workshop in workshops) {
                string[] tokens = workshop.Split(',');
                
                string id = tokens[0];
                int cap = int.Parse(tokens[1]);
                int res = int.Parse(tokens[2]);

                Workshop aWorkshop = new Workshop(id, cap, res);
                Workshops.Add(aWorkshop);
            }

            return Workshops;
        }
        else
            return null;
    }
}
