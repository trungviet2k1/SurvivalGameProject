using UnityEngine;

public class Blueprint
{
    public string itemName;

    public string req1;
    public string req2;

    public int ReqAmount1;
    public int ReqAmount2;

    public int numOfRequirements;

    public Blueprint(string name, int reqNum, string r1, int r1Num, string r2, int r2Num)
    {
        itemName = name;
        numOfRequirements = reqNum;
        req1 = r1;
        req2 = r2;

        ReqAmount1 = r1Num;
        ReqAmount2 = r2Num;
    }
}
