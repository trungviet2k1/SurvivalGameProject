using System;

[System.Serializable]
public class Blueprint
{
    public string itemName;
    public int numOfRequirements;
    public string[] requiredItems;
    public int[] numberOfRequests;
    public int numberOfItemsCreated;

    public Blueprint(string name, int producedItems, params Tuple<string, int>[] reqs)
    {
        itemName = name;
        numberOfItemsCreated = producedItems;
        numOfRequirements = reqs.Length;
        requiredItems = new string[numOfRequirements];
        numberOfRequests = new int[numOfRequirements];

        for (int i = 0; i < numOfRequirements; i++)
        {
            requiredItems[i] = reqs[i].Item1;
            numberOfRequests[i] = reqs[i].Item2;
        }
    }
}