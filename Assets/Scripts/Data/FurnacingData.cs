using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnacingData", menuName = "ScriptableObject/FurnacingData", order = 1)]
public class FurnacingData : ScriptableObject
{
    public List<string> validFuels = new();
    public List<CalcinableMinerals> validMinerals = new();
}

[System.Serializable]
public class CalcinableMinerals
{
    public string name;
    public float timeToCalcine;
    public string calcinedMineralName;
}