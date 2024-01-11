[System.Serializable]
public class Blueprint
{
    public string itemName;
    public int numOfRequirements;

    public string req1;
    public string req2;

    public int ReqAmount1;
    public int ReqAmount2;

    public int numOfResults;

    public Blueprint(string name, int numReq, string r1, int r1Amt, string r2, int r2Amt, int numResults)
    {
        itemName = name;
        numOfRequirements = numReq;
        req1 = r1;
        ReqAmount1 = r1Amt;
        req2 = r2;
        ReqAmount2 = r2Amt;
        numOfResults = numResults;
    }
}